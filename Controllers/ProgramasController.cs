using API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.IO;
using Newtonsoft.Json;
using System;

namespace API.Controllers
{
    [RoutePrefix("api/programas")]
    public class ProgramasController : ApiController
    {
        private const string CAMINHO_ARQUIVO = Configuracoes.CAMINHO_ARQUIVOS + @"\alimentos.json";

        private Programa[] CarregarProgramas()
        {
            string stringAlimentos = File.ReadAllText(CAMINHO_ARQUIVO);
            if (string.IsNullOrWhiteSpace(stringAlimentos)) return new Programa[0];

            Programa[] alimentos = JsonConvert.DeserializeObject<Programa[]>(stringAlimentos);
            return alimentos;
        }

        private static void Salvar(IEnumerable<Programa> listaAlimentos)
        {
            string stringAlimentos = JsonConvert.SerializeObject(listaAlimentos);
            File.WriteAllText(CAMINHO_ARQUIVO, stringAlimentos);
        }

        // GET: api/programas/{id}
        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult VerPrograma(int id)
        {
            var programa = CarregarProgramas().FirstOrDefault(p => p.id == id);
            if (programa == null) return NotFound();
            return Ok(programa);
        }

        [HttpGet]
        public IHttpActionResult ListarProgramas()
        {
            var lista = CarregarProgramas();
            return Ok(lista);
        }

        // POST: api/programas/criar
        [Route("criar")]
        [HttpPost]
        public IHttpActionResult CriarPrograma(Programa programa)
        {
            var listaAlimentos = CarregarProgramas().ToList();

            if (listaAlimentos.Count > 0)
                programa.id = listaAlimentos.Max(a => a.id) + 1;
            else
                programa.id = 1;

            listaAlimentos.Add(programa);
            Salvar(listaAlimentos);

            return Ok(programa);
        }

        // POST: api/programas/editar
        [Route("editar")]
        [HttpPost]
        public IHttpActionResult EditarPrograma(Programa programa)
        {
            var listaAlimentos = CarregarProgramas().ToList();
            var alimentoSelecionado = listaAlimentos.Where(a => a.id == programa.id).FirstOrDefault();

            if (alimentoSelecionado == null) return NotFound();
            if (alimentoSelecionado.customizado) return Unauthorized();

            //Deep copy
            alimentoSelecionado.id = programa.id;
            alimentoSelecionado.nome = programa.nome;
            alimentoSelecionado.alimento = programa.alimento;
            alimentoSelecionado.tempo = programa.tempo;
            alimentoSelecionado.potencia = programa.potencia;
            alimentoSelecionado.instrucoes = programa.instrucoes;

            Salvar(listaAlimentos);
            return Ok(programa);
        }

        // POST: api/programas/editar
        [Route("deletar/{id}")]
        [HttpDelete]
        public IHttpActionResult DeletarPrograma(int id)
        {
            var listaAlimentos = CarregarProgramas().ToList();
            Programa alimentoSelecionado = listaAlimentos.Where(a => a.id == id).FirstOrDefault();

            if (alimentoSelecionado == null) return NotFound();
            if (alimentoSelecionado.customizado) return Unauthorized();

            listaAlimentos.Remove(alimentoSelecionado);
            Salvar(listaAlimentos);

            return Ok();
        }
    }
}
