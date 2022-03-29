using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FitFactoryCodeGeneratorV2
{
    public class SalesOrder 
    {
        [Key]
        [Required]
        [MaxLength(100)]
        public string? Id { get; set; } 

        [Required]
        public int No { get; set; } 


    }
}
