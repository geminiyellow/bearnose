using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MicroSB.Server.Models.Comments;
using MicroSB.Server.Models.Shops;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MicroSB.Server.Models.Users
{
	public class ApplicationUser: IdentityUser
	{
		#region Properties

		public string DisplayName { get; set; }

		public string Notes { get; set; }

		[Required]
		public int Type { get; set; }

		[Required]
		public int Flags { get; set; }

		[Required]
		public DateTime CreatedDate { get; set; }

		[Required]
		public DateTime LastModifiedDate { get; set; }

		#endregion Properties

		#region Related Properties

		/// <summary>
		/// A list of items wrote by this user: this property will be loaded on first use using EF's Lazy-Loading feature.
		/// </summary>
		public virtual List<Shop> Shops { get; set; }

		/// <summary>
		/// A list of comments wrote by this user: this property will be loaded on first use using EF's Lazy-Loading feature.
		/// </summary>
		public virtual List<Comment> Comments { get; set; }

		#endregion Related Properties
	}
}
