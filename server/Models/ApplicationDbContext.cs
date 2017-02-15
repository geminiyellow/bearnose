using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MicroSB.Server.Models.Shops;
using MicroSB.Server.Models.Users;
using MicroSB.Server.Models.Comments;

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

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ApplicationUser>().ToTable("Users");
			modelBuilder.Entity<ApplicationUser>().HasMany(u => u.Shops).WithOne(i => i.Author);
			modelBuilder.Entity<ApplicationUser>().HasMany(u => u.Comments).WithOne(c => c.Author).HasPrincipalKey(u => u.Id);

			modelBuilder.Entity<Shop>().ToTable("Shops");
			modelBuilder.Entity<Shop>().Property(i => i.Id).ValueGeneratedOnAdd();
			modelBuilder.Entity<Shop>().HasOne(i => i.Author).WithMany(u => u.Shops);
			modelBuilder.Entity<Shop>().HasMany(i => i.Comments).WithOne(c => c.Shop);

			modelBuilder.Entity<Comment>().ToTable("Comments");
			modelBuilder.Entity<Comment>().HasOne(c => c.Author).WithMany(u => u.Comments).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);
			modelBuilder.Entity<Comment>().HasOne(c => c.Shop).WithMany(i => i.Comments);
			modelBuilder.Entity<Comment>().HasOne(c => c.Parent).WithMany(c => c.Children);
			modelBuilder.Entity<Comment>().HasMany(c => c.Children).WithOne(c => c.Parent);
		}

		#endregion Methods

		 #region Properties

		 public DbSet<Shop> Shops { get; set; }
		 public DbSet<Comment> Comments { get; set; }
		 public DbSet<ApplicationUser> Users { get; set; }

		 #endregion Properties

	}
}
