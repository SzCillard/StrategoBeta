using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using StrategoBeta.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StrategoBeta.WPFClient
{
    internal class MainWindowViewModel : ObservableRecipient
    {
        BlueWindow blueWindow;
        RedWindow redWindow;
        Team actualTeam;
		bool canPlacePiece;
		SelectedGridCell selectedgridcell;
		public SelectedGridCell SelectedGridCell
		{
			get { return selectedgridcell; }
			set 
			{ 
				SetProperty(ref selectedgridcell, value);
			}
		}
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

		public ObservableCollection<Piece> Pieces { set; get; } 
        public MainWindowViewModel()
        {
            
        }
        public MainWindowViewModel(BlueWindow bluewindow,RedWindow redwindow)
        {	Pieces = new ObservableCollection<Piece>();
			actualTeam = Team.Blue;
			CommandSetup();
			blueWindow = bluewindow;
			blueWindow.ButtonClickedEvent += ClickOnPlayingFieldEvent;
			redWindow = redwindow;
        }

		private void ClickOnPlayingFieldEvent(object? sender, BlueWindow.ButtonClickedEventArgs e)
		{
			// Handle the button click event here
			int row = e.Row;
			int column = e.Column;
			Button button = e.button;
			if(canPlacePiece)
			{
				Pieces.Add(new Character(Rank.Marshal, Team.Blue), row, column);
			}
			else 
			{ }
		}

		void CommandSetup()
        {
			AddMarshalCommand = new RelayCommand(
				() => SelectedRank=Rank.Marshal,
				() => !Pieces.Any(piece => piece.Character.RankPower == 10)
				);
			AddGeneralCommand = new RelayCommand(
				() => Pieces.Add(new Piece(new Character(Rank.General, actualTeam), SelectedGridCell.Row, SelectedGridCell.Column)),
				() => !Pieces.Any(piece => piece.Character.RankPower == 9)
				); 
			AddColonelCommand = new RelayCommand(
				() => Pieces.Add(new Piece(new Character(Rank.Colonel, actualTeam), 1, 1)),
				() => Pieces.Count(piece => piece.Character.RankPower == 8) <= 2
				); 
			AddMajorCommand = new RelayCommand(
				() => Pieces.Add(new Piece(new Character(Rank.Major, actualTeam), 1, 1)),
				() => Pieces.Count(piece => piece.Character.RankPower == 7) <= 3
				); 
			AddCaptainCommand = new RelayCommand(
				() => Pieces.Add(new Piece(new Character(Rank.Captain, actualTeam), 1, 1)),
				() => Pieces.Count(piece => piece.Character.RankPower == 6) <= 4
				); 
			AddLieutenantCommand = new RelayCommand(
				() => Pieces.Add(new Piece(new Character(Rank.Lieutenant, actualTeam),1,1)),
				() => Pieces.Count(piece => piece.Character.RankPower == 5) <=4
				); 
			AddSergeantCommand = new RelayCommand(
				() => Pieces.Add(new Piece(new Character(Rank.Sergeant, actualTeam),1,1)),
				() => Pieces.Count(piece => piece.Character.RankPower == 4) <=4
				);
			AddMinerCommand = new RelayCommand(
				() => Pieces.Add(new Piece(new Character(Rank.Miner, actualTeam), 1, 1)),
				() => Pieces.Count(piece => piece.Character.RankPower == 3)<=5
				);
			AddScoutCommand = new RelayCommand(
				() => Pieces.Add(new Piece(new Character(Rank.Scout, actualTeam), 1, 1)),
				() => Pieces.Count(piece => piece.Character.RankPower == 2)<=8
				);
			AddSpyCommand = new RelayCommand(
				() => Pieces.Add(new Piece(new Character(Rank.Spy, actualTeam), 1, 1)),
				() => !Pieces.Any(piece => piece.Character.RankPower == 1)
				);
			AddMineCommand = new RelayCommand(
				() => Pieces.Add(new Piece(new Character(Rank.Mine, actualTeam), 1, 1)),
				() => Pieces.Count(piece => piece.Character.RankPower == 11)<=6
				);
			AddFlagCommand = new RelayCommand(
				() => Pieces.Add(new Piece(new Character(Rank.Flag, actualTeam), 1, 1)),
				() => !Pieces.Any(piece => piece.Character.RankPower == 0)
				);
		}
		void PieceMoving()
		{ }
		void Battle()
		{ }
    }
}
