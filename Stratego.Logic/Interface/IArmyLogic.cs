using StrategoBeta.Models;

namespace StrategoBeta.Logic.Interface
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