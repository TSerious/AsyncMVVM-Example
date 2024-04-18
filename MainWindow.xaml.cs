using System.Runtime.InteropServices;
using System.Windows;

namespace AsyncMvvm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [LibraryImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        private static partial int AllocConsole();

        public MainWindow()
        {
            Thread.CurrentThread.Name = "UI thread";
            _ = AllocConsole();
            InitializeComponent();
            Console.WriteLine($"Current (main/ui) thread is: {Thread.CurrentThread.Name}");
        }

        private void ClearConsole_Click(object sender, RoutedEventArgs e)
        {
            Console.Clear();
        }
    }
}