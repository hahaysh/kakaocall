using System.ComponentModel.DataAnnotations;

namespace KakaocallWebApp.Models
{
    public class NotifyModel
    {
        // 순번 validation 제거
        public int OrderNumber { get; set; }

        [Required]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "전화번호는 숫자만 10~11자리로 입력하세요.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;
    }

    public class KakaoSettings
    {
        public string KakaoApiKey { get; set; } = string.Empty;
        public string DefaultMessage { get; set; } = string.Empty;
    }
}
