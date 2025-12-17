namespace P1700.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using P1700.Api.Dtos;
using P1700.Api.Services;

[ApiController]
[Route("Empleados")]
public class EmpleadosController : ControllerBase
{
    private readonly EmpleadosService _service;
    public EmpleadosController(EmpleadosService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAll());

    [HttpGet("{empleadoId:int}")]
    public async Task<IActionResult> GetById(int empleadoId)
    {
        try { return Ok(await _service.GetById(empleadoId)); }
        catch (Exception ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmpleadoCreateDto dto)
    {
        try
        {
            var id = await _service.Create(dto);
            return StatusCode(201, new { empleadoId = id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{empleadoId:int}")]
    public async Task<IActionResult> Update(int empleadoId, [FromBody] EmpleadoUpdateDto dto)
    {
        try
        {
            await _service.Update(empleadoId, dto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{empleadoId:int}")]
    public async Task<IActionResult> Delete(int empleadoId)
    {
        try
        {
            await _service.Delete(empleadoId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
