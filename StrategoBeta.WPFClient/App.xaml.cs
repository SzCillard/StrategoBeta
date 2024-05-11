using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using StrategoBeta.Logic.ArmyLogic;
using StrategoBeta.Logic.Logic;
using System.Configuration;
using System.Data;
using System.Windows;

namespace StrategoBeta.WPFClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
	{
        public App()
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                .AddSingleton<IArmyLogic, ArmyLogic>()
                .AddSingleton<IMessenger>(WeakReferenceMessenger.Default)
                .BuildServiceProvider()
                );
        }
    }

}
