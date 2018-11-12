using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	public class MyContext : DbContext
	{
		public DbSet<Role> Roles { get; set; }

		public MyContext()
			: base(GetOptions())
		{

		}

		private static DbContextOptions<MyContext> GetOptions()
		{
			var ds = Environment.MachineName;
			var dbName = "EFTest2";
			var connString = $"Data Source={ds};Initial Catalog={dbName};Integrated Security=True;Persist Security Info=False;Pooling=True;Min Pool Size=0;Max Pool Size=200;Connect Timeout=10;Load Balance Timeout=0;Workstation ID={ds};User Instance=False";
			DbContextOptionsBuilder<MyContext> optionsBuilder = new DbContextOptionsBuilder<MyContext>();
			optionsBuilder.UseSqlServer(connString);
			return optionsBuilder.Options;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			//Role
			modelBuilder.Entity<Role>().ToTable("Role");
			modelBuilder.Entity<Role>()
				.HasIndex(x => x.Name)
				.HasName("UX_Role_Name")
				.IsUnique(true);
			modelBuilder.Entity<Role>().Property(x => x.Name).IsUnicode();
		}
		
		public override Int32 SaveChanges()
		{
			//This override exists to handle providing IDs to records that need them.  We are NOT using any kind of auto-numbering
			//  in the database.  It's all done by hand, right here.  This determines the highest available ID and uses it, keeping
			//  a Dictionary along the way to minimize reads.  From there, it's in sequence.  This does NOT prevent gaps in the ID
			//  sequences, but it WILL reuse the IDs of previously deleted records, if they fell at the end of the previous sequence.
			var dict = new Dictionary<Type, Int32>();
			foreach (var change in ChangeTracker.Entries())
			{
				if (change.State == EntityState.Added)
				{
					PropertyEntry property = null;
					try { property = change.Property("ID"); }
					catch (ArgumentException) { }  //if the ID property doesn't exist, then there's nothing to do
					if (property != null)
					{
						var id = (Int32)change.Property("ID").CurrentValue;
						if (id == 0)
						{
							var type = change.Entity.GetType();
							if (dict.ContainsKey(type))
							{
								dict[type]++;
								id = dict[type];
							}
							else
							{
								id = GetNextID(type.Name);
								dict.Add(type, id);
							}
							if (id > 0)
								change.Property("ID").CurrentValue = id;
						}
					}
				}
			}

			var savedEntries = 0;
			try { savedEntries = base.SaveChanges(); }
			catch (DbUpdateException dbUpdateEx)
			{
				var sb = new StringBuilder();
				sb.AppendLine($"Error trying to save entities:");
				foreach (var entry in dbUpdateEx.Entries)
					sb.AppendLine($"{entry.Entity.GetType().Name} ({entry.State})");
				sb.AppendLine(dbUpdateEx.ToString());
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error: {ex}");
			}
			return savedEntries;
		}

		/// <summary>
		/// Returns the next ID for the table if possible
		/// </summary>
		/// <param name="tableName">The table name that you are getting the next ID for.</param>
		/// <returns>Int32 either the next ID to be used or a -1 if something went wrong.</returns>
		private Int32 GetNextID(String tableName)
		{
			Int32 nextID = -1;
			var sqlStr = $"SELECT COALESCE(MAX([ID]), 0)+1 FROM [{tableName}]";
			var conn = this.Database.GetDbConnection();
			try
			{
				conn.Open();
				var cmd = conn.CreateCommand();
				cmd.CommandText = sqlStr;
				var reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					nextID = reader.GetInt32(0);
				}
			}
			finally
			{
				conn.Close();
			}

			return nextID;
		}

	}
}
