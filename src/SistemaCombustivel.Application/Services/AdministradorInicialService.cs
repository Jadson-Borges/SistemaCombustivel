using SistemaCombustivel.Application.Abstractions;
using SistemaCombustivel.Application.Common;
using SistemaCombustivel.Domain.Entities;
namespace SistemaCombustivel.Application.Services
{
    public class AdministradorInicialService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly RegistroUsuarioService _registroUsuarioService;

        public AdministradorInicialService(
            IUsuarioRepository usuarioRepository,
            RegistroUsuarioService registroUsuarioService)
        {
            _usuarioRepository = usuarioRepository;
            _registroUsuarioService = registroUsuarioService;
        }

        public async Task<Resultado> GarantirAsync(string nomeUsuario, string email, string senha)
        {
            if (await _usuarioRepository.ExisteAdministradorAsync())
                return Resultado.Ok(); //já existe registro então não faz nada

            return await _registroUsuarioService.CriarAsync(
                nomeUsuario, email, senha, TipoUsuario.Administrador);
        }

    }
}
