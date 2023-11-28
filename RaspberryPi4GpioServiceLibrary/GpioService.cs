using Microsoft.Extensions.Logging;
using System.Device.Gpio;
using System.Reflection;

namespace RaspberryPi4GpioServiceLibrary;
public class GpioService
{

    #region Fields
    private readonly Dictionary<(int, PinEventTypes), PinChangeEventHandler> callbackDictionary;
    readonly GpioController gpioController;
    readonly ILogger<GpioService> logger;
    #endregion

    #region Constructors
    public GpioService(ILogger<GpioService> logger)
    {
        this.logger = logger;
        this.gpioController = new GpioController();
        this.callbackDictionary = [];
    }
    #endregion

    #region Private methods
    private void OpenAllPins()
    {
        for (int i = 2; i < 28; i++)
        {
            this.gpioController.OpenPin(i);
            Thread.Sleep(50);
        }
    }
    /// <summary>
    /// Method to register a callback on a pin.
    /// </summary>
    /// <param name="pinNumber"></param>
    /// <param name="eventType"></param>
    public void RegisterPinCallback(int pinNumber, PinEventTypes eventType)
    {
        // Create a lambda expression for the callback
        PinChangeEventHandler callback = (sender, args) =>
        {
            this.logger.LogDebug("Value for pin {PinNumber} has changed with event type of {EventType}", pinNumber, eventType);
        };

        // Register the callback with the GPIO controller
        this.gpioController.RegisterCallbackForPinValueChangedEvent(pinNumber, eventType, callback);

        // Store the callback in the dictionary for later reference
        callbackDictionary[(pinNumber, eventType)] = callback;
    }
    /// <summary>
    /// Method to unregister a callback on a pin.
    /// </summary>
    /// <param name="pinNumber"></param>
    /// <param name="eventType"></param>
    public void UnregisterPinCallback(int pinNumber, PinEventTypes eventType)
    {
        // Check if the callback is in the dictionary
        if (callbackDictionary.TryGetValue((pinNumber, eventType), out PinChangeEventHandler callback))
        {
            // Unregister the callback with the GPIO controller
            this.gpioController.UnregisterCallbackForPinValueChangedEvent(pinNumber, callback);

            // Remove the callback from the dictionary
            callbackDictionary.Remove((pinNumber, eventType));
        }
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
    // Example usage
    public void SetupCallbacks()
    {
        // Register callbacks for pin 23
        RegisterPinCallback(23, PinEventTypes.Falling);
        RegisterPinCallback(23, PinEventTypes.Rising);

        // Register callbacks for pin 22
        RegisterPinCallback(22, PinEventTypes.Falling);
        RegisterPinCallback(22, PinEventTypes.Rising);

        // Unregister a specific callback (example: pin 23, Falling)
        UnregisterPinCallback(23, PinEventTypes.Falling);
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
