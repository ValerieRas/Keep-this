using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.KeepThis.Model;

[Table("notebooks")]
public partial class Notebook
{
    [Key]
    [Column("id_notebook")]
    public int IdNotebook { get; set; }

    [Column("title_notebook")]
    [StringLength(50)]
    public string? TitleNotebook { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("id_user")]
    [StringLength(36)]
    public string? IdUser { get; set; }

    [ForeignKey("IdUser")]
    [InverseProperty("Notebooks")]
    public virtual User? IdUserNavigation { get; set; }

    [ForeignKey("IdNotebook")]
    [InverseProperty("IdNotebooks")]
    public virtual ICollection<Note> IdNotes { get; set; } = new List<Note>();
}
