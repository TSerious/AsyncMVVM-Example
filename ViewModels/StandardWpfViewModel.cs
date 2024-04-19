using AsyncMvvm.Model;
using System.ComponentModel;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

namespace AsyncMvvm.ViewModels
{
    public class StandardWpfViewModel : INotifyPropertyChanged
    {
        private readonly PrimeNumberCalculator calculator = new ();
        private long lastPrimeNumber = 0;

        public event PropertyChangedEventHandler? PropertyChanged;

        public StandardWpfViewModel()
        {
            this.calculator.NewPrimeNumberFound += Calculator_NewPrimeNumberFound;

            // sync
            this.StartSearch = new IntCommandHandler(this.calculator.StartSearch, () => !this.calculator.IsSearching);
            this.StartSearchRunTaskSyncCommand = new VoidCommandHandler(this.SearchRunTask, () => !this.calculator.IsSearching);
            
            // async command
            this.StartSearchRunTaskAsync = new AsyncCommand(this.calculator.StartSearchRunTask);
            this.StartSearchRunTaskAsyncAsync = new AsyncCommand(this.calculator.StartSearchRunTaskAsync);
            this.StartSearchAsyncRunTask = new AsyncCommand(this.calculator.StartSearchAsyncRunTaskAction);
            this.StartSearchAsyncAwaitRunTaskAction = new AsyncCommand(this.calculator.StartSearchAsyncAwaitRunTaskActionAwaitAsyncTask);
            this.StartSearchAsyncAwaitRunTask = new AsyncCommand(this.calculator.StartSearchAsyncRunTask);

            // await async command
            this.AwaitStartSearchRunTaskAsync = new AwaitAsyncCommand(this.calculator.StartSearchRunTask);

            // start async command
            this.StartCreateSearchTask = new AsyncCommandStart(this.calculator.CreateSearchTask);
            this.StartStartSearchAsyncTask = new AsyncCommandStart(this.calculator.StartSearchAsyncTask);
            this.StartStartSearchAsyncTaskAsync = new AsyncCommandStart(this.calculator.StartSearchAsyncTaskAsync);

            this.StartSearchAsyncTask = new AsyncCommand(this.calculator.StartSearchAsyncTask);
            this.StartSearchAsyncTaskAsync = new AsyncCommand(this.calculator.StartSearchAsyncTaskAsync);

            // stop
            this.StopSearch = new VoidCommandHandler(this.calculator.StopSearch, () => true);
        }

        public long LastPrimeNumber 
        {
            get => this.lastPrimeNumber;
            private set
            {
                if (value == this.lastPrimeNumber) return;

                this.lastPrimeNumber = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.LastPrimeNumber)));
            }
        }

        public ICommand StartSearch { get; }

        public ICommand StartSearchRunTaskSyncCommand { get; }

        public ICommand StartSearchRunTaskAsync { get; }

        public ICommand AwaitStartSearchRunTaskAsync { get; }

        public ICommand StartCreateSearchTask { get; }

        public ICommand StartStartSearchAsyncTask { get; }

        public ICommand StartStartSearchAsyncTaskAsync { get; }

        public ICommand StartSearchRunTaskAsyncAsync { get; }

        public ICommand StartSearchAsyncRunTask { get; }

        public ICommand StartSearchAsyncAwaitRunTaskAction { get; }

        public ICommand StartSearchAsyncAwaitRunTask { get; }

        public ICommand StartSearchAsyncTask { get; }

        public ICommand StartSearchAsyncTaskAsync { get; }

        public ICommand StopSearch { get; }

        private void Calculator_NewPrimeNumberFound(object sender, PrimeNumberEventArgs e)
        {
            //Console.WriteLine($"New prime number found in thread {Thread.CurrentThread.Name}");
            this.LastPrimeNumber = e.Number;
        }

        private void SearchRunTask()
        {
            Console.WriteLine("-------------------------------------------------------------------");
            Console.WriteLine($"{nameof(this.SearchRunTask)} called in {Thread.CurrentThread.Name}.");
            this.calculator.StartSearchRunTask(1);
            Console.WriteLine($"{nameof(this.SearchRunTask)} called in {Thread.CurrentThread.Name} done.");
        }
    }
}
