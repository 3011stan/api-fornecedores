using DevIO.Api.Configuration;
using DevIO.Api.Extensions;
using DevIO.Business.Interfaces;
using DevIO.Business.Notifications;
using DevIO.Business.Services;
using DevIO.Data.Context;
using DevIO.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DevIO.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        // esse código foi escrito numa classe separada para deixar o Startup.cs mais enxuto.
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<MeuDbContext>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();
            services.AddScoped<IFornecedorService, FornecedorService>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();
            //services.AddScoped<IEnderecoService, EnderecoService>(); // teste
            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IProdutoService, ProdutoService>();

            // resolvendo dependencias do identity/claims/validaçoes
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // singleton porque é pra toda a aplicação
            services.AddScoped<IUser, AspNetUser>();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            return services;
        }
    }
}

// Existem três formas de injeção de dependência no .NET Core:
// 1. Scoped: Se quiser manter o estado do serviço em um request. Eles só irão mudar sempre quando fizer um novo request. Durante todo o request utiliza-se
// a mesma instância de objeto, qando a requisição termina, então o objeto é removido da memória.

// 2. Singleton: Onde você precisa reutilizar o serviço em várias pontas de sua aplicação como por exemplo: configurações do aplicativo ou parâmetros,
// serviço de log, armazenamento em cache de dados. Aqui utiliza-se o mesmo objeto para a aplicação.

// 3. Transient: Temporário, passageiro como tradução literal. Será criada uma nova instância do objeto toda vez que fizer uma requisição. Mas, uma vez que eles são criados, eles usarão mais memória e
// recursos, e podem ter o impacto negativo no desempenho. Então use para o serviço leve com pouco ou nenhum estado.

// Singleton é o oposto do Transient