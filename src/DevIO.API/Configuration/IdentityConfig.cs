using DevIO.Api.Data;
using DevIO.Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DevIO.Api.Configuration
{
    // classe criada para lidar com autenticações
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuração do dbcontext e conexão com o banco de dados e garantia de acesso ao bonco pelo "ApplicationDbContext"
            // é de boa prática utilizar um contexto para manipular os dados da aplicação e outro para tratar dos dados dos usuários
            // no entanto, o ApplicationDbContext herdamos de IdentityDbContext, que é uma classa já preparada pro identity.
            services.AddDbContext<ApplicationDbContext>(options => // configuração da conexão com o banco
              options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>() // customizar politica de roles (possibilidade)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddErrorDescriber<IdentityMensagensPortugues>()
                .AddDefaultTokenProviders(); // recurso para gerar tokens (resetar senhas, autenticação de e-mails), através de tokens

            // JWT
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            // obtendo os dados
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret); // encodificando segredo

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false; //
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // true para validar quem emitou token
                    IssuerSigningKey = new SymmetricSecurityKey(key), // a vakudação será feita através da chave, não apenas através do ValidIssuer: emissor apssado
                    ValidateIssuer = true, // validar somente o issuer conforme o nome
                    ValidateAudience = true, // valider por "valido em"
                    ValidAudience = appSettings.ValidoEm,
                    ValidIssuer = appSettings.Emissor
                };
            });

            return services;
        }
    }
}
