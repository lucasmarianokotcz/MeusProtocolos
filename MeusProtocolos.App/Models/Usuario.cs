using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace MeusProtocolos.App.Models
{
    public class Usuario
    {
        [Key]
        [Required(ErrorMessage = "Digite seu login.")]
        [StringLength(20, ErrorMessage = "Mínimo de {2} e máximo de {1} caracteres.", MinimumLength = 3)]
        public string Login { get; set; }

        [Required(ErrorMessage = "Digite sua senha.")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Mínimo de {2} e máximo de {1} caracteres.", MinimumLength = 6)]
        public string Senha { get; set; }

        [HiddenInput]
        [NotMapped]
        public string ReturnUrl { get; set; }

        public ICollection<Protocolo> Protocolos { get; set; }
    }
}
