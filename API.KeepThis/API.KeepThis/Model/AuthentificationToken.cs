using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.KeepThis.Model;

[Table("authentification_tokens")]
public partial class AuthentificationToken
{
    [Key]
    [Column("token")]
    public string Token { get; set; } = null!;

    [Column("timestamp_token", TypeName = "timestamp without time zone")]
    public DateTime TimestampToken { get; set; }

    [Column("id_user")]
    [StringLength(36)]
    public string? IdUser { get; set; }

    [ForeignKey("IdUser")]
    [InverseProperty("AuthentificationTokens")]
    public virtual User? IdUserNavigation { get; set; }
}
