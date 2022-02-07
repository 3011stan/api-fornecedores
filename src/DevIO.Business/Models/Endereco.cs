using System;

namespace DevIO.Business.Models
{
    public class Endereco : Entity
    {
        // foreign key
        public Guid FornecedorId { get; set; }

        public string Logradouro { get; set; }

        public string Numero { get; set; }
        public string Complemento { get; set; }

        public string Cep { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public string Estado { get; set; }

        /*EF Relation <<-- propriedade para o Entity Framework fazer o mapeamento corretamente*/

        public Fornecedor Fornecedor { get; set; }



    }
}
