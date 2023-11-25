using Grpc.Core;
using RaspberryPi4GpioServiceLibrary;
using Rpi4GpioGrpcService;

namespace Rpi4GpioGrpcService.Services;
public class CommanderService : Commander.CommanderBase
{
    private readonly ILogger<CommanderService> logger;
    private readonly GpioService gpioService;
    public CommanderService(ILogger<CommanderService> logger, GpioService gpioService)
    {
        this.logger = logger;
        this.gpioService = gpioService;
    }

    public override async Task<IsPinOpenReply> IsPinOpen(IsPinOpenRequest request, ServerCallContext context)
    {
        bool result = this.gpioService.IsPinOpen(request.PinNumber);
        return await Task.FromResult(new IsPinOpenReply
        {
            IsPinOpen = result
        });
    }
}
