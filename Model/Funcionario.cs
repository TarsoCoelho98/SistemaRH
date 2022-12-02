using Newtonsoft.Json;

namespace SistemaRH.Model
{
    public class Funcionario
    {
        public string Nome { get; set; } 
        public int Codigo { get; set; } 
        public double TotalReceber { get; set; }
        public double HorasExtras { get; set; } 
        public double HorasDebito { get; set; } 
        public int DiasFalta { get; set; } 
        public int DiasExtras { get; set; } 
        public int DiasTrabalhados { get; set; } 
        [JsonIgnore]
        public double ValorHora { get; set; } 
    }
}
