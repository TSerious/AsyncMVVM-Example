using AsyncMvvm.ViewModels;
using System.Windows.Controls;

namespace AsyncMvvm.Views
{
    /// <summary>
    /// Interaction logic for StandardWpf.xaml
    /// </summary>
    public partial class StandardWpf : UserControl
    {
        public StandardWpf()
        {
            InitializeComponent();
            this.DataContext = this.ViewModel;
        }

        public StandardWpfViewModel ViewModel
        {
            get;
        }

        = new ();
    }
}
