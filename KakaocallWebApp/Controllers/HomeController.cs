using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KakaocallWebApp.Models;
using Microsoft.Extensions.Configuration;

namespace KakaocallWebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public IActionResult Call([FromForm] CallRequest request, [FromServices] IConfiguration config)
    {
        // 환경파일에서 카카오 API 정보 읽기
        var kakaoSection = config.GetSection("Kakao");
        var apiUrl = kakaoSection["ApiUrl"];
        var accessToken = kakaoSection["AccessToken"];

        // 실제 카카오톡 API 호출은 추후 구현, 현재는 더미 처리
        // TODO: API 준비되면 실제 호출 코드로 교체
        // 예시 메시지
        var message = $"{request.Number}번 손님, 입장해 주세요!";

        // 성공 메시지 반환(실제 구현시 결과에 따라 변경)
        ViewBag.Result = $"카카오톡 메시지 전송 완료: {message}";
        return View("Index");
    }
}
