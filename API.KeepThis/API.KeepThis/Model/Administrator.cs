using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.KeepThis.Model;

[Table("administrators")]
public partial class Administrator
{
    [Key]
    [Column("id_admin")]
    [StringLength(36)]
    public string IdAdmin { get; set; } = null!;

    [Column("name_admin")]
    [StringLength(50)]
    public string NameAdmin { get; set; } = null!;

    [Column("email_admin")]
    [StringLength(100)]
    public string EmailAdmin { get; set; } = null!;

    [Column("password_admin")]
    [StringLength(100)]
    public string PasswordAdmin { get; set; } = null!;

    [Column("salt_admin")]
    [StringLength(100)]
    public string SaltAdmin { get; set; } = null!;
}
