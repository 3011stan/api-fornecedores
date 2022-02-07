namespace DevIO.Api.Extensions
{
    public class AppSettings
    {
        public string Secret { get; set; } // chave de criptografica (o segredo que só nós sabemos)
        public double ExpiracaoHoras { get; set; }
        public string Emissor { get; set; } // qm emite
        public string ValidoEm { get; set; } // quais urls o token é válido
    }
}
