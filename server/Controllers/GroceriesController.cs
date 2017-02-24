using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MicroSB.Server.Models;
using MicroSB.Server.Models.Shops;
using MicroSB.Server.ViewModels.Shops;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MicroSB.Server.Controllers
{
	[Route("api/[controller]")]
	public class GroceriesController: Controller
	{
		#region Private Fields

		private ApplicationDbContext _dbContext;

		#endregion Private Fields

		#region Constructor

		public GroceriesController(ApplicationDbContext context)
		{
			_dbContext = context;
		}

		#endregion Constructor

		#region Attribute-based Routing

		// GET api/groceries/latest/5
		[HttpGet("latest/{num}")]
		public JsonResult GetLatest(int num)
		{
			var items = _dbContext.Shops.OrderByDescending(i => i.CreatedDate).Take(num).ToArray();
			return new JsonResult(Mapper.Map<List<ShopViewModel>>(items), DefaultJsonSettings);
		}

		// GET api/groceries/most/5
		[HttpGet("most/{num}")]
		public JsonResult GetMostViewed(int num)
		{
			var items = _dbContext.Shops.OrderByDescending(i => i.ViewCount).Take(num).ToArray();
			return new JsonResult(Mapper.Map<List<ShopViewModel>>(items), DefaultJsonSettings);
		}

		// GET api/groceries/random/5
		[HttpGet("random/{num}")]
		public JsonResult GetRandom(int num)
		{
			var items = _dbContext.Shops.OrderBy(i => Guid.NewGuid()).Take(num).ToArray();
			return new JsonResult(Mapper.Map<List<ShopViewModel>>(items), DefaultJsonSettings);
		}

		// GET api/groceries
		[HttpGet]
		public IActionResult Get()
		{
			var sources = _dbContext.Shops;
			if (sources == null) return NotFound(new { Error = "not found" });

			return new JsonResult(Mapper.Map<List<ShopViewModel>>(sources), DefaultJsonSettings);
		}

		// GET api/groceries/5
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			var shop = _dbContext.Shops.Where(i => i.Id == id).FirstOrDefault();
			if (shop == null) return NotFound(new { Error = string.Format("Shop {0} has not been found", id) });
			return new JsonResult(Mapper.Map<ShopViewModel>(shop), DefaultJsonSettings);
		}

		// POST api/groceries
		[HttpPost]
		public IActionResult Post([FromBody]ShopViewModel value)
		{
			// return a generic HTTP Status 500 (Not Found) if the client payload is invalid.
			if (value == null) return new StatusCodeResult(500);

			var shop = Mapper.Map<Shop>(value);
			shop.CreatedDate = shop.LastModifiedDate = DateTime.Now;
			// TODO: replace the following with the current user's id when authentication will be available.
			shop.UserId = _dbContext.Users.Where(u => u.UserName == "Admin").FirstOrDefault().Id;
			// add the new shop
			_dbContext.Shops.Add(shop);
			// persist the changes into the Database.
			_dbContext.SaveChanges();
			// return the newly-created shop to the client.
			return new JsonResult(Mapper.Map<ShopViewModel>(shop), DefaultJsonSettings);
		}

		// PUT api/groceries/5
		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody]ShopViewModel value)
		{
			// return a generic HTTP Status 500 (Not Found) if the client payload is invalid.
			if (value == null) return new StatusCodeResult(500);

			var shop = _dbContext.Shops.Where(i => i.Id == id).FirstOrDefault();
			// return a HTTP Status 404 (Not Found) if we couldn't find a suitable shop.
			if (value == null) return NotFound(new { Error = String.Format("shop ID {0} has not been found", id) });

			// handle the update (on per-property basis)
			//shop.UserId = value.UserId;
			//shop.Description = value.Description;
			//shop.Flags = value.Flags;
			//shop.Notes = value.Notes;
			//shop.Text = value.Text;
			shop.Title = value.Title;
			//shop.Type = value.Type;

			// override any property that could be wise to set from server - side only
			shop.LastModifiedDate = DateTime.Now;

			// persist the changes into the Database.
			_dbContext.SaveChanges();

			// return the updated shop to the client.
			return new JsonResult(Mapper.Map<ShopViewModel>(shop), DefaultJsonSettings);
		}

		// DELETE api/groceries/5
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			var item = _dbContext.Shops.Where(i => i.Id == id).FirstOrDefault();

			// return a HTTP Status 404 (Not Found) if we couldn't find a suitable item.
			if (item == null) return NotFound(new { Error = String.Format("Item ID {0} has not been found", id) });

			// remove the item to delete from the DbContext.
			_dbContext.Shops.Remove(item);
			// persist the changes into the Database.
			_dbContext.SaveChanges();
			// return an HTTP Status 200 (OK).
			return new OkResult();
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Returns a suitable JsonSerializerSettings object that can be used to generate the JsonResult return value for this Controller's  methods.
		/// </summary>
		private JsonSerializerSettings DefaultJsonSettings
		{
			get
			{
				return new JsonSerializerSettings()
				{
					Formatting = Formatting.Indented
				};
			}
		}
		#endregion
	}
}
