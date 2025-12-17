namespace P1700.Api.Services;
using P1700.Api.Dtos;
using P1700.Api.Dtosl;
using P1700.Api.Repositories;

public class CatalogosService
{
    private readonly CatalogosRepository _repo;
    public CatalogosService(CatalogosRepository repo) => _repo = repo;

    public Task<List<PerfilDto>> GetPerfiles() => _repo.GetPerfiles();
    public Task<List<TiendaDto>> GetTiendas() => _repo.GetTiendas();
}
