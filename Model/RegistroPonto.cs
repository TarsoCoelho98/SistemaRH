using System;

namespace SistemaRH.Model
{
    internal class RegistroPonto
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public double ValorHora { get; set; }
        public DateTime Data { get; set; }
        public DateTime Entrada { get; set; }
        public DateTime Saida { get; set; }
        public TimeSpan Almoco { get; set; }
    }
}
