using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantasBackend.Interfaces
{
    public interface IOneData<T>
    {
        Task<T> GetOneData(string name);        
    }
}