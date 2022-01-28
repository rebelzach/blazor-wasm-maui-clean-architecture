using Thing.SharedKernel.Interfaces;

namespace Thing.Designer.Infrastructure.Services;

internal class GuidGenerator : IGuidGenerator
{
    public Guid Create()
    {
        return Guid.NewGuid();
    }
}
