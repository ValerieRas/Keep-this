using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.KeepThis.Model;

[Table("note_versions")]
public partial class NoteVersion
{
    [Key]
    [Column("id_note_versions")]
    public int IdNoteVersions { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("note_content")]
    public string NoteContent { get; set; } = null!;

    [Column("id_note")]
    public int IdNote { get; set; }

    [ForeignKey("IdNote")]
    [InverseProperty("NoteVersions")]
    public virtual Note IdNoteNavigation { get; set; } = null!;
}
