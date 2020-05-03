using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Docs.Models
{
    [Table("file")]
    public class FileModel
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public byte[] FILE { get; set; }
    }
}
