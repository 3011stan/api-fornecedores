using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IFornecedorRepository : IRepository<Fornecedor> // DÚVIDA: O QUE É ESSE REPOSITORY? R: Interface mais próxima do banco.
    {
        Task<Fornecedor> ObterFornecedorEndereco(Guid id);

        Task<Fornecedor> ObterFornecedorProdutosEnderecos(Guid id);

        Task<int> Excluir(Guid id);
    }
}

// interface abstrai os métodos