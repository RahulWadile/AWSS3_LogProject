using AWSS3BucketWithNetCore.Domain.Services.APIModels;
using AWSS3BucketWithNetCore.Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWSS3BucketWithNetCore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCatalogAsync(AddCatalogRequest catalogRequest)
        {
            try
            {

                await _catalogService.WriteLog("start AddCatalogAsync method");

                var response = await _catalogService.AddCatalogAsync(catalogRequest);

                await _catalogService.WriteLog("success AddCatalogAsync method");
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _catalogService.WriteLog(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCatalogAsync()
        {
            try
            {
                await _catalogService.WriteLog("start GetAllCatalogAsync method");
                var response = await _catalogService.GetAllCatalogAsync();
                await _catalogService.WriteLog("success GetAllCatalogAsync method");

                return Ok(response);
            }
            catch (Exception ex)
            {
                await _catalogService.WriteLog(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCatalogByIdAsync(int id)
        {
            try
            {
                await _catalogService.WriteLog("Call to GetCatalogByIdAsync Method");

                var response = await _catalogService.GetCatalogByIdAsync(id);

                await _catalogService.WriteLog("success GetCatalogByIdAsync method");
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _catalogService.WriteLog(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }




        [HttpGet("{filename}")]
        public async Task<IActionResult> GetCatalogByIdAsync(string filename)
        {
            try
            {
                await _catalogService.WriteLog("Call to GetCatalogByIdAsync Method");
                var response = await _catalogService.GetLogDetails(filename);

                await _catalogService.WriteLog("success GetCatalogByIdAsync method");
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _catalogService.WriteLog(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }


        [HttpPut]
        public async Task<IActionResult> UpdateCatalogAsync(PutCatalogRequest catalogRequest)
        {
            try
            {
                await _catalogService.WriteLog("start UpdateCatalogAsync method");

                var response = await _catalogService.UpdateCatalogAsync(catalogRequest);
                await _catalogService.WriteLog("success UpdateCatalogAsync method");
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _catalogService.WriteLog(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCatalogAsync(int id)
        {
            try
            {
                await _catalogService.WriteLog("start DeleteCatalogAsync method");
                var response = await _catalogService.DeleteCatalogAsync(id);
                await _catalogService.WriteLog("success DeleteCatalogAsync method");

                return Ok(response);
            }
            catch (Exception ex)
            {
                await _catalogService.WriteLog(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }


       
    }
}
