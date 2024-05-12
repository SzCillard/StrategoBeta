using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.VisualBasic;
using StrategoBeta.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StrategoBeta.WPFClient
{
	internal class MainWindowViewModel : ObservableRecipient
	{
		BlueWindow blueWindow;
		RedWindow redWindow;
		Team actualTeam;
        bool first = true;
        public bool InitialPlacement { get; set; } = true;
		bool placed = true;
		bool ReadyToPlace = false;
		int actualSelectedidx;
		int actualSelectedRow;
		int actualSelectedColumn;
		int oldRow;
		int oldCol;
		Rank selectedRank;
		public Rank SelectedRank
		{
			get { return selectedRank; }
			set
			{
				SetProperty(ref selectedRank, value);
			}
		}
		//side bar Command for placing piecies
		public event EventHandler RankSelectionEvent;
		public event EventHandler ReadyEvent;
		public ICommand AddMarshalCommand { get; set; }
		public ICommand AddGeneralCommand { get; set; }
		public ICommand AddColonelCommand { get; set; }
		public ICommand AddMajorCommand { get; set; }
		public ICommand AddCaptainCommand { get; set; }
		public ICommand AddLieutenantCommand { get; set; }
		public ICommand AddSergeantCommand { get; set; }
		public ICommand AddMinerCommand { get; set; }
		public ICommand AddScoutCommand { get; set; }
		public ICommand AddSpyCommand { get; set; }
		public ICommand AddMineCommand { get; set; }
		public ICommand AddFlagCommand { get; set; }
		public ICommand ReadyCommand { get; set; }
		public ICommand EndTurnCommand { get; set; }
		public ObservableCollection<Piece> Pieces { set; get; } = new ObservableCollection<Piece>();
		public MainWindowViewModel()
		{

        }
		public MainWindowViewModel(BlueWindow bluewindow, RedWindow redwindow)
		{
			FillWithEmptyButtons();
			CommandSetup();
			actualTeam = Team.Blue;
			blueWindow = bluewindow;
			blueWindow.Show();
			blueWindow.DataContext = this;
			blueWindow.ButtonClickedEvent += ClickOnPlayingFieldEvent;
			redWindow = redwindow;
			redWindow.DataContext = this;
			redwindow.ButtonClickedEvent += ClickOnPlayingFieldEvent;
		}

        private void ClickOnPlayingFieldEvent(object? sender, RedWindow.ButtonClickedEventArgs e)
        {
            // Gets the row and column of the button that was clicked
            int row = e.Row;
            int column = e.Column;
            Button button = e.button;

            //Only while pieces are being placed
            if (InitialPlacement && !placed)
            {
                var index = (10 * (row - 1) + column) - 1;
                Pieces[index] = new Piece(new Character(SelectedRank, Team.Red), row, column);
                placed = true;
                SelectedRank = Rank.Empty;
                SetStyle(button, Team.Red);
                AddPicture(Pieces[index], button, Team.Red);
            }
            else
            {
                //Movement
                if (ReadyToPlace)
                {
                    //Sets the value of the variables to the position where the piece will move
                    actualSelectedRow = row;
                    actualSelectedColumn = column;

                    //Calculates the place where the piece will move
                    int selectedIdx = (10 * (actualSelectedRow - 1) + actualSelectedColumn) - 1;

                    //Calculates if a piece can move according to it's maximum step
                    if (CalcIfCanMove(Pieces[actualSelectedidx], oldCol, actualSelectedColumn, oldRow, actualSelectedRow))
                    {
                        PieceMoving(button, selectedIdx);
                        ReadyToPlace = false;
                    }
                }
                else
                {
                    //Sets the value of the variables to the position where the piece moved from
                    oldRow = row;
                    oldCol = column;

                    //Calculates the index of the place where the piece was
                    actualSelectedidx = (10 * (oldRow - 1) + oldCol) - 1;

                    //Selects the piece that will be moved
                    SelectedRank = Pieces[actualSelectedidx].Character.Rank;
                    if (SelectedRank == Rank.Empty)
                    {

                    }
                    else
                    {
                        //Sets the style for the button in the old position
                        SetStyle(button, Team.Empty);
                        button.Background = null;
                        ReadyToPlace = true;
                    }
                }
            }
        }

        private void ClickOnPlayingFieldEvent(object? sender, BlueWindow.ButtonClickedEventArgs e)
		{
			// Gets the row and column of the button that was clicked
			int row = e.Row;
			int column = e.Column;
			Button button = e.button;

            //Only while pieces are being placed
            if (InitialPlacement && !placed)
			{
				var index = (10 * (row - 1) + column) - 1;
				Pieces[index] = new Piece(new Character(SelectedRank, Team.Blue), row, column);
				placed = true;
				SelectedRank = Rank.Empty;
				SetStyle(button, Team.Blue);
                AddPicture(Pieces[index], button, Team.Blue);
            }
			else 
			{
				//Movement
				if (ReadyToPlace)
				{
					//Sets the value of the variables to the position where the piece will move
					actualSelectedRow = row;
					actualSelectedColumn = column;

                    //Calculates the place where the piece will move
                    int selectedIdx = (10 * (actualSelectedRow - 1) + actualSelectedColumn) - 1;

					//Calculates if a piece can move according to it's maximum step
					if (CalcIfCanMove(Pieces[actualSelectedidx],oldCol,actualSelectedColumn,oldRow,actualSelectedRow))
					{
                        PieceMoving(button, selectedIdx);
                        ReadyToPlace = false;
                    }
				}
				else
				{
                    //Sets the value of the variables to the position where the piece moved from
                    oldRow = row;
                    oldCol = column;

					//Calculates the index of the place where the piece was
                    actualSelectedidx = (10 * (oldRow - 1) + oldCol) - 1;

                    //Selects the piece that will be moved
                    SelectedRank = Pieces[actualSelectedidx].Character.Rank;
					if (SelectedRank == Rank.Empty)
					{

					}
					else
					{
                        //Sets the style for the button in the old position
                        SetStyle(button, Team.Empty);
                        button.Background = null;
                        ReadyToPlace = true;
                    }
                }
			}
		}

		void CommandSetup()
		{
			AddMarshalCommand = new RelayCommand(
				() => SelectMarshal(),
				() => !Pieces.Any(piece => piece.Character.RankPower == 10)
				);
			AddGeneralCommand = new RelayCommand(
				() => SelectGeneral(),
				() => !Pieces.Any(piece => piece.Character.RankPower == 9)
				);
			AddColonelCommand = new RelayCommand(
				() => SelectColonel(),
				() => Pieces.Count(piece => piece.Character.RankPower == 8) <= 2
				);
			AddMajorCommand = new RelayCommand(
				() => SelectMajor(),
				() => Pieces.Count(piece => piece.Character.RankPower == 7) <= 3
				);
			AddCaptainCommand = new RelayCommand(
				() => SelectCaptain(),
				() => Pieces.Count(piece => piece.Character.RankPower == 6) <= 4
				);
			AddLieutenantCommand = new RelayCommand(
				() => SelectLieutenant(),
				() => Pieces.Count(piece => piece.Character.RankPower == 5) <= 4
				);
			AddSergeantCommand = new RelayCommand(
				() => SelectSergeant(),
				() => Pieces.Count(piece => piece.Character.RankPower == 4) <= 4
				);
			AddMinerCommand = new RelayCommand(
				() => SelectMiner(),
				() => Pieces.Count(piece => piece.Character.RankPower == 3) <= 5
				);
			AddScoutCommand = new RelayCommand(
				() => SelectScout(),
				() => Pieces.Count(piece => piece.Character.RankPower == 2) <= 8
				);
			AddSpyCommand = new RelayCommand(
				() => SelectSpy(),
				() => !Pieces.Any(piece => piece.Character.RankPower == 1)
				);
			AddMineCommand = new RelayCommand(
				() => SelectMine(),
				() => Pieces.Count(piece => piece.Character.RankPower == 11) <= 6
				);
			AddFlagCommand = new RelayCommand(
				() => SelectFlag(),
				() => !Pieces.Any(piece => piece.Character.RankPower == 0)
				);
			ReadyCommand = new RelayCommand(() => Ready());
			EndTurnCommand = new RelayCommand(() => EndTurn());
		}

		void PieceMoving(Button button, int selectedIdx)
		{
			//Checks if the grid that the piece will move to is empty
			bool gridIsEmpty = Pieces[selectedIdx].Character.Rank == Rank.Empty;
			if (gridIsEmpty)
			{
				//Moves the piece to the selected position
				Pieces[selectedIdx] = new Piece(new Character(SelectedRank, actualTeam), actualSelectedRow, actualSelectedColumn);
				Pieces[selectedIdx] = new Piece(new Character(SelectedRank, Pieces[selectedIdx].Character.Team), actualSelectedRow, actualSelectedColumn);
				//Sets the style for the button in the new position
				SetStyle(button, Pieces[selectedIdx].Character.Team);
				AddPicture(Pieces[selectedIdx], button, Pieces[selectedIdx].Character.Team);

				//Sets sets the old position to an empty character
                Pieces[actualSelectedidx] = new Piece(new Character(Rank.Empty, Team.Empty), oldRow, oldCol);
            }
			else
			{
				var index = (10 * (actualSelectedRow - 1) + actualSelectedColumn) - 1;
				bool IsPieceFriendly = Pieces[index].Character.Team.Equals(actualTeam);
				if (IsPieceFriendly)
				{

				}
				else
				{
					Battle(Pieces[actualSelectedidx], Pieces[index], button);
				}
				
			}
		}
		void Battle(Piece attacker, Piece defender, Button button)
		{	
				if (attacker.Character.RankPower<defender.Character.RankPower)
				{
					Pieces[actualSelectedidx] = new Piece(new Character(Rank.Empty, Team.Empty), actualSelectedRow, actualSelectedColumn);
				}
				else if (attacker.Character.RankPower>defender.Character.RankPower)
				{

					Pieces[actualSelectedidx] = new Piece(attacker.Character, defender.Row, defender.Column);
				}
				else
				{
					Pieces[actualSelectedidx] = new Piece(new Character(Rank.Empty, Team.Empty), defender.Row, defender.Column);
					var index = (10 * (oldRow - 1) + oldCol) - 1;
					Pieces[index] = new Piece(new Character(Rank.Empty, Team.Empty), defender.Row, defender.Column);
				}

		}
		void FillWithEmptyButtons()
		{
			//Fills the Pieces collection with buttons so they show up in the window.
			for (int i = 0; i < 12; i++)
			{
				for (int j = 0; j < 12; j++)
				{
					if (i >= 1 && i < 11 && j >= 1 && j < 11)
					{
						Pieces.Add(new Piece(new Character(Rank.Empty, Team.Empty), i, j));
					}
				}
			}
		}

		//God forgive me for i have sinned (i do not feel proud of this)
		private void SelectMarshal()
		{
			SelectedRank = Rank.Marshal;
			RankSelectionEvent?.Invoke(this, null);
			placed = false;
		}
		private void SelectGeneral()
		{
			SelectedRank = Rank.General;
			RankSelectionEvent?.Invoke(this, null);
			placed = false;
		}
		private void SelectColonel()
		{
			SelectedRank = Rank.Colonel;
			RankSelectionEvent?.Invoke(this, null);
			placed = false;
		}
		private void SelectMajor()
		{
			SelectedRank = Rank.Major;
			RankSelectionEvent?.Invoke(this, null);
			placed = false;
		}
		private void SelectCaptain()
		{
			SelectedRank = Rank.Captain;
			RankSelectionEvent?.Invoke(this, null);
			placed = false;
		}
		private void SelectLieutenant()
		{
			SelectedRank = Rank.Lieutenant;
			RankSelectionEvent?.Invoke(this, null);
			placed = false;
		}
		private void SelectSergeant()
		{
			SelectedRank = Rank.Sergeant;
			RankSelectionEvent?.Invoke(this, null);
			placed = false;
		}
		private void SelectMiner()
		{
			SelectedRank = Rank.Miner;
			RankSelectionEvent?.Invoke(this, null);
			placed = false;
		}
		private void SelectScout()
		{
			SelectedRank = Rank.Scout;
			RankSelectionEvent?.Invoke(this, null);
			placed = false;
		}
		private void SelectSpy()
		{
			SelectedRank = Rank.Spy;
			RankSelectionEvent?.Invoke(this, null);
			placed = false;
		}
		private void SelectMine()
		{
			SelectedRank = Rank.Mine;
			RankSelectionEvent?.Invoke(this, null);
			placed = false;
		}
		private void SelectFlag()
		{
			SelectedRank = Rank.Flag;
			RankSelectionEvent?.Invoke(this, null);
			placed = false;
		}
		//Changes initialPlacement to True (finished placing down the pieces)
		private void Ready()
		{
			InitialPlacement = false;
			var a = Pieces;
			ReadyEvent?.Invoke(this, null);
		}
		public void AddPicture(Piece piece, Button button, Team team)
		{
			if (team == Team.Blue)
			{
                switch (piece.Character.Rank)
                {
                    case Rank.Flag:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/blueFlag.png", UriKind.Relative)));
                        break;
                    case Rank.Spy:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/bluePiece10.png", UriKind.Relative)));
                        break;
                    case Rank.Scout:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/bluePiece9.png", UriKind.Relative)));
                        break;
                    case Rank.Miner:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/bluePiece8.png", UriKind.Relative)));
                        break;
                    case Rank.Sergeant:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/bluePiece7.png", UriKind.Relative)));
                        break;
                    case Rank.Lieutenant:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/bluePiece6.png", UriKind.Relative)));
                        break;
                    case Rank.Captain:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/bluePiece5.png", UriKind.Relative)));
                        break;
                    case Rank.Major:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/bluePiece4.png", UriKind.Relative)));
                        break;
                    case Rank.Colonel:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/bluePiece3.png", UriKind.Relative)));
                        break;
                    case Rank.General:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/bluePiece2.png", UriKind.Relative)));
                        break;
                    case Rank.Marshal:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/bluePiece1.png", UriKind.Relative)));
                        break;
                    case Rank.Mine:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/blue_bomb.png", UriKind.Relative)));
                        break;
                }
            }
			else if (team == Team.Red)
			{
                switch (piece.Character.Rank)
                {
                    case Rank.Flag:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/redFlag.png", UriKind.Relative)));
                        break;
                    case Rank.Spy:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/redPiece10.png", UriKind.Relative)));
                        break;
                    case Rank.Scout:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/redPiece9.png", UriKind.Relative)));
                        break;
                    case Rank.Miner:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/redPiece8.png", UriKind.Relative)));
                        break;
                    case Rank.Sergeant:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/redPiece7.png", UriKind.Relative)));
                        break;
                    case Rank.Lieutenant:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/redPiece6.png", UriKind.Relative)));
                        break;
                    case Rank.Captain:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/redPiece5.png", UriKind.Relative)));
                        break;
                    case Rank.Major:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/redPiece4.png", UriKind.Relative)));
                        break;
                    case Rank.Colonel:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/redPiece3.png", UriKind.Relative)));
                        break;
                    case Rank.General:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/redPiece2.png", UriKind.Relative)));
                        break;
                    case Rank.Marshal:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/redPiece1.png", UriKind.Relative)));
                        break;
                    case Rank.Mine:
                        button.Background = new ImageBrush(new BitmapImage(new Uri(@"Images/red_bomb.png", UriKind.Relative)));
                        break;
                }
            }
        }
		private void SetStyle(Button button, Team team)
		{
			if (team == Team.Blue)
			{
                button.Style = blueWindow.FindResource("BlueCharacterButton") as Style;

            }
			else if (team == Team.Red)
			{
                button.Style = redWindow.FindResource("RedCharacterButton") as Style;
            }
			else
			{
                button.Style = redWindow.FindResource("HiddenButton") as Style;
            }
        }
		void EndTurn()
		{
			if (actualTeam is Team.Blue)
			{
				actualTeam = Team.Red;
				blueWindow.Visibility = Visibility.Hidden;
				redWindow.Show();
				if (first)
				{
                    InitialPlacement = true;
					placed = false;
					ReadyToPlace = false;
					first = false;
				}
			}
			else
			{
				actualTeam = Team.Blue;
				redWindow.Visibility = Visibility.Hidden;
				blueWindow.Show();
			}
		}
		private bool CalcIfCanMove(Piece piece, int oldCol, int selectedCol, int oldRow, int actualSelectedRow)
		{
			int calculatedStep = actualSelectedRow - oldRow;
			if (selectedCol == oldCol && piece.Character.MaxStep >= calculatedStep && calculatedStep != 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
