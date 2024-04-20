using System.ComponentModel.DataAnnotations;

namespace Electronic_Mall
{
    public class RegisterViewModel
    {
   
        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        public string Username { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "يجب أن يكون البريد الإلكتروني صالحًا")]
        public string Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمات المرور غير متطابقة")]
        public string ConfirmPassword { get; set; }
    }

}
