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

    public IActionResult Index([FromServices] IConfiguration config)
    {
        // 환경설정에서 기본 메시지 템플릿 읽기
        var kakaoSection = config.GetSection("Kakao");
        var defaultMessage = kakaoSection["DefaultMessage"] ?? "[전화번호] 님의 순번 [순번] 의 순서가 되었습니다. 식당 입구로 와주세요.";
        ViewBag.DefaultMessage = defaultMessage;
        return View();
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

    [HttpPost]
    public IActionResult Call([FromForm] CallRequest request, [FromForm] string Message, [FromServices] IConfiguration config)
    {
        var kakaoSection = config.GetSection("Kakao");
        var apiUrl = kakaoSection["ApiUrl"];
        var accessToken = kakaoSection["AccessToken"];
        var defaultMessage = kakaoSection["DefaultMessage"] ?? "[전화번호] 님의 순번 [순번] 의 순서가 되었습니다. 식당 입구로 와주세요.";

        var number = request.Number;
        var phone = request.Phone;
        // 환경설정 메시지 템플릿에서 값 치환
        string msgTemplate = string.IsNullOrWhiteSpace(Message) ? defaultMessage : Message;
        var message = msgTemplate.Replace("[전화번호]", string.IsNullOrEmpty(phone) ? "[전화번호]" : phone)
                                 .Replace("[순번]", string.IsNullOrEmpty(number) ? "[순번]" : number);

        ViewBag.SelectedNumber = number;
        ViewBag.Phone = phone;
        ViewBag.Message = msgTemplate;
        ViewBag.DefaultMessage = defaultMessage;
        ViewBag.Result = $"카카오톡 메시지 전송 완료: {message}";
        return View("Index");
    }
}
