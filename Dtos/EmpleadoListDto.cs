namespace P1700.Api.Dtos;
public class EmpleadoListDto
{
    public int EmpleadoId { get; set; }
    public string Cedula { get; set; } = "";
    public string NombreCompleto { get; set; } = "";
    public string? Telefono { get; set; }
    public string? TipoEmpleado { get; set; }
    public decimal? Salario { get; set; }
    public string Perfil { get; set; } = "";

    // info extra útil para UI (última asignación)
    public DateTime? FechaUltimaAsignacion { get; set; }
    public string? TiendaUltima { get; set; }
    public string? SupervisorUltimo { get; set; }
}
