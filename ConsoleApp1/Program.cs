using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	class Program
	{
		static void Main(string[] args)
		{
			List<Role> newRoles = new List<Role>();

			for (Int32 i = 1; i < 5; i++)
			{
				newRoles.Add(new Role { Name = $"New Role {i}" });
			}

			using (var ctx = new MyContext())
			{
				ctx.Roles.AddRange(newRoles);
			}
		}
	}
}
