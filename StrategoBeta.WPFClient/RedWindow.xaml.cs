using StrategoBeta.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StrategoBeta.WPFClient
{
	/// <summary>
	/// Interaction logic for BlueWindow.xaml
	/// </summary>
	public partial class RedWindow : Window
	{
		public event EventHandler<ButtonClickedEventArgs> ButtonClickedEvent;
		int row;
		int column;
		int selectedRowForMoving;
		int selectedColumnforMoving;
		MainWindowViewModel viewModel;
		public RedWindow()
		{
			InitializeComponent();
			//DataContext = new MainWindowViewModel();
			this.Loaded += new RoutedEventHandler(OnLoaded);
			SubscribeToButtonClickEvents();
		}
		public class ButtonClickedEventArgs : EventArgs
		{
			public int Row { get; }
			public int Column { get; }
			public Button button { get; }
			public ButtonClickedEventArgs(int row, int column, Button button)
			{
				Row = row;
				Column = column;
				this.button = button;
			}
		}

		private void SubscribeToButtonClickEvents()
		{
			foreach (var button in playingField.Children.OfType<Button>())
			{
				button.Click += Field_Click;
			}
		}

		private void Field_Click(object sender, RoutedEventArgs e)
		{
			//Gets the clicked buttons row and column from the DataCotext and sends it to the ViewModel
			Button button = sender as Button;
			Piece currentPiece = button.DataContext as Piece;
			row = currentPiece.Row;
			column = currentPiece.Column;
			ButtonClickedEvent?.Invoke(this, new ButtonClickedEventArgs(row, column, button));
		}

		public void OnLoaded(object sender, RoutedEventArgs e)
		{
			viewModel = this.DataContext as MainWindowViewModel;
		}
	}
}
