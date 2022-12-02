using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace SistemaRH.Model
{
    public class Departamento
    {
        [JsonProperty("Departamento")]
        public string Nome { get; set; }
        public string MesVigencia { get; set; }
        public int AnoVigencia { get; set; }
        public double TotalPagar => Funcionarios.Sum(x => x.TotalReceber);
        public double TotalDescontos => Funcionarios.Sum(x => x.HorasDebito * x.ValorHora);
        public double TotalExtras => Funcionarios.Sum(x => x.HorasExtras * x.ValorHora);
        public List<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();
    }
}
