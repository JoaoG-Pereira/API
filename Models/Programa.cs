using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Programa
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string alimento { get; set; }
        public TimeSpan tempo { get; set; }
        public int potencia { get; set; }
        public string instrucoes { get; set; }
        public bool customizado { get; set; }
    }
}