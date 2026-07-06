namespace EventPractice
{
    public class TemperatureSensor
    {
        public event Action<double>? TemperatureChanged;
        private double _temperature;

        public double Temperature
        {
            get => _temperature;
            set
            {
                _temperature = value;
                TemperatureChanged?.Invoke(value);
            }
        }
    }
}