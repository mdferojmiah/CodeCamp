using AdapterPattern.Interfaces;

namespace AdapterPattern.Classes
{
    public class SocketAdapter : IUSASocket
    {
        private readonly EuropeanPlug _europeanPlug;

        public SocketAdapter(EuropeanPlug europeanPlug)
        {
            _europeanPlug = europeanPlug;
        }

        public string GetUSAScoket() => $"<-> Socket Converted: {_europeanPlug.GetEuropeanSocket()}";
    }
}