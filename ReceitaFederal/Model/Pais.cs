using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReceitaFederal.Model
{
    public class Pais
    {
        [Key]
        [Index(0)]
        public string codigo { get; set; }
        [Index(1)]
        public string denominacao { get; set; }
    }
}
