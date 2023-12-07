using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Milky.Models
{
	public class Product
	{
		[Key] //primary key
		public int id { get; set; } //get-read, set -write

		[Required] // The name variables after this line of code shouldnt be null

		[MaxLength(30)] //validations

		[DisplayName("Product Name")]
		public string ProductName { get; set; }

		public string Description { get; set; }


		[Range (1,100)]
		public double Price { get; set; }


		[DisplayName("Milk Fat Percentage")]
		public string MilkFat { get; set; }

		public int CategoryID { get; set; }
		[ForeignKey("CategoryID")]
		public Category Category { get; set; }

	}
}
