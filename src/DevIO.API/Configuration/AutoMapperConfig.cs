using AutoMapper;
using DevIO.Api.ViewModel;
using DevIO.Business.Models;

namespace DevIO.Api.Configuration
{
    public class AutoMapperConfig : Profile
    {
        // td q estiver aqui será resolvido quando a aplicação iniciar.
        public AutoMapperConfig()
        {
            CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();

            CreateMap<Produto, ProdutoViewModel>().ReverseMap();

            CreateMap<Produto, ProdutoViewModel>()
                .ForMember(dest => dest.NomeFornecedor, opt => opt.MapFrom(src => src.Fornecedor.Nome));

            CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
        }
    }
}
