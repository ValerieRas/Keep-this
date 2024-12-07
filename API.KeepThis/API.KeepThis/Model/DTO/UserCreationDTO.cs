using System.ComponentModel.DataAnnotations;

namespace API.KeepThis.Model.DTO
{
    public class UserCreationDTO
    {
        [Required(ErrorMessage = "L'adresse e-mail est obligatoire.")]
        [EmailAddress(ErrorMessage = "L'adresse e-mail n'est pas valide.")]
        [StringLength(100, ErrorMessage = "L'adresse e-mail ne peut pas dépasser 100 caractères.")]
        public string UserEmail { get; set; } = null!;

        [Required(ErrorMessage = "Veuillez confirmer votre adresse e-mail.")]
        [EmailAddress]
        [Compare("UserEmail", ErrorMessage = "Les adresses e-mail ne correspondent pas.")]
        public string ConfirmUserEmail { get; set; } = null!;

        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [StringLength(100, MinimumLength = 8)]
        public string UserPassword { get; set; } = null!;

        [Required(ErrorMessage = "La confirmation du mot de passe est obligatoire.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Le mot de passe doit comporter entre 8 et 100 caractères.")]
        [Compare("UserPassword", ErrorMessage = "Les mots de passe ne correspondent pas.")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "Le nom d'utilisateur est obligatoire.")]
        [StringLength(50, ErrorMessage = "Le nom d'utilisateur ne peut pas dépasser 50 caractères.")]
        public string Username { get; set; } = null!;
    }
}