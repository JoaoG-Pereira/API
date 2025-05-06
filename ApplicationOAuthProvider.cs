using API;
using API.Models;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
{
    public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
    {
        context.Validated();
        return Task.CompletedTask;
    }

    public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
    {
        context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

        Usuario usuario = new Usuario(context.UserName, context.Password);
        if (ValidarLogin(usuario))
        {
            var identity = new ClaimsIdentity("JWT");
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            context.Validated(identity);
        }
        else
        {
            context.SetError("invalid_grant", "Senha ou nome de usuário inválidos.");
        }

        return Task.CompletedTask;
    }

    private bool ValidarLogin(Usuario usuarioLogin)
    {
        //Leitura do JSON
        string CAMINHO_ARQUIVO = Configuracoes.CAMINHO_ARQUIVOS + @"\usuarios.json";
        string stringUsuarios = File.ReadAllText(CAMINHO_ARQUIVO);
        if (string.IsNullOrWhiteSpace(stringUsuarios)) return false;

        //Encontra o usuário
        Usuario[] listaUsuarios = JsonConvert.DeserializeObject<Usuario[]>(stringUsuarios);
        Usuario usuario = listaUsuarios.FirstOrDefault(u => u.nome == usuarioLogin.nome);

        //Validação
        if (usuario == null) return false;
        return usuarioLogin.senha == usuario.senha;
    }
}
