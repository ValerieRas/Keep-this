using System.ComponentModel.DataAnnotations;

namespace API.KeepThis.Model.DTO
{
    public class UpdateEmailDTO
    {
        [Required(ErrorMessage = "L'identifiant utilisateur est obligatoire.")]
        public string UserId { get; set; } = null!;

        [Required(ErrorMessage = "Le nouvel email est obligatoire.")]
        [EmailAddress(ErrorMessage = "Format de l'email invalide.")]
        public string NewEmail { get; set; } = null!;

        [Required(ErrorMessage = "La confirmation de l'email est obligatoire.")]
        [EmailAddress]
        [Compare("NewEmail", ErrorMessage = "L'adresse email et sa confirmation ne correspondent pas.")]
        public string ConfirmEmail { get; set; } = null!;
    }
}