using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StrategoBeta.Models
{
    public enum Rank
    {
        Flag = 0, Spy = 1, Scout = 2, Miner = 3, Sergeant = 4, Lieutenant = 5, Captain = 6, Major = 7, Colonel = 8, General = 9, Marshal = 10, Mine = 11, Empty = 12, Lake=13
    }
    public enum Team
    {
        Blue, Red, Empty, Lake
    }
    public class Character
    {
        public Rank Rank {  get; set; }
        public Team Team { get; set; }

        public Style Style { get; set; }

        string rank;
        int rankPower;
        int maxStep;
        string team;
        public Character(Rank rank, Team team)
        {
            //this.rank = rank.ToString();
            rankPower = (int)rank;
            if (rankPower == 2)
            {
                maxStep = 9;
            }
            else if (rankPower == 0 || rankPower == 11)
            {
                maxStep = 0;
            }
            else
            {
                maxStep = 1;
            }
            //this.Team= team.ToString();
            Rank = rank;
            Team = team;
        }
        public Character(Rank rank, Team team, Style style)
        {
            //this.rank = rank.ToString();
            rankPower = (int)rank;
            if (rankPower == 2)
            {
                maxStep = 9;
            }
            else if (rankPower == 0 || rankPower == 11)
            {
                maxStep = 0;
            }
            else
            {
                maxStep = 1;
            }
            //this.Team= team.ToString();
            Rank = rank;
            Team = team;
            Style = style;
        }

        //public string Rank { get => rank; set => rank = value; }
        public int RankPower { get => rankPower; set => rankPower = value; }
        public int MaxStep { get => maxStep; set => maxStep = value; }
		//public string Team { get => team; set => team = value; }
	}
}
