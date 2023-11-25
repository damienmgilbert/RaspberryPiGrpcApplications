using Grpc.Core;
using RaspberryPi4GpioServiceLibrary;
using Rpi4GpioGrpcService;

namespace Rpi4GpioGrpcService.Services;
public class CommanderService : Commander.CommanderBase
{

    #region Fields
    private readonly GpioService gpioService;
    private readonly ILogger<CommanderService> logger;
    #endregion

    #region Constructors
    public CommanderService(ILogger<CommanderService> logger, GpioService gpioService)
    {
        this.logger = logger;
        this.gpioService = gpioService;

    }
    #endregion

    #region Public methods
    public override async Task<ClosePinReply> ClosePin(ClosePinRequest request, ServerCallContext context)
    {
        bool result = this.gpioService.ClosePin(request.PinNumber);
        return await Task.FromResult(new ClosePinReply
        {
            IsPinClosed = result
        });
    }
    public override async Task<GetNumberingSchemeReply> GetNumberingScheme(GetNumberingSchemeRequest request, ServerCallContext context)
    {
        var numberingScheme = this.gpioService.GetNumberingScheme();
        return await Task.FromResult(new GetNumberingSchemeReply { NumberingScheme = numberingScheme });
    }
    public override async Task<GetPinCountReply> GetPinCount(GetPinCountRequest request, ServerCallContext context)
    {
        var pinCount = this.gpioService.GetPinCount();
        return await Task.FromResult(new GetPinCountReply { PinCount = pinCount });
    }
    public override async Task<GetPinModeReply> GetPinMode(GetPinModeRequest request, ServerCallContext context)
    {
        string pinMode = this.gpioService.GetPinMode(request.PinNumber);
        return await Task.FromResult(new GetPinModeReply
        {
            PinMode = pinMode
        });
    }
    public override async Task<IsPinModeSupportedReply> IsPinModeSupported(IsPinModeSupportedRequest request, ServerCallContext context)
    {
        var isPinModeSupported = this.gpioService.IsPinModeSupported(request.PinNumber, request.PinMode);
        return await Task.FromResult(new IsPinModeSupportedReply { IsPinModeSupported = isPinModeSupported });
    }
    public override async Task<IsPinOpenReply> IsPinOpen(IsPinOpenRequest request, ServerCallContext context)
    {
        bool result = this.gpioService.IsPinOpen(request.PinNumber);
        return await Task.FromResult(new IsPinOpenReply
        {
            IsPinOpen = result
        });
    }
    public override async Task<OpenPinReply> OpenPin(OpenPinRequest request, ServerCallContext context)
    {
        bool result = this.gpioService.OpenPin(request.PinNumber);
        return await Task.FromResult(new OpenPinReply
        {
            IsPinOpen = result
        });
    }
    public override async Task<ReadReply> Read(ReadRequest request, ServerCallContext context)
    {
        var pinValue = this.gpioService.Read(request.PinNumber);
        return await Task.FromResult(new ReadReply { PinValue = pinValue });
    }
    public override async Task<SetPinModeReply> SetPinMode(SetPinModeRequest request, ServerCallContext context)
    {
        var isSet = this.gpioService.SetPinMode(request.PinNumber, request.PinMode);
        return await Task.FromResult(new SetPinModeReply { IsSet = isSet });
    }
    public override async Task<WriteReply> Write(WriteRequest request, ServerCallContext context)
    {
        var didWrite = this.gpioService.Write(request.PinNumber, request.PinValue);
        return await Task.FromResult(new WriteReply { DidWrite = didWrite });
    }
    #endregion

}
