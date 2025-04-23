using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Services.Abstractions;

namespace Services
{
    public class CasheService (ICasheRepository casheRepository) : ICasheService
    {
        public async Task<string?> GetCasheValueAsynk(string key)
        {
            var value = await casheRepository.GetAsynk(key);
            return value == null ? null : value;
        }

        public async Task SetCasheValueAsynk(string key, object value, TimeSpan duration)
        {
            await casheRepository.SetAsync(key, value, duration);   
        }
    }
}
