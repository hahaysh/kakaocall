using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KakaocallWebApp.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace KakaocallWebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _config;

    public HomeController(ILogger<HomeController> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new NotifyModel());
    }

    [HttpPost]
    public async Task<IActionResult> Index(NotifyModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // 환경설정(appsettings.Kakao.json)에서 API키, 기본메시지 불러오기
        var kakaoSettings = new KakaoSettings();
        _config.Bind(kakaoSettings);

        // 카카오톡 메시지 전송
        var result = await SendKakaoMessage(model.PhoneNumber, model.Message, kakaoSettings.KakaoApiKey);
        ViewBag.SendResult = result;
        return View(model);
    }

    private async Task<string> SendKakaoMessage(string phone, string message, string apiKey)
    {
        // 실제 카카오톡 메시지 API 연동 예시 (실제 API에 맞게 수정 필요)
        if (string.IsNullOrEmpty(apiKey)) return "API 키가 없습니다.";
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var payload = new
            {
                phone_number = phone,
                message = message
            };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            // 실제 카카오 API 엔드포인트로 변경 필요
            var response = await client.PostAsync("https://kapi.kakao.com/v2/api/talk/memo/default/send", content);
            if (response.IsSuccessStatusCode)
                return "메시지 전송 성공";
            else
                return $"전송 실패: {response.StatusCode}";
        }
        catch (Exception ex)
        {
            return $"오류: {ex.Message}";
        }
    }

    [HttpPost]
    public IActionResult GetDefaultMessage(int orderNumber, string phoneNumber)
    {
        // 환경설정(appsettings.Kakao.json)에서 기본 메시지 불러오기
        var defaultMsg = _config["DefaultMessage"] ?? "[전화번호] 님의 순번 [순번] 의 순서가 되었습니다. 식당 입구로 와주세요.";
        var msg = defaultMsg.Replace("[순번]", orderNumber.ToString()).Replace("[전화번호]", phoneNumber);
        return Json(new { message = msg });
    }

    public IActionResult Privacy()
    {
        return NotFound(); // Privacy 페이지 제거
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
