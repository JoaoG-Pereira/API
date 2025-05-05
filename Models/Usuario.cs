using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Usuario
    {
        public string nome;
        public string senha;

        public Usuario(string nome, string senha)
        {
            this.nome = nome;
            this.senha = senha;
        }
    }
}