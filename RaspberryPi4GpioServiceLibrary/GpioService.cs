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
    public PinMode GetPinMode(int pinNumber) => this.gpioController.GetPinMode(pinNumber);
    public bool IsPinModeSupported(int pinNumber, PinMode pinMode)
    {
        bool result = false;
        try
        {
            result = this.gpioController.IsPinModeSupported(pinNumber, pinMode);
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
    public PinValue Read(int pinNumber)
    {
        PinValue pinValue = new();
        try
        {
            pinValue = this.gpioController.Read(pinNumber);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unable to read value from pin.");
        }
        return pinValue;
    }
    public bool SetPinMode(int pinNumber, PinMode pinMode)
    {
        bool result = false;
        try
        {
            this.gpioController.SetPinMode(pinNumber, pinMode);
            result = true;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Could not set pin mode.");
            result = false;
        }
        return result;
    }
    public bool Write(int pinNumber, PinValue value)
    {
        bool result;
        try
        {
            this.gpioController.Write(pinNumber, value);
            result = true;
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
