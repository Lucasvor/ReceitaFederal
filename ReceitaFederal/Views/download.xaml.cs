

using ReceitaFederal.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ReceitaFederal.Views
{
    /// <summary>
    /// Interaction logic for download.xaml
    /// </summary>
    public partial class download : UserControl
    {

        string url = @"http://200.152.38.155/CNPJ/";
        ObservableCollection<ItemDownload> list;
        static Random rnd = new Random();
        List<string> lista = new List<string>();
        List<Task<bool>> listTask = new List<Task<bool>>();
        int i;

        public download()
        {
            InitializeComponent();
            list = new ObservableCollection<ItemDownload>();
            downloads.ItemsSource = list;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                list.Clear();
                WebRequest request = WebRequest.Create(url);
                string icon = default;
                bool spin = default;
                int angle = default;
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
                                if (CheckFileExists(match.Value))
                                {
                                    icon = "Check";
                                    spin = false;
                                    angle = 0;
                                }
                                else
                                {
                                    icon = "Plus";
                                    spin = false;
                                    angle = 45;
                                }
                                var task = new ItemDownload() { FileName = match.Value, DateMod = matchesDate[i].Value, IconName = icon, Spin = spin,AngleIcon=angle };
                                list.Add(task);
                                i++;
                                //listTask.Add(task.DownloadFile());
                            }
                        }
                    }
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERRO");
                //throw new Exception(ex.Message, ex);
            }

        }
        private bool CheckFileExists(string value)
        {
            return File.Exists(Path.Combine("Zip", value));
        }

        private async void downloads_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                var item = (ItemDownload)downloads.SelectedItem;

                if (File.Exists("Zip//"+item.FileName))
                {
                    var result = MessageBox.Show("Arquivo que você está tentando baixar já existe, deseja baixar novamente?", "Receita Federal", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        await item.DownloadFile();
                    }
                }
                else
                {
                    
                    await item.DownloadFile();

                }
                //Task.Factory.StartNew(() => item.DownloadFile());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
