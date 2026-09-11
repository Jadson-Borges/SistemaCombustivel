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

        public async Task<Resultado> RegistrarAsync(string nomeUsuario, string email, string senha)
        {
            var emailJaExiste = await _usuarioRepository.ExisteEmailAsync(email);
            if (emailJaExiste)
                return Resultado.Falha("Já existe um usuário cadastrado com este e-mail.");

            Resultado resultadoFinal = Resultado.Ok();

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
                        resultadoFinal = Resultado.Falha(resultadoAuth.Erro!);
                        throw new InvalidOperationException(resultadoAuth.Erro);
                    }
                });
            }
            catch (InvalidOperationException)
            {
                // Rollback já foi feito pelo UnitOfWork; resultadoFinal já contém o motivo da falha.
                // bloco colocado em try catch para não entrar em loop quando ocorrer um erro não tratado
            }

            return resultadoFinal;
        }
    }
}