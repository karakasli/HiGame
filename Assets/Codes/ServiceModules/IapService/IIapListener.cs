namespace Codes.ServiceModules.IapService
{
    public interface IIapListener
    {
        void OnProductsQuerySuccess();
        void OnProductsQueryFailed();
        void OnPurchaseSuccess(string productId);
    }
}