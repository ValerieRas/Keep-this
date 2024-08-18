using System.ComponentModel.DataAnnotations;

namespace API.KeepThis.Model.DTO
{
    public class UpdateUsernameDTO
    {
        [Required(ErrorMessage = "L'identifiant utilisateur est obligatoire.")]
        public string UserId { get; set; } = null!;

        [Required(ErrorMessage = "Le nouveau nom d'utilisateur est obligatoire.")]
        [StringLength(50, ErrorMessage = "Le nom d'utilisateur ne peut pas dépasser 50 caractères.")]
        public string NewUsername { get; set; } = null!;
    }
}