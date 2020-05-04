using Docs.Models.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Numerics;
using System.Text;

namespace Docs.Models
{
    public enum FileType
    {
        _ERROR,
        PDF,
        DOC,
        XLS,
        DOCX,
        XLSX
    }

    [Table("document")]
    public class DocumentModel
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "O Código é obrigatório!")]
        [Range(1, int.MaxValue, ErrorMessage = "O Código é obrigatório!")]
        public int CODE { get; set; } 
        [Required(ErrorMessage = "O Título é obrigatório!")]
        [StringLength(30, ErrorMessage = "O Título deve possuir entre 3 e 30 caracteres!", MinimumLength = 3)]
        public string TITLE { get; set; }

        [Required(ErrorMessage = "O Processo é obrigatório!")]
        [StringLength(30, ErrorMessage = "O Processo deve possuir entre 3 e 30 caracteres!", MinimumLength = 3)]
        public string PROCESS { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CATEGORY_ID { get; set; }

        [Required]
        [RequiredEnum(1,5)]
        public FileType FILE_TYPE { get; set; }
        [Required]
        public string FILE_NAME { get; set; }

        [Required]
        public int FILE_ID { get; set; }
        
        [NotMapped]
        public FileModel FILE { get; set; }

    }
}
