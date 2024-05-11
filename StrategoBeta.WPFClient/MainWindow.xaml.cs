using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StrategoBeta.WPFClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		BlueWindow blueWindow;
		RedWindow redWindow;
		MainWindowViewModel viewModel;
		public MainWindow()
		{
			InitializeComponent();
			blueWindow = new BlueWindow();
			redWindow = new RedWindow();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			viewModel = new MainWindowViewModel(blueWindow, redWindow);
			blueWindow.Show();
			this.Close();
		}

		private void Button_ClickQuit(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}