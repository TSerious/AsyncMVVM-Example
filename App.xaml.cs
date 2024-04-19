using ReactiveUI;
using Splat;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Windows;

namespace AsyncMvvm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
        }
    }

}
