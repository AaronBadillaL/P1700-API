namespace P1700.Api.Dtos;
public class UserRegisterDto
{
    public string Cedula { get; set; } = "";
    public string NombreCompleto { get; set; } = "";
    public string Password { get; set; } = "";
    public int PerfilId { get; set; }
    public int TiendaId { get; set; }
}
