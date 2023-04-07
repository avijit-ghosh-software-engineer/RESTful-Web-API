using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logger;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        private readonly ILogger<VillaApiController> _logger;
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public VillaApiController(ILogger<VillaApiController> logger, AppDbContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }
        //private readonly ILogging _logger;
        //public VillaApiController(ILogging logger)
        //{
        //    _logger = logger;
        //}

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            _logger.LogInformation("Get all villas","info");
            IEnumerable<Villa> villas = await _dbContext.Villas.ToListAsync();
            return Ok(_mapper.Map<List<VillaDto>>(villas));
        }

        [HttpGet("{id:int}",Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("No villa found with id {0}", id);
                return BadRequest();
            }

            Villa villa = await _dbContext.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if (villa == null)
            {
                _logger.LogError("No villa found with id {0}", id);
                return NotFound();
            }
            _logger.LogInformation("Vill found with id {0}",id);

            return Ok(_mapper.Map<VillaDto>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VillaDto>> CreateVilla([FromBody]VillaCreateDTO createDTO)
        {
            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }
            else if (await _dbContext.Villas.FirstOrDefaultAsync(x=>x.Name.ToLower()== createDTO.Name.ToLower())!=null)
            {
                ModelState.AddModelError("CustomError", "Villa name must be unique.");
                return BadRequest(ModelState);
            }
            Villa data = _mapper.Map<Villa>(createDTO);
            data.CreatedDate = DateTime.Now;
            data.UpdatedDate = DateTime.Now;
            await _dbContext.Villas.AddAsync(data);
            await _dbContext.SaveChangesAsync();
            return CreatedAtRoute("GetVilla",new { id = data.Id}, _mapper.Map<VillaDto>(data));
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id==0)
            {
                return BadRequest();
            }

            Villa villa = await _dbContext.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if (villa==null)
            {
                return NotFound();
            }
            _dbContext.Villas.Remove(villa);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateVilla(int id,VillaUpdateDTO updateDTO)
        {
            if (updateDTO == null || id != updateDTO.Id)
            {
                return BadRequest();
            }
            Villa data = _mapper.Map<Villa>(updateDTO);
            data.UpdatedDate = DateTime.Now;
            _dbContext.Villas.Update(data);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            Villa villaObj = await _dbContext.Villas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (villaObj == null)
            {
                return NotFound();
            }

            VillaUpdateDTO villaUpdateDTO = _mapper.Map<VillaUpdateDTO>(villaObj);

            patchDTO.ApplyTo(villaUpdateDTO, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa data = _mapper.Map<Villa>(villaUpdateDTO);
            data.UpdatedDate = DateTime.Now;
            _dbContext.Villas.Update(data);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
