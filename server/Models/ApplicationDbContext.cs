using MicroSB.Server.Models.Comments;
using MicroSB.Server.Models.Shops;
using MicroSB.Server.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MicroSB.Server.Models
{
	public class ApplicationDbContext: DbContext
	{
		#region Constructor

		public ApplicationDbContext(DbContextOptions options) : base(options) 
		{
		}

		#endregion Constructor

		#region Methods

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<ApplicationUser>().ToTable("Users");
			builder.Entity<ApplicationUser>().HasMany(u => u.Shops).WithOne(i => i.Author);
			builder.Entity<ApplicationUser>().HasMany(u => u.Comments).WithOne(c => c.Author).HasPrincipalKey(u => u.Id);

			builder.Entity<Shop>().ToTable("Shops");
			builder.Entity<Shop>().Property(i => i.Id).ValueGeneratedOnAdd();
			builder.Entity<Shop>().HasOne(i => i.Author).WithMany(u => u.Shops);
			builder.Entity<Shop>().HasMany(i => i.Comments).WithOne(c => c.Shop);

			builder.Entity<Comment>().ToTable("Comments");
			builder.Entity<Comment>().HasOne(c => c.Author).WithMany(u => u.Comments).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);
			builder.Entity<Comment>().HasOne(c => c.Shop).WithMany(i => i.Comments);
			builder.Entity<Comment>().HasOne(c => c.Parent).WithMany(c => c.Children);
			builder.Entity<Comment>().HasMany(c => c.Children).WithOne(c => c.Parent);
		}

		#endregion Methods

		 #region Properties

		 public DbSet<Shop> Shops { get; set; }
		 public DbSet<Comment> Comments { get; set; }
		 public DbSet<ApplicationUser> Users { get; set; }

		 #endregion Properties

	}
}
