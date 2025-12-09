using System.ComponentModel.DataAnnotations;

namespace AgendamentoApp.Models
{
    public class Municipio
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<Agendamento> Agendamentos { get; set; } = new();
    }
}
