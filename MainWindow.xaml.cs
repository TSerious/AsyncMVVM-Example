using AsyncMvvm.ViewModels;
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

        [LibraryImport("kernel32.dll")]
        private static partial IntPtr GetConsoleWindow();

        [LibraryImport("user32.dll")]
        private static partial uint GetDpiForWindow(IntPtr hwnd);

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
            this.DataContext = this;

            Console.WriteLine($"Current (main/ui) thread is: {Thread.CurrentThread.Name}");

            this.Loaded += this.MainWindow_Loaded;
        }

        public ReactiveControlViewModel AdditionalTab { get; } = new("10", "Test message");

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
            var dpi = GetDpiForWindow(this.consoleWindow);
            int scaledX = (int)((this.Left + this.ActualWidth) * (dpi / 96));

            SetWindowPos(
                this.consoleWindow,
                HWNDTOP,
                scaledX,
                0,
                0,
                0,
                SWPNOSIZE | SWPNOZORDER);
        }
    }
}