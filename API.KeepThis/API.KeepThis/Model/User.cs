﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.KeepThis.Model;

[Table("users")]
public partial class User
{
    [Key]
    [Column("id_user")]
    [StringLength(36)]
    public string IdUser { get; set; } = null!;

    [Column("nom_user")]
    [StringLength(50)]
    public string NomUser { get; set; } = null!;

    [Column("certified_email_user")]
    [StringLength(100)]
    public string? CertifiedEmailUser { get; set; }

    [Column("temp_email_user")]
    [StringLength(100)]
    public string TempEmailUser { get; set; } = null!;

    [Column("password_user")]
    [StringLength(100)]
    public string PasswordUser { get; set; } = null!;

    [Column("salt_user")]
    [StringLength(100)]
    public string SaltUser { get; set; } = null!;

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("failed_login_attemps")]
    public int? FailedLoginAttemps { get; set; }

    [Column("locked_out_end", TypeName = "timestamp without time zone")]
    public DateTime? LockedOutEnd { get; set; }

    [JsonIgnore]
    [InverseProperty("IdUserNavigation")]
    public virtual ICollection<AuthentificationToken> AuthentificationTokens { get; set; } = new List<AuthentificationToken>();

    [JsonIgnore]
    [InverseProperty("IdAcceptorNavigation")]
    public virtual ICollection<Friendship> FriendshipIdAcceptorNavigations { get; set; } = new List<Friendship>();

    [JsonIgnore]
    [InverseProperty("IdRequestorNavigation")]
    public virtual ICollection<Friendship> FriendshipIdRequestorNavigations { get; set; } = new List<Friendship>();

    [JsonIgnore]
    [InverseProperty("IdUserNavigation")]
    public virtual ICollection<NoteSharing> NoteSharings { get; set; } = new List<NoteSharing>();

    [JsonIgnore]
    [InverseProperty("IdUserNavigation")]
    public virtual ICollection<Notebook> Notebooks { get; set; } = new List<Notebook>();

    [JsonIgnore]
    [InverseProperty("IdUserNavigation")]
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}
