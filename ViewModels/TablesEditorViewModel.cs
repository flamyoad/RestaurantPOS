using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlamyPOS.ViewModels
{
    public class TablesEditorViewModel : ObservableObject
    {
        public ICommand AddTableCommand { get; set; }
        public ICommand ShowDataCommand { get; set; }

        private double LayoutHeight;
        private double LayoutWidth;
    }
}
