using AsyncMvvm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AsyncMvvm
{
    public class AwaitAsyncCommand(Func<int, Task> command) : AsyncCommand(command)
    {
        public override async Task ExecuteAsync(int level)
        {
            Console.WriteLine($"{Tabs.Add(level)}async {nameof(this.ExecuteAsync)} called in {Thread.CurrentThread.Name}.");
            await this.command(level + 1);
            Console.WriteLine($"{Tabs.Add(level)}async {nameof(this.ExecuteAsync)} called in {Thread.CurrentThread.Name} done.");
        }
    }
}
