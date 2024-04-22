using System;
using System.Reactive.Disposables;
using System.Windows;
using AsyncMvvm.ViewModels;
using ReactiveUI;

namespace AsyncMvvm.Views
{
    /// <summary>
    /// Interaction logic for ReactiveControl.xaml
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

        /// <summary>
        /// Gets or sets the text that is also used for <see cref="ReactiveControlViewModel.ModelTitle"/>.
        /// </summary>
        /// <remarks>The purpose of this property is only to set 
        /// <see cref="ReactiveControlViewModel.ModelTitle"/>in xaml </remarks>
        public string ViewTitle { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the text that is also used for <see cref="ReactiveControlViewModel.ModelTitleMessage"/>.
        /// </summary>
        /// <remarks>The purpose of this property is only to set 
        /// <see cref="ReactiveControlViewModel.ModelTitleMessage"/>in xaml </remarks>
        public string ViewMessage { get; set; } = string.Empty;

        /// <summary>
        /// See <see cref="ReactiveControlViewModel.SetIsLoadingOnDeactivation"/>.
        /// </summary>
        // <remarks>The purpose of this property is only to set 
        /// <see cref="ReactiveControlViewModel.SetIsLoadingOnDeactivation"/>in xaml </remarks>
        public bool SetIsLoadingOnDeactivation { get; set; } = true;

        /// <summary>
        /// See <see cref="ReactiveControlViewModel.DoLongRunningActivation"/>.
        /// </summary>
        // <remarks>The purpose of this property is only to set 
        /// <see cref="ReactiveControlViewModel.DoLongRunningActivation"/>in xaml </remarks>
        public bool DoLongRunningActivation { get; set; } = false;

        /// <summary>
        /// See <see cref="ReactiveControlViewModel.RunTaskForLongRunningActivation"/>.
        /// </summary>
        // <remarks>The purpose of this property is only to set 
        /// <see cref="ReactiveControlViewModel.RunTaskForLongRunningActivation"/>in xaml </remarks>
        public bool RunTaskForLongRunningActivation { get; set; } = false;

        /// <summary>
        /// See <see cref="ReactiveControlViewModel.UseAsyncViewModel"/>.
        /// </summary>
        // <remarks>The purpose of this property is only to set 
        /// <see cref="ReactiveControlViewModel.UseAsyncViewModel"/>in xaml </remarks>
        public bool UseAsyncViewModel { get; set; } = false;

        private void HandleActivation(CompositeDisposable disposables)
        {
            

            if (this.DataContext is not ReactiveControlViewModel vm)
            {
                this.ViewModel ??= this.UseAsyncViewModel
                    ? new AsyncActivationViewModel(
                        this.ViewTitle,
                        this.ViewMessage,
                        this.SetIsLoadingOnDeactivation,
                        this.DoLongRunningActivation,
                        this.RunTaskForLongRunningActivation)
                    : new ReactiveControlViewModel(
                        this.ViewTitle,
                        this.ViewMessage,
                        this.SetIsLoadingOnDeactivation,
                        this.DoLongRunningActivation,
                        this.RunTaskForLongRunningActivation);
            }
            else if (this.ViewModel == null)
            {
                this.ViewModel = vm;
            }

            Console.WriteLine($"{this}: {this.ViewModel?.ModelTitle}: Activation");

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
            Console.WriteLine($"{this}: {this.ViewModel?.ModelTitle}: DEactivation");
        }
    }
}
