using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using ReceitaFederal.Model;
using ReceitaFederal.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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

namespace ReceitaFederal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    /// Implementações para fazer
    /// Exibição de erro
    /// Fazer uma opção debaixar varios de uma vez
    /// uma barra de notificação embaixo
    /// Colocar data de modificação dos arquivos
    /// Deixar os valores size e downloaded em mb
    /// Melhorar interface
    public partial class MainWindow : MetroWindow
    {
        string url = @"http://200.152.38.155/CNPJ/";
        ObservableCollection<ItemDownload> list;
        static Random rnd = new Random();
        List<string> lista = new List<string>();
        List<Task<bool>> listTask = new List<Task<bool>>();
        int i;
        
        public MainWindow()
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 30;
            InitializeComponent();
            list = new ObservableCollection<ItemDownload>();
            
            //downloads.ItemsSource = list;
        }
        private void HamburgerMenuControl_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
            this.HamburgerMenuControl.Content = e.InvokedItem;

            if (!e.IsItemOptions && this.HamburgerMenuControl.IsPaneOpen)
            {
                // You can close the menu if an item was selected
                // this.HamburgerMenuControl.SetCurrentValue(HamburgerMenuControl.IsPaneOpenProperty, false);
            }
            var item = (HamburgerMenuIconItem)e.InvokedItem;
            if (item.Label == "Salva no banco")
            {
                var baixar = (Importa)item.Tag;
                baixar.ReloadFiles();
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            

            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Regex regex = new Regex("(?<=href=\").*?(?=\")");
            Regex regexData = new Regex(@"\d{4}-\d{2}-\d{2} \d{2}:\d{2}");

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var result = reader.ReadToEnd();
                var matches = regex.Matches(result);
                var matchesDate = regexData.Matches(result);
                i = 0;
                if (matches.Count == 0)
                {
                    MessageBox.Show("Não foi possível localizar nenhum link");
                }
                else
                {
                    foreach (Match match in matches)
                    {
                        if (match.Value.Length > 8)
                        {

                            var task = new ItemDownload() { FileName = match.Value,DateMod = matchesDate[i].Value };
                            list.Add(task);
                            i++;
                            //listTask.Add(task.DownloadFile());
                        }
                    }
                }
            }
            
        }

        private async void  downloads_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //try
            //{
            //    var item = (ItemDownload)downloads.SelectedItem;

            //    if (File.Exists(item.FileName))
            //    {
            //        var result = MessageBox.Show("Arquivo que você está tentando baixar já existe, deseja baixar novamente?","Receita Federal",MessageBoxButton.YesNo);
            //        if(result == MessageBoxResult.Yes)
            //        {
            //            await item.DownloadFile();
            //        }
            //    }
            //    else
            //    {
            //        await item.DownloadFile();

            //    }
            //    //Task.Factory.StartNew(() => item.DownloadFile());
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                string error = default;
                using(var db = new Db())
                {
                    // Verifica e cria banco de dados
                    db.Database.SqlQuery<int?>("IF NOT EXISTS (SELECT name FROM master.sys.databases WHERE name = N'ReceitaFederal') begin  CREATE DATABASE [ReceitaFederal] select 1  end else begin  select 0  end");
                    // Verifica e cria tabela cnae
                    db.Database.SqlQuery<int?>("use ReceitaFederal IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tbl_cnae')  BEGIN    USE [ReceitaFederal] GO CREATE TABLE [dbo].[tbl_cnae]([codigo] [varchar](50) NOT NULL, [denominacao] [varchar](150) NOT NULL) ON [PRIMARY] GO END");
                    // Verifica e cria tabela empresas
                    db.Database.SqlQuery<int?>("use ReceitaFederal IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tbl_empresas')  BEGIN    USE [ReceitaFederal] GO CREATE TABLE [dbo].[tbl_empresas]([cnpj_base] [varchar](8) NOT NULL,[razao_social] [varchar](150) NOT NULL,[cod_natureza_juridica] [int] NOT NULL,[cod_qualificacao_resposavel] [varchar](50) NOT NULL,[capital_social] [numeric](14, 2) NOT NULL,[porte] [varchar](50) NOT NULL,[ente_federativo_responsavel] [varchar](max) NULL) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] GO END");
                    // Verifica e cria tabela estabelecimentos
                    db.Database.SqlQuery<int?>("use ReceitaFederal IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tbl_estabelecimentos') BEGIN   CREATE TABLE [dbo].[tbl_estabelecimentos]([cnpj] [varchar](14) NOT NULL,[cnpj_base] [varchar](8) NOT NULL,[cnpj_ordem] [varchar](4) NOT NULL,[cnpj_dv] [varchar](2) NOT NULL,[identificador_mat_fil] [varchar](max) NULL,[nome_fantasia] [varchar](max) NULL,[situacao_cadastral] [varchar](max) NULL,[data_situacao_cadastral] [varchar](max) NULL,[cod_motivo_sit_cadastral] [varchar](max) NULL,[cidade_exterior] [varchar](max) NULL,[cod_pais] [varchar](max) NULL,[data_inicio] [varchar](max) NULL,[cnae_pri] [varchar](max) NULL,[cnae_sec] [varchar](max) NULL,[log_tipo] [varchar](max) NULL,[log_nome] [varchar](max) NULL,[log_num] [varchar](max) NULL,[log_comp] [varchar](max) NULL,[log_bairro] [varchar](max) NULL,[log_cep] [varchar](max) NULL,[log_uf] [varchar](max) NULL,[log_cod_municipio] [varchar](max) NULL,[ddd_1] [varchar](max) NULL,[tel_1] [varchar](max) NULL,[ddd_2] [varchar](max) NULL,[tel_2] [varchar](max) NULL,[ddd_fax] [varchar](max) NULL,[tel_fax] [varchar](max) NULL,[email] [varchar](max) NULL,[situacao_especial] [varchar](max) NULL,[data_situacao_especial] [varchar](max) NULL) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] END");
                    // Verifica e cria tabela motivoSituacaocadastral
                    db.Database.SqlQuery<int?>("use ReceitaFederal IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tbl_motivosituacaocadastral')  BEGIN    USE [ReceitaFederal] GO CREATE TABLE [dbo].[tbl_motivosituacaocadastral]([codigo] [varchar](50) NOT NULL,[denominacao] [varchar](100) NOT NULL) ON [PRIMARY] END");
                    // Verifica e cria tabela Municipio
                    db.Database.SqlQuery<int?>("use ReceitaFederal IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tbl_municipio')  BEGIN   USE [ReceitaFederal] GO CREATE TABLE [dbo].[tbl_municipio]([codigo] [varchar](50) NOT NULL,[denominacao] [varchar](50) NOT NULL) ON [PRIMARY] END");
                    // verifica e cria tabela natureza juridica
                    db.Database.SqlQuery<int?>("use ReceitaFederal IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tbl_natureza_juridica')  BEGIN   USE [ReceitaFederal] GO CREATE TABLE [dbo].[tbl_natureza_juridica]([codigo] [varchar](50) NOT NULL,[denominacao] [varchar](100) NOT NULL) ON [PRIMARY] END");
                    // verifica e cria tabela pais
                    db.Database.SqlQuery<int?>("use ReceitaFederal IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tbl_pais')  BEGIN   USE [ReceitaFederal] GO CREATE TABLE [dbo].[tbl_pais]([codigo] [varchar](50) NOT NULL,[denominacao] [varchar](50) NOT NULL) ON [PRIMARY] END");
                    // verifica e cria tabela qualificacao socio
                    db.Database.SqlQuery<int?>("use ReceitaFederal IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tbl_qualificacao_socio')  BEGIN   USE [ReceitaFederal] CREATE TABLE [dbo].[tbl_qualificacao_socio]([codigo] [varchar](50) NOT NULL,[denominacao] [varchar](100) NOT NULL) ON [PRIMARY] END");
                    // verifica e cria tabela simples
                    db.Database.SqlQuery<int?>("use ReceitaFederal IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tbl_simples')  BEGIN   USE [ReceitaFederal] go CREATE TABLE [dbo].[tbl_simples]([cnpj_base] [varchar](max) NOT NULL,[opcao_simples] [varchar](50) NOT NULL,[data_entrada_simples] [datetime2](7) NULL,[data_exclusao_simples] [datetime2](7) NULL,[opcao_mei] [varchar](50) NOT NULL,[data_entrada_mei] [datetime2](7) NULL,[data_exclusao_mei] [datetime2](7) NULL) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] GO END");
                    // verifica e cria tabela socios
                    db.Database.SqlQuery<int?>("use ReceitaFederal IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tbl_socios')  BEGIN   USE [ReceitaFederal] GO CREATE TABLE [dbo].[tbl_socios]([cnpj_base] [varchar](max) NOT NULL,[identificador_socio] [varchar](max) NOT NULL,[nome] [varchar](200) NULL,[cpf_cnpj] [varchar](50) NULL,[cod_qualificacao_socio] [varchar](max) NOT NULL,[data_entrada_sociedade] [datetime2](7) NULL,[cod_pais] [varchar](50) NOT NULL,[replegal_cpf] [varchar](50) NULL,[replegal_nome] [varchar](200) NULL,[replegal_cod_qualificacao] [varchar](50) NULL,[faixa_etaria] [varchar](50) NOT NULL) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] GO END");
                    //var objCtx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext;
                    //await objCtx.ExecuteStoreCommandAsync($"");
                }
                
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
