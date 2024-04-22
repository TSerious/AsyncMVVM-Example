using System.Reactive.Disposables;

namespace AsyncMvvm.ViewModels
{
    internal class AsyncActivationViewModel(
        string title,
        string message,
        bool setIsLoadingOnDeactivation,
        bool doLongRunningActivation,
        bool runTaskForLongRunningActivation) : 
        ReactiveControlViewModel(title, message, setIsLoadingOnDeactivation, doLongRunningActivation, runTaskForLongRunningActivation)
    {
        protected override async void HandleActivation(CompositeDisposable disposables)
        {
            await Task.Delay(1000);
            base.HandleActivation(disposables);
        }
    }
}
