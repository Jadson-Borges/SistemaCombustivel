using SistemaCombustivel.Application.Abstractions;
using SistemaCombustivel.Application.Common;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Services
{
    public class RegistroUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IServicoAutenticacao _servicoAutenticacao;
        private readonly IUnitOfWork _unitOfWork;

        public RegistroUsuarioService(
            IUsuarioRepository usuarioRepository,
            IServicoAutenticacao servicoAutenticacao,
            IUnitOfWork unitOfWork)
        {
            _usuarioRepository = usuarioRepository;
            _servicoAutenticacao = servicoAutenticacao;
            _unitOfWork = unitOfWork;
        }

        public Task<Resultado> RegistrarAsync(string nomeUsuario, string email, string senha)
            => CriarAsync(nomeUsuario, email, senha, TipoUsuario.Comum);

        public async Task<Resultado> CriarAsync(string nomeUsuario, string email, string senha, TipoUsuario tipo)
        
        {
            var emailJaExiste = await _usuarioRepository.ExisteEmailAsync(email);
            if (emailJaExiste)
                return Resultado.Falha("Já existe um usuário cadastrado com este e-mail.");

            string? erroDeAutenticacao = null;

            try
            {
                await _unitOfWork.ExecutarEmTransacaoAsync(async () =>
                {
                    var usuario = new Usuario(nomeUsuario, email, TipoUsuario.Comum);

                    await _usuarioRepository.AdicionarAsync(usuario);
                    await _usuarioRepository.SalvarAlteracoesAsync();

                    var resultadoAuth = await _servicoAutenticacao.RegistrarAsync(email, senha, usuario.Id);

                    if (!resultadoAuth.Sucesso)
                    {
                        erroDeAutenticacao = resultadoAuth.Erro;
                        throw new FalhaNoRegistroException(); //Falha no rollback da transação
                    }

                });
            }
            catch (FalhaNoRegistroException)
            {
                return Resultado.Falha(erroDeAutenticacao!);
            }

            return Resultado.Ok();
        }

        // Exceção interna: só serve para sinalizar "desfaça a transação".
        private sealed class FalhaNoRegistroException : Exception { }
    }
}