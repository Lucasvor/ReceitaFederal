using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceitaFederal.Model
{
    public class Municipio
    {
        [Key]
        [Index(0)]
        public string codigo { get; set; }
        [Index(1)]
        public string denominacao { get; set; }
    }
}
