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
		MainWindowViewModel viewModel;
		public MainWindow()
		{
			InitializeComponent();
			
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			viewModel = new MainWindowViewModel(new BlueWindow());
			this.Close();
		}

		private void Button_ClickQuit(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}