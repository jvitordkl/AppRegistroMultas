using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRegistroMultas.Models
{
    public class Multa
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public decimal ValorMulta { get; set; }
        public int VeiculoId {  get; set; } 
    }
}
