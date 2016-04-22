using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices;
using Windows.Devices.Gpio;
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
        private GpioPin _pin;

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
            }

            var gpioController = await GpioController.GetDefaultAsync();
            if (gpioController == null)
            {
                StatusMessage.Text = "There is no GPIO controller on this device.";
                return;
            }
            _pin = gpioController.OpenPin(22);
            _pin.SetDriveMode(GpioPinDriveMode.Output);
            _pin.Write(GpioPinValue.Low);
        }

        private async void ClickMe_Click(object sender, RoutedEventArgs e)
        {
            _pin.Write(GpioPinValue.High);
            await Task.Delay(500);
            _pin.Write(GpioPinValue.Low);
        }
    }
}
