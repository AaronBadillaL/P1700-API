using Microsoft.AspNetCore.Mvc;
using P1700.Api.Services;


[ApiController]
[Route("Catalogos")]
public class CatalogosController : ControllerBase
{
    private readonly CatalogosService _service;
    public CatalogosController(CatalogosService service) => _service = service;

    [HttpGet("Perfiles")]
    public async Task<IActionResult> Perfiles() => Ok(await _service.GetPerfiles());

    [HttpGet("Tiendas")]
    public async Task<IActionResult> Tiendas() => Ok(await _service.GetTiendas());
}
