using GrapeCity.Enterprise.Identity.ExternalIdentityProvider;
using GrapeCity.Enterprise.Identity.ExternalIdentityProvider.Configuration;
using GrapeCity.Enterprise.Identity.SecurityProvider;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SqlDBSecurityProvider
{
	public class SqlDBSecurityProvider : ISecurityProvider
	{
		public string ProviderName => Consts.ProviderName;

		private readonly DbConnection _dbConn;

		protected SqlDBSecurityProvider() { }
		public SqlDBSecurityProvider(IEnumerable<ConfigurationItem> configs)
		{
			_dbConn = GetDbConnection(configs);
		}

		private DbConnection GetDbConnection(IEnumerable<ConfigurationItem> configs)
		{
			var settings = new ConfigurationCollection(configs);
			var connStr = settings.Text(Consts.ConfigurationItemDBConnectionString);
			if (string.IsNullOrEmpty(connStr))
			{
				throw new Exception($"Database connection string cannot be empty.");
			}
			var conn = new SqlConnection(connStr);
			try
			{
				conn.Open();
				return conn;
			}
			catch (Exception e)
			{
				Logger.Exception(e, $"Open database connection failed.");
				throw;
			}
		}

		public Task DisposeTokenAsync(string token)
		{
			return Task.Run(() => SqlDBUsers.Remove(token));
		}

		public async Task<string> GenerateTokenAsync(string username, string password, object customizedParam = null)
		{
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
			{
				return null;
			}
			var user = await SqlHelper.GetUserInfoAsync(_dbConn, username, password);
			if (null != user)
			{
				var token = Guid.NewGuid().ToString();
				if (SqlDBUsers.Add(token, user))
				{
					return token;
				}
				else
				{
					Logger.Error($"Store token failed.");
				}
			}
			return null;
		}

		public Task<IExternalUserContext> GetUserContextAsync(string token)
		{
			return Task.Run(() =>
			{
				var (user, expiration) = SqlDBUsers.Get(token);
				if (ValidUser(user, expiration))
				{
					return user as IExternalUserContext;
				}
				else
				{
					SqlDBUsers.Remove(token);
					return null;
				}
			});
		}

		public Task<IExternalUserDescriptor> GetUserDescriptorAsync(string token)
		{
			return Task.Run(() =>
			{
				var (user, expiration) = SqlDBUsers.Get(token);
				if (ValidUser(user, expiration))
				{
					return user as IExternalUserDescriptor;
				}
				else
				{
					SqlDBUsers.Remove(token);
					return null;
				}
			});
		}

		public Task<string[]> GetUserOrganizationsAsync(string token)
		{
			return Task.Run(() =>
			{
				var (user, expiration) = SqlDBUsers.Get(token);
				if (ValidUser(user, expiration))
				{
					return user.Organizations.ToArray();
				}
				else
				{
					return new string[0];
				}
			});
		}

		public Task<string[]> GetUserRolesAsync(string token)
		{
			return Task.Run(() =>
			{
				var (user, expiration) = SqlDBUsers.Get(token);
				if (ValidUser(user, expiration))
				{
					return user.Roles.ToArray();
				}
				else
				{
					return new string[0];
				}
			});
		}

		private bool ValidUser(SqlDBUser user, DateTime expiration)
		{
			return null != user && DateTime.UtcNow <= expiration;
		}

		public Task<bool> ValidateTokenAsync(string token)
		{
			return Task.Run(() =>
			{
				var (user, expiration) = SqlDBUsers.Get(token);
				return ValidUser(user, expiration);
			});
		}
	}
}
