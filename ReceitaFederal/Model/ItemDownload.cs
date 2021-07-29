using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ReceitaFederal.Model
{

    public class ItemDownload : INotifyPropertyChanged
    {
        static readonly string[] SizeSuffixes =
                   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };




        public event PropertyChangedEventHandler PropertyChanged;
        private string size;
        private string downloaded;
        private int valueProgress;
        private string iconName;
        private int angleIcon;
        private bool spin;

        public string FileName { get; set; }
        public string DateMod { get; set; }
        public string IconName { get { return iconName; } set { iconName = value; OnPropertyChanged(); } }
        public int AngleIcon { get { return angleIcon; } set { angleIcon = value; OnPropertyChanged(); } }
        public bool Spin { get { return spin; } set { spin = value; OnPropertyChanged(); } }
        public string Size { get { return size; } set { size = value; OnPropertyChanged(); } }
        public string Downloaded { get { return downloaded; } set { downloaded = value; OnPropertyChanged(); } }
        public int ValueProgress { get { return valueProgress; } set { valueProgress = value; OnPropertyChanged(); } }


        public async Task<bool> DownloadFile()
        {
            try
            {
                IconName = "Cycle";
                Spin = true;
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadProgressChanged += (s, e) =>
                    {
                        this.ValueProgress = e.ProgressPercentage;
                        this.Size = SizeSuffix(e.TotalBytesToReceive); //e.TotalBytesToReceive.ToString();
                        this.Downloaded = SizeSuffix(e.BytesReceived);//e.BytesReceived.ToString();
                    };
                    if (!System.IO.Directory.Exists("Zip"))
                    {
                        System.IO.Directory.CreateDirectory("Zip");
                    }
                    await wc.DownloadFileTaskAsync($"http://200.152.38.155/CNPJ/{FileName}", Path.Combine("Zip", FileName));
                    //Task.Delay(10000);
                    //var file = new FileInfo(Path.Combine("Zip", FileName));
                    //if (!IsFileLocked(file))
                    //    throw new Exception("Arquivo baixado não está completo.");
                }
                IconName = "Check";
                AngleIcon = 0;
                Spin = false;
                return true;
            }
            catch (Exception e)
            {
                iconName = "Plus";
                AngleIcon = 45;
                spin = false;
                throw new Exception($"O arquivo: {FileName} teve um problema: {e.Message}");
                
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        static string SizeSuffix(Int64 value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + SizeSuffix(-value, decimalPlaces); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }
        static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
    }

}