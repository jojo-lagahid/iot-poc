using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Gpio;
using TrafficLight.Domain;

namespace TrafficLight
{
    public enum TrafficState
    {
        STOP,
        READY,
        GO
    }

    public class TrafficLightModel
    {
        private readonly GpioController _gpio;
        
        public TrafficLightModel()
        {
            ChangeStateCommand = new ToggleStateCommand();

            _gpio = GpioController.GetDefault();
            CarTrafficLight = new CarTrafficLight(1, TrafficState.GO, _gpio, 5, 6, 13);

            PedestrianTrafficLight = new PedestrianLight(2, TrafficState.STOP, _gpio, 17, 27, 22);
        }
        
        public CarTrafficLight CarTrafficLight { get; set; }

        public PedestrianLight PedestrianTrafficLight { get; set; }

        public string ToggleButton => "TOGGLE";

        public ICommand ChangeStateCommand { get; set; }

        public async Task ChangeState()
        {
            CarTrafficLight.ToggleState();
            await PedestrianTrafficLight.ToggleState();
        }
    }

    public class ToggleStateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (parameter != null && ((TrafficLightModel)parameter).CarTrafficLight.TrafficTrafficState == TrafficState.READY)
            {
                return false;
            }

            return true;
        }

        public void Execute(object parameter)
        {
            ((TrafficLightModel)parameter).ChangeState();
        }
    }
}
