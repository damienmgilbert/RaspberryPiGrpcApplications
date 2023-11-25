using Grpc.Core;
using Rpi4GpioGrpcService;

namespace Rpi4GpioGrpcService.Services;
public class CommanderService : Commander.CommanderBase
{
    private readonly ILogger<CommanderService> _logger;
    public CommanderService(ILogger<CommanderService> logger)
    {
        _logger = logger;
    }

    public override async Task<IsPinOpenReply> IsPinOpen(IsPinOpenRequest request, ServerCallContext context)
    {
        return await Task.FromResult(new IsPinOpenReply
        {
            IsPinOpen = false
        });
    }
}
