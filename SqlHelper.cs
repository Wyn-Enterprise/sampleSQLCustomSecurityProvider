using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace SqlDBSecurityProvider
{
	public static class SqlHelper
	{
		public static async Task<SqlDBUser> GetUserInfoAsync(DbConnection dbConnection, string username, string password)
		{
			var user = await GetUserAsync(dbConnection, username, password);
			if (null != user)
			{
				var roles = await GetUserRolesAsync(dbConnection, user.ExternalUserId);
				user.SetRoles(roles);
				var organizations = await GetUserOrganizationsAsync(dbConnection, user.ExternalUserId);
				user.SetOrganizations(organizations);
			}
			return user;
		}

		private static async Task<SqlDBUser> GetUserAsync(DbConnection dbConnection, string username, string password)
		{
			using (var cmd = dbConnection.CreateCommand())
			{
				cmd.CommandText = $"SELECT [password], [id], [name], [email], [height], [weight], [birthday] FROM [users] WHERE [name] = '{username}'";
				using (var reader = await cmd.ExecuteReaderAsync())
				{
					if (!reader.HasRows)
					{
						Logger.Information($"User '{username}' does not exist.");
						return null;
					}
					else
					{
						await reader.ReadAsync();
						if (Convert.ToBase64String(Encoding.UTF8.GetBytes(password)) == reader.GetString(0))
						{
							var id = reader.GetString(1);
							var name = reader.GetString(2);
							var context = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
							for (var i = 3; i < reader.FieldCount; i++)
							{
								var fieldName = reader.GetName(i);
								if (reader.IsDBNull(i))
								{
									context.Add(fieldName, string.Empty);
								}
								else
								{
									context.Add(fieldName, reader.GetValue(i).ToString());
								}
							}
							return new SqlDBUser(id, name, context);
						}
						else
						{
							Logger.Information($"Invalid password for user '{username}'.");
							return null;
						}
					}
				}
			}
		}
		private static async Task<IEnumerable<string>> GetUserRolesAsync(DbConnection dbConnection, string userId)
		{
			var roles = new List<string>();
			using (var cmd = dbConnection.CreateCommand())
			{
				cmd.CommandText = $"SELECT [role] FROM [user_roles] WHERE [user_id] = '{userId}'";
				using (var reader = await cmd.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						if (!reader.IsDBNull(0))
						{
							roles.Add(reader.GetString(0));
						}
					}
				}
			}
			return roles;
		}
		private static async Task<IEnumerable<string>> GetUserOrganizationsAsync(DbConnection dbConnection, string userId)
		{
			var orgs = new List<string>();
			using (var cmd = dbConnection.CreateCommand())
			{
				cmd.CommandText = $"SELECT [organization] FROM [user_orgs] WHERE [user_id] = '{userId}'";
				using (var reader = await cmd.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						if (!reader.IsDBNull(0))
						{
							orgs.Add(reader.GetString(0));
						}
					}
				}
			}
			return orgs;
		}
	}
}
