﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MicroSB.Server.Models.Shops;
using MicroSB.Server.Models.Comments;

namespace MicroSB.Server.Models.Users
{
	public class ApplicationUser
	{
		#region Constructor

		public ApplicationUser()
		{
		}

		#endregion Constructor

		#region Properties

		[Key]
		[Required]
		public string Id { get; set; }

		[Required]
		[MaxLength(128)]
		public string UserName { get; set; }

		[Required]
		public string Email { get; set; }

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
