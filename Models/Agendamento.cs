using AgendamentoApp.Enums;
using AgendamentoApp.Services;
using System.ComponentModel.DataAnnotations;

namespace AgendamentoApp.Models
{
    public class Agendamento
    {
        public int Id { get; set; }
        public SituacaoAgendamento Situacao { get; private set; }

        public TimeSpan? Entrada { get; set; }

        public TimeSpan Agendado { get; set; }

        public string Nome { get; set; }

        public string NomeRepresentanteMunicipio { get; set; }

        public int MunicipioId { get; set; }

        public string InformadoA { get; set; }

        public string Destino { get; set; }

        public string AutorizadoPor { get; set; }

        public TimeSpan? HorarioAtendimento { get; set; }

        public string Telefone { get; set; }

        public TipoDocumento TipoDocumento { get; set; }

        public string NumeroDocumento { get; set; }

        public string PlacaVeiculo { get; set; }

        public Municipio Municipio { get; set; }

        public void DefinirSituacao()
        {
            if (Entrada == null)
            { 
                Situacao = SituacaoAgendamento.Pendente;

                return;
            }
            Situacao = Entrada <= Agendado ? SituacaoAgendamento.NoHorario : SituacaoAgendamento.Atrasado;
        }
    }   
}
