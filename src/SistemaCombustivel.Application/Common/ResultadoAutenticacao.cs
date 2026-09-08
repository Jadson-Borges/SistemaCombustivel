namespace SistemaCombustivel.Application.Abstractions
{
    public class ResultadoAutenticacao
    {
        public bool Sucesso { get; }
        public string? Erro { get; }
        public string? ApplicationUserId { get; }

        private ResultadoAutenticacao(bool sucesso, string? applicationUserId, string? erro)
        {
            Sucesso = sucesso;
            ApplicationUserId = applicationUserId;
            Erro = erro;
        }

        public static ResultadoAutenticacao Ok(string applicationUserId) => new(true, applicationUserId, null);
        public static ResultadoAutenticacao Falha(string erro) => new(false, null, erro);
    }
}