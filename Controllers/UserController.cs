namespace P1700.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using P1700.Api.Dtos;
using P1700.Api.Services;


[ApiController]
[Route("User")]
public class UserController : ControllerBase
{
    private readonly UserService _service;
    public UserController(UserService service) => _service = service;

    [HttpPost("Registrar")]
    public async Task<IActionResult> Registrar([FromBody] UserRegisterDto dto)
    {
        try
        {
            var id = await _service.Register(dto);
            return StatusCode(201, new { usuarioId = id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("Autenticar")]
    public async Task<IActionResult> Autenticar([FromBody] UserLoginDto dto)
    {
        try
        {
            var result = await _service.Authenticate(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
