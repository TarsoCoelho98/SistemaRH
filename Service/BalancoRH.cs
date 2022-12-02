using Newtonsoft.Json;
using SistemaRH.Model;
using SistemaRH.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaRH.Service
{
    internal class BalancoRH
    {
        const string caminhoInput = @"C:\TesteAuvoInput\";
        const string caminhoOutput = @"C:\TesteAuvoOutput\";
        List<string> arquivosLidos = new List<string>();


        public RegistroPonto GetRegistroPonto(string[] csvValores)
        {
            var registro = new RegistroPonto();
            registro.Codigo = Convert.ToInt32(csvValores[0]);
            registro.Nome = csvValores[1];
            string valor = csvValores[2].Replace("R$", "").Replace(" ", "").Replace(",", ".");
            registro.ValorHora = Convert.ToDouble(valor, CultureInfo.InvariantCulture);
            registro.Data = Convert.ToDateTime(csvValores[3]).Date;
            registro.Entrada = registro.Data.AddHours(Convert.ToDateTime(csvValores[4]).Hour);
            registro.Saida = registro.Data.AddHours(Convert.ToDateTime(csvValores[5]).Hour);
            var horarioSaida = Convert.ToDateTime(csvValores[6].Trim().Split("-")[0]);
            var horarioRetorno = Convert.ToDateTime(csvValores[6].Trim().Split("-")[1]);
            registro.Almoco = horarioRetorno - horarioSaida;
            return registro;
        }

        public async Task<List<RegistroPonto>> LerArquivo(string caminhoArquivo)
        {
            var registrosPonto = new List<RegistroPonto>();

            using (var reader = new StreamReader(caminhoArquivo, Encoding.UTF8))
            {
                bool pularLinha = true;

                while (!reader.EndOfStream)
                {
                    string line = string.Empty;
                    line = await reader.ReadLineAsync(); 

                    if (pularLinha)
                    {
                        pularLinha = false;
                        continue;
                    }

                    string[] valores = line.Split(';');
                    registrosPonto.Add(GetRegistroPonto(valores));
                }

                arquivosLidos.Add(Path.GetFileNameWithoutExtension(caminhoArquivo));
            }

            return registrosPonto;
        }

        public async void GerarBalancoMes(int mes)
        {
            string nomeMes = Funcoes.GetNomeMes(mes);
            Console.WriteLine("3. Iniciando Processamento ...");

            if (!Directory.Exists(caminhoInput))
            {
                Console.WriteLine("4. Diretório especificado não existe.");
                Thread.Sleep(2000);
                Environment.Exit(0);
                return;
            }

            string[] arquivos = Directory.GetFiles(caminhoInput);

            if (!arquivos.Any(x => x.ToLower().Contains(nomeMes.ToLower().Substring(0, 3))))
            {
                Console.WriteLine("4. Não há arquivos para este período.");
                Thread.Sleep(2000);
                Environment.Exit(0);
                return;
            }

            if (!arquivos.Any(x => !arquivosLidos.Contains(Path.GetFileNameWithoutExtension(x))))
            {
                Console.WriteLine("4. Não há novos arquivos para processar...");
                return;
            }

            var departamentos = await GetLstDepartamento(arquivos);
            string output = JsonConvert.SerializeObject(departamentos, Formatting.Indented);

            if (Directory.Exists(caminhoOutput))
            {
                File.WriteAllText(caminhoOutput + "Balanco_" + nomeMes + ".json", output);
            }
        }

        public async Task<List<Departamento>> GetLstDepartamento(string[] arquivos)
        {
            var departamentos = new List<Departamento>();
            string mesVigencia = string.Empty;

            foreach (string arquivo in arquivos)
            {
                Departamento dep = new Departamento();
                FileInfo info = new FileInfo(arquivo);
                string nomeArquivo = Path.GetFileNameWithoutExtension(info.FullName);
                Console.WriteLine("- Arquivo: " + nomeArquivo);
                string[] elements = nomeArquivo.Split('-');
                dep.Nome = elements[0];
                dep.MesVigencia = elements[1];

                if (string.IsNullOrEmpty(mesVigencia))
                    mesVigencia = dep.MesVigencia;

                dep.AnoVigencia = Convert.ToInt32(elements[2]);
                var registrosPonto = await LerArquivo(arquivo);
                List<int> ids = registrosPonto.Select(x => x.Codigo).Distinct().ToList();

                foreach (var id in ids)
                {
                    var equivalente = registrosPonto.First(x => x.Codigo == id);
                    var funcionario = new Funcionario();
                    funcionario.Nome = equivalente.Nome;
                    funcionario.Codigo = equivalente.Codigo;
                    funcionario.ValorHora = equivalente.ValorHora;
                    funcionario.DiasTrabalhados = registrosPonto.Where(x => x.Codigo == id).Count();
                    int qntDiasUteis = Funcoes.GetQntDiasUteisMes(registrosPonto.First().Data);
                    int diferencaDias = qntDiasUteis - funcionario.DiasTrabalhados;

                    // #info: Seção Dias
                    funcionario.DiasExtras = diferencaDias >= 0 ? 0 : Math.Abs(diferencaDias);
                    funcionario.DiasFalta = diferencaDias <= 0 ? 0 : diferencaDias;

                    // #info: Seção Horas
                    double qntHorasQuitar = qntDiasUteis * 8;
                    double qntHorasTrabalhadas = registrosPonto.Where(x => x.Codigo == id).Sum(x => (x.Saida.Hour - x.Entrada.Hour) - x.Almoco.TotalHours);
                    double diferencaHoras = qntHorasQuitar - qntHorasTrabalhadas;
                    double faturamentoHoras = qntHorasTrabalhadas * funcionario.ValorHora;
                    funcionario.HorasExtras = diferencaHoras >= 0 ? 0 : Math.Abs(diferencaHoras);
                    funcionario.HorasDebito = diferencaHoras <= 0 ? 0 : diferencaHoras;
                    funcionario.TotalReceber = faturamentoHoras;

                    // #info: Inserção em Departamentos.
                    if (departamentos.Any(x => x.Nome == dep.Nome))
                        departamentos.First(x => x.Nome == dep.Nome).Funcionarios.Add(funcionario);
                    else
                    {
                        dep.Funcionarios.Add(funcionario);
                        departamentos.Add(dep);
                    }
                }
            }
            return departamentos;
        }
    }
}
