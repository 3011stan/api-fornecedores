using AutoMapper;
using DevIO.Api.ViewModel;
using DevIO.Api.Controllers;
using DevIO.Api.Extensions;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DevIO.Api.V1.Controllers
{
    [Authorize] // precisa de autorização p/ todas as rotas abaixo, exceti às descritas
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FornecedoresController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FornecedoresController> _logger;
        public FornecedoresController(IFornecedorRepository fornecedorRepository, IMapper mapper,
                                        IFornecedorService fornecedorService, INotificador notificador, IEnderecoRepository enderecoRepository,
            IUser user, ILogger<FornecedoresController> logger) : base(notificador, user)
        {   // injeção por dependência
            _fornecedorRepository = fornecedorRepository; // para ler do banco
            _mapper = mapper;
            _fornecedorService = fornecedorService; // valida regras de negócio, e se passar, chama o repository
            _enderecoRepository = enderecoRepository;
            _logger = logger;
        }

        [AllowAnonymous] // permite requisiçoes de qualquer um mesmo não estando autenticado
        [HttpGet] // -> transforma o método em endpoint (Por que mesmo sem o get isso funciona?)
        public async Task<IEnumerable<FornecedorViewModel>> ObterTodos()

        {
            // perceba que await _fornecedorRepository.ObterTodos() entrega uma lista de Fornecedores (classe q vem do banc)
            // porém o Map o transforma em um FornecedorViewModel, algo modelado para ser exibido.
            // nunca devemos retornar o dado como é pego lá no banco, sempre devemos criar um outro modelo
            // que será retornado para API.
            _logger.LogInformation("Log de teste.");
            var fornecedor = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            return fornecedor;
        }

        //[AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
        {
            var fornecedor = await ObterFornecedorProdutosEndereco(id);

            //var fornecedor = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEnderecos(id));

            if (fornecedor == null) return NotFound();

            return fornecedor;
        }

        [ClaimsAuthorize("Fornecedor", "Adicionar")]
        [HttpPost]
        public async Task<ActionResult<FornecedorViewModel>> Adicionar([FromBody] FornecedorViewModel fornecedorViewModel) // por esse método consegue reconheceu o JSON sem [FromBody] no curso, mas aqui não?
        {
            if (UsuarioAutenticado) // se o usuário estiver autenticado já teremos o id desse usuário...
            {
                var userName = UsuarioId;
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);
            await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(fornecedorViewModel));
            return CustomResponse(fornecedorViewModel);
        }

        [ClaimsAuthorize("Fornecedor", "Atualizar")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Atualizar(Guid id, [FromBody] FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query.");
                return CustomResponse(fornecedorViewModel);
            }
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            await _fornecedorService.Atualizar(_mapper.Map<Fornecedor>(fornecedorViewModel));
            return CustomResponse(fornecedorViewModel);
        }

        [ClaimsAuthorize("Fornecedor", "Excluir")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Excluir(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);
            if (fornecedorViewModel == null) return NotFound();
            await _fornecedorService.Remover(id);
            return CustomResponse(fornecedorViewModel);

        }
        [AllowAnonymous]
        [HttpGet("/obter-endereco/{id:guid}")]
        public async Task<EnderecoViewModel> ObterEnderecoPorId(Guid id)
        {
            return _mapper.Map<EnderecoViewModel>(await _enderecoRepository.ObterPorId(id));
        }

        [ClaimsAuthorize("Fornecedor", "Atualizar")]
        [HttpPut("/atualizar-endereco/{id:guid}")]
        public async Task<IActionResult> AtualizarEndereco(Guid id, [FromBody] EnderecoViewModel enderecoViewModel)
        {
            if (id != enderecoViewModel.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query.");
                return CustomResponse(enderecoViewModel);
            }
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var endereco = _mapper.Map<Endereco>(enderecoViewModel);
            await _fornecedorService.AtualizarEndereco(endereco);

            return CustomResponse(enderecoViewModel);
        }

        // métodos feitos para encapsulamento
        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEnderecos(id));
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
        }
    }
}
