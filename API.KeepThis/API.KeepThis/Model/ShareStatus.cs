using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.KeepThis.Model;

[Table("share_status")]
public partial class ShareStatus
{
    [Key]
    [Column("title_status")]
    [StringLength(50)]
    public string TitleStatus { get; set; } = null!;

    [InverseProperty("TitleStatusNavigation")]
    public virtual ICollection<Friendship> Friendships { get; set; } = new List<Friendship>();

    [InverseProperty("TitleStatusNavigation")]
    public virtual ICollection<NoteSharing> NoteSharings { get; set; } = new List<NoteSharing>();
}
