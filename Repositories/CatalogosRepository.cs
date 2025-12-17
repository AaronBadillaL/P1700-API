namespace P1700.Api.Repositories;

using Dapper;
using P1700.Api.Dtos;
using P1700.Api.Dtosl;
using System.Data;

public class CatalogosRepository
{
    private readonly IDbConnection _db;
    public CatalogosRepository(IDbConnection db) => _db = db;

    public async Task<List<PerfilDto>> GetPerfiles()
    {
        var sql = "SELECT PerfilId, Nombre FROM Perfiles ORDER BY Nombre";
        var result = await _db.QueryAsync<PerfilDto>(sql);
        return result.ToList();
    }

    public async Task<List<TiendaDto>> GetTiendas()
    {
        var sql = "SELECT TiendaId, Nombre FROM Tiendas ORDER BY Nombre";
        var result = await _db.QueryAsync<TiendaDto>(sql);
        return result.ToList();
    }
}
