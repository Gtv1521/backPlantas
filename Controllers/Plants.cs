using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlantasBackend.Dto;
using PlantasBackend.Interfaces;
using PlantasBackend.Models;
using PlantasBackend.Models.Responses;
using PlantasBackend.Services;

namespace PlantasBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Plants : ControllerBase
    {
        private readonly ILogger<PlantsModel> _logger;
        private readonly PlantsService _service;
        public Plants(ILogger<PlantsModel> logger, PlantsService service)
        {
            _logger = logger;
            _service = service;
        }


        /// <summary>
        ///  Get an list of something plants.
        /// </summary>
        /// <remarks>
        /// <returns>true</returns>
        /// <response code="200">All plants registered.</response>
        /// <response code="404">Not found plants.</response>
        /// <response code="500">Server error</response>
        [HttpGet]
        [Route("view_all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PlantsModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation($"Get list of plants registered -- {DateTime.UtcNow}.");
                var result = await _service.GetAllAsync();
                if (result.Count == 0) return NotFound(new ResultData { StatusCode = 404, Message = "Not found data for plants" });
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError("Failed at get list of plants - {Time} - {@ex}", DateTime.UtcNow, ex.Message);
                return Problem(ex.Message, "/api/plants/view_all", 500, "Server error");
            }
        }

        /// <summary>
        ///  Get one plants for id.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     GET /api/plants/view_plant_id/{id}
        ///     {
        ///         "id" = "668d4a0d986faf64730e222b"
        ///     }
        /// </remarks>
        /// 
        /// <returns>true</returns>
        /// <response code="200">Something plants.</response>
        /// <response code="404">Not found data.</response>
        /// <response code="500">Server error</response>
        [HttpGet]
        [Route("view_plant/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> GetByPlant(string id)
        {
            try
            {
                _logger.LogInformation($"Get iinformation about plant whichid : {id} -- {DateTime.UtcNow}.");
                var Result = await _service.GetOneAsync(id);
                if (Result == null) return NotFound(new ResultData { StatusCode = 404, Message = "Not found data" });
                return Ok(Result);

            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Feiles at get data of plant which id: {id} -- {DateTime.UtcNow} -- {ex.Message}");
                return Problem(ex.Message, $"/api/plants/view_plant/{id}", 500, "Server error");
            }
        }

        /// <summary>
        /// Get a plant for the name of plant
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request: 
        /// 
        ///     GET /api/plant/view_plant_name
        ///     {
        ///         "Name": "Curuba"
        ///     }
        /// </remarks>
        /// <response code="200">Plant data</response>
        /// <response code="404">Not found data of plant</response>
        /// <response code="500">Server error</response>
        [HttpGet]
        [Route("view_plant_name/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlantsModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                _logger.LogInformation($"Get data of plant which name: {name}.");
                var Result = await _service.GetByNameAsync(name);
                if (Result == null) return NotFound(new ResultData { StatusCode = 404, Message = "Not found data of plant" });
                return Ok(Result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Failed in get data family -- {DateTime.UtcNow} -- {ex.Message}");
                return Problem(ex.Message, "/api/plants/view_plant_name", 500, "Server error");
            }
        }

        /// <summary>
        /// Insert one plant.
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     POST /api/plants/insert
        ///     {
        ///         "Name" : "Lulo",
        ///         "Description" : "planta con frutos espinosos",
        ///         "FamilyId" : "673424ba711637b1f4ee2418",
        ///         "DiseaseIds" : {
        ///             "673523dc5b21f11d8693fdc1",
        ///             "668d4a0d986faf64739e222b",
        ///             "668d4a0d986faf64770e222c"
        ///             },
        ///         "Image" : ""
        ///     }
        /// </remarks>
        /// <returns>true</returns>
        /// <response code="200">Something plants.</response>
        /// <response code="400">Not fount data</response>
        /// <response code="500">Server error</response>
        [HttpPost]
        [Route("insert")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Post([FromForm] AllPlantDto model)
        {
            try
            {
                _logger.LogInformation($"Insert new plant -- {DateTime.UtcNow}.");
                var result = await _service.InsertOneAsync(model);
                if (result) return Ok(new ResultData { StatusCode = 200, Message = "Insert plant successfully." });
                return BadRequest(new ResultData { StatusCode = 400, Message = "Failed insert data of plant" });
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Failed in insert data family -- {DateTime.UtcNow} -- {ex.Message}");
                return Problem(ex.Message, "/api/plants/insert", 500, "Server error");
            }
        }

        /// <summary>
        /// Update data of one plant.
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     POST /api/plants/update_plant
        ///     {
        ///         "Id": "668d4a0d986faf64730e222b"
        ///         "Name" : "Lulo",
        ///         "Description" : "planta con frutos espinosos",
        ///         "FamilyId" : "673424ba711637b1f4ee2418",
        ///         "DiseaseIds" : {
        ///             "673523dc5b21f11d8693fdc1",
        ///             "668d4a0d986faf64739e222b",
        ///             "668d4a0d986faf64770e222c"
        ///             },
        ///         "Image" : ""
        ///     }
        /// </remarks>
        /// <returns>true</returns>
        /// <response code="200">Updated.</response>
        /// <response code="404">Not fount data</response>
        /// <response code="500">Server error</response>
        [HttpPost]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateOne([FromForm] IdPlantsDto model)
        {
            try
            {
                _logger.LogInformation($"Update uno plant id: {model.Id} -- {DateTime.UtcNow}");
                PlantsModel modelo = new PlantsModel
                {
                    Name = model.Name,
                    Description = model.Description,
                };
                var result = await _service.UpdateOneAsync(modelo);
                if (result) return Ok(new ResultData { StatusCode = 200, Message = "Update successfully." });
                return BadRequest(new ResultData { StatusCode = 400, Message = "Failed update data for plant." });
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Failed in update data family -- {DateTime.UtcNow} -- {ex.Message}");
                return Problem(ex.Message, "/api/plants/update", 500, "Server error");
            }
        }

        /// <summary>
        /// Delete one plant from the service.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE /api/Plant/delete/{id}
        ///     {
        ///         "id": "668eab036a05f5dfcb196f32"
        ///     }
        /// </remarks>
        /// <response code="200">Deleted</response>
        /// <response code="400">Error internal server</response>
        /// <response code="500">Server error</response>
        [HttpDelete]
        [Route("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteOne(string id)
        {
            try
            {
                _logger.LogInformation($"Delete plant which id: {id} -- {DateTime.UtcNow} ");
                var result = await _service.DeleteOneAsync(id);
                if (result) return Ok(new ResultData { StatusCode = 200, Message = "Plant deleted successfully." });
                return NotFound(new ResultData { StatusCode = 404, Message = "Not found the plant." });
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Failed in delete data family -- {DateTime.UtcNow} -- {ex.Message}");
                return Problem(ex.Message, "/api/plants/delete", 500, "Server error");
            }
        }
    }
}