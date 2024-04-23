using AsyncMvvm.ViewModels;
using System.Windows.Controls;
using System.Windows.Data;

namespace AsyncMvvm.Views
{
    /// <summary>
    /// Interaction logic for StandardWpf.xaml
    /// </summary>
    public partial class StandardWpf : UserControl
    {
        private object? lockObject;

        public StandardWpf()
        {
            this.InitializeComponent();
            this.DataContext = this.ViewModel;
            this.FoundPrimeNumbers.ItemsSource = this.ViewModel.FoundPrimeNumbers;
        }

        public StandardWpfViewModel ViewModel
        {
            get;
        }

        = new ();

        private void SyncBinding_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.lockObject != null)
            {
                return;
            }

            this.lockObject = new object ();
            this.FoundPrimeNumbers.ItemsSource = null;
            BindingOperations.EnableCollectionSynchronization(this.ViewModel.FoundPrimeNumbers, this.lockObject);
            this.FoundPrimeNumbers.ItemsSource = this.ViewModel.FoundPrimeNumbers;
        }

        private void SyncBinding_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.lockObject == null)
            {
                return;
            }

            this.FoundPrimeNumbers.ItemsSource = null;
            BindingOperations.DisableCollectionSynchronization(this.ViewModel.FoundPrimeNumbers);
            this.FoundPrimeNumbers.ItemsSource = this.ViewModel.FoundPrimeNumbers;
            this.lockObject = null;
        }
    }
}
