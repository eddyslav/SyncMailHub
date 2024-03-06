namespace Modules.Hub.Application.TokenService;

public interface ITokenService
{
	GeneratedTokenResult GenerateToken(GenerateTokenRequest request);
}
