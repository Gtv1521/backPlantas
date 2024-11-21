using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PlantasBackend.Controllers;

namespace PlantasBackend.Dto
{
    public class ImageDto : PlantsDto
    {
        [Required]
        public IFormFile? Image { get; set; }
    }
}