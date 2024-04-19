using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
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
using AsyncMvvm.ViewModels;
using ReactiveUI;

namespace AsyncMvvm.Views
{
    /// <summary>
    /// Interaction logic for ReactiveControl1.xaml
    /// </summary>
    public partial class ReactiveControl : ReactiveUserControl<ReactiveControlViewModel>, IViewFor<ReactiveControlViewModel>
    {
        public ReactiveControl()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                Disposable.Create(this.HandleDeactivation)
                    .DisposeWith(disposables);

                this.HandleActivation(disposables);
            });
        }

        public string ViewName { get; set; } = string.Empty;

        public bool SetIsLoadingOnDeactivation { get; set; } = true;

        private void HandleActivation(CompositeDisposable disposable)
        {
            Console.WriteLine($"{this}:{this.ViewName}.{nameof(this.HandleActivation)}");

            this.ViewModel ??= new ReactiveControlViewModel();
            this.ViewModel.ModelName = this.ViewName;
            this.ViewModel.SetIsLoadingOnDeactivation = this.SetIsLoadingOnDeactivation;
            this.DataContext = this.ViewModel;

            /*
            this.Bind(this.ViewModel, vm => vm.IsLoading, v => v.BusyIndicator.IsBusy)
                .DisposeWith(disposable);
            */
        }

        private void HandleDeactivation()
        {
            Console.WriteLine($"{this}:{this.ViewName}.{nameof(this.HandleDeactivation)}");
        }
    }
}
