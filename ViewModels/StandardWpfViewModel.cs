using AsyncMvvm.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AsyncMvvm.ViewModels
{
    internal class StandardWpfViewModel : INotifyPropertyChanged
    {
        private readonly PrimeNumberCalculator calculator = new ();
        private long lastPrimeNumber = 0;
        private long number = 0;
        private bool numberIsPrime = false;
        private Task? searchForPrimeNumbers;
        private ICommand? checkSync;
        private ICommand? startSearch;
        private ICommand? startSearchInTask;
        private ICommand? startSearchInTaskAsync;
        private ICommand? startSearchInTaskAsyncAsync;
        private ICommand? startSearchAsync;
        private ICommand? startSearchAsyncInTask;
        private ICommand? startSearchAsyncTaskAsync;
        private ICommand? stopSearch;

        public event PropertyChangedEventHandler? PropertyChanged;

        public StandardWpfViewModel()
        {
            this.calculator.NewPrimeNumberFound += Calculator_NewPrimeNumberFound;
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

        public long Number
        {
            get => this.number;
            set
            {
                if (value == this.number) return;

                this.number = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Number)));
            }
        }

        public bool NumberIsPrime
        {
            get => this.numberIsPrime;
            set
            {
                if (value == this.numberIsPrime) return;

                this.numberIsPrime = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.NumberIsPrime)));
            }
        }

        public ICommand StartSearch
        {
            get
            {
                return this.startSearch ??= new CommandHandler(this.calculator.StartSearch, () => !this.calculator.IsSearching);
            }
        }

        public ICommand StartSearchInTask
        {
            get
            {
                return this.startSearchInTask ??= new StopCommandHandler(this.SearchInTask, () => !this.calculator.IsSearching);
            }
        }

        public ICommand StartSearchInTaskAsync
        {
            get
            {
                return this.startSearchInTaskAsync ??= new AsyncCommand(this.calculator.StartSearchInTask);
            }
        }

        public ICommand AwaitStartSearchInTaskAsync
        {
            get
            {
                return new AwaitAsyncCommand(this.calculator.StartSearchInTask);
            }
        }

        public ICommand StartSearchInTaskAsyncAsync
        {
            get
            {
                return this.startSearchInTaskAsyncAsync ??= new AsyncCommand(this.calculator.StartSearchInTaskAsync);
            }
        }

        public ICommand StartSearchAsync
        {
            get
            {
                return this.startSearchAsync ??= new AsyncCommand(this.calculator.StartSearchAsync);
            }
        }

        public ICommand StartSearchAsyncInTask
        {
            get
            {
                return this.startSearchAsyncInTask ??= new AsyncCommand(this.calculator.StartSearchAsyncInTask);
            }
        }

        public ICommand StartSearchAsyncTaskAsync
        {
            get
            {
                return this.startSearchAsyncTaskAsync ??= new AsyncCommand(this.calculator.StartSearchAsyncTaskAsync);
            }
        }

        public ICommand StopSearch
        {
            get
            {
                return this.stopSearch ??= new StopCommandHandler(this.calculator.StopSearch, () => true);
            }
        }

        private void Calculator_NewPrimeNumberFound(object sender, PrimeNumberEventArgs e)
        {
            //Console.WriteLine($"New prime number found in thread {Thread.CurrentThread.Name}");
            this.LastPrimeNumber = e.Number;
        }

        private void CheckNumberSync()
        {
            if (this.searchForPrimeNumbers != null ||
                this.calculator.IsSearching)
            {
                return;
            }

            this.calculator.CurrentNumber = this.number;
            this.calculator.CheckCurrentNumber();
            this.NumberIsPrime = this.calculator.IsPrime;
        }

        private void SearchInTask()
        {
            Console.WriteLine("-------------------------------------------------------------------");
            Console.WriteLine($"{nameof(this.SearchInTask)} called in {Thread.CurrentThread.Name}.");
            this.calculator.StartSearchInTask(1);
            Console.WriteLine($"{nameof(this.SearchInTask)} called in {Thread.CurrentThread.Name} done.");
        }
    }
}
