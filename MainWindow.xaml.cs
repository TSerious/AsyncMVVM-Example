using ReactiveUI;
using System.Runtime.InteropServices;
using System.Windows;
using System.Reactive.Linq;

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

        [LibraryImport("kernel32.dll")]
        private static partial IntPtr GetConsoleWindow();

        [LibraryImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        private const UInt32 SWPNOSIZE = 0x0001;
        private const UInt32 SWPNOZORDER = 0x0004;
        private static readonly IntPtr HWNDTOP = new(0);
        private readonly IntPtr consoleWindow;


        public MainWindow()
        {
            Thread.CurrentThread.Name = "UI thread";
            _ = AllocConsole();
            this.consoleWindow = GetConsoleWindow();
            this.InitializeComponent();

            Console.WriteLine($"Current (main/ui) thread is: {Thread.CurrentThread.Name}");

            this.Loaded += this.MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, EventArgs e)
        {
            this.AlignConsole();
        }

        private void ClearConsole_Click(object sender, RoutedEventArgs e)
        {
            Console.Clear();
        }

        private void AlignConsole()
        {
            SetWindowPos(
                this.consoleWindow,
                HWNDTOP,
                (int)(this.Left + this.ActualWidth) + 100,
                0,
                0,
                0,
                SWPNOSIZE | SWPNOZORDER);
        }
    }
}