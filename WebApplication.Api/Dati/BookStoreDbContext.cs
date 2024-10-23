using Microsoft.EntityFrameworkCore;

namespace WebApplicationApi.Dati;

public partial class BookStoreDbContext : DbContext
{
	public BookStoreDbContext()
	{
	}

	public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
		: base(options)
	{
	}

	public virtual DbSet<Author> Authors { get; set; }

	public virtual DbSet<Book> Books { get; set; }

	//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
	//        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookStoreDB;Integrated Security=True;Pooling=False;Encrypt=True;Trust Server Certificate=False");

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Author>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Authors__3214EC0779795DC6");

			entity.Property(e => e.Bio).HasMaxLength(250);
			entity.Property(e => e.FirstName).HasMaxLength(50);
			entity.Property(e => e.LastName).HasMaxLength(50);
		});

		modelBuilder.Entity<Book>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Books__3214EC079095BBE3");

			entity.HasIndex(e => e.Isbn, "UQ__Books__447D36EABF53C677").IsUnique();

			entity.Property(e => e.Image).HasMaxLength(50);
			entity.Property(e => e.Isbn)
				.HasMaxLength(50)
				.HasColumnName("ISBN");
			entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
			entity.Property(e => e.Summary).HasMaxLength(250);
			entity.Property(e => e.Title).HasMaxLength(50);

			entity.HasOne(d => d.Author).WithMany(p => p.Books)
				.HasForeignKey(d => d.AuthorId)
				.HasConstraintName("FK_Books_ToTable");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
