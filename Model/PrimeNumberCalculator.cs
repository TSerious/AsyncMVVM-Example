using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AsyncMvvm.Model
{
    internal class PrimeNumberCalculator
    {
        private long currentNumber = 0;

        public event PrimeNumberEventHandler? NewPrimeNumberFound;

        public bool IsSearching
        {
            get;
            private set;
        }

        = false;

        public bool IsPrime
        {
            get;
            private set;
        }

        public long CurrentNumber
        {
            get => this.currentNumber;
            set
            {
                if (this.IsSearching)
                {
                    return;
                }

                this.currentNumber = value;
            }
        }

        public void CheckCurrentNumber()
        {
            this.IsPrime = true;

            if (this.CurrentNumber <= 2)
            {
                this.IsPrime = true;
                return;
            }

            for (long i = 2; i <= Math.Sqrt(this.CurrentNumber); i++)
            {
                if (this.CurrentNumber % i == 0)
                {
                    // Set not a prime flag to true.
                    this.IsPrime = false;
                    break;
                }
            }
        }

        /// <summary>
        /// This starts a seperate task and is therefore executy async.
        /// </summary>
        /// <returns>The task that is executed. The task can be awaited but doesn't need to.</returns>
        public Task StartSearchInTask(int level = 0)
        {
            Console.WriteLine($"{Tabs.Add(level)}{nameof(StartSearchInTask)} ...");

            if (this.IsSearching)
            {
                return Task.CompletedTask;
            }

            this.IsSearching = true;
            var t = Task.Run(() =>
            {
                Thread.CurrentThread.Name = "My search worker";
                Search(level + 1);
            });

            Console.WriteLine($"{Tabs.Add(level)}{nameof(StartSearchInTask)} done.");

            return t;
        }

        /// <summary>
        /// This starts a seperate task and is therefore executy async.
        /// </summary>
        /// <returns></returns>
        public async Task StartSearchInTaskAsync(int level = 0)
        {
            Console.WriteLine($"{Tabs.Add(level)}{nameof(StartSearchInTaskAsync)} ...");
            await StartSearchInTask(level + 1);
            Console.WriteLine($"{Tabs.Add(level)}{nameof(StartSearchInTaskAsync)} done.");
        }

        public async Task StartSearchAsync(int level = 0)
        {
            Console.WriteLine($"{Tabs.Add(level)}{nameof(StartSearchAsync)} ...");
            this.StartSearch(level + 1);
            Console.WriteLine($"{Tabs.Add(level)}{nameof(StartSearchAsync)} done.");
        }

        public async Task StartSearchAsyncInTask(int level = 0)
        {
            Console.WriteLine($"{Tabs.Add(level)}{nameof(StartSearchAsyncInTask)} ...");
            await Task.Run(() => this.StartSearch(level + 1));
            Console.WriteLine($"{Tabs.Add(level)}{nameof(StartSearchAsyncInTask)} done.");
        }

        public async Task StartSearchAsyncInTaskAwaitTask(int level )
        {
            Console.WriteLine($"{Tabs.Add(level)}{nameof(StartSearchAsyncInTaskAwaitTask)} ...");
            await Task.Run(async() => await StartSearchAsyncTask(level + 1));
            Console.WriteLine($"{Tabs.Add(level)}{nameof(StartSearchAsyncInTaskAwaitTask)} done.");
        }

        public async Task StartSearchAsyncTaskAsync(int level = 0)
        {
            Console.WriteLine($"{Tabs.Add(level)}{nameof(StartSearchAsyncTaskAsync)} ...");
            await this.StartSearchAsyncTask(level + 1);
            Console.WriteLine($"{Tabs.Add(level)}{nameof(StartSearchAsyncTaskAsync)} done.");
        }

        public async Task StartSearchAsyncTask(int level = 0)
        {
            Console.WriteLine($"{Tabs.Add(level)}{nameof(StartSearchAsyncTask)} ...");
            this.StartSearch(level + 1);
            Console.WriteLine($"{Tabs.Add(level)}{nameof(StartSearchAsyncTask)} done.");
        }

        public void StartSearch(int level = 0)
        {
            Console.WriteLine($"{Tabs.Add(level)}{nameof(StartSearch)} ...");
            if (this.IsSearching)
            {
                Console.WriteLine($"{Tabs.Add(level)}{nameof(StartSearch)} done.");
                return;
            }

            this.IsSearching = true;
            this.Search(level + 1);
            Console.WriteLine($"{Tabs.Add(level)}{nameof(StartSearch)} done.");
        }

        public void StopSearch()
        {
            this.IsSearching = false;
        }

        private void Search(int level = 0)
        {
            Console.WriteLine($"{Tabs.Add(level)}Search started in thread {Thread.CurrentThread.Name}.");

            while (this.IsSearching && this.CurrentNumber < long.MaxValue)
            {
                this.currentNumber++;
                this.CheckCurrentNumber();

                if (this.IsPrime)
                {
                    this.NewPrimeNumberFound?.Invoke(this, new PrimeNumberEventArgs(this.CurrentNumber));
                }
            }

            Console.WriteLine($"{Tabs.Add(level)}Search stopped.");
        }
    }
}
