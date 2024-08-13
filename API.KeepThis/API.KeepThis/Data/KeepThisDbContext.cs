using API.KeepThis.Model;
using Microsoft.EntityFrameworkCore;

namespace API.KeepThis.Data;

public partial class KeepThisDbContext : DbContext, IKeepThisDbContext
{
    public KeepThisDbContext()
    {
    }

    public KeepThisDbContext(DbContextOptions<DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrator> Administrators { get; set; }

    public virtual DbSet<AuthentificationToken> AuthentificationTokens { get; set; }

    public virtual DbSet<Friendship> Friendships { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<NoteSharing> NoteSharings { get; set; }

    public virtual DbSet<NoteVersion> NoteVersions { get; set; }

    public virtual DbSet<Notebook> Notebooks { get; set; }

    public virtual DbSet<ShareStatus> ShareStatuses { get; set; }

    public virtual DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasKey(e => e.IdAdmin).HasName("administrators_pkey");
        });

        modelBuilder.Entity<AuthentificationToken>(entity =>
        {
            entity.HasKey(e => e.Token).HasName("authentification_tokens_pkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.AuthentificationTokens).HasConstraintName("authentification_tokens_id_user_fkey");
        });

        modelBuilder.Entity<Friendship>(entity =>
        {
            entity.HasKey(e => e.IdFriendship).HasName("friendships_pkey");

            entity.HasOne(d => d.IdAcceptorNavigation).WithMany(p => p.FriendshipIdAcceptorNavigations).HasConstraintName("friendships_id_acceptor_fkey");

            entity.HasOne(d => d.IdRequestorNavigation).WithMany(p => p.FriendshipIdRequestorNavigations).HasConstraintName("friendships_id_requestor_fkey");

            entity.HasOne(d => d.TitleStatusNavigation).WithMany(p => p.Friendships).HasConstraintName("friendships_title_status_fkey");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.IdNote).HasName("notes_pkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Notes).HasConstraintName("notes_id_user_fkey");

            entity.HasMany(d => d.IdNotebooks).WithMany(p => p.IdNotes)
                .UsingEntity<Dictionary<string, object>>(
                    "NotebookContain",
                    r => r.HasOne<Notebook>().WithMany()
                        .HasForeignKey("IdNotebook")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("notebook_contains_id_notebook_fkey"),
                    l => l.HasOne<Note>().WithMany()
                        .HasForeignKey("IdNote")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("notebook_contains_id_note_fkey"),
                    j =>
                    {
                        j.HasKey("IdNote", "IdNotebook").HasName("notebook_contains_pkey");
                        j.ToTable("notebook_contains");
                        j.IndexerProperty<int>("IdNote").HasColumnName("id_note");
                        j.IndexerProperty<int>("IdNotebook").HasColumnName("id_notebook");
                    });
        });

        modelBuilder.Entity<NoteSharing>(entity =>
        {
            entity.HasKey(e => e.IdPooling).HasName("note_sharings_pkey");

            entity.HasOne(d => d.IdNoteNavigation).WithMany(p => p.NoteSharings).HasConstraintName("note_sharings_id_note_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.NoteSharings).HasConstraintName("note_sharings_id_user_fkey");

            entity.HasOne(d => d.TitleStatusNavigation).WithMany(p => p.NoteSharings).HasConstraintName("note_sharings_title_status_fkey");
        });

        modelBuilder.Entity<NoteVersion>(entity =>
        {
            entity.HasKey(e => e.IdNoteVersions).HasName("note_versions_pkey");

            entity.HasOne(d => d.IdNoteNavigation).WithMany(p => p.NoteVersions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("note_versions_id_note_fkey");
        });

        modelBuilder.Entity<Notebook>(entity =>
        {
            entity.HasKey(e => e.IdNotebook).HasName("notebooks_pkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Notebooks).HasConstraintName("notebooks_id_user_fkey");
        });

        modelBuilder.Entity<ShareStatus>(entity =>
        {
            entity.HasKey(e => e.TitleStatus).HasName("share_status_pkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("users_pkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
