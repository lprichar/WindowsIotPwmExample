using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices;
using Windows.Devices.Gpio;
using Windows.Devices.Pwm;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.IoT.Lightning.Providers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WindowsIotPwmExample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private GpioPin _pin22;
        private PwmPin _pin27;

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (LightningProvider.IsLightningEnabled)
            {
                LowLevelDevicesController.DefaultProvider = LightningProvider.GetAggregateProvider();

                var pwmControllers = await PwmController.GetControllersAsync(LightningPwmProvider.GetPwmProvider());
                var pwmController = pwmControllers[1]; // the on-device controller
                pwmController.SetDesiredFrequency(50); // try to match 50Hz

                _pin27 = pwmController.OpenPin(27);
                _pin27.SetActiveDutyCyclePercentage(0);
                _pin27.Start();
            }

            var gpioController = await GpioController.GetDefaultAsync();
            if (gpioController == null)
            {
                StatusMessage.Text = "There is no GPIO controller on this device.";
                return;
            }
            _pin22 = gpioController.OpenPin(22);
            _pin22.SetDriveMode(GpioPinDriveMode.Output);
            _pin22.Write(GpioPinValue.Low);
        }

        private async void ClickMe_Click(object sender, RoutedEventArgs e)
        {
            _pin22.Write(GpioPinValue.High);
            await Task.Delay(500);
            _pin22.Write(GpioPinValue.Low);
        }

        private void LedBrightness_OnValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            _pin27.SetActiveDutyCyclePercentage(ledBrightness.Value * .01);
            ledPercent.Text = ledBrightness.Value + "%";
        }
    }
}
