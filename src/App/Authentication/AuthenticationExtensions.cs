using System.Text;

using Microsoft.IdentityModel.Tokens;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace App.Authentication;

internal static class AuthenticationExtensions
{
	private static readonly string sectionName = "Authentication:Jwt";

	private static JwtAuthenticationConfiguration GetJwtConfiguration(IConfiguration configuration)
	{
		var jwtConfiguration = Activator.CreateInstance<JwtAuthenticationConfiguration>();

		configuration.GetRequiredSection(sectionName)
			.Bind(jwtConfiguration);

		return jwtConfiguration!;
	}

	private static AuthenticationBuilder AddJwtBearer(this AuthenticationBuilder builder, IConfiguration configuration)
	{
		var jwtConfiguration = GetJwtConfiguration(configuration);

		return builder.AddJwtBearer(options =>
		{
			options.TokenValidationParameters = new TokenValidationParameters()
			{
				ValidateIssuer = true,
				ValidIssuer = jwtConfiguration.Issuer,
				ValidateAudience = true,
				ValidAudience = jwtConfiguration.Audience,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(jwtConfiguration.SecretKey)),
				ValidateLifetime = true,
				ClockSkew = TimeSpan.Zero,
			};
		});
	}

	public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		return services
			.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(configuration)
			.Services;
	}
}
