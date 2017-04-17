using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace TrafficLight.Domain
{
    public class CarTrafficLight
    {
        private const string InactiveColor = "DarkGray";

        public CarTrafficLight(int id, TrafficState defaultTrafficState = TrafficState.STOP, GpioController gpioController = null, int? redPin = null, int? yellowPin = null, int? greenPin = null)
        {
            bool stop = defaultTrafficState == TrafficLight.TrafficState.STOP;

            RedLight = new LightUnit("Red", InactiveColor, stop, gpioController, redPin);
            YellowLight = new LightUnit("Yellow", InactiveColor, false, gpioController, yellowPin);
            GreenLight = new LightUnit("Green", InactiveColor, !stop, gpioController, greenPin);
            Id = id;
            TrafficTrafficState = defaultTrafficState;
        }

        public int Id { get; }
        public LightUnit RedLight { get; set; }
        public LightUnit YellowLight { get; set; }
        public LightUnit GreenLight { get; set; }

        public TrafficState TrafficTrafficState { get; set; }

        public async void ToggleState()
        {
            if (TrafficTrafficState == TrafficState.STOP)
            {
                TrafficTrafficState = TrafficLight.TrafficState.READY;
                
                await Task.Delay(4000);

                TrafficTrafficState = TrafficLight.TrafficState.GO;
                RedLight.ToggleState();
                GreenLight.ToggleState();
                return;
            }

            TrafficTrafficState = TrafficState.READY;
            GreenLight.ToggleState();
            YellowLight.ToggleState();
            await Task.Delay(3000);

            TrafficTrafficState = TrafficState.STOP;
            YellowLight.ToggleState();
            RedLight.ToggleState();
        }
    }
}
