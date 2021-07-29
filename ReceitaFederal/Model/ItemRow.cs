using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ReceitaFederal.Model
{
    public class ItemRow : INotifyPropertyChanged
    {
        private string _fileName;
        public string FileName { get { return _fileName; } set { _fileName = value; RaisePropertyChanged("FileName"); } }

        private TiposProgresso _type;
        public TiposProgresso Type { get { return _type; } set { _type = value;

                Color = ChangeColor(value);
                RaisePropertyChanged("Type"); } }

        private Brush _color;

        public ItemRow(string fileName, TiposProgresso type)
        {
            FileName = fileName;
            Type = type;
            Color = new SolidColorBrush(Colors.White);

            
        }

        public Brush Color { get { return _color; } set { _color = value; RaisePropertyChanged("Color"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        private SolidColorBrush ChangeColor(TiposProgresso value)
        {
            if (value == TiposProgresso.InProgress)
            {
                return  new SolidColorBrush(Colors.Yellow);
                //System.Windows.Media.Color.FromRgb(205, 92, 92)
            }
            else if(value == TiposProgresso.Finished)
            {
                return new SolidColorBrush(Colors.Green);
            }
            else
            {
                return new SolidColorBrush(Colors.White);
            }
        }
        private void RaisePropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public enum TiposProgresso
    {
        Finished,
        InProgress,
        Waiting
    }
}
