using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PlantasBackend.Dto.Familys
{
    public class FamilyIdDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string? Id { get; set; }
    }

    public class FamilyNameDto : FamilyIdDto
    {
        [Required]
        public string? Name { get; set; }
    }
}