using Microsoft.Extensions.Logging;
using System.Device.Gpio;

namespace RaspberryPi4GpioServiceLibrary;
public class GpioService
{
    readonly ILogger<GpioService> logger;
    readonly GpioController gpioController;
    public GpioService(ILogger<GpioService> logger)
    {
        this.logger = logger;
        this.gpioController = new GpioController();
        OpenAllPins();

        void OpenAllPins()
        {
            for (int i = 2; i < 28; i++)
            {
                this.gpioController.OpenPin(i);
                Thread.Sleep(50);
            }
        }
    }

    public bool IsPinOpen(int pinNumber) => this.gpioController.IsPinOpen(pinNumber);
}
