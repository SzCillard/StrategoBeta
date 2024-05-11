using StrategoBeta.Models;

namespace StrategoBeta.Logic.Logic
{
    public interface IArmyLogic
    {
        int TimeLeft { get; set; }

        event EventHandler<int> TimeChanged;

        string Battle(Character attacker, Character defender);
        void Start();
        void Stop();
    }
}