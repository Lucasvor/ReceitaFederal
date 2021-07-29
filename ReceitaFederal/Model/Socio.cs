using CsvHelper.Configuration.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace ReceitaFederal.Model
{
    public class Socio
    {
        [Key]
        [Index(0)]
        public string cnpj_base { get; set; }
        [Index(1)]
        public int identificador_socio { get; set; }
        [Index(2)]
        public string nome { get; set; }
        [Index(3)]
        public string cpf_cnpj { get; set; }
        [Index(4)]
        public string cod_qualificacao_socio { get; set; }
        [Index(5)]
        public DateTime? data_entrada_sociedade { get; set; }
        [Index(6)]
        public string cod_pais { get; set; }
        [Index(7)]
        public string replegal_cpf { get; set; }
        [Index(8)]
        public string replegal_nome { get; set; }
        [Index(9)]
        public string replegal_cod_qualificacao { get; set; }
        [Index(10)]
        public int faixa_etaria { get; set; }

    }
}
