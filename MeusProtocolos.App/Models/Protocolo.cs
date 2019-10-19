using System;
using System.ComponentModel.DataAnnotations;

namespace MeusProtocolos.App.Models
{
    public class Protocolo
    {
        [Key]
        [Required]
        public int CodProtocolo { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "Informe o título do protocolo.")]
        [StringLength(30, ErrorMessage = "Máximo de {1} caracteres.")]
        public string Titulo { get; set; }

        [Display(Name = "Número")]
        [Required(ErrorMessage = "Informe o número do protocolo.")]
        [StringLength(30, ErrorMessage = "Máximo de {1} caracteres.")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "Informe o motivo de estar registrando este protocolo.")]
        [StringLength(30, ErrorMessage = "Máximo de {1} caracteres.")]
        public string Motivo { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Informe a descrição completa do protocolo.")]
        [StringLength(300, ErrorMessage = "Máximo de {1} caracteres.")]
        public string Descricao { get; set; }

        [Display(Name = "Data")]
        [Required(ErrorMessage = "Informe o dia do atendimento.")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [CustomDateRange]
        public DateTime Dia { get; set; }

        [Required(ErrorMessage = "Informe a hora do atendimento.")]
        [DataType(DataType.Time)]
        public DateTime Hora { get; set; }

        [StringLength(30, ErrorMessage = "Máximo de {1} caracteres.")]
        public string Atendente { get; set; }

        [Display(Name = "Outras informações")]
        [StringLength(300, ErrorMessage = "Máximo de {1} caracteres.")]
        public string OutrasInfo { get; set; }

        [Display(Name = "Está resolvido?")]
        public bool Resolvido { get; set; }

        public Usuario Usuario { get; set; }
        public string Login { get; set; }
    }

    public class CustomDateRangeAttribute : RangeAttribute
    {
        public CustomDateRangeAttribute() : base(typeof(DateTime), DateTime.Now.AddYears(-20).ToString(), DateTime.Now.ToString())
        { ErrorMessage = "A data do protocolo não pode ser maior do que a data atual."; }
    }
}
