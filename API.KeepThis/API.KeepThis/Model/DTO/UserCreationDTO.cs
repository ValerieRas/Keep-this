using System.ComponentModel.DataAnnotations;

namespace API.KeepThis.Model.DTO
{
    public class UserCreationDTO
    {
        [Required(ErrorMessage = "L'adresse e-mail est obligatoire.")]
        [EmailAddress(ErrorMessage = "L'adresse e-mail n'est pas valide.")]
        [StringLength(100, ErrorMessage = "L'adresse e-mail ne peut pas dépasser 100 caractères.")]
        public string TempEmailUser { get; set; } = null!;

        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Le mot de passe doit comporter entre 8 et 100 caractères.")]
        public string PasswordUser { get; set; } = null!;

        [Required(ErrorMessage = "Le nom d'utilisateur est obligatoire.")]
        [StringLength(50, ErrorMessage = "Le nom d'utilisateur ne peut pas dépasser 50 caractères.")]
        public string NomUser { get; set; } = null!;
    }
}
