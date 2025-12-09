using AgendamentoApp.Enums;
using AgendamentoApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AgendamentoApp.ViewModel
{
    public class AgendamentoViewModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Situação")]
        public string? Situacao { get; set; }

        public TimeSpan? Entrada { get; set; }

        [Required(ErrorMessage = "Horário previsto obrigatório")]
        public TimeSpan? Agendado { get; set; }

        [Required(ErrorMessage = "Nome obrigatório")]
        [StringLength(50)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Nome do representante obrigatório")]
        [StringLength(50)]
        [Display(Name = "Nome do representando do município")]
        public string NomeRepresentanteMunicipio { get; set; }

        [Required(ErrorMessage = "Campo 'informado a' é obrigatório")]
        [StringLength(50)]
        [Display(Name = "Informado")]
        public string InformadoA { get; set; }

        [Required(ErrorMessage = "Destino obrigatório")]
        [StringLength(50)]
        public string Destino { get; set; }

        [Required(ErrorMessage = "Nome do autorizador obrigatório")]
        [StringLength(50)]
        [Display(Name = "Autorizado por")]
        public string AutorizadoPor { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Horario do atendimento")]
        public TimeSpan? HorarioAtendimento { get; set; }

        [Required(ErrorMessage = "Contato obrigatório")]
        [RegularExpression(@"^(\+55\s?)?\(?[1-9]{2}\)?[\s-]?\d{4,5}[\s-]?\d{4}$",
            ErrorMessage = "Contato invalido.")]
        [DataType(DataType.PhoneNumber)]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Tipo do documento obrigatório")]
        [Display(Name = "Tipo de documento")]
        public TipoDocumento? TipoDocumento { get; set; }

        [Required(ErrorMessage = "Número do documento obrigatório")]
        [Display(Name = "Número do documento")]
        public string NumeroDocumento { get; set; }

        [Required(ErrorMessage = "Placa do veículo obrigatório")]
        [RegularExpression(@"^([A-Za-z]{3}\-\d{4}|[A-Za-z]{3}\d[A-Za-z]\d{2})$",
            ErrorMessage = "Placa inválida. Formato aceito: ABC-1234 ou ABC1D23.")]
        [Display(Name = "Placa do veículo")]
        public string PlacaVeiculo { get; set; }

        [Required(ErrorMessage = "Município obrigatório")]
        [Display(Name = "Município")]
        public int MunicipioId { get; set; }


        public string? Municipio { get; set; }

        public IEnumerable<SelectListItem>? Municipios { get; set; }
    }
}
