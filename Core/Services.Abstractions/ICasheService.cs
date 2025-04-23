using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface ICasheService
    {
         Task SetCasheValueAsynk ( string key , object value , TimeSpan duration );
        Task<string?> GetCasheValueAsynk(string key);

    }
}
