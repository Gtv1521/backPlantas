using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PlantasBackend.Dto
{
    public class AllPlantDto : PlantsDto
    {
        public string? Imagen { get; set; }

        public string? IdImagen { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? PlantFamilyId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string>? DiseaseIds
        {
            get; set;
        }
    }
}