using App.Core.DTO_s.RegionDTO_s;
using App.Core.DTOs.RegionDTO_s;
using App.Core.Exceptions;
using App.Core.Filters;
using App.Core.ServicesContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimpleProjectWebAPIwithDIandEF.ExtentionMethods;

using System.Threading.Tasks;


namespace SimpleProjectWebAPIwithDIandEF.Controllers
{
    [TypeFilter(typeof(GlobalLoggerFilter))]
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(WrapperFilter))]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly ILogger<RegionsController> _logger;
        private readonly IRegionService _regionService;
        public RegionsController(IRegionService regionService, ILogger<RegionsController> logger)
        {
            _regionService = regionService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<ReadRegionDTO> regions = await _regionService.GetAllRegions();   
            
            return Ok(regions);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if(id == null)
            {
                return BadRequest("id must be provided");
            }
            try
            {
                ReadRegionDTO r =await _regionService.GetRegionById(id);
                if(r == null)
                {
                    return NotFound();
                }
                return Ok(r);
            }catch(RegionNotFoundException ex)
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

        [HttpPost]
        public async Task<IActionResult> AddRegion(InputRegionDTO r)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(e=> e.Errors).SelectMany(e=>e.ErrorMessage));
            }
            try
            {
                ResultFromAddRegion addedRegion = await _regionService.AddRegion(r);
                return CreatedAtAction(nameof(GetById), new { id = addedRegion.Id }, addedRegion);
            }catch(UnableToAddToDbException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }



        [HttpPut("{id:guid}")]
        public async Task<IActionResult> updateRegion(Guid id, InputRegionDTO r)
        {
            if (!ModelState.IsValid) { 
                return BadRequest(ModelState.Values.SelectMany(e=> e.Errors).SelectMany(e=> e.ErrorMessage));
            }
            try
            {
                ReadRegionDTO updatedRegion = await _regionService.updateRegion(id, r);
                return Ok(updatedRegion);

            }
            catch (RegionNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteRegion(Guid id)
        {
            if(id == null)
            {
                _logger.LogWarning("id must be provided");
                return BadRequest("id must be provided");
            }
            try
            {
                int rows = await _regionService.DeleteRegion(id);
                return Ok($"{rows} deleted successfully");
            }
            catch (UnableToDeleteFromDbException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

        }

    }

}
