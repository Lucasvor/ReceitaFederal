using CsvHelper.Configuration.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
namespace ReceitaFederal.Model
{
    public class Estabelecimento
    {
        public string cnpj { get; set; }
        [Key]
        [Index(0)]
        public string cnpj_base { get; set; }

        [Index(1)]
        public string cnpj_ordem { get; set; }

        [Index(2)]
        public string cnpj_dv { get; set; }

        [Index(3)]
        public string identificador_mat_fil { get; set; }

        [Index(4)]
        public string nome_fantasia { get; set; }

        [Index(5)]
        public string situacao_cadastral { get; set; }

        [Index(6)]
        public DateTime? data_situacao_cadastral { get; set; }

        [Index(7)]
        public string cod_motivo_sit_cadastral { get; set; }

        [Index(8)]
        public string cidade_exterior { get; set; }

        [Index(9)]
        public string cod_pais { get; set; }

        [Index(10)]
        public DateTime? data_inicio { get; set; }

        [Index(11)]
        public string cnae_pri { get; set; }

        [Index(12)]
        public string cnae_sec { get; set; }

        [Index(13)]
        public string log_tipo { get; set; }
        [Index(14)]
        public string log_nome { get; set; }
        [Index(15)]
        public string log_num { get; set; }
        [Index(16)]
        public string log_comp { get; set; }
        [Index(17)]
        public string log_bairro { get; set; }

        [Index(18)]
        public string log_cep { get; set; }


        [Index(19)]
        public string log_uf { get; set; }

        [Index(20)]
        public string log_cod_municipio { get; set; }

        [Index(21)]
        public string ddd_1 { get; set; }
        [Index(22)]
        public string tel_1 { get; set; }


        [Index(23)]
        public string ddd_2 { get; set; }

        [Index(24)]
        public string tel_2 { get; set; }

        [Index(25)]
        public string ddd_fax { get; set; }

        [Index(26)]
        public string tel_fax { get; set; }

        [Index(27)]
        public string email { get; set; }
        [Index(28)]
        public string situacao_especial { get; set; }

        [Index(29)]
        public DateTime? data_situacao_especial { get; set; }

    }
}
