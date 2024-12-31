using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantasBackend.Interfaces
{
    public interface ISearch<T>
    {
        Task<T> SearchAsync(string search);
    }
}