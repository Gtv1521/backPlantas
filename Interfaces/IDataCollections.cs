using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantasBackend.Interfaces
{
    public interface IDataCollections<T>
    {
        Task<bool> InsertOneAsync(T model);
        Task<bool> UpdateOneAsync(T model);
        Task<bool> DeleteOneAsync(string id);
        Task<T> GetOneAsync(string id);
        Task<List<T>> GetAllAsync();
    }
}