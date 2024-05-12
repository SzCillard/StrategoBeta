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
		bool initialPlacement = true;
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

		public ObservableCollection<Piece> Pieces { set; get; } = new ObservableCollection<Piece>();
        public MainWindowViewModel()
        {
			//(már javitott)Négyszer fut le a konstruktor, egyszer amikor a MainWindowban letrehozza, majd egyszer-egyszer a blue és red window konstruktora miatt
           
        }
        public MainWindowViewModel(BlueWindow bluewindow,RedWindow redwindow)
		{
			//Egyszer fut le
			FillWithEmptyButtons();
			CommandSetup();
			ReadyCommand = new RelayCommand(() => Ready());
			actualTeam = Team.Blue;
            blueWindow = bluewindow;
			blueWindow.Show();
			blueWindow.DataContext = this;
			blueWindow.ButtonClickedEvent += ClickOnPlayingFieldEvent;
			redWindow = redwindow;
			redWindow.DataContext = this;
        }

		private void ClickOnPlayingFieldEvent(object? sender, BlueWindow.ButtonClickedEventArgs e)
		{
			// Handle the button click event here
			int row = e.Row;
			int column = e.Column;
			//Calculates the index of the collection
			int idx = (10*(row-1) + column)-1;
			Button button = e.button;
            if (initialPlacement)
			{
				/////////////////////////////////////////////////////
				//TODO: Selected rank stuck on FLAG and no clue why//
				/////////////////////////////////////////////////////
				Pieces[idx] = new Piece(new Character(SelectedRank, Team.Blue), row, column);
			}
			else 
			{
				
			}
		}

		void CommandSetup()
        {
            AddMarshalCommand = new RelayCommand(
				() => SelectMarshal(),
				() => !Pieces.Any(piece => piece.Character.RankPower == 9)
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
				() => Pieces.Count(piece => piece.Character.RankPower == 5) <=4
				); 
			AddSergeantCommand = new RelayCommand(
                () => SelectSergeant(),
				() => Pieces.Count(piece => piece.Character.RankPower == 4) <=4
				);
			AddMinerCommand = new RelayCommand(
                () => SelectMiner(),
				() => Pieces.Count(piece => piece.Character.RankPower == 3)<=5
				);
			AddScoutCommand = new RelayCommand(
                () => SelectScout(),
				() => Pieces.Count(piece => piece.Character.RankPower == 2)<=8
				);
			AddSpyCommand = new RelayCommand(
                () => SelectSpy(),
				() => !Pieces.Any(piece => piece.Character.RankPower == 1)
				);
			AddMineCommand = new RelayCommand(
                () => SelectMine(),
				() => Pieces.Count(piece => piece.Character.RankPower == 11)<=6
				);
			AddFlagCommand = new RelayCommand(
                () => SelectFlag(),
				() => !Pieces.Any(piece => piece.Character.RankPower == 0)
				);
		}
		void PieceMoving()
		{ }
		void Battle()
		{ }
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
		}
        private void SelectGeneral()
        {
            SelectedRank = Rank.General;
            RankSelectionEvent?.Invoke(this, null);
        }
        private void SelectColonel()
        {
            SelectedRank = Rank.Colonel;
            RankSelectionEvent?.Invoke(this, null);
        }
        private void SelectMajor()
        {
            SelectedRank = Rank.Major;
            RankSelectionEvent?.Invoke(this, null);
        }
        private void SelectCaptain()
        {
            SelectedRank = Rank.Captain;
            RankSelectionEvent?.Invoke(this, null);
        }
        private void SelectLieutenant()
        {
            SelectedRank = Rank.Lieutenant;
            RankSelectionEvent?.Invoke(this, null);
        }
        private void SelectSergeant()
        {
            SelectedRank = Rank.Sergeant;
            RankSelectionEvent?.Invoke(this, null);
        }
        private void SelectMiner()
        {
            SelectedRank = Rank.Miner;
            RankSelectionEvent?.Invoke(this, null);
        }
        private void SelectScout()
        {
            SelectedRank = Rank.Scout;
            RankSelectionEvent?.Invoke(this, null);
        }
        private void SelectSpy()
        {
            SelectedRank = Rank.Spy;
            RankSelectionEvent?.Invoke(this, null);
        }
        private void SelectMine()
        {
            SelectedRank = Rank.Mine;
            RankSelectionEvent?.Invoke(this, null);
        }
        private void SelectFlag()
        {
            SelectedRank = Rank.Flag;
            RankSelectionEvent?.Invoke(this, null);
        }

		//Changes initialPlacement to True (finished placing down the pieces)
		private void Ready()
		{
			initialPlacement = false;
			var a = Pieces;
            ReadyEvent?.Invoke(this, null);
        }

    }
}
