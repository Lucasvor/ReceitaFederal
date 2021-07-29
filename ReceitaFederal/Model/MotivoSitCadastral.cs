﻿using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;

namespace ReceitaFederal.Model
{
    public class MotivoSitCadastral
    {
        [Key]
        [Index(0)]
        public string codigo { get; set; }
        [Index(1)]
        public string denominacao { get; set; }
    }
}
