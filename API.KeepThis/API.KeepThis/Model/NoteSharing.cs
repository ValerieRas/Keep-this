using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.KeepThis.Model;

[Table("note_sharings")]
public partial class NoteSharing
{
    [Key]
    [Column("id_pooling")]
    public int IdPooling { get; set; }

    [Column("id_note")]
    public int? IdNote { get; set; }

    [Column("title_status")]
    [StringLength(50)]
    public string? TitleStatus { get; set; }

    [Column("id_user")]
    [StringLength(36)]
    public string? IdUser { get; set; }

    [ForeignKey("IdNote")]
    [InverseProperty("NoteSharings")]
    public virtual Note? IdNoteNavigation { get; set; }

    [ForeignKey("IdUser")]
    [InverseProperty("NoteSharings")]
    public virtual User? IdUserNavigation { get; set; }

    [ForeignKey("TitleStatus")]
    [InverseProperty("NoteSharings")]
    public virtual ShareStatus? TitleStatusNavigation { get; set; }
}
