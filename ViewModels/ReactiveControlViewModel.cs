using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.ComponentModel;
using System.Reactive.Disposables;

namespace AsyncMvvm.ViewModels
{
    public class ReactiveControlViewModel : ReactiveObject, IActivatableViewModel
    {
        public ViewModelActivator Activator { get; } = new ();

        public ReactiveControlViewModel() 
        {
            this.WhenActivated(disposables =>
            {
                this.HandleActivation(disposables);

                Disposable
                    .Create(this.HandleDeactivation)
                    .DisposeWith(disposables);
            });
        }

        [Reactive]
        public string ModelName { get; set; } = string.Empty;

        [Reactive]
        public bool IsLoading { get; set; } = true;

        public bool SetIsLoadingOnDeactivation { get; set; } = true;

        private async void HandleActivation(CompositeDisposable disposables)
        {
            Console.WriteLine($"{this}:{this.ModelName}.{nameof(this.HandleActivation)}");
            await Task.Delay(1000);
            this.IsLoading = true;
            await Task.Delay(1000);
            this.IsLoading = false;
        }

        private void HandleDeactivation()
        {
            Console.WriteLine($"{this}:{this.ModelName}.{nameof(this.HandleDeactivation)}");

            if (this.SetIsLoadingOnDeactivation)
            {
                this.IsLoading = true;
            }
            
        }
    }
}
