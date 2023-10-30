using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logger;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dtos;
using MagicVilla_VillaAPI.Repository;
using MagicVilla_VillaAPI.Repository.IRepostiory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    public class VillaNumberApiController : ControllerBase
    {
        protected APIResponse _response;
        private readonly ILogger<VillaNumberApiController> _logger;
        private readonly IMapper _mapper;
        private readonly IVillaNumberRepository _villaNumberRepository;
        private readonly IVillaRepository _villaRepository;

        public VillaNumberApiController(ILogger<VillaNumberApiController> logger, IMapper mapper,
            IVillaNumberRepository villaNumberRepository, IVillaRepository villaRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _villaNumberRepository = villaNumberRepository;
            _response = new();
            _villaRepository = villaRepository;
        }
        //private readonly ILogging _logger;
        //public VillaApiController(ILogging logger)
        //{
        //    _logger = logger;
        //}

        
        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                _logger.LogInformation("Get all villaNumbers", "info");
                IEnumerable<VillaNumber> villaNumbers = await _villaNumberRepository.GetAllAsync(includeProperties: "Villa");
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumbers);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{villaNo:int}", Name = "GetVillaNumber")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int villaNo)
        {
            try
            {
                if (villaNo == 0)
                {
                    _logger.LogError("No villa number found with villa No {0}", villaNo);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>() { "No villa number found with villa No " + villaNo };
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                VillaNumber villaNumber = await _villaNumberRepository.GetAsync(x => x.VillaNo == villaNo, includeProperties: "Villa");
                if (villaNumber == null)
                {
                    _logger.LogError("No villa number found with villa No {0}", villaNo);
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string>() { "No villa number found with villa No " + villaNo };
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                _logger.LogInformation("Villa number found with villa No {0}", villaNo);
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>() { "Data is required for creating a villa number" };
                    _response.IsSuccess = false;
                    _response.Result = createDTO;
                    return BadRequest(_response);
                }
                else if (await _villaNumberRepository.GetAsync(x => x.VillaNo == createDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa number must be unique");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>() { "Villa number must be unique" };
                    _response.IsSuccess = false;
                    _response.Result = ModelState;
                    return BadRequest(_response);
                }
                else if (await _villaRepository.GetAsync(x => x.Id == createDTO.VillaID) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa id is invalid");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>() { "Villa id is invalid" };
                    _response.IsSuccess = false;
                    _response.Result = ModelState;
                    return BadRequest(_response);
                }
                VillaNumber data = _mapper.Map<VillaNumber>(createDTO);
                data.CreatedDate = DateTime.Now;
                data.UpdatedDate = DateTime.Now;
                await _villaNumberRepository.CreateAsync(data);
                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = _mapper.Map<VillaNumberDTO>(data);
                return CreatedAtRoute("GetVillaNumber", new { villaNo = data.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{villaNo:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int villaNo)
        {
            try
            {
                if (villaNo == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>() { "No villa number found with villaNo " + villaNo };
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                VillaNumber villaNumber = await _villaNumberRepository.GetAsync(x => x.VillaNo == villaNo);
                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string>() { "No villa number found with villaNo " + villaNo };
                    _response.IsSuccess = false;
                    _response.Result = villaNumber;
                    return NotFound(_response);
                }
                await _villaNumberRepository.RemoveAsync(villaNumber);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{villaNo:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int villaNo, VillaNumberUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || villaNo != updateDTO.VillaNo)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>() { "This is a bad request" };
                    _response.IsSuccess = false;
                    _response.Result = updateDTO;
                    return BadRequest(_response);
                }
                VillaNumber villaNumberObj = await _villaNumberRepository.GetAsync(x => x.VillaNo == villaNo, tracked: false);

                if (villaNumberObj == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string>() { "No villa number found with villaNo " + villaNo };
                    _response.IsSuccess = false;
                    _response.Result = villaNumberObj;
                    return NotFound(_response);
                }

                if (await _villaRepository.GetAsync(x => x.Id == updateDTO.VillaID) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa id is invalid");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>() { "Villa id is invalid" };
                    _response.IsSuccess = false;
                    _response.Result = ModelState;
                    return BadRequest(_response);
                }
                VillaNumber data = _mapper.Map<VillaNumber>(updateDTO);
                data.UpdatedDate = DateTime.Now;
                data.CreatedDate = villaNumberObj.CreatedDate;
                VillaNumber result = await _villaNumberRepository.UpdateAsync(data);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.Result = _mapper.Map<VillaNumberUpdateDTO>(result);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("{villaNo:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int villaNo, JsonPatchDocument<VillaNumberUpdateDTO> patchDTO)
        {
            try
            {
                if (patchDTO == null || villaNo == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>() { "This is a bad request" };
                    _response.IsSuccess = false;
                    _response.Result = patchDTO;
                    return BadRequest(_response);
                }
                VillaNumber villaNumberObj = await _villaNumberRepository.GetAsync(x => x.VillaNo == villaNo, tracked: false);

                if (villaNumberObj == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string>() { "No villa number found with villaNo " + villaNo };
                    _response.IsSuccess = false;
                    _response.Result = villaNumberObj;
                    return NotFound(_response);
                }

                VillaNumberUpdateDTO villaNumberUpdateDTO = _mapper.Map<VillaNumberUpdateDTO>(villaNumberObj);

                patchDTO.ApplyTo(villaNumberUpdateDTO, ModelState);

                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.Result = ModelState;
                    return BadRequest(_response);
                }

                VillaNumber data = _mapper.Map<VillaNumber>(villaNumberUpdateDTO);
                data.UpdatedDate = DateTime.Now;
                data.CreatedDate = villaNumberObj.CreatedDate;
                VillaNumber result = await _villaNumberRepository.UpdateAsync(data);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.Result = _mapper.Map<VillaNumberUpdateDTO>(result);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
