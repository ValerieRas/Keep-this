using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.KeepThis.Model;

[Table("notes")]
public partial class Note
{
    [Key]
    [Column("id_note")]
    public int IdNote { get; set; }

    [Column("create_at", TypeName = "timestamp without time zone")]
    public DateTime CreateAt { get; set; }

    [Column("content_content")]
    public string ContentContent { get; set; } = null!;

    [Column("update_at", TypeName = "timestamp without time zone")]
    public DateTime UpdateAt { get; set; }

    [Column("archived_at", TypeName = "timestamp without time zone")]
    public DateTime ArchivedAt { get; set; }

    [Column("status_note")]
    [StringLength(50)]
    public string? StatusNote { get; set; }

    [Column("id_user")]
    [StringLength(36)]
    public string? IdUser { get; set; }

    [ForeignKey("IdUser")]
    [InverseProperty("Notes")]
    public virtual User? IdUserNavigation { get; set; }

    [InverseProperty("IdNoteNavigation")]
    public virtual ICollection<NoteSharing> NoteSharings { get; set; } = new List<NoteSharing>();

    [InverseProperty("IdNoteNavigation")]
    public virtual ICollection<NoteVersion> NoteVersions { get; set; } = new List<NoteVersion>();

    [ForeignKey("IdNote")]
    [InverseProperty("IdNotes")]
    public virtual ICollection<Notebook> IdNotebooks { get; set; } = new List<Notebook>();
}
