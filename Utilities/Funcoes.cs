using System;

namespace SistemaRH.Utilities
{
    static class Funcoes
    {
        public static int GetQntDiasUteisMes(DateTime data)
        {
            int totalDias = DateTime.DaysInMonth(data.Year, data.Month);
            int diasUteis = 0;

            for (int i = 1; i <= totalDias; i++)
            {
                DateTime dia = new DateTime(data.Year, data.Month, i);
                if (dia.DayOfWeek == DayOfWeek.Sunday || dia.DayOfWeek == DayOfWeek.Saturday)
                    continue;
                diasUteis++;
            }

            return diasUteis;
        }

        public static string GetNomeMes(int mes)
        {
            string nome = string.Empty;

            switch (mes)
            {
                case 1:
                    nome = "Janeiro";
                    break;
                case 2:
                    nome = "Fevereiro";
                    break;
                case 3:
                    nome = "Março";
                    break;
                case 4:
                    nome = "Abril";
                    break;
                case 5:
                    nome = "Maio";
                    break;
                case 6:
                    nome = "Junho";
                    break;
                case 7:
                    nome = "Julho";
                    break;
                case 8:
                    nome = "Agosto";
                    break;
                case 9:
                    nome = "Setembro";
                    break;
                case 10:
                    nome = "Outubro";
                    break;
                case 11:
                    nome = "Novembro";
                    break;
                case 12:
                    nome = "Dezembro";
                    break;
            }

            return nome;
        }
    }
}
