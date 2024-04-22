using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
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

        public string ViewTitle { get; set; } = string.Empty;

        public string ViewMessage { get; set; } = string.Empty;

        public bool SetIsLoadingOnDeactivation { get; set; } = true;

        public bool DoLongRunningActivation { get; set; } = false;

        public bool RunTaskForLongRunningActivation { get; set; } = false;

        public bool UseAsyncViewModel { get; set; } = false;

        private void HandleActivation(CompositeDisposable disposables)
        {
            Console.WriteLine($"{this}: {this.ViewTitle}: Activation");

            this.ViewModel ??= this.UseAsyncViewModel 
                ? new AsyncActivationViewModel(this.ViewTitle) 
                : new ReactiveControlViewModel(this.ViewTitle);

            this.ViewModel.ModelTitle = this.ViewTitle;
            this.ViewModel.ModelTitleMessage = this.ViewMessage;
            this.ViewModel.SetIsLoadingOnDeactivation = this.SetIsLoadingOnDeactivation;
            this.ViewModel.DoLongRunningActivation = this.DoLongRunningActivation;
            this.ViewModel.RunTaskForLongRunningActivation = this.RunTaskForLongRunningActivation;

            this.DataContext = this.ViewModel;

            this.BindCommand(this.ViewModel, vm => vm.StopSearch, v => v.Stop)
                .DisposeWith(disposables);
            this.BindCommand(this.ViewModel, vm => vm.StartSearchAsyncTask, v => v.StartSearchAsyncTask)
                .DisposeWith(disposables);
            this.BindCommand(this.ViewModel, vm => vm.StartSearchTaskAsyncTask, v => v.StartSearchTaskAsyncTask)
                .DisposeWith(disposables);
            this.BindCommand(this.ViewModel, vm => vm.StartSearchInBackground, v => v.StartSearchInBackground)
                .DisposeWith(disposables);
        }

        private void HandleDeactivation()
        {
            Console.WriteLine($"{this}: {this.ViewTitle}: DEactivation");
        }
    }
}
