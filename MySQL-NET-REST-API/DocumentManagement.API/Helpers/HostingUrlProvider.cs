using AuthChecker;

namespace DocumentManagement.API.Helpers;

public class HostingUrlProvider : IHostingUrlProvider
{
    private readonly ServerUrlAccessor _accessor;

    public HostingUrlProvider(ServerUrlAccessor accessor)
    {
        _accessor = accessor;
    }

    public string? GetHostingUrl() => _accessor.Url;
}
