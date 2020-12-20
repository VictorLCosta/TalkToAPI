using System.ComponentModel.DataAnnotations;

namespace TalkToAPI.V1.Models.DTO
{
    public class DTOUser : DTOBase
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Insira um e-mail válido")]
        public string Email { get; set; }

        public string Slogan { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; }
    }
}