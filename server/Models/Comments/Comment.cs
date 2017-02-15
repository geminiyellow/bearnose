using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using MicroSB.Server.Models.Shops;
using MicroSB.Server.Models.Users;

namespace MicroSB.Server.Models.Comments
{
	public class Comment
	{
		#region Constructor

		public Comment()
		{
		}

		#endregion Constructor

		#region Properties
		[Key]
		[Required]
		public int Id { get; set; }

		[Required]
		public int ShopId { get; set; }

		[Required]
		public string Text { get; set; }

		[Required]
		public int Type { get; set; }

		[Required]
		public int Flags { get; set; }

		[Required]
		public string UserId { get; set; }

		public int? ParentId { get; set; }

		[Required]
		public DateTime CreatedDate { get; set; }

		[Required]
		public DateTime LastModifiedDate { get; set; }

		#endregion Properties

		#region Related Properties

		/// <summary>
		/// Current Comment's Item: this property will be loaded on first use using EF's Lazy-Loading feature.
		/// </summary>
		[ForeignKey("ShopId")]
		public virtual Shop Shop { get; set; }

		/// <summary>
		/// Current Comment's Author: this property will be loaded on first use using EF's Lazy-Loading feature.
		/// </summary>
		[ForeignKey("UserId")]
		public virtual ApplicationUser Author { get; set; }

		/// <summary>
		/// Parent comment, or NULL if this is a root comment: this property will be loaded on first use using EF's Lazy-Loading feature.
		/// </summary>
		[ForeignKey("ParentId")]
		public virtual Comment Parent { get; set; }

		/// <summary>
		/// Children comments (if present).
		/// </summary>
		public virtual List<Comment> Children { get; set; }

		#endregion Related Properties
	}
}
