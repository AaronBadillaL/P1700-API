namespace P1700.Api.Dtos;
public class EmpleadoDetailDto
{
    public int EmpleadoId { get; set; }
    public string Cedula { get; set; } = "";
    public string NombreCompleto { get; set; } = "";
    public string? Telefono { get; set; }
    public string? TipoEmpleado { get; set; }
    public decimal? Salario { get; set; }
    public int PerfilId { get; set; }
    public string Perfil { get; set; } = "";
}
