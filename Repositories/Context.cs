using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PlantasBackend.Models.Responses;
using PlantasBackend.Models.settings;

namespace PlantasBackend.Repositories
{
    public class Context
    {
        private readonly IMongoDatabase _db;

        // Connect to the database
        public Context(IOptions<ContextModel> settings)
        {
            try
            {
                if (settings.Value.ConnectionStrings != null && settings.Value.DatabaseName != null)
                {
                    var client = new MongoClient(settings.Value.ConnectionStrings);
                    _db = client.GetDatabase(settings.Value.DatabaseName);
                }
            }
            catch (MongoException ex)
            {
                System.Console.WriteLine($"Error: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"No connection to Mongo {ex.Message}");
            }
        }

        // Get somethings collections 
        public IMongoCollection<T> GetCollection<T>(string collection)
        {
            return _db.GetCollection<T>(collection);
        }
    }
}