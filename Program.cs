using SistemaRH.Service;
using System;
using System.Threading;

namespace SistemaRH
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var balanco = new BalancoRH();
            int mesVigencia = 0;
            bool primeiroProcessamento = true;

            while (true)
            {
                if (primeiroProcessamento)
                {
                    Console.WriteLine("1. Sistema RH");
                    Console.Write("2. Digite o mês de vigência, ex. 1 (Janeiro), 2 (Fevereiro): ");
                    mesVigencia = int.Parse(Console.ReadLine());
                    balanco.GerarBalancoMes(mesVigencia);
                    Console.WriteLine("(!) Processo concluído, aguardando novos arquivos para processamento.");
                    primeiroProcessamento = false;
                }
                else
                    balanco.GerarBalancoMes(mesVigencia);

                Thread.Sleep(2000);
            }
        }
    }
}
