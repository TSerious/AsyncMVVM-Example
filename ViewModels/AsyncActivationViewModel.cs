using System.Reactive.Disposables;

namespace AsyncMvvm.ViewModels
{
    internal class AsyncActivationViewModel(string title) : ReactiveControlViewModel(title)
    {
        protected override async void HandleActivation(CompositeDisposable disposables)
        {
            await Task.Delay(1000);
            base.HandleActivation(disposables);
        }
    }
}
