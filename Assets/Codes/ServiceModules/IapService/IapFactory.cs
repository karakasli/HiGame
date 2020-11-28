using Codes.ServiceModules.IapService.Providers;

namespace Codes.ServiceModules.IapService
{
    internal static class IapFactory
    {
        /**
         * This function decides to available IAP service and return available service provider
         */
        public static IIapProvider GetAvailableIapProvider()
        {
            return UnityIapProvider.Instance;
        }
    }
}