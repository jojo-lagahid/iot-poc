using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Devices.Gpio;
using TrafficLight.Properties;

namespace TrafficLight.Domain
{
    public class LightUnit : INotifyPropertyChanged
    {
        private readonly string _activeColor;
        private readonly string _inactiveColor;
        private readonly GpioPin _gpioPin;
        private readonly GpioController _gpioController;

        public LightUnit(string activeColor, string inactiveColor, bool defaultIsActive,
            GpioController gpioController = null, int? devicePinNo = null)
        {
            _activeColor = activeColor;
            _inactiveColor = inactiveColor;
            _gpioController = gpioController;
            IsActive = defaultIsActive;

            if (_gpioController == null || devicePinNo == null) return;

            _gpioPin = _gpioController.OpenPin(devicePinNo.Value);

            var pinValue = defaultIsActive ? GpioPinValue.Low : GpioPinValue.High;
            _gpioPin.Write(pinValue);
            _gpioPin.SetDriveMode(GpioPinDriveMode.Output);
        }

        public bool IsActive { get; set; }

        public string Color => IsActive ? _activeColor : _inactiveColor;

        public void ToggleState()
        {
            if (IsActive)
            {
                if (_gpioController != null)
                    _gpioPin.Write(GpioPinValue.High);

                IsActive = false;

            }
            else
            {
                if (_gpioController != null)
                    _gpioPin.Write(GpioPinValue.Low);

                IsActive = true;
            }

            OnPropertyChanged(nameof(Color));
            OnPropertyChanged(nameof(IsActive));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
