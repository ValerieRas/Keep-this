using System.ComponentModel.DataAnnotations;

namespace API.KeepThis.Model.DTO
{
    public class UpdatePasswordDTO
    {
        [Required(ErrorMessage = "L'identifiant utilisateur est obligatoire.")]
        public string UserId { get; set; } = null!;

        [Required(ErrorMessage = "Le mot de passe actuel est obligatoire.")]
        [StringLength(100, MinimumLength = 6)]
        public string CurrentPassword { get; set; } = null!;

        [Required(ErrorMessage = "Le nouveau mot de passe est obligatoire.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir entre 6 et 100 caractères.")]
        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessage = "La confirmation du mot de passe est obligatoire.")]
        [Compare("NewPassword", ErrorMessage = "Le mot de passe et sa confirmation ne correspondent pas.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
