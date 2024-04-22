using AsyncMvvm.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace AsyncMvvm.ViewModels
{
    public class ReactiveControlViewModel : ReactiveObject, IActivatableViewModel
    {
        private readonly PrimeNumberCalculator calculator = new();
        private int longRunningActivationTime = 1;
        private readonly string title = "blub";

        /// <inheritdoc/>
        public ViewModelActivator Activator { get; } = new();

        public ReactiveControlViewModel(
            string title,
            string message,
            bool setIsLoadingOnDeactivation = true,
            bool doLongRunningActivation = false,
            bool runTaskForLongRunningActivation = false)
        {
            this.title = title;
            this.ModelTitleMessage = message;
            this.SetIsLoadingOnDeactivation = setIsLoadingOnDeactivation;
            this.DoLongRunningActivation = doLongRunningActivation;
            this.RunTaskForLongRunningActivation = runTaskForLongRunningActivation;

            this.WhenActivated(disposables =>
            {
                this.HandleActivation(disposables);

                Disposable
                    .Create(this.HandleDeactivation)
                    .DisposeWith(disposables);
            });
        }

        /// <summary>
        /// Gets or sets the title that is shown at the top.
        /// </summary>
        public string ModelTitle
        {
            get => this.title;
        }

        /// <summary>
        /// Gets or sets a message that is shown at the top.
        /// </summary>
        [Reactive]
        public string ModelTitleMessage { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is loading.
        /// </summary>
        [Reactive]
        public bool IsLoading { get; set; } = true;

        [Reactive]
        public string Message { get; set; } = string.Empty;

        [Reactive]
        public long LastPrimeNumber { get; private set; } = 0;

        [Reactive]
        public long LastPrimeNumberBgThread { get; private set; } = 0;

        public bool SetIsLoadingOnDeactivation { get; } = true;

        public bool DoLongRunningActivation { get; } = false;

        public bool RunTaskForLongRunningActivation { get; } = false;

        public ReactiveCommand<Unit, Unit>? StopSearch { get; private set; }

        public ReactiveCommand<int, Unit>? StartSearchAsyncTask { get; private set; }

        public ReactiveCommand<int, Unit>? StartSearchTaskAsyncTask { get; private set; }

        public ReactiveCommand<int, Unit>? StartSearchInBackground { get; private set; }

        protected virtual void HandleActivation(CompositeDisposable disposables)
        {
            Console.WriteLine($"{this}: {this.ModelTitle}: Activation");

            this.Message = string.Empty;

            this.StopSearch = 
                ReactiveCommand.Create(() =>
                {
                    Console.WriteLine($"Stopping search in thread: {Thread.CurrentThread.Name}.");
                    this.calculator.StopSearch();
                })
                .DisposeWith(disposables);

            this.StopSearch.Subscribe((x) => Console.WriteLine("Search stopped.")).DisposeWith(disposables);

            this.StartSearchAsyncTask = 
                ReactiveCommand.Create<int>(async (int level) => await this.calculator.StartSearchAsyncTask(level))
                .DisposeWith(disposables);

            this.StartSearchTaskAsyncTask = 
                ReactiveCommand.CreateFromTask<int>(this.calculator.StartSearchAsyncTask)
                .DisposeWith(disposables);

            this.StartSearchInBackground = 
                ReactiveCommand.CreateRunInBackground<int>((int level) => this.calculator.StartSearch(level))
                .DisposeWith(disposables);

            Observable.FromEventPattern<PrimeNumberEventHandler, PrimeNumberEventArgs>(
                h => this.calculator.NewPrimeNumberFound += h,
                h => this.calculator.NewPrimeNumberFound -= h)
                .Select(e => e.EventArgs.Number)
                .Subscribe(n =>
                {
                    this.LastPrimeNumber = n;
                })
                .DisposeWith(disposables);

            Observable.FromEventPattern<PrimeNumberEventHandler, PrimeNumberEventArgs>(
                h => this.calculator.NewPrimeNumberFound += h,
                h => this.calculator.NewPrimeNumberFound -= h)
                .ObserveOn(NewThreadScheduler.Default)
                .Select(e => e.EventArgs.Number)
                .Subscribe(n =>
                {
                    this.LastPrimeNumberBgThread = n;
                })
                .DisposeWith(disposables);

            this.StartStopLoading(true);

            if (this.DoLongRunningActivation)
            {
                if (this.RunTaskForLongRunningActivation)
                {
                    Task.Run(LongRunningActivation);
                }
                else
                {
                    this.LongRunningActivation();
                }
            }

            this.StartStopLoading(false);
        }

        private async void StartStopLoading(bool start)
        {
            await Task.Delay(start ? 1000 : 3000);
            this.IsLoading = start;
        }

        private void HandleDeactivation()
        {
            Console.WriteLine($"{this}: {this.ModelTitle}: DEactivation");

            this.Message = string.Empty;

            if (this.SetIsLoadingOnDeactivation)
            {
                this.IsLoading = true;
            }
        }

        private async Task LongRunningActivation()
        {
            this.Message = $"Executing long running method while activation in thread {Thread.CurrentThread.Name}.";
            
            Thread.Yield();
            Thread.Sleep(this.longRunningActivationTime);

            if (this.longRunningActivationTime == 1)
            {
                this.longRunningActivationTime = 7000;
            }

            this.Message = $"Long running method while activation is completed.";
        }
    }
}
