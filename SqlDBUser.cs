using GrapeCity.Enterprise.Identity.ExternalIdentityProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SqlDBSecurityProvider
{
	public class SqlDBUser : IExternalUserContext, IExternalUserDescriptor
	{
		private readonly string _id;
		private readonly string _name;
		private readonly IDictionary<string, string> _context;
		private IEnumerable<string> _roles;
		private IEnumerable<string> _organizations;

		protected SqlDBUser() { }
		public SqlDBUser(string id, string name, IDictionary<string, string> context)
		{
			if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException();
			}
			_id = id;
			_name = name;
			_context = context;
		}

		public void SetRoles(IEnumerable<string> roles)
		{
			_roles = roles;
		}
		public void SetOrganizations(IEnumerable<string> organizations)
		{
			_organizations = organizations;
		}

		public string ExternalUserId => _id;

		public string ExternalUserName => _name;

		public string ExternalProvider => Consts.ProviderName;

		public IEnumerable<string> Roles => null == _roles ? Enumerable.Empty<string>() : _roles;

		public IEnumerable<string> Organizations => null == _organizations ? Enumerable.Empty<string>() : _organizations;

		public IEnumerable<string> Keys => null == _context ? Enumerable.Empty<string>() : _context.Keys;

		public Task<string> GetValueAsync(string key)
		{
			return Task.Run(() => (null != _context && _context.ContainsKey(key)) ? _context[key] : string.Empty);
		}

		public Task<IEnumerable<string>> GetValuesAsync(string key)
		{
			return Task.Run(() => (null != _context && _context.ContainsKey(key)) ? new[] { _context[key] } : Enumerable.Empty<string>());
		}
	}
}
