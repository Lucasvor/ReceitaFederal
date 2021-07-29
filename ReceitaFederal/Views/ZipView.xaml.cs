using Ionic.Zip;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ZipView.xaml
    /// </summary>
    public partial class ZipView : UserControl
    {
        int totalZip, Izip;
        ObservableCollection<string> items = default;
        public ZipView()
        {
            InitializeComponent();
            items = new ObservableCollection<string>();
            //foreach(var item in Directory.EnumerateFiles("Zip", "*.zip"))
            //{
            //    items.Add(System.IO.Path.GetFileName(item));
            //}
            listview.ItemsSource = items;//Directory.EnumerateFiles("Zip", "*.zip").Select(x => System.IO.Path.GetFileName(x)).ToList();
        }

        [STAThread]
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            totalZip = Izip = 0;
            //quantidade de arquivos
            
            foreach (var item in Directory.EnumerateFiles("Zip", "*.zip"))
            {
                items.Add(System.IO.Path.GetFileName(item));
            }
            //var files = Directory.EnumerateFiles("Zip","*.zip");
            var qtd = items.Count;
            if(qtd > 0)
            {
                try
                {
                    //ContadorMensagem.Text = $"Tem {qtd} arquivos";
                    await Task.Run(() =>
                    {
                        foreach (var item in items.ToList())
                        {
                            using (var zip = new ZipFile("Zip//" + item))
                            {
                                try
                                {
                                    zip.ExtractProgress += Zip_ExtractProgress;
                                    zip.ExtractAll("Zip", ExtractExistingFileAction.InvokeExtractProgressEvent);
                                    Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                                    {
                                        items.Remove(System.IO.Path.GetFileName(item));
                                    }));

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, "ERROR");
                                    Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                                    {
                                        items.Remove(System.IO.Path.GetFileName(item));
                                    }));
                                }
                            }
                        }
                        updateProgressText(progressBar, ContadorMensagem, 100, "Processo FInalizado");
                    });
                }catch(Exception ex)
                {
                    ContadorMensagem.Text = $"{ex.Message}";
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                ContadorMensagem.Text = "Não existe arquviso zip na pasta";
            }
        }
        private void updateProgressText(MetroProgressBar progressBar,TextBlock text, double valorporc,string mensagem)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                progressBar.Value = valorporc;
                text.Text = mensagem;
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                {
                    progressBar.Value = valorporc;
                    text.Text = mensagem;
                }));
            }
        }
        private void Zip_ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            // tpb.CustomText = "Extraindo";


            if (e.EventType == ZipProgressEventType.Extracting_EntryBytesWritten)
            {

                if (e.BytesTransferred == e.TotalBytesToTransfer)
                {
                    //tpb.Value = (int)(iZip++ / (0.01 * totalZip));
                    //updateProgressBar(tpb, (int)(iZip++ /( (0.01 * totalZip)));
                    //progressBar.Value = (int)(Izip++ / (0.01 * totalZip));
                }
                else
                {
                    updateProgressText(progressBar, ContadorMensagem, e.BytesTransferred / (0.01 * e.TotalBytesToTransfer), $"Extraindo arquivo { e.CurrentEntry.FileName} - {(e.BytesTransferred / (0.01 * e.TotalBytesToTransfer)).ToString("N2")}%");
                    // progressBar.Value = e.BytesTransferred / (0.01 * e.TotalBytesToTransfer);
                    //ContadorMensagem.Text = $"Extraindo arquivo { e.CurrentEntry.FileName} - {progressBar.Value}%";

                }
                //tpb.Value = (int)(e.BytesTransferred / (0.01 * e.TotalBytesToTransfer));
            }
            
            if (e.EventType == ZipProgressEventType.Extracting_BeforeExtractAll)
            {
                //updateProgressBar(tpb, qtdZip, "Extraindo arquivo.");
                //ContadorMensagem.Text = $"Iniciando processo de extração de {System.IO.Path.GetFileNameWithoutExtension(e.ArchiveName)} ";
                //tpb.CustomText = "Extraindo arquivo.";
            }
            if (e.EventType == ZipProgressEventType.Extracting_BeforeExtractEntry)
            {
                //tpb.CustomText = $"Extraindo arquivo: {e.CurrentEntry.FileName}";
                //tpb.CustomText = $"Extraindo arquivos {e.CurrentEntry.FileName} - ({iZip}/{totalZip})";
                //updateProgressBar(tpb, qtdZip, $"Extraindo arquivos {e.CurrentEntry.FileName} - ({iZip}/{totalZip})");
                //ContadorMensagem.Text = $"Extraindo arquivos {e.CurrentEntry.FileName} - ({Izip}/{totalZip})";
                totalZip = e.EntriesTotal;
            }
            

            //tpb.Value = (int)(1.0d / e.TotalBytesToTransfer * e.BytesTransferred * 100.0d);
        }
    }
}
