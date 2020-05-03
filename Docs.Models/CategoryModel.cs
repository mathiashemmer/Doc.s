using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docs.Models
{
    [Table("category")]
    public class CategoryModel
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Nome é obrigatório!")]
        [StringLength(30, ErrorMessage = "O nome só pode possuir de 3 a 30 caracteres!", MinimumLength = 3)]
        public string NAME { get; set; }
    }
}
