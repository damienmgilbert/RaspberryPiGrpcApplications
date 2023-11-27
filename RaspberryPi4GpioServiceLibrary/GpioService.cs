using Microsoft.Extensions.Logging;
using System.Device.Gpio;

namespace RaspberryPi4GpioServiceLibrary;
public class GpioService
{

    #region Fields
    readonly GpioController gpioController;
    readonly ILogger<GpioService> logger;
    #endregion

    #region Constructors
    public GpioService(ILogger<GpioService> logger)
    {
        this.logger = logger;
        this.gpioController = new GpioController();

        OpenAllPins();

        this.gpioController.RegisterCallbackForPinValueChangedEvent(23, PinEventTypes.Rising, Pin23CallbackForRisingEdgeValueChanged);
        this.gpioController.RegisterCallbackForPinValueChangedEvent(23, PinEventTypes.Falling, Pin23CallbackForFallingEdgeValueChanged);

        void OpenAllPins()
        {
            for (int i = 2; i < 28; i++)
            {
                this.gpioController.OpenPin(i);
                Thread.Sleep(50);
            }
        }
    }
    #endregion

    #region Private methods
    void Pin23CallbackForFallingEdgeValueChanged(object sender, PinValueChangedEventArgs pinValueChangedEventArgs)
    {
        this.logger.LogDebug("Value for pin 23 has changed");
    }
    void Pin23CallbackForRisingEdgeValueChanged(object sender, PinValueChangedEventArgs pinValueChangedEventArgs)
    {
        this.logger.LogDebug("Value for pin 23 has changed");
    }
    #endregion

    #region Public methods
    public bool ClosePin(int pinNumber)
    {
        bool result;
        try
        {
            this.gpioController.ClosePin(pinNumber);
            result = true;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Could not close pin");
            result = false;
        }
        return result;
    }
    public string GetNumberingScheme() => this.gpioController.NumberingScheme.ToString();
    public int GetPinCount() => this.gpioController.PinCount;
    public string GetPinMode(int pinNumber) => this.gpioController.GetPinMode(pinNumber).ToString();
    public bool IsPinModeSupported(int pinNumber, string pinMode)
    {
        bool result = false;
        try
        {
            switch (pinMode)
            {
                case "InputPullUp":
                    {
                        result = this.gpioController.IsPinModeSupported(pinNumber, PinMode.InputPullUp);
                        break;
                    }
                case "InputPullDown":
                    {
                        result = this.gpioController.IsPinModeSupported(pinNumber, PinMode.InputPullDown);
                        break;
                    }
                case "Input":
                    {
                        result = this.gpioController.IsPinModeSupported(pinNumber, PinMode.Input);
                        break;
                    }
                case "Output":
                    {
                        result = this.gpioController.IsPinModeSupported(pinNumber, PinMode.Output);
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException("Pin mode is not supported.");
                    }
            }
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Something went wrong when trying to figure out if pin mode was supported.");
            result = false;
        }
        return result;
    }
    public bool IsPinOpen(int pinNumber) => this.gpioController.IsPinOpen(pinNumber);
    public bool OpenPin(int pinNumber)
    {
        bool result;
        try
        {
            this.gpioController.OpenPin(pinNumber);
            result = true;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Could not open pin");
            result = false;
        }
        return result;
    }
    public string Read(int pinNumber)
    {
        string result = string.Empty;
        try
        {
            result = this.gpioController.Read(pinNumber).ToString();
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unable to read value from pin.");
        }
        return result;
    }
    public bool SetPinMode(int pinNumber, string pinMode)
    {
        bool result = false;
        try
        {
            switch (pinMode)
            {
                case "InputPullUp":
                    {
                        this.gpioController.SetPinMode(pinNumber, PinMode.InputPullUp);
                        result = true;
                        break;
                    }
                case "InputPullDown":
                    {
                        this.gpioController.SetPinMode(pinNumber, PinMode.InputPullDown);
                        result = true;
                        break;
                    }
                case "Input":
                    {
                        this.gpioController.SetPinMode(pinNumber, PinMode.Input);
                        result = true;
                        break;
                    }
                case "Output":
                    {
                        this.gpioController.SetPinMode(pinNumber, PinMode.Output);
                        result = true;
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException("Pin mode is not supported.");
                    }
            }
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Could not set pin mode.");
            result = false;
        }
        return result;
    }
    public bool Write(int pinNumber, string value)
    {
        bool result;
        try
        {
            switch (value)
            {
                case "HIGH":
                    {
                        this.gpioController.Write(pinNumber, PinValue.High);
                        result = true;
                        break;
                    }
                case "LOW":
                    {
                        this.gpioController.Write(pinNumber, PinValue.Low);
                        result = true;
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException("Pin value is not supported");
                    }
            }
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Pin value is not supported");
            result = false;
        }
        return result;
    }
    #endregion

}
