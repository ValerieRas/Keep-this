using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.KeepThis.Model;

[Table("friendships")]
public partial class Friendship
{
    [Key]
    [Column("id_friendship")]
    public int IdFriendship { get; set; }

    [Column("id_acceptor")]
    [StringLength(36)]
    public string? IdAcceptor { get; set; }

    [Column("id_requestor")]
    [StringLength(36)]
    public string? IdRequestor { get; set; }

    [Column("title_status")]
    [StringLength(50)]
    public string? TitleStatus { get; set; }

    [ForeignKey("IdAcceptor")]
    [InverseProperty("FriendshipIdAcceptorNavigations")]
    public virtual User? IdAcceptorNavigation { get; set; }

    [ForeignKey("IdRequestor")]
    [InverseProperty("FriendshipIdRequestorNavigations")]
    public virtual User? IdRequestorNavigation { get; set; }

    [ForeignKey("TitleStatus")]
    [InverseProperty("Friendships")]
    public virtual ShareStatus? TitleStatusNavigation { get; set; }
}
