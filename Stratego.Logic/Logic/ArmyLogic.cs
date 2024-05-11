using CommunityToolkit.Mvvm.Messaging;
using StrategoBeta.Logic.Interface;
using StrategoBeta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace StrategoBeta.Logic.ArmyLogic
{
    public class ArmyLogic : IArmyLogic
	{
		IList<Character> blue;
		IList<Character> red;
		IMessenger messenger;

		public System.Timers.Timer timer;
		private int timeLeft;
		public int TimeLeft { get; set; }

		public event EventHandler<int> TimeChanged;

		public ArmyLogic(IMessenger messenger)
		{
			this.messenger = messenger;
			SetupCollections();

			//Timer
            timeLeft =120;
            timer = new System.Timers.Timer(1000); // 1000 milliseconds = 1 second
            timer.Elapsed += Timer_Elapsed;
        }
		void SetupCollections()
		{
			// Flag 1, Spy 1, Scout 8, Miner 5, Sergeant 4, Lieutenant 4, Captain 4, Major 3, Colonel 2, General 1, Marshal 1, Mine 6

			// blue team

			blue = new List<Character>();

			// red team

			red = new List<Character>();

		}
		public string Battle(Character attacker, Character defender)
		{
			if (attacker.RankPower == 1 && defender.RankPower == 10) //spy wins against marshal
			{
				if (attacker.Team == Team.Blue)
				{
					red.Remove(defender);
				}
				else
				{
					blue.Remove(defender);
				}
				return "attacker";
			}
			else if (attacker.RankPower == 3 && defender.RankPower == 11) //miner wins against mine
			{
				if (attacker.Team == Team.Blue)
				{
					red.Remove(defender);
				}
				else
				{
					blue.Remove(defender);
				}
				return "attacker";
			}
			else if (defender.RankPower == 0) //defender is the flag
			{
				if (attacker.Team == Team.Blue)
				{
					//blue wins
					red.Remove(defender);
				}
				else
				{
					//red wins
					blue.Remove(defender);
				}
				//game ends, attacker wins
				return "attacker";
			}
			else if (attacker.RankPower > defender.RankPower)
			{
				if (attacker.Team == Team.Blue)
				{
					red.Remove(defender);
				}
				else
				{
					blue.Remove(defender);
				}
				return "attacker";
			}
			else if (attacker.RankPower < defender.RankPower)
			{
				if (attacker.Team == Team.Blue)
				{
					blue.Remove(attacker);
				}
				else
				{
					red.Remove(attacker);
				}
				return "defender";
			}
			else
			{
				if (attacker.Team == Team.Blue)
				{
					blue.Remove(attacker);
					red.Remove(defender);
				}
				else
				{
					blue.Remove(defender);
					red.Remove(attacker);
				}
				return "draw";
			}
		}
		public void Start()
		{
			timer.Start();
		}
		public void Stop()
		{
			timer.Stop();
		}
		private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			TimeLeft--;

			if (TimeLeft <= 0)
			{
				Stop();
				TimeLeft = 30;
			}
			messenger.Send("Timer", "TimeInfo");
            TimeChanged?.Invoke(this, TimeLeft);
        }
    
	}
}
