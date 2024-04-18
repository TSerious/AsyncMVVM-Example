using AsyncMvvm.Model;
using System.Windows.Input;

namespace AsyncMvvm
{
    // Source: https://learn.microsoft.com/en-us/archive/msdn-magazine/2014/april/async-programming-patterns-for-asynchronous-mvvm-applications-commands
    public class AsyncCommand(Func<int, Task> command) : ICommand
    {
        protected readonly Func<int, Task> command = command;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public virtual Task ExecuteAsync(int level)
        {
            Console.WriteLine($"{Tabs.Add(level)}{nameof(this.ExecuteAsync)} called in {Thread.CurrentThread.Name}.");
            var t = this.command(level + 1);
            Console.WriteLine($"{Tabs.Add(level)}{nameof(this.ExecuteAsync)} called in {Thread.CurrentThread.Name} done.");
            return t;
        }

        public async void Execute(object? parameter)
        {
            int level = parameter == null ? 0 : (int)parameter;
            if (level == 0 )
            {
                Console.WriteLine("-------------------------------------------------------------------");
            }

            Console.WriteLine($"{Tabs.Add(level)}{nameof(this.Execute)} called in {Thread.CurrentThread.Name}.");
            await ExecuteAsync(level + 1);
            Console.WriteLine($"{Tabs.Add(level)}{nameof(this.Execute)} called in {Thread.CurrentThread.Name} done.");
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
