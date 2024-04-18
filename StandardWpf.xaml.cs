using AsyncMvvm.ViewModels;
using System;
using System.Collections.Generic;
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

namespace AsyncMvvm
{
    /// <summary>
    /// Interaction logic for StandardWpf.xaml
    /// </summary>
    public partial class StandardWpf : UserControl
    {
        private readonly StandardWpfViewModel vm;
        

        public StandardWpf()
        {
            InitializeComponent();
            vm = new StandardWpfViewModel();
            this.DataContext = vm;
        }

        

        
    }
}
