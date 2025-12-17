namespace P1700.Api.Services;
using P1700.Api.Dtos;
using P1700.Api.Repositories;
using P1700.Api.Security;

public class UserService
{
    private readonly UserRepository _repo;

    public UserService(UserRepository repo) => _repo = repo;

    public async Task<int> Register(UserRegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Cedula) ||
            string.IsNullOrWhiteSpace(dto.NombreCompleto) ||
            string.IsNullOrWhiteSpace(dto.Password))
            throw new Exception("Datos incompletos.");

        var exists = await _repo.ExistsByCedula(dto.Cedula);
        if (exists > 0)
            throw new Exception("Ya existe un usuario con esa cédula.");

        var (hash, salt) = PasswordHasher.CreateHash(dto.Password);

        return await _repo.InsertUser(dto.Cedula, dto.NombreCompleto, hash, salt, dto.PerfilId, dto.TiendaId);
    }

    public async Task<AuthResponseDto> Authenticate(UserLoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Cedula) || string.IsNullOrWhiteSpace(dto.Password))
            throw new Exception("Datos incompletos.");

        var user = await _repo.GetUserForAuth(dto.Cedula);
        if (user == null)
            throw new Exception("Credenciales inválidas.");

        if (!PasswordHasher.Verify(dto.Password, (byte[])user.PasswordHash, (byte[])user.PasswordSalt))
            throw new Exception("Credenciales inválidas.");

        var permisos = await _repo.GetPermisosByUsuarioId((int)user.UsuarioId);

        return new AuthResponseDto
        {
            NombreCompleto = (string)user.NombreCompleto,
            Perfil = (string)user.Perfil,
            Permisos = permisos
        };
    }
}
