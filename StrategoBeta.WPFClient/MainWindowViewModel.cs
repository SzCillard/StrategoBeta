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
        Team actualTeam;
        bool first;
        public bool InitialPlacement { get; set; }
        bool placed;
        bool ReadyToPlace;
        bool readyIsEnabled;
		public bool ReadyIsEnabled {
            get 
            { return readyIsEnabled; }
            set 
            { 
                SetProperty(ref readyIsEnabled, value);
                (ReadyCommand as RelayCommand).NotifyCanExecuteChanged();
            } }
        int actualSelectedidx;
        int actualSelectedRow;
        int actualSelectedColumn;
        int oldRow;
        int oldCol;
        Button oldButton;
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
        public MainWindowViewModel(BlueWindow bluewindow)
        {
			first = true;
            InitialPlacement = true;
            placed = true;
            ReadyToPlace = false;
            CommandSetup();
			ReadyIsEnabled = true;
			actualTeam = Team.Blue;
            blueWindow = bluewindow;
			blueWindow.DataContext = this;
			FillWithEmptyButtons();
			blueWindow.ButtonClickedEvent += ClickOnPlayingFieldEvent;
			blueWindow.Show();

		}
		//Manages blue window
		private void ClickOnPlayingFieldEvent(object? sender, BlueWindow.ButtonClickedEventArgs e)
        {
            // Gets the row and column of the button that was clicked
            int row = e.Row;
            int column = e.Column;
            Button button = e.button;

            //Only while pieces are being placed
            if (InitialPlacement && !placed)
            {
                if (actualTeam is Team.Blue)
                {
					if (row >= 7)
					{
                        var index = CalcPieceIndex(row, column);
						Pieces[index] = new Piece(new Character(SelectedRank, actualTeam), row, column);
						placed = true;
						SelectedRank = Rank.Empty;
						SetStyle(button, Pieces[index], actualTeam);
					}

				}
                if (actualTeam is Team.Red)
                {
					if (row <=4)
					{
						var index = CalcPieceIndex(row, column);
						Pieces[index] = new Piece(new Character(SelectedRank, actualTeam), row, column);
						placed = true;
						SelectedRank = Rank.Empty;
						SetStyle(button, Pieces[index], actualTeam);
					}
				}
               
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
                    int selectedIdx = CalcPieceIndex(actualSelectedRow, actualSelectedColumn);

                    //Calculates if a piece can move according to it's maximum step
                    if (CalcIfCanMove(Pieces[actualSelectedidx], oldRow, oldCol))
                    {
                        //Keeps track of the button that was pressed 1 turn before
                        if (Pieces[selectedIdx].Character.Team == Team.Empty)
                        {
                            oldButton = button;
                        }
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
                    actualSelectedidx =CalcPieceIndex(oldRow, oldCol);

                    //Selects the piece that will be moved
                    SelectedRank = Pieces[actualSelectedidx].Character.Rank;
                    if (SelectedRank == Rank.Empty)
                    {

                    }
                    else if (SelectedRank == Rank.Flag || SelectedRank == Rank.Mine || Pieces[actualSelectedidx].Character.Team != actualTeam)
                    {

                    }
                    else
                    {
                        //Sets the style for the button in the old position
                        SetStyle(button, Team.Empty);
                        ReadyToPlace = true;
                    }
                }
            }
        }
        void CommandSetup()
        {
            AddMarshalCommand = new RelayCommand(
                () => SelectMarshal(),
                () => Pieces.Where(piece => piece.Character.Team == actualTeam).Count(piece => piece.Character.RankPower == 10) <= 0
                );
            AddGeneralCommand = new RelayCommand(
                () => SelectGeneral(),
                () => Pieces.Where(piece => piece.Character.Team == actualTeam).Count(piece => piece.Character.RankPower == 9) <= 0
                );
            AddColonelCommand = new RelayCommand(
                () => SelectColonel(),
                () => Pieces.Where(piece => piece.Character.Team == actualTeam).Count(piece => piece.Character.RankPower == 8) < 2
                );
            AddMajorCommand = new RelayCommand(
                () => SelectMajor(),
                () => Pieces.Where(piece => piece.Character.Team == actualTeam).Count(piece => piece.Character.RankPower == 7) < 3
                );
            AddCaptainCommand = new RelayCommand(
                () => SelectCaptain(),
                () => Pieces.Where(piece => piece.Character.Team == actualTeam).Count(piece => piece.Character.RankPower == 6) < 4
                );
            AddLieutenantCommand = new RelayCommand(
                () => SelectLieutenant(),
                () => Pieces.Where(piece => piece.Character.Team == actualTeam).Count(piece => piece.Character.RankPower == 5) < 4
                );
            AddSergeantCommand = new RelayCommand(
                () => SelectSergeant(),
                () => Pieces.Where(piece => piece.Character.Team == actualTeam).Count(piece => piece.Character.RankPower == 4) < 4
                );
            AddMinerCommand = new RelayCommand(
                () => SelectMiner(),
                () => Pieces.Where(piece => piece.Character.Team == actualTeam).Count(piece => piece.Character.RankPower == 3) < 5
                );
            AddScoutCommand = new RelayCommand(
                () => SelectScout(),
                () => Pieces.Where(piece => piece.Character.Team == actualTeam).Count(piece => piece.Character.RankPower == 2) < 8
                );
            AddSpyCommand = new RelayCommand(
                () => SelectSpy(),
                () => Pieces.Where(piece => piece.Character.Team == actualTeam).Count(piece => piece.Character.RankPower == 1) <= 0
                );
            AddMineCommand = new RelayCommand(
                () => SelectMine(),
                () => Pieces.Where(piece => piece.Character.Team == actualTeam).Count(piece => piece.Character.RankPower == 11) < 6
                );
            AddFlagCommand = new RelayCommand(
                () => SelectFlag(),
                () => Pieces.Where(piece => piece.Character.Team == actualTeam).Count(piece => piece.Character.RankPower == 0) <= 0
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
                Pieces[selectedIdx] = new Piece(new Character(SelectedRank, Pieces[actualSelectedidx].Character.Team), actualSelectedRow, actualSelectedColumn);
                //Sets the style for the button in the new position
                SetStyle(button, Pieces[selectedIdx], Pieces[selectedIdx].Character.Team);

                //Sets sets the old position to an empty character
                Pieces[actualSelectedidx] = new Piece(new Character(Rank.Empty, Team.Empty), oldRow, oldCol);
            }
            else
            {
                var index = CalcPieceIndex(actualSelectedRow, actualSelectedColumn);
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
            if (attacker.Character.RankPower < defender.Character.RankPower)
            {
                if (defender.Character.Rank == Rank.Mine && attacker.Character.Rank == Rank.Miner)
                {
                    //Sets the style (old position) of the winning piece
                    SetStyle(oldButton, Team.Empty);
                    int lostIdx = CalcPieceIndex(defender.Row, defender.Column);
                    //Moves the attacker to the defenders position
                    Pieces[lostIdx] = new Piece(attacker.Character, defender.Row, defender.Column);
                    //Sets the attackers original position to Empty
                    Pieces[actualSelectedidx] = new Piece(new Character(Rank.Empty, Team.Empty), attacker.Row, attacker.Column);
                    //Sets the style (new position) of the winning piece
                    SetStyle(button, attacker, attacker.Character.Team);
                }
                else
                {
                    //Sets the attackers to Empty
                    Pieces[actualSelectedidx] = new Piece(new Character(Rank.Empty, Team.Empty), attacker.Row, attacker.Column);
                    //Sets the style of the losing piece to empty
                    SetStyle(oldButton, Team.Empty);
                }
            }
            else if (attacker.Character.RankPower > defender.Character.RankPower)
            {
                //Sets the style (old position) of the winning piece
                SetStyle(oldButton, Team.Empty);
                int lostIdx = CalcPieceIndex(defender.Row, defender.Column);
                //Moves the attacker to the defenders position
                Pieces[lostIdx] = new Piece(attacker.Character, defender.Row, defender.Column);
                //Sets the attackers original position to Empty
                Pieces[actualSelectedidx] = new Piece(new Character(Rank.Empty, Team.Empty), attacker.Row,attacker.Column);
                //Sets the style (new position) of the winning piece
                SetStyle(button, attacker, attacker.Character.Team);
                if (defender.Character.Rank == Rank.Flag)
                {
                    //If the flag is defeated the game ends and goes back to the menu
                    MessageBox.Show($"{actualTeam} team won");
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    blueWindow.Close();
                }
            }
            else
            {
                //Claculates defender idx and sets the piece to empty
                int attackerIdx = CalcPieceIndex(defender.Row, defender.Column);
				Pieces[attackerIdx] = new Piece(new Character(Rank.Empty, Team.Empty), defender.Row, defender.Column);
                SetStyle(oldButton, Team.Empty);
                //Calculates attacker idx and sets the piece to empty
                var defenderIdx = CalcPieceIndex(oldRow, oldCol);
				Pieces[defenderIdx] = new Piece(new Character(Rank.Empty, Team.Empty), attacker.Row, attacker.Column);
                SetStyle(button, Team.Empty);
            }

        }
        void FillWithEmptyButtons()
        {
            //Fills the Pieces collection with buttons so they show up in the window.
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    if ((i == 5 || i == 6) && (j == 3 || j == 4 || j == 7 || j == 8))
                    {
                        Pieces.Add(new Piece(new Character(Rank.Lake, Team.Lake, blueWindow.FindResource("LakeCharacterButton") as Style), i, j));
                    }
                    else
                    {
                        Pieces.Add(new Piece(new Character(Rank.Empty, Team.Empty, blueWindow.FindResource("HiddenButton") as Style), i, j));
                    }					
				}
            }
            //foreach (Button button in blueWindow.playingField.Children.OfType<Button>())
            //{
            //    //Set the style of grid cell where it is LAKE
            //    int row = Grid.GetRow(button);
            //    int column = Grid.GetColumn(button);
            //    if (row == 5 || row == 6)
            //    {
            //        if (column == 3 || column == 4 || column == 7 || column == 8)
            //        {
            //            button.Style = blueWindow.FindResource("LakeCharacterButton") as Style;
            //        }
            //    }
            //}
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
        private void Ready()
        {
            //Changes initialPlacement to True (finished placing down the pieces)
            InitialPlacement = false;
            var a = Pieces;
            ReadyEvent?.Invoke(this, null);
            ReadyIsEnabled = false;
        }
        private void EndTurn()
        {
            if (actualTeam is Team.Blue)
            {
                actualTeam = Team.Red;
                blueWindow.Title = "Red";
                blueWindow.ChangeMenu();
                if (first)
                {
                    InitialPlacement = true;
                    placed = false;
                    ReadyToPlace = false;
                    first = false;
                    selectedRank = Rank.Empty;
                    ReadyIsEnabled = true;
                }
            }
            else
            {
                actualTeam = Team.Blue;
                blueWindow.Title = "Blue";
                blueWindow.ChangeMenu();
            }
        }
        private void AddPicture(Button button, Piece piece,Team team)
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
        void SetStyle(Button button, Piece piece, Team team)
        {
            if (team == Team.Blue)
            {
                button.Style = blueWindow.FindResource("BlueCharacterButton") as Style;
            }
            else if (team == Team.Red)
            {
                button.Style = blueWindow.FindResource("RedCharacterButton") as Style;
            }
            AddPicture(button, piece, team);
        }
        void SetStyle(Button button, Team team)
        { 
            if (team == Team.Empty) 
            {
				button.Style = blueWindow.FindResource("HiddenButton") as Style;
				button.Background = null;
			} 
        }
        private bool CalcIfCanMove(Piece piece, int oldRow,int oldCol)
        {
            int calculatedRow = Math.Abs(actualSelectedRow - oldRow);
            int calculatedCol = Math.Abs(actualSelectedColumn- oldCol);
            if ((piece.Character.MaxStep >= calculatedRow && piece.Character.MaxStep>=calculatedCol) && (oldRow == actualSelectedRow || oldCol == actualSelectedColumn))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        int CalcPieceIndex(int row,int col) 
        { 
            return (10 * (row - 1) + col) - 1;
		}

    }
}
