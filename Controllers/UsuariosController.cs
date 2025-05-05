using API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;

namespace API.Controllers
{
    [RoutePrefix("api/usuarios")]
    public class UsuariosController : ApiController
    {
        private const string CAMINHO_ARQUIVO = Configuracoes.CAMINHO_ARQUIVOS + @"\usuarios.json";

        // POST: api/usuarios/criar
        [Route("criar")]
        [HttpPost]
        public IHttpActionResult CriarUsuario(Usuario usuario)
        {
            var listaUsuarios = CarregarUsuarios().ToList();

            if (listaUsuarios.Where(u => u.nome == usuario.nome).Count() > 0) return BadRequest();

            listaUsuarios.Add(usuario);
            Salvar(listaUsuarios);

            return Ok(usuario);
        }

        // POST: api/usuarios/criar
        [Route("{id}")]
        [HttpPost]
        public IHttpActionResult AutenticarUsuario(Usuario usuarioLogin)
        {
            Usuario usuario = CarregarUsuarios().FirstOrDefault(u => u.nome == usuarioLogin.nome);

            //Validação
            if (usuarioLogin == null) return NotFound();
            if (usuarioLogin.senha != usuario.senha) return Unauthorized();
            return Ok();
        }

        private Usuario[] CarregarUsuarios()
        {
            string stringUsuarios = File.ReadAllText(CAMINHO_ARQUIVO);
            if (string.IsNullOrWhiteSpace(stringUsuarios)) return new Usuario[0];

            Usuario[] usuarios = JsonConvert.DeserializeObject<Usuario[]>(stringUsuarios);
            return usuarios;
        }

        private static void Salvar(IEnumerable<Usuario> listaUsuarios)
        {
            string stringUsuarios = JsonConvert.SerializeObject(listaUsuarios);
            File.WriteAllText(CAMINHO_ARQUIVO, stringUsuarios);
        }
    }
}