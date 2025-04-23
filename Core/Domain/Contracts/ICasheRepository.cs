using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ICasheRepository
    {
        Task SetAsync ( string key , object value , TimeSpan duration );
        Task<string?> GetAsynk(string key );

    }
}
