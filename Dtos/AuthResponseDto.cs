namespace P1700.Api.Dtos;
public class AuthResponseDto
{
    public string NombreCompleto { get; set; } = "";
    public string Perfil { get; set; } = "";
    public List<string> Permisos { get; set; } = new();
}
