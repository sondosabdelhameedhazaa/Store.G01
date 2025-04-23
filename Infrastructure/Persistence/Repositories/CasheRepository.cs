using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using StackExchange.Redis;

namespace Persistence.Repositories
{
    public class CasheRepository (IConnectionMultiplexer connection) : ICasheRepository
    {
         private readonly IDatabase _database = connection.GetDatabase();
        public async Task<string?> GetAsynk(string key)
        {
            var value = await _database.StringGetAsync(key);
            return !value.IsNullOrEmpty ? value : default;
        }

        public async Task SetAsync(string key, object value, TimeSpan duration)
        {
            var redisValue = JsonSerializer.Serialize(value);
             await _database.StringSetAsync(key, redisValue , duration);

        }
    }
}
