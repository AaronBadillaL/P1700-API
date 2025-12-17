namespace P1700.Api.Repositories;
using Dapper;
using P1700.Api.Dtos;
using System.Data;

public class EmpleadosRepository
{
    private readonly IDbConnection _db;
    public EmpleadosRepository(IDbConnection db) => _db = db;

    public async Task<List<EmpleadoListDto>> GetAll()
    {
        // Trae empleados + perfil + última asignación (si existe)
        var sql = @"
SELECT 
    e.EmpleadoId,
    e.Cedula,
    e.NombreCompleto,
    e.Telefono,
    e.TipoEmpleado,
    e.Salario,
    p.Nombre AS Perfil,
    a.FechaUltimaAsignacion,
    a.TiendaUltima,
    a.SupervisorUltimo
FROM Empleados e
JOIN Perfiles p ON p.PerfilId = e.PerfilId
OUTER APPLY (
    SELECT TOP 1
        etd.Fecha AS FechaUltimaAsignacion,
        t.Nombre AS TiendaUltima,
        s.NombreCompleto AS SupervisorUltimo
    FROM EmpleadoTiendaDia etd
    JOIN Tiendas t ON t.TiendaId = etd.TiendaId
    JOIN Empleados s ON s.EmpleadoId = etd.SupervisorId
    WHERE etd.EmpleadoId = e.EmpleadoId
    ORDER BY etd.Fecha DESC
) a
ORDER BY e.NombreCompleto;";

        var result = await _db.QueryAsync<EmpleadoListDto>(sql);
        return result.ToList();
    }

    public Task<EmpleadoDetailDto?> GetById(int empleadoId)
    {
        var sql = @"
SELECT
    e.EmpleadoId,
    e.Cedula,
    e.NombreCompleto,
    e.Telefono,
    e.TipoEmpleado,
    e.Salario,
    e.PerfilId,
    p.Nombre AS Perfil
FROM Empleados e
JOIN Perfiles p ON p.PerfilId = e.PerfilId
WHERE e.EmpleadoId = @empleadoId;";

        return _db.QueryFirstOrDefaultAsync<EmpleadoDetailDto>(sql, new { empleadoId });
    }

    public Task<int> ExistsByCedula(string cedula) =>
        _db.ExecuteScalarAsync<int>("SELECT COUNT(1) FROM Empleados WHERE Cedula = @cedula", new { cedula });

    public Task<int> Insert(EmpleadoCreateDto dto)
    {
        var sql = @"
INSERT INTO Empleados (Cedula, NombreCompleto, Telefono, TipoEmpleado, Salario, PerfilId)
VALUES (@Cedula, @NombreCompleto, @Telefono, @TipoEmpleado, @Salario, @PerfilId);
SELECT CAST(SCOPE_IDENTITY() as int);";

        return _db.ExecuteScalarAsync<int>(sql, dto);
    }

    public Task<int> Update(int empleadoId, EmpleadoUpdateDto dto)
    {
        var sql = @"
UPDATE Empleados
SET NombreCompleto = @NombreCompleto,
    Telefono = @Telefono,
    TipoEmpleado = @TipoEmpleado,
    Salario = @Salario,
    PerfilId = @PerfilId
WHERE EmpleadoId = @empleadoId;";

        return _db.ExecuteAsync(sql, new
        {
            empleadoId,
            dto.NombreCompleto,
            dto.Telefono,
            dto.TipoEmpleado,
            dto.Salario,
            dto.PerfilId
        });
    }

    public Task<int> Delete(int empleadoId)
    {
        // OJO: si el empleado tiene filas en EmpleadoTiendaDia, el delete fallará por FK.
        // Para prueba, puedes decidir: borrar asignaciones primero o bloquear el delete.
        return _db.ExecuteAsync("DELETE FROM Empleados WHERE EmpleadoId = @empleadoId", new { empleadoId });
    }

    public Task<int> HasAsignaciones(int empleadoId) =>
        _db.ExecuteScalarAsync<int>("SELECT COUNT(1) FROM EmpleadoTiendaDia WHERE EmpleadoId = @empleadoId", new { empleadoId });
}
