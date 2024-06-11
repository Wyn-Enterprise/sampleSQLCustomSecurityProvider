using GrapeCity.Enterprise.Identity.ExternalIdentityProvider.Configuration;
using GrapeCity.Enterprise.Identity.SecurityProvider;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlDBSecurityProvider
{
	public class SqlDBSecurityProviderFactory : ISecurityProviderFactory
	{
		public string ProviderName => Consts.ProviderName;

		public string Description => Consts.ProviderDescription;

		public IEnumerable<ConfigurationItem> SupportedSettings => new List<ConfigurationItem>
		{
			new ConfigurationItem(Consts.ConfigurationItemDBConnectionString, "ConnectionString", "SQLServer database connection string")
			{
				Restriction = ConfigurationItemRestriction.Mandatory,
				ValueType = ConfigurationItemValueType.Text,
				Value = "Data Source=localhost;Initial Catalog=sample_db;User ID=sa;Password=password;Encrypt=False;" //Please change the Encrypt value to True if you are using encrypted connection
			}
		};

		public Task<ISecurityProvider> CreateAsync(IEnumerable<ConfigurationItem> settings, ILogger logger)
		{
			Logger.SetLogger(logger);
			return Task.Run(() =>
			{
				try
				{
					var securityProvider = new SqlDBSecurityProvider(settings);
					return securityProvider as ISecurityProvider;
				}
				catch (Exception e)
				{
					Logger.Exception(e, $"Create security provider '{Consts.ProviderName}' failed.");
					return null;
				}
			});
		}
	}
}
