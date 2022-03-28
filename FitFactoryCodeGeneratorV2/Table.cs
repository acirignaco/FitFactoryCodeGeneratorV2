using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitFactoryCodeGeneratorV2
{
    public class Table
    {
        public string? PropertyName { get; set; }

        public bool Required { get; set; }

        public int Length { get; set; }

        public string? Type { get; set; }

        public bool IsKey { get; set; }
    }


}
