using System;
using System.Collections.Concurrent;

namespace SqlDBSecurityProvider
{
	public static class SqlDBUsers
	{
		private static TimeSpan _expirePeriod = TimeSpan.FromHours(2);
		private static ConcurrentDictionary<string, (SqlDBUser user, DateTime expiration)> _users = new ConcurrentDictionary<string, (SqlDBUser, DateTime)>();

		public static (SqlDBUser user, DateTime expiration) Get(string key)
		{
			return _users.TryGetValue(key, out var value) ? value : (null, DateTime.UtcNow);
		}
		public static bool Add(string key, SqlDBUser user)
		{
			return _users.TryAdd(key, (user, DateTime.UtcNow.Add(_expirePeriod)));
		}
		public static bool Remove(string key)
		{
			return _users.TryRemove(key, out _);
		}
	}
}
