namespace P1700.Api.Services;


using P1700.Api.Dtos;
using P1700.Api.Repositories;

public class EmpleadosService
{
    private readonly EmpleadosRepository _repo;
    public EmpleadosService(EmpleadosRepository repo) => _repo = repo;

    public Task<List<EmpleadoListDto>> GetAll() => _repo.GetAll();

    public async Task<EmpleadoDetailDto> GetById(int empleadoId)
    {
        var emp = await _repo.GetById(empleadoId);
        if (emp == null) throw new Exception("Empleado no existe.");
        return emp;
    }

    public async Task<int> Create(EmpleadoCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Cedula) || string.IsNullOrWhiteSpace(dto.NombreCompleto))
            throw new Exception("Cédula y NombreCompleto son requeridos.");

        var exists = await _repo.ExistsByCedula(dto.Cedula);
        if (exists > 0) throw new Exception("Ya existe un empleado con esa cédula.");

        return await _repo.Insert(dto);
    }

    public async Task Update(int empleadoId, EmpleadoUpdateDto dto)
    {
        var updated = await _repo.Update(empleadoId, dto);
        if (updated == 0) throw new Exception("Empleado no existe.");
    }

    public async Task Delete(int empleadoId)
    {
        // Recomendación: si tiene asignaciones, no permitir borrar (para evitar romper historial)
        var has = await _repo.HasAsignaciones(empleadoId);
        if (has > 0) throw new Exception("No se puede eliminar: el empleado tiene asignaciones diarias.");

        var deleted = await _repo.Delete(empleadoId);
        if (deleted == 0) throw new Exception("Empleado no existe.");
    }
}
