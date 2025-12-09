using AgendamentoApp.Models;
using AgendamentoApp.ViewModel;

namespace AgendamentoApp.Extensions
{
    public static class AgendamentoExtensions
    {
        public static Agendamento ToEntity(this AgendamentoViewModel agendamentoVM)
        {
            return new Agendamento()
            {
                Id = agendamentoVM.Id,
                Agendado = agendamentoVM.Agendado!.Value,
                Entrada = agendamentoVM.Entrada,
                HorarioAtendimento = agendamentoVM.HorarioAtendimento,
                Nome = agendamentoVM.Nome,
                TipoDocumento = agendamentoVM.TipoDocumento!.Value,
                NumeroDocumento = agendamentoVM.NumeroDocumento,
                Telefone = agendamentoVM.Telefone,
                MunicipioId = agendamentoVM.MunicipioId,
                NomeRepresentanteMunicipio = agendamentoVM.NomeRepresentanteMunicipio,
                Destino = agendamentoVM.Destino,
                AutorizadoPor = agendamentoVM.AutorizadoPor,
                InformadoA = agendamentoVM.InformadoA,
                PlacaVeiculo = agendamentoVM.PlacaVeiculo
            };
        }
        public static AgendamentoViewModel ToViewModel(this Agendamento agendamento)
        {
            return new AgendamentoViewModel()
            {
                Id = agendamento.Id,
                Situacao = agendamento.Situacao.ToString(),
                Agendado = agendamento.Agendado,
                Entrada = agendamento.Entrada,
                HorarioAtendimento = agendamento.HorarioAtendimento,
                Nome = agendamento.Nome,
                TipoDocumento = agendamento.TipoDocumento,
                NumeroDocumento = agendamento.NumeroDocumento,
                Telefone = agendamento.Telefone,
                MunicipioId = agendamento.MunicipioId,
                Municipio = agendamento.Municipio.Nome,
                NomeRepresentanteMunicipio = agendamento.NomeRepresentanteMunicipio,
                Destino = agendamento.Destino,
                AutorizadoPor = agendamento.AutorizadoPor,
                InformadoA = agendamento.InformadoA,
                PlacaVeiculo = agendamento.PlacaVeiculo
            };
        }
    }
}
