using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace TrafficLight.Domain
{
    public class PedestrianLight : CarTrafficLight
    {
        public PedestrianLight(int id, TrafficState defaultTrafficState = TrafficLight.TrafficState.STOP,
            GpioController gpioController = null, int? redPin = default(int?),
            int? yellowPin = default(int?), int? greenPin = default(int?)) : base(id, defaultTrafficState, gpioController, redPin, yellowPin, greenPin)
        {

        }

        public new async Task ToggleState()
        {
            if (TrafficTrafficState == TrafficLight.TrafficState.STOP)
            {
                TrafficTrafficState = TrafficLight.TrafficState.READY;

                await Task.Delay(4000);

                TrafficTrafficState = TrafficLight.TrafficState.GO;
                RedLight.ToggleState();
                GreenLight.ToggleState();
                return;
            }

            TrafficTrafficState = TrafficLight.TrafficState.READY;
            GreenLight.ToggleState();
            await Blink(3000, 300, RedLight);

            TrafficTrafficState = TrafficState.STOP;
            if (!RedLight.IsActive)
            {
                RedLight.ToggleState();
            }
        }


        private async Task Blink(int duration, int interval, LightUnit light)
        {
            for (int i = 0; i < duration; i = i + interval)
            {
                light.ToggleState();
                await Task.Delay(interval);
            }
        }
    }
}
