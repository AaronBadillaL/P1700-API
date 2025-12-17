namespace P1700.Api.Repositories;
using System.Data;
using Dapper;

public class UserRepository
{
    private readonly IDbConnection _db;
    public UserRepository(IDbConnection db) => _db = db;

    public Task<int> ExistsByCedula(string cedula) =>
        _db.ExecuteScalarAsync<int>("SELECT COUNT(1) FROM Usuarios WHERE Cedula = @cedula", new { cedula });

    public Task<int> InsertUser(string cedula, string nombreCompleto, byte[] hash, byte[] salt, int perfilId, int tiendaId)
    {
        var sql = @"
INSERT INTO Usuarios (Cedula, NombreCompleto, PasswordHash, PasswordSalt, PerfilId, TiendaId)
VALUES (@cedula, @nombreCompleto, @hash, @salt, @perfilId, @tiendaId);
SELECT CAST(SCOPE_IDENTITY() as int);";

        return _db.ExecuteScalarAsync<int>(sql, new
        {
            cedula,
            nombreCompleto,
            hash,
            salt,
            perfilId,
            tiendaId
        });
    }

    public Task<dynamic?> GetUserForAuth(string cedula) =>
        _db.QueryFirstOrDefaultAsync<dynamic>(@"
SELECT u.UsuarioId, u.NombreCompleto, u.PasswordHash, u.PasswordSalt, p.Nombre AS Perfil
FROM Usuarios u
JOIN Perfiles p ON p.PerfilId = u.PerfilId
WHERE u.Cedula = @cedula AND u.Activo = 1;", new { cedula });

    public async Task<List<string>> GetPermisosByUsuarioId(int usuarioId)
    {
        var sql = @"
SELECT pe.Nombre
FROM Usuarios u
JOIN PerfilPermiso pp ON pp.PerfilId = u.PerfilId
JOIN Permisos pe ON pe.PermisoId = pp.PermisoId
WHERE u.UsuarioId = @usuarioId
ORDER BY pe.Nombre;";

        var result = await _db.QueryAsync<string>(sql, new { usuarioId });
        return result.ToList();
    }
}
