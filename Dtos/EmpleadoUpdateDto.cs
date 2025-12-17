namespace P1700.Api.Dtos;
public class EmpleadoUpdateDto
{
    public string NombreCompleto { get; set; } = "";
    public string? Telefono { get; set; }
    public string? TipoEmpleado { get; set; }
    public decimal? Salario { get; set; }
    public int PerfilId { get; set; }
}