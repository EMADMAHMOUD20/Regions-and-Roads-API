using App.Core.DTO_s.RoadDTO_s;
using App.Core.DTOs.RegionDTO_s;
using App.Core.DTOs.RoadDTO_s;
using App.Core.Exceptions;
using App.Core.Exceptions.RoadsExceptions;
using App.Core.Filters;
using App.Core.ServicesContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.Identity.Client;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks.Dataflow;

namespace SimpleProjectWebAPIwithDIandEF.Controllers
{
    [TypeFilter(typeof(WrapperFilter))]
    [TypeFilter(typeof(GlobalLoggerFilter))]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoadsController : ControllerBase
    {
        private readonly IRoadService _roadService;
        private readonly ILogger<RoadsController> _logger;
        public RoadsController(IRoadService roadService,ILogger<RoadsController> logger)
        {
            _roadService = roadService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRoads([FromQuery,SwaggerParameter("Easy, Medium, Hard")] string? DifficultyLevel
            ,[FromQuery,SwaggerParameter("Asc, Desc")] string? RoadLengthOrder
            , [FromQuery] string? SearchByName)
        {
            try
            {
                List<ReadRoadDTO> roads = await _roadService.GetAllRoads(DifficultyLevel,RoadLengthOrder,SearchByName);

                return Ok(roads);

            }
            catch (NoRoadFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetRoadById(Guid id)
        {
            if(id == null)
            {
                _logger.LogWarning("id must be provided");
                return BadRequest("id can't be null");
            }
            try
            {
                ReadRoadDTO road = await _roadService.GetRoadById(id);
                return Ok(road);
            }
            catch (NoRoadFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);  
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddRoad(InputRoadDTO addedRoad) {
            if (!ModelState.IsValid) {
                _logger.LogWarning("Error in validation");

                return BadRequest(ModelState.Values.SelectMany(e => e.Errors).SelectMany(e => e.ErrorMessage));
            }
            try
            {
                ReadRoadDTO returnedFromAdd = await _roadService.AddRoad(addedRoad);
                return Ok(returnedFromAdd);
            }
            catch (DuplicatedRoadFoundException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (ErrorWhileAddException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateRoad(Guid id,InputRoadDTO addedRoad)
        {
            if (id==null)
            {
                _logger.LogWarning("id equal to null");
                return BadRequest("id must be valid");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("error in validation ");
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors).SelectMany(e => e.ErrorMessage));
            }
            try
            {
                ReadRoadDTO returnedfromUpdate = await _roadService.UpdateRoad(id, addedRoad);
                return Ok(returnedfromUpdate);
            }
            catch (RoadNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (ErrorWhileAddException ex) {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteRoad(Guid id)
        {

            if (id == null)
            {
                _logger.LogWarning("id equal null");
                return BadRequest("id must be valid");
            }
            try
            {
                int rows = await _roadService.DeleteRoad(id);
                return Ok($"{rows} deleted Successfully");
            }catch(NoRoadFoundWithThisIdException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }catch(CannotDeleteRoadException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
