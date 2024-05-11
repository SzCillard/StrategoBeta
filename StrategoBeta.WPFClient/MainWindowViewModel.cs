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
using System.Windows.Media;

namespace StrategoBeta.WPFClient
{
    internal class MainWindowViewModel
    {
        public ObservableCollection<Piece> Pieces { set; get; } = new ObservableCollection<Piece>();

        public MainWindowViewModel()
        {
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    if (i >= 1 && i < 11 && j >= 1 && j < 11)
                    {
                        Pieces.Add(new Piece() {Column = i, Row = j, Character = new Character(0,Team.Blue) });
                    }
                }
            }
        }
    }
}
