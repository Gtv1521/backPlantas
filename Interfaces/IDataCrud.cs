using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantasBackend.Interfaces
{
    public interface IDataCrud<T>
    {
        Task<T> GetById(string id); 
        Task<List<T>> GetAllData(); 
        Task DeleteById(string id);
        Task UpdateData(T model);
        Task InsertData(T model);

    }
}