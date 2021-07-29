using ReceitaFederal.Model;
using SqlBulkTools;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ReceitaFederal
{
    public class Db : DbContext
    {
        public Db(): base("")
        {
            
        }

        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Pais> Pais { get; set; }
        public DbSet<Natureza_Juridica> Natureza_Juridicas { get; set; }
        public DbSet<MotivoSitCadastral> MotivoSitCadastrals { get; set; }
        public DbSet<QualificacaoSocio> QualificacaoSocios { get; set; }
        public DbSet<Cnae> Cnaes { get; set; }
        public DbSet<Simples> Simples { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Municipio>().ToTable("tbl_municipio");
            modelBuilder.Entity<Pais>().ToTable("tbl_pais");
            modelBuilder.Entity<Natureza_Juridica>().ToTable("tbl_natureza_juridica");
            modelBuilder.Entity<MotivoSitCadastral>().ToTable("tbl_motivosituacaocadastral");
            modelBuilder.Entity<QualificacaoSocio>().ToTable("tbl_qualificacao_socio");
            modelBuilder.Entity<Cnae>().ToTable("tbl_cnae");
            modelBuilder.Entity<Simples>().ToTable("tbl_simples");
            modelBuilder.Entity<Socio>().ToTable("tbl_socios");
            modelBuilder.Entity<Empresa>().ToTable("tbl_empresas");
            modelBuilder.Entity<Estabelecimento>().ToTable("tbl_estabelecimentos");
        }

        public bool insertBulkSimples(List<Simples> list)
        {
            var bulk = new BulkOperations();
            using(TransactionScope trans = new TransactionScope())
            {
                using(SqlConnection con = new SqlConnection("Data Source=192.168.8.2;Initial Catalog=ReceitaFederal;User ID=sa;Password=wra5apAs&u"))
                {
                    bulk.Setup<Simples>()
                        .ForCollection(list)
                        .WithTable("tbl_simples")
                        .AddAllColumns()
                        .BulkInsert()
                        .Commit(con);
                }
                trans.Complete();
            }

            return true;
        }
        public  bool insertBulkSocios(List<Socio> list)
        {
            var bulk = new BulkOperations();
            using (TransactionScope trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (SqlConnection con = new SqlConnection("Data Source=192.168.8.2;Initial Catalog=ReceitaFederal;User ID=sa;Password=wra5apAs&u"))
                {
                    bulk.Setup<Socio>()
                        .ForCollection(list)
                        .WithTable("tbl_socios")
                        .AddAllColumns()
                        .BulkInsert()
                        .Commit(con);
                }
                trans.Complete();
            }

            return true;
        }

        public bool insertBulkEmpresa(List<Empresa> list)
        {
            var bulk = new BulkOperations();
            using (TransactionScope trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (SqlConnection con = new SqlConnection("Data Source=192.168.8.2;Initial Catalog=ReceitaFederal;User ID=sa;Password=wra5apAs&u"))
                {
                    bulk.Setup<Empresa>()
                        .ForCollection(list)
                        .WithTable("tbl_empresas")
                        .AddAllColumns()
                        .BulkInsert()
                        .Commit(con);
                }
                trans.Complete();
            }

            return true;
                
        }
        public bool insertBulkestabelecimentos(List<Estabelecimento> list)
        {
            var bulk = new BulkOperations();
                using (SqlConnection con = new SqlConnection("Data Source=192.168.8.2;Initial Catalog=ReceitaFederal;User ID=sa;Password=wra5apAs&u"))
                {
                    bulk.Setup<Estabelecimento>()

                        .ForCollection(list)
                        .WithTable("tbl_estabelecimentos")
                        .WithBulkCopySettings(new BulkCopySettings { BatchSize = 20000,SqlBulkCopyOptions = SqlBulkCopyOptions.TableLock})
                        .AddAllColumns()
                        .BulkInsert()
                        .Commit(con);
                }

            return true;

        }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(
        //       "Data Source=192.168.8.9;Initial Catalog=ONR;User ID=sa;Password=wra5apAs&u");
        //}
        public bool TableExists(string tablename)
        {
            bool exist = this.Database.SqlQuery<int?>($"use ReceitaFederal IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'{tablename}') BEGIN   select 1 END ELSE BEGIN   select 0 End").FirstOrDefault() > 0;
            return exist;

        }

        

    }
      
}


