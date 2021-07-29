using CsvHelper;
using CsvHelper.Configuration;
using ReceitaFederal.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReceitaFederal.Views
{
    /// <summary>
    /// Interaction logic for Importa.xaml
    /// Criar uma lista para exibir os estado de cada arquivo importado na listview
    /// </summary>
    public partial class Importa : UserControl
    {
        bool working;
        List<string> bad = new List<string>();
        List<Municipio> good = new List<Municipio>();
        ObservableCollection<ItemRow> files = new ObservableCollection<ItemRow>();
        bool isRecordBad;
        bool multipleFiles;

        public Importa()
        {
            InitializeComponent();
            foreach (var item in Directory.EnumerateFiles("Zip", "*.*").Where(x => !x.EndsWith(".zip")).Select(x => System.IO.Path.GetFileName(x)))
            {
                files.Add(new ItemRow(item, TiposProgresso.Waiting));
            }
            listView.ItemsSource = files;
        }
        public void ReloadFiles()
        {
            files.Clear();
            foreach (var item in Directory.EnumerateFiles("Zip", "*.*").Where(x => !x.EndsWith(".zip")).Select(x => System.IO.Path.GetFileName(x)))
            {
                files.Add(new ItemRow(item,TiposProgresso.Waiting));
            }
        }

        private async void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tablename;
            try
            {
                //configuração do CSV
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {

                    HasHeaderRecord = false,
                    Delimiter = ";",
                    BadDataFound =
                    context =>
                    {
                        isRecordBad = true;
                        bad.Add(context.Field);
                    }


                };

                if (listView.SelectedItem != null)
                {
                    var op = ((ItemRow)listView.SelectedItem).FileName;
                    if (op.Contains("MUNICCSV"))
                    {
                        tablename = "tbl_municipio";
                        using (var db = new Db())
                        {
                            // verifica a existencia da tabela no banco de dados
                            if (db.TableExists(tablename))
                            {
                                var index = files.IndexOf((ItemRow)listView.SelectedItem);
                                files[index].Type = TiposProgresso.InProgress;

                                using (var reader = new StreamReader(System.IO.Path.Combine("Zip", op), Encoding.Default))
                                using (var csv = new CsvReader(reader, config))
                                {
                                    var records = csv.GetRecords<Municipio>().ToList();
                                    var resposta = MessageBox.Show("Deseja realmente apagar os dados da tabela tbl_municipio?", "Inserção de dados", MessageBoxButton.YesNo);
                                    if (resposta == MessageBoxResult.Yes)
                                    {
                                        Mensagem("Excluindo dados da tabela");
                                        var objCtx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext;
                                        objCtx.ExecuteStoreCommand($"TRUNCATE TABLE {tablename}");
                                    }
                                    Mensagem($"Adicionando dados do CSV na tabela {tablename}");
                                    db.Municipios.AddRange(records);
                                    db.SaveChanges();

                                    files[index].Type = TiposProgresso.Finished;
                                }
                            }
                            else
                            {
                                throw new Exception("Tabela municipio não existe!");
                            }
                        }
                    }
                    else if (op.Contains("PAISCSV"))
                    {
                        tablename = "tbl_pais";
                        using (var db = new Db())
                        {
                            // verifica a existencia da tabela no banco de dados
                            if (db.TableExists(tablename))
                            {
                                var index = files.IndexOf((ItemRow)listView.SelectedItem);
                                files[index].Type = TiposProgresso.InProgress;

                                using (var reader = new StreamReader(System.IO.Path.Combine("Zip", op), Encoding.Default))
                                using (var csv = new CsvReader(reader, config))
                                {
                                    var records = csv.GetRecords<Pais>().ToList();
                                    var resposta = MessageBox.Show($"Deseja realmente apagar os dados da tabela {tablename}?", "Inserção de dados", MessageBoxButton.YesNo);
                                    if (resposta == MessageBoxResult.Yes)
                                    {
                                        Mensagem("Excluindo dados da tabela");
                                        var objCtx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext;
                                        objCtx.ExecuteStoreCommand($"TRUNCATE TABLE {tablename}");
                                    }
                                    Mensagem($"Adicionando dados do CSV na tabela {tablename}");
                                    db.Pais.AddRange(records);
                                    db.SaveChanges();
                                    files[index].Type = TiposProgresso.Finished;
                                }
                            }
                            else
                            {
                                throw new Exception("Tabela municipio não existe!");
                            }
                        }
                    }
                    else if (op.Contains("NATJUCSV"))
                    {
                        tablename = "tbl_natureza_juridica";
                        using (var db = new Db())
                        {
                            // verifica a existencia da tabela no banco de dados
                            if (db.TableExists(tablename))
                            {

                                var index = files.IndexOf((ItemRow)listView.SelectedItem);
                                files[index].Type = TiposProgresso.InProgress;

                                using (var reader = new StreamReader(System.IO.Path.Combine("Zip", op), Encoding.Default))
                                using (var csv = new CsvReader(reader, config))
                                {
                                    var records = csv.GetRecords<Natureza_Juridica>().ToList();
                                    var resposta = MessageBox.Show($"Deseja realmente apagar os dados da tabela {tablename}?", "Inserção de dados", MessageBoxButton.YesNo);
                                    if (resposta == MessageBoxResult.Yes)
                                    {
                                        Mensagem("Excluindo dados da tabela");
                                        var objCtx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext;
                                        objCtx.ExecuteStoreCommand($"TRUNCATE TABLE {tablename}");
                                    }
                                    Mensagem($"Adicionando dados do CSV na tabela {tablename}");
                                    db.Natureza_Juridicas.AddRange(records);
                                    db.SaveChanges();
                                    files[index].Type = TiposProgresso.Finished;
                                }
                            }
                            else
                            {
                                throw new Exception("Tabela municipio não existe!");
                            }
                        }
                    }
                    else if (op.Contains("MOTICSV"))
                    {
                        tablename = "tbl_motivosituacaocadastral";
                        using (var db = new Db())
                        {
                            // verifica a existencia da tabela no banco de dados
                            if (db.TableExists(tablename))
                            {
                                var index = files.IndexOf((ItemRow)listView.SelectedItem);
                                files[index].Type = TiposProgresso.InProgress;

                                using (var reader = new StreamReader(System.IO.Path.Combine("Zip", op), Encoding.Default))
                                using (var csv = new CsvReader(reader, config))
                                {
                                    var records = csv.GetRecords<MotivoSitCadastral>().ToList();
                                    var resposta = MessageBox.Show($"Deseja realmente apagar os dados da tabela {tablename}?", "Inserção de dados", MessageBoxButton.YesNo);
                                    if (resposta == MessageBoxResult.Yes)
                                    {
                                        Mensagem("Excluindo dados da tabela");
                                        var objCtx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext;
                                        objCtx.ExecuteStoreCommand($"TRUNCATE TABLE {tablename}");
                                    }
                                    Mensagem($"Adicionando dados do CSV na tabela {tablename}");
                                    db.MotivoSitCadastrals.AddRange(records);
                                    db.SaveChanges();
                                    files[index].Type = TiposProgresso.Finished;
                                }
                            }
                            else
                            {
                                throw new Exception("Tabela municipio não existe!");
                            }
                        }
                    }
                    else if (op.Contains("QUALSCSV"))
                    {
                        tablename = "tbl_qualificacao_socio";
                        using (var db = new Db())
                        {
                            // verifica a existencia da tabela no banco de dados
                            if (db.TableExists(tablename))
                            {
                                var index = files.IndexOf((ItemRow)listView.SelectedItem);
                                files[index].Type = TiposProgresso.InProgress;

                                using (var reader = new StreamReader(System.IO.Path.Combine("Zip", op), Encoding.Default))
                                using (var csv = new CsvReader(reader, config))
                                {
                                    var records = csv.GetRecords<QualificacaoSocio>().ToList();
                                    var resposta = MessageBox.Show($"Deseja realmente apagar os dados da tabela {tablename}?", "Inserção de dados", MessageBoxButton.YesNo);
                                    if (resposta == MessageBoxResult.Yes)
                                    {
                                        Mensagem("Excluindo dados da tabela");
                                        var objCtx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext;
                                        objCtx.ExecuteStoreCommand($"TRUNCATE TABLE {tablename}");
                                    }
                                    Mensagem($"Adicionando dados do CSV na tabela {tablename}");
                                    db.QualificacaoSocios.AddRange(records);
                                    db.SaveChanges();
                                    files[index].Type = TiposProgresso.Finished;
                                }
                            }
                            else
                            {
                                throw new Exception("Tabela municipio não existe!");
                            }
                        }
                    }
                    else if (op.Contains("CNAECSV"))
                    {
                        tablename = "tbl_cnae";
                        using (var db = new Db())
                        {
                            // verifica a existencia da tabela no banco de dados
                            if (db.TableExists(tablename))
                            {
                                var index = files.IndexOf((ItemRow)listView.SelectedItem);
                                files[index].Type = TiposProgresso.InProgress;

                                using (var reader = new StreamReader(System.IO.Path.Combine("Zip", op), Encoding.Default))
                                using (var csv = new CsvReader(reader, config))
                                {
                                    var records = csv.GetRecords<Cnae>().ToList();
                                    var resposta = MessageBox.Show($"Deseja realmente apagar os dados da tabela {tablename}?", "Inserção de dados", MessageBoxButton.YesNo);
                                    if (resposta == MessageBoxResult.Yes)
                                    {
                                        Mensagem("Excluindo dados da tabela");
                                        var objCtx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext;
                                        objCtx.ExecuteStoreCommand($"TRUNCATE TABLE {tablename}");
                                    }
                                    Mensagem($"Adicionando dados do CSV na tabela {tablename}");
                                    db.Cnaes.AddRange(records);
                                    db.SaveChanges();
                                    files[index].Type = TiposProgresso.Finished;
                                }
                            }
                            else
                            {
                                throw new Exception("Tabela municipio não existe!");
                            }
                        }
                    }
                    else if (op.Contains("SIMPLES.CSV"))
                    {
                        tablename = "tbl_simples";
                        int contador = 0;
                        var list = new List<Simples>();
                        using (var db = new Db())
                        {
                            // verifica a existencia da tabela no banco de dados
                            if (db.TableExists(tablename))
                            {
                                using (var reader = new StreamReader(System.IO.Path.Combine("Zip", op), Encoding.Default))
                                using (var csv = new CsvReader(reader, config))
                                {
                                    csv.Context.TypeConverterCache.AddConverter<DateTime?>(new DateConverter("yyyyMMdd"));
                                    var dateConverter = new DateConverter("yyyyMMdd");
                                    var resposta = MessageBox.Show($"Deseja realmente apagar os dados da tabela {tablename}?", "Inserção de dados", MessageBoxButton.YesNo);
                                    if (resposta == MessageBoxResult.Yes)
                                    {
                                        Mensagem("Excluindo dados da tabela");
                                        var objCtx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext;
                                        objCtx.ExecuteStoreCommand($"TRUNCATE TABLE {tablename}");
                                    }
                                    //db.Configuration.AutoDetectChangesEnabled = false;
                                    while (await csv.ReadAsync())
                                    {
                                        list.Add(new Simples() { cnpj_base = csv.GetField(0), opcao_simples = csv.GetField(1), data_entrada_simples = dateConverter.ConvertFromString(csv.GetField(2)), data_exclusao_simples = dateConverter.ConvertFromString(csv.GetField(3)), opcao_mei = csv.GetField(4), data_entrada_mei = dateConverter.ConvertFromString(csv.GetField(5)), data_exclusao_mei = dateConverter.ConvertFromString(csv.GetField(5)) });

                                        //db.Simples.Add(new Simples() { cnpj_base = csv.GetField(0), opcao_simples = csv.GetField(1), data_entrada_simples = dateConverter.ConvertFromString(csv.GetField(2)), data_exclusao_simples = dateConverter.ConvertFromString(csv.GetField(3)), opcao_mei = csv.GetField(4), data_entrada_mei = dateConverter.ConvertFromString(csv.GetField(5)), data_exclusao_mei = dateConverter.ConvertFromString(csv.GetField(5)) });
                                        //await Task.Delay(5);
                                        await MensagemAsync($"Lendo e gravando no banco {contador++} linhas do CSV");
                                        if (contador % 1000000 == 0)
                                        {
                                            db.insertBulkSimples(list);
                                            list.Clear();
                                        }
                                        //if (contador % 10000 == 0)
                                        //{
                                        //    db.Simples.AddRange(list);

                                        //    await db.SaveChangesAsync();
                                        //    list.Clear();
                                        //    GC.Collect();
                                        //}
                                    }
                                    db.insertBulkSimples(list);
                                    Mensagem($"Processo concluído, importado {contador} linhas");
                                    //var records = csv.GetRecords<Simples>().ToList();
                                    //Mensagem($"Adicionando dados do CSV na tabela {tablename}");
                                    //db.Simples.AddRange(records);
                                    //db.SaveChanges();
                                }
                            }
                            else
                            {
                                throw new Exception("Tabela municipio não existe!");
                            }
                        }
                    }else if (op.Contains("SOCIOCSV"))
                    {
                        tablename = "tbl_socios";
                        var ops = files.Where(x => x.FileName.Contains("SOCIOCSV"));
                        if(ops.Count() > 1)
                        {
                            var resposta = MessageBox.Show("Deseja importar todos os arquivos SOCIOCSV?", "Importação Socio", MessageBoxButton.YesNo);
                            if (resposta == MessageBoxResult.Yes)
                            {
                                multipleFiles = true;
                            }
                            else
                            {
                                multipleFiles = false;
                            }
                        }
                        int contador = 0;
                        var list = new List<Socio>();
                        using(var db = new Db())
                        {
                            if (db.TableExists(tablename))
                            {
                                var resposta = MessageBox.Show($"Deseja realmente apagar os dados da tabela {tablename}?", "Inserção de dados", MessageBoxButton.YesNo);
                                if (resposta == MessageBoxResult.Yes)
                                {
                                    Mensagem("Excluindo dados da tabela");
                                    var objCtx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext;
                                    objCtx.ExecuteStoreCommand($"TRUNCATE TABLE {tablename}");
                                }

                                if (multipleFiles)
                                {
                                    foreach(var item in ops)
                                    {
                                        contador = 0;
                                        list.Clear();
                                        var itemRow = files.First(x => x.FileName == item.FileName);
                                        files[files.IndexOf(itemRow)].Type = TiposProgresso.InProgress;
                                        

                                        using (var reader = new StreamReader(System.IO.Path.Combine("Zip", item.FileName), Encoding.Default))
                                        using (var csv = new CsvReader(reader, config))
                                        {
                                            var dateConverter = new DateConverter("yyyyMMdd");

                                            while (await csv.ReadAsync())
                                            {
                                                list.Add(new Socio() { cnpj_base = csv.GetField(0), identificador_socio = Convert.ToInt32(csv.GetField(1)), nome = csv.GetField(2), cpf_cnpj = csv.GetField(3), cod_qualificacao_socio = csv.GetField(4), data_entrada_sociedade = dateConverter.ConvertFromString(csv.GetField(5)), cod_pais = csv.GetField(6), replegal_cpf = csv.GetField(7), replegal_nome = csv.GetField(8), replegal_cod_qualificacao = csv.GetField(9), faixa_etaria = Convert.ToInt32(csv.GetField(10)) });

                                                await MensagemAsync($"Arquivo: {item.FileName}, {contador++} linhas do CSV");
                                                if (contador % 1000000 == 0)
                                                {
                                                    await MensagemAsync($"Importando 1000000 linhas para o banco!");
                                                    await Task.Run(() => db.insertBulkSocios(list));
                                                    list.Clear();
                                                }
                                            }
                                            await Task.Run(() => db.insertBulkSocios(list));
                                            files[files.IndexOf(itemRow)].Type = TiposProgresso.Finished;
                                            Mensagem($"Processo concluído, importado {contador} linhas");
                                        }
                                    }
                                }
                                else
                                {
                                    var index = files.IndexOf((ItemRow)listView.SelectedItem);
                                    files[index].Type = TiposProgresso.InProgress;
                                    

                                    using (var reader = new StreamReader(System.IO.Path.Combine("Zip", op), Encoding.Default))
                                    using (var csv = new CsvReader(reader, config))
                                    {
                                        //csv.Context.TypeConverterCache.AddConverter<DateTime?>(new DateConverter("yyyyMMdd"));
                                        var dateConverter = new DateConverter("yyyyMMdd");

                                        while (await csv.ReadAsync())
                                        {
                                            list.Add(new Socio() { cnpj_base = csv.GetField(0), identificador_socio = Convert.ToInt32(csv.GetField(1)), nome = csv.GetField(2), cpf_cnpj = csv.GetField(3), cod_qualificacao_socio = csv.GetField(4), data_entrada_sociedade = dateConverter.ConvertFromString(csv.GetField(5)), cod_pais = csv.GetField(6), replegal_cpf = csv.GetField(7), replegal_nome = csv.GetField(8), replegal_cod_qualificacao = csv.GetField(9), faixa_etaria = Convert.ToInt32(csv.GetField(10)) });

                                            await MensagemAsync($"Lendo e gravando no banco {contador++} linhas do CSV");
                                            if (contador % 1000000 == 0)
                                            {
                                                await MensagemAsync($"Importando 1000000 linhas para o banco!");
                                                await Task.Run(() => db.insertBulkSocios(list));
                                                list.Clear();
                                            }
                                        }
                                        await Task.Run(() => db.insertBulkSocios(list));
                                        files[index].Type = TiposProgresso.Finished;
                                        Mensagem($"Processo concluído, importado {contador} linhas");
                                    }
                                }
                                
                            }
                            else
                            {
                                throw new Exception("Tabela municipio não existe!");
                            }
                        }
                    }else if (op.Contains("EMPRECSV"))
                    {

                        tablename = "tbl_empresas";
                        var ops = files.Where(x => x.FileName.Contains("EMPRECSV"));
                        if (ops.Count() > 1)
                        {
                            var resposta = MessageBox.Show("Deseja importar todos os arquivos da EMPRESA?", "Importação empresa", MessageBoxButton.YesNo);
                            if(resposta == MessageBoxResult.Yes)
                            {
                                multipleFiles = true;
                            }
                            else
                            {
                                multipleFiles = false;
                            }
        
                        }
                        
                        int contador = 0;
                        var list = new List<Empresa>();
                        using (var db = new Db())
                        {
                            if (db.TableExists(tablename))
                            {
                                var resposta = MessageBox.Show($"Deseja realmente apagar os dados da tabela {tablename}?", "Inserção de dados", MessageBoxButton.YesNo);
                                if (resposta == MessageBoxResult.Yes)
                                {
                                    Mensagem("Excluindo dados da tabela");
                                    var objCtx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext;
                                    objCtx.ExecuteStoreCommand($"TRUNCATE TABLE {tablename}");
                                }

                                if (multipleFiles)
                                {

                                    foreach (var item in ops)
                                    {
                                        list.Clear();
                                        contador = 0;
                                        var itemRow = files.First(x => x.FileName == item.FileName);
                                        //files[files.IndexOf(itemRow)].Type = TiposProgresso.InProgress;
                                        itemRow.Type = TiposProgresso.InProgress;

                                        //itemRow.Type = TiposProgresso.InProgress;
                                        using (var reader = new StreamReader(System.IO.Path.Combine("Zip", item.FileName), Encoding.Default))
                                        using(var csv = new CsvReader(reader, config))
                                        {
                                            var dateConverter = new DateConverter("yyyyMMdd");
                                            
                                            while (await csv.ReadAsync())
                                            {
                                                list.Add(new Empresa()
                                                {
                                                    cnpj_base = csv.GetField(0),
                                                    razao_social = csv.GetField(1),
                                                    cod_natureza_juridica = Convert.ToInt32(csv.GetField(2)),
                                                    cod_qualificacao_resposavel = csv.GetField(3),
                                                    capital_social = Convert.ToDecimal(csv.GetField(4)),
                                                    porte = csv.GetField(5),
                                                    ente_federativo_responsavel = csv.GetField(6)

                                                });

                                                await MensagemAsync($"Arquivo: {item.FileName}, {contador++} linhas do CSV");
                                                if (contador % 1000000 == 0)
                                                {
                                                    await MensagemAsync($"Importando 1000000 linhas para o banco!");
                                                    await Task.Run(() => db.insertBulkEmpresa(list));
                                                    list.Clear();
                                                }
                                            }
                                            await Task.Run(() => db.insertBulkEmpresa(list));
                                            //files[files.IndexOf(itemRow)].Type = TiposProgresso.Finished;
                                            itemRow.Type = TiposProgresso.Finished;
                                            Mensagem($"Processo concluído, importado {contador} linhas");
                                        }
                                    }
                                }
                                else
                                {
                                    var index = files.IndexOf((ItemRow)listView.SelectedItem);
                                    files[index].Type = TiposProgresso.InProgress;

                                    using (var reader = new StreamReader(System.IO.Path.Combine("Zip", op), Encoding.Default))
                                    using (var csv = new CsvReader(reader, config))
                                    {
                                        //csv.Context.TypeConverterCache.AddConverter<DateTime?>(new DateConverter("yyyyMMdd"));
                                        var dateConverter = new DateConverter("yyyyMMdd");
                                        


                                        while (await csv.ReadAsync())
                                        {
                                            list.Add(new Empresa()
                                            {
                                                cnpj_base = csv.GetField(0),
                                                razao_social = csv.GetField(1),
                                                cod_natureza_juridica = Convert.ToInt32(csv.GetField(2)),
                                                cod_qualificacao_resposavel = csv.GetField(3),
                                                capital_social = Convert.ToDecimal(csv.GetField(4)),
                                                porte = csv.GetField(5),
                                                ente_federativo_responsavel = csv.GetField(6)

                                            });

                                            await MensagemAsync($"Lendo e gravando no banco {contador++} linhas do CSV");
                                            if (contador % 1000000 == 0)
                                            {
                                                await MensagemAsync($"Importando 1000000 linhas para o banco!");
                                                await Task.Run(() => db.insertBulkEmpresa(list));
                                                list.Clear();
                                            }
                                        }
                                        await Task.Run(() => db.insertBulkEmpresa(list));
                                        files[index].Type = TiposProgresso.Finished;
                                        Mensagem($"Processo concluído, importado {contador} linhas");
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception("Tabela municipio não existe!");
                            }
                        }
                    }else if (op.Contains("ESTABELE"))
                    {
                        tablename = "tbl_estabelecimentos";
                        var ops = files.Where(x => x.FileName.Contains("ESTABELE"));
                        if (ops.Count() > 1)
                        {
                            var resposta = MessageBox.Show("Deseja importar todos os arquivos da Estabelecimentos?", "Importação Estabelecimentos", MessageBoxButton.YesNo);
                            if (resposta == MessageBoxResult.Yes)
                            {
                                multipleFiles = true;
                            }
                            else
                            {
                                multipleFiles = false;
                            }

                        }

                        int contador = 0;
                        List<Estabelecimento> lista = new List<Estabelecimento>();
                        List<List<Estabelecimento>> ListaLista = new List<List<Estabelecimento>>();
                        //System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
                        using (var db = new Db())
                        {
                            if (db.TableExists(tablename))
                            {
                                var resposta = MessageBox.Show($"Deseja realmente apagar os dados da tabela {tablename}?", "Inserção de dados", MessageBoxButton.YesNo);
                                if (resposta == MessageBoxResult.Yes)
                                {
                                    Mensagem("Excluindo dados da tabela");
                                    var objCtx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext;
                                    objCtx.ExecuteStoreCommand($"TRUNCATE TABLE {tablename}");
                                }

                                if (multipleFiles)
                                {
                                    foreach (var item in ops)
                                    {
                                        lista.Clear();
                                        contador = 0;
                                        var itemRow = files.First(x => x.FileName == item.FileName);
                                        //files[files.IndexOf(itemRow)].Type = TiposProgresso.InProgress;
                                        itemRow.Type = TiposProgresso.InProgress;

                                        //itemRow.Type = TiposProgresso.InProgress;
                                        using (var reader = new StreamReader(System.IO.Path.Combine("Zip", item.FileName), Encoding.Default))
                                        using (var csv = new CsvReader(reader, config))
                                        {
                                            var dateConverter = new DateConverter("yyyyMMdd");

                                            while (await csv.ReadAsync())
                                            {
                                                
                                                    lista.Add(new Estabelecimento()
                                                    {
                                                        cnpj_base = csv.GetField(0),
                                                        cnpj_ordem = csv.GetField(1),
                                                        cnpj_dv = csv.GetField(2),
                                                        cnpj = csv.GetField(0) + csv.GetField(1) + csv.GetField(2),
                                                        identificador_mat_fil = csv.GetField(3),
                                                        nome_fantasia = csv.GetField(4),
                                                        situacao_cadastral = csv.GetField(5),
                                                        data_situacao_cadastral = dateConverter.ConvertFromString(csv.GetField(6)),
                                                        cod_motivo_sit_cadastral = csv.GetField(7),
                                                        cidade_exterior = csv.GetField(8),
                                                        cod_pais = csv.GetField(9),
                                                        data_inicio = dateConverter.ConvertFromString(csv.GetField(10)),
                                                        cnae_pri = csv.GetField(11),
                                                        cnae_sec = csv.GetField(12),
                                                        log_tipo = csv.GetField(13),
                                                        log_nome = csv.GetField(14).Trim(),
                                                        log_num = csv.GetField(15),
                                                        log_comp = csv.GetField(16).Trim(),
                                                        log_bairro = csv.GetField(17),
                                                        log_cep = csv.GetField(18),
                                                        log_uf = csv.GetField(19),
                                                        log_cod_municipio = csv.GetField(20),
                                                        ddd_1 = csv.GetField(21),
                                                        tel_1 = csv.GetField(22),
                                                        ddd_2 = csv.GetField(23),
                                                        tel_2 = csv.GetField(24),
                                                        ddd_fax = csv.GetField(25),
                                                        tel_fax = csv.GetField(26),
                                                        email = csv.GetField(27),
                                                        situacao_especial = csv.GetField(28),
                                                        data_situacao_especial = dateConverter.ConvertFromString(csv.GetField(29))


                                                    });
                                                

                                                await MensagemAsync($"Arquivo: {item.FileName}, {contador++} linhas do CSV");
                                                if (contador % 500000 == 0)
                                                {
                                                    //timer.Start();
                                                    for(int k = 0; k < 4; k++)
                                                    {
                                                        var dr = lista.Skip(k * 125000).Take(125000).ToList();
                                                        ListaLista.Add(lista.Skip(k * 125000).Take(125000).ToList());
                                                    }
                                                    lista.Clear();
                                                    await Task.Run(() => { 
                                                    Parallel.ForEach(ListaLista, l =>
                                                    {
                                                        db.insertBulkestabelecimentos(l);
                                                    });
                                                    });
                                                    ListaLista.Clear();
                                                    //timer.Stop();
                                                    //TimeSpan timeTaken = timer.Elapsed;
                                                    //timer.Reset();
                                                    //MessageBox.Show("Time taken: " + timeTaken.ToString(@"m\:ss\.fff"));
                                                    await MensagemAsync($"Importando 500000 linhas para o banco!");
                                                    
                                                    //await Task.Run(() => db.insertBulkestabelecimentos(lista));
                                                }
                                            }
                                            await Task.Run(() => db.insertBulkestabelecimentos(lista));
                                            //files[files.IndexOf(itemRow)].Type = TiposProgresso.Finished;
                                            itemRow.Type = TiposProgresso.Finished;
                                            Mensagem($"Processo concluído, importado {contador} linhas");
                                        }
                                    }
                                }
                                else
                                {
                                    var index = files.IndexOf((ItemRow)listView.SelectedItem);
                                    files[index].Type = TiposProgresso.InProgress;

                                    using (var reader = new StreamReader(System.IO.Path.Combine("Zip", op), Encoding.Default))
                                    using (var csv = new CsvReader(reader, config))
                                    {
                                        //csv.Context.TypeConverterCache.AddConverter<DateTime?>(new DateConverter("yyyyMMdd"));
                                        var dateConverter = new DateConverter("yyyyMMdd");



                                        while (await csv.ReadAsync())
                                        {
                                            lista.Add(new Estabelecimento()
                                            {
                                                cnpj_base = csv.GetField(0),
                                                cnpj_ordem = csv.GetField(1),
                                                cnpj_dv = csv.GetField(2),
                                                cnpj = csv.GetField(0) + csv.GetField(1) + csv.GetField(2),
                                                identificador_mat_fil = csv.GetField(3),
                                                nome_fantasia = csv.GetField(4),
                                                situacao_cadastral = csv.GetField(5),
                                                data_situacao_cadastral = dateConverter.ConvertFromString(csv.GetField(6)),
                                                cod_motivo_sit_cadastral = csv.GetField(7),
                                                cidade_exterior = csv.GetField(8),
                                                cod_pais = csv.GetField(9),
                                                data_inicio = dateConverter.ConvertFromString(csv.GetField(10)),
                                                cnae_pri = csv.GetField(11),
                                                cnae_sec = csv.GetField(12),
                                                log_tipo = csv.GetField(13),
                                                log_nome = csv.GetField(14).Trim(),
                                                log_num = csv.GetField(15),
                                                log_comp = csv.GetField(16).Trim(),
                                                log_bairro = csv.GetField(17),
                                                log_cep = csv.GetField(18),
                                                log_uf = csv.GetField(19),
                                                log_cod_municipio = csv.GetField(20),
                                                ddd_1 = csv.GetField(21),
                                                tel_1 = csv.GetField(22),
                                                ddd_2 = csv.GetField(23),
                                                tel_2 = csv.GetField(24),
                                                ddd_fax = csv.GetField(25),
                                                tel_fax = csv.GetField(26),
                                                email = csv.GetField(27),
                                                situacao_especial = csv.GetField(28),
                                                data_situacao_especial = dateConverter.ConvertFromString(csv.GetField(29))


                                            });

                                            await MensagemAsync($"Lendo e gravando no banco {contador++} linhas do CSV");
                                            if (contador % 500000 == 0)
                                            {
                                                await MensagemAsync($"Importando 1000000 linhas para o banco!");
                                                await Task.Run(() => db.insertBulkestabelecimentos(lista));
                                                lista.Clear();
                                            }
                                        }
                                        await Task.Run(() => db.insertBulkestabelecimentos(lista));
                                        files[index].Type = TiposProgresso.Finished;
                                        Mensagem($"Processo concluído, importado {contador} linhas");
                                    }
                                }

                            }
                            else
                            {
                                throw new Exception("Tabela municipio não existe!");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            listView.SelectedIndex = -1;
        }
        private void Mensagem(string text)
        {
            this.Dispatcher.Invoke(() => {
                txtBlock.Text = text;
            }
            );
            
        }
        private Task<bool> MensagemAsync(string text)
        {
            this.Dispatcher.Invoke(() => {
                txtBlock.Text = text;
            }
            );
            return Task.FromResult(true);
        }

    }
}
