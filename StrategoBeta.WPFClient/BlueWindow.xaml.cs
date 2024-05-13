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
	public partial class BlueWindow : Window
	{
		public event EventHandler<ButtonClickedEventArgs> ButtonClickedEvent;
		int row;
		int column;
		int selectedRowForMoving;
		int selectedColumnforMoving;
		MainWindowViewModel viewModel;
		public BlueWindow()
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
			ButtonClickedEvent?.Invoke(this, new ButtonClickedEventArgs(row, column,button));
		}

        public void OnLoaded(object sender, RoutedEventArgs e)
        {
            viewModel = this.DataContext as MainWindowViewModel;
        }
        public void ChangeMenu()
        {
            if (Title == "Red")
            {
                ControlTemplate ct = buttonFlag.Template;
                Image btnImage = (Image)ct.FindName("buttonFlagIMG", buttonFlag);
                btnImage.Source = new BitmapImage(new Uri(@"Images/redFlag.png", UriKind.Relative));

                ct = buttonSpy.Template;
                btnImage = (Image)ct.FindName("buttonSpyIMG", buttonSpy);
                btnImage.Source = new BitmapImage(new Uri(@"Images/redPiece9.png", UriKind.Relative));

                ct = buttonScout.Template;
                btnImage = (Image)ct.FindName("buttonScoutIMG", buttonScout);
                btnImage.Source = new BitmapImage(new Uri(@"Images/redPiece9.png", UriKind.Relative));

                ct = buttonMiner.Template;
                btnImage = (Image)ct.FindName("buttonMinerIMG", buttonMiner);
                btnImage.Source = new BitmapImage(new Uri(@"Images/redPiece8.png", UriKind.Relative));

                ct = buttonSergeant.Template;
                btnImage = (Image)ct.FindName("buttonSergeantIMG", buttonSergeant);
                btnImage.Source = new BitmapImage(new Uri(@"Images/redPiece7.png", UriKind.Relative));

                ct = buttonLieutenant.Template;
                btnImage = (Image)ct.FindName("buttonLieutenantIMG", buttonLieutenant);
                btnImage.Source = new BitmapImage(new Uri(@"Images/redPiece6.png", UriKind.Relative));

                ct = buttonCaptain.Template;
                btnImage = (Image)ct.FindName("buttonCaptainIMG", buttonCaptain);
                btnImage.Source = new BitmapImage(new Uri(@"Images/redPiece5.png", UriKind.Relative));

                ct = buttonMajor.Template;
                btnImage = (Image)ct.FindName("buttonMajorIMG", buttonMajor);
                btnImage.Source = new BitmapImage(new Uri(@"Images/redPiece4.png", UriKind.Relative));

                ct = buttonColonel.Template;
                btnImage = (Image)ct.FindName("buttonColonelIMG", buttonColonel);
                btnImage.Source = new BitmapImage(new Uri(@"Images/redPiece3.png", UriKind.Relative));

                ct = buttonGeneral.Template;
                btnImage = (Image)ct.FindName("buttonGeneralIMG", buttonGeneral);
                btnImage.Source = new BitmapImage(new Uri(@"Images/redPiece2.png", UriKind.Relative));

                ct = buttonMarshal.Template;
                btnImage = (Image)ct.FindName("buttonMarshalIMG", buttonMarshal);
                btnImage.Source = new BitmapImage(new Uri(@"Images/redPiece1.png", UriKind.Relative));

                ct = buttonMine.Template;
                btnImage = (Image)ct.FindName("buttonMineIMG", buttonMine);
                btnImage.Source = new BitmapImage(new Uri(@"Images/red_bomb.png", UriKind.Relative));
            }
            else if (Title == "Blue")
            {
                ControlTemplate ct = buttonFlag.Template;
                Image btnImage = (Image)ct.FindName("buttonFlagIMG", buttonFlag);
                btnImage.Source = new BitmapImage(new Uri(@"Images/blueFlag.png", UriKind.Relative));

                ct = buttonSpy.Template;
                btnImage = (Image)ct.FindName("buttonSpyIMG", buttonSpy);
                btnImage.Source = new BitmapImage(new Uri(@"Images/bluePiece9.png", UriKind.Relative));

                ct = buttonScout.Template;
                btnImage = (Image)ct.FindName("buttonScoutIMG", buttonScout);
                btnImage.Source = new BitmapImage(new Uri(@"Images/bluePiece9.png", UriKind.Relative));

                ct = buttonMiner.Template;
                btnImage = (Image)ct.FindName("buttonMinerIMG", buttonMiner);
                btnImage.Source = new BitmapImage(new Uri(@"Images/bluePiece8.png", UriKind.Relative));

                ct = buttonSergeant.Template;
                btnImage = (Image)ct.FindName("buttonSergeantIMG", buttonSergeant);
                btnImage.Source = new BitmapImage(new Uri(@"Images/bluePiece7.png", UriKind.Relative));

                ct = buttonLieutenant.Template;
                btnImage = (Image)ct.FindName("buttonLieutenantIMG", buttonLieutenant);
                btnImage.Source = new BitmapImage(new Uri(@"Images/bluePiece6.png", UriKind.Relative));

                ct = buttonCaptain.Template;
                btnImage = (Image)ct.FindName("buttonCaptainIMG", buttonCaptain);
                btnImage.Source = new BitmapImage(new Uri(@"Images/bluePiece5.png", UriKind.Relative));

                ct = buttonMajor.Template;
                btnImage = (Image)ct.FindName("buttonMajorIMG", buttonMajor);
                btnImage.Source = new BitmapImage(new Uri(@"Images/bluePiece4.png", UriKind.Relative));

                ct = buttonColonel.Template;
                btnImage = (Image)ct.FindName("buttonColonelIMG", buttonColonel);
                btnImage.Source = new BitmapImage(new Uri(@"Images/bluePiece3.png", UriKind.Relative));

                ct = buttonGeneral.Template;
                btnImage = (Image)ct.FindName("buttonGeneralIMG", buttonGeneral);
                btnImage.Source = new BitmapImage(new Uri(@"Images/bluePiece2.png", UriKind.Relative));

                ct = buttonMarshal.Template;
                btnImage = (Image)ct.FindName("buttonMarshalIMG", buttonMarshal);
                btnImage.Source = new BitmapImage(new Uri(@"Images/bluePiece1.png", UriKind.Relative));

                ct = buttonMine.Template;
                btnImage = (Image)ct.FindName("buttonMineIMG", buttonMine);
                btnImage.Source = new BitmapImage(new Uri(@"Images/blue_bomb.png", UriKind.Relative));
            }
        }
    }
}
