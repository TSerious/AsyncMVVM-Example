using AsyncMvvm.Model;

namespace AsyncMvvm
{
    public class AsyncCommandStart(Func<int, Task> command) : AsyncCommand(command)
    {
        public override Task ExecuteAsync(int level)
        {
            Console.WriteLine($"{Tabs.Add(level)}async {nameof(this.ExecuteAsync)} called in {Thread.CurrentThread.Name}.");
            var t = this.command(level + 1);
            t.Start();
            Console.WriteLine($"{Tabs.Add(level)}async {nameof(this.ExecuteAsync)} called in {Thread.CurrentThread.Name} done.");
            return t;
        }
    }
}
