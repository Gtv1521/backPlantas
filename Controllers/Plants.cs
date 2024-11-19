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
        /// Sample Request:
        /// 
        ///     GET /api/plants/viewall
        /// </remarks>
        /// <returns>true</returns>
        /// <response code="200">Todos los usuarios.</response>
        /// <response code="500">No se encuentran datos</response>
        [HttpGet]
        [Route("view_all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlantsModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultData))]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation("Obteniendo lista de plantas.");
                return Ok(await _service.GetAllAsync());
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Error al obtener lista de plantas: {ex.Message}");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        /// <summary>
        ///  Get one plants for id.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     GET /api/plants/view_plant_id
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
        [Route("view_plant_id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultData))]
        public async Task<IActionResult> GetByPlant(string id)
        {
            try
            {
                _logger.LogInformation("Obteniendo información de una planta.");
                var Result = await _service.GetOneAsync(id);
                if (Result == null)
                {
                    return NotFound(new ResultData
                    {
                        StatusCode = 404,
                        Message = "Not found data"
                    });
                }
                return Ok(Result);

            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Error al obtener información de la planta: {ex.Message}");
                return StatusCode(500, "Error interno del servidor.");
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
        ///         "name": "granadilla"
        ///     }
        /// </remarks>
        /// <response code="200">Plant data</response>
        /// <response code="400">Failed get plant</response>
        /// <response code="404">Not found plant</response>
        /// <exception cref="ApplicationException"></exception>
        [HttpGet]
        [Route("view_plant_name")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                _logger.LogInformation("Obteniendo información de una planta por nombre.");
                var Result = await _service.GetByNameAsync(name);
                if (Result == null)
                {
                    return NotFound(new ResultData
                    {
                        StatusCode = 404,
                        Message = "Not found data"
                    });
                }
                return Ok(Result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new ResultData
                {
                    StatusCode = 400,
                    Message = "Could not get plant by name"
                });
                throw new ApplicationException($"Could not get plant name - {ex.Message}");
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
        ///         
        ///     }
        /// </remarks>
        /// <returns>true</returns>
        /// <response code="200">Something plants.</response>
        /// <response code="400">Not fount data</response>
        /// <response code="500">Server error</response>
        [HttpPost]
        [Route("insert")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AllPlantDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultData))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultData))]
        public async Task<IActionResult> Post([FromForm] PlantsDto model)
        {
            try
            {
                if (await _service.InsertOneAsync(model))
                {
                    _logger.LogInformation("Insertando nueva planta.");
                    return Ok(new ResultData
                    {
                        StatusCode = 200,
                        Message = "Planta insertada correctamente."
                    });
                }
                return StatusCode(500, "Error interno del servidor.");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Error al insertar planta: {ex.Message}");
                return BadRequest(new ResultData
                {
                    StatusCode = 400,
                    Message = "Error interno del servidor."
                });
            }
        }


        [HttpPost]
        [Route("update_plant")]
        public async Task<IActionResult> UpdateOne([FromForm] PlantsDto model)
        {
            try
            {
                PlantsModel modelo = new PlantsModel
                {
                    Name = model.Name,
                    Description = model.Description,
                    // Add other properties as needed.
                };
                if (await _service.UpdateOneAsync(modelo))
                {
                    return Ok(new ResultData
                    {
                        StatusCode = 200,
                        Message = "Update successfully."
                    });
                }
                else
                {
                    return NotFound(new ResultData
                    {
                        StatusCode = 404,
                        Message = "Plant not found."
                    });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(new ResultData
                {
                    StatusCode = 400,
                    Message = "Error interno del servidor."
                });
                throw new ApplicationException($"Failed update service {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("delete_plant")]
        public async Task<IActionResult> DeleteOne(string id)
        {
            try
            {
                if (await _service.DeleteOneAsync(id) == true)
                {
                    return Ok(new ResultData
                    {
                        StatusCode = 200,
                        Message = "Plant deleted successfully."
                    });
                }
                else
                {
                    return NotFound(new ResultData
                    {
                        StatusCode = 404,
                        Message = "Plant not found."
                    });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(new ResultData
                {
                    StatusCode = 400,
                    Message = "Error interno del servidor."
                });
                throw new ApplicationException($"Failed delete service {ex.Message}");
            }
        }
    }
}