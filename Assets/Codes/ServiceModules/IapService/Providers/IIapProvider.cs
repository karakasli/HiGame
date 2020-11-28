using Codes.ServiceModules.IapService.Entities;

namespace Codes.ServiceModules.IapService.Providers
{
    public interface IIapProvider
    {
        void Init(LocalProduct localProduct, IIapListener listener);
        void Buy();
    }
}