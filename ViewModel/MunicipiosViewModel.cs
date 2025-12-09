using AgendamentoApp.Models;
using System.ComponentModel.DataAnnotations;

namespace AgendamentoApp.ViewModel
{
    public class MunicipiosViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nome do municipio obrigatório")]
        [StringLength(50)]
        public string Nome { get; set; }
    }
}
