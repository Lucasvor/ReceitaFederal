using System;
using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
namespace ReceitaFederal.Model
{
    public class Simples
    {
        [Key]
        [Index(0)]
        public string cnpj_base { get; set; }
        [Index(1)]
        public string opcao_simples { get; set; }
        [Index(2)]
        public DateTime? data_entrada_simples { get; set; }
        [Index(3)]
        public DateTime? data_exclusao_simples { get; set; }
        [Index(4)]
        public string opcao_mei { get; set; }
        [Index(5)]
        public DateTime? data_entrada_mei { get; set; }
        [Index(6)]
        public DateTime? data_exclusao_mei { get; set; }
    }
    public class SimplesMap: ClassMap<Simples>
    {
        public SimplesMap()
        {
            Map(m => m.cnpj_base);
            Map(m => m.opcao_simples);
            Map(m => m.data_entrada_simples);
            Map(m => m.data_exclusao_simples);
            Map(m => m.opcao_mei);
            Map(m => m.data_entrada_mei);
            Map(m => m.data_exclusao_mei);

        }
    }
}
