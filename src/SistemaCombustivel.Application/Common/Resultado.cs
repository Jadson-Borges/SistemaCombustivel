namespace SistemaCombustivel.Application.Common
{
    public class Resultado
    {
        public bool Sucesso { get; }
        public string? Erro { get; }

        protected Resultado(bool sucesso, string? erro)
        {
            if (sucesso && erro is not null)
                throw new InvalidOperationException("Um resultado de sucesso não pode ter mensagem de erro.");

            if (!sucesso && erro is null)
                throw new InvalidOperationException("Um resultado de falha precisa ter uma mensagem de erro.");

            Sucesso = sucesso;
            Erro = erro;
        }

        public static Resultado Ok() => new(true, null);
        public static Resultado Falha(string erro) => new(false, erro);
    }

    public class Resultado<T> : Resultado
    {
        public T? Valor { get; }

        private Resultado(bool sucesso, T? valor, string? erro) : base(sucesso, erro)
        {
            Valor = valor;
        }

        public static Resultado<T> Ok(T valor) => new(true, valor, null);
        public static new Resultado<T> Falha(string erro) => new(false, default, erro);
    }
}