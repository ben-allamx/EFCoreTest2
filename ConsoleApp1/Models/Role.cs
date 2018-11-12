using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	/// <summary>
	/// Encapsulates the data stored in a record in the Role table of the database.
	/// </summary>
	public class Role
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Display(Name = "Role ID")]
		public Int32 ID { get; set; }

		[DefaultValue(null)]
		[StringLength(100)]  //AD Spec (see User table)
		[Display(Name = "Role Name")]
		public String Name { get; set; }
	}
}