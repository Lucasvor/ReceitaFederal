using CsvHelper.Configuration.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace ReceitaFederal.Model
{
    public class Empresa
    {
        [Key]
        [Index(0)]
        public string cnpj_base { get; set; }
        [Index(1)]
        public string razao_social { get; set; }
        [Index(2)]
        public int cod_natureza_juridica { get; set; }
        [Index(3)]
        public string cod_qualificacao_resposavel { get; set; }
        [Index(4)]
        public decimal capital_social { get; set; }
        [Index(5)]
        public string porte { get; set; }
        [Index(6)]
        public string ente_federativo_responsavel { get; set; }



    }
}
