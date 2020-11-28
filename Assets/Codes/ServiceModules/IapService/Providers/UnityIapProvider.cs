using Codes.ServiceModules.IapService.Entities;
using Codes.ServiceModules.Utils;
using HmsPlugin;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Codes.ServiceModules.IapService.Providers
{
    public class UnityIapProvider: Singleton<UnityIapProvider>, IIapProvider, IStoreListener
    {
        private IIapListener _listener;
        
        private static IStoreController _unityStoreController;
        private static IExtensionProvider  _unityStoreExtension;

        private static string TAG = "UnityIapProvider";

        private LocalProduct _localProduct;

        public void Init(LocalProduct localProduct, IIapListener listener)
        {
            _listener = listener;
            _localProduct = localProduct;

            // If Products already installed fetch them from UnityStoreController
            if (IsInitialized())
            {
                FetchedProducts(_unityStoreController.products.all);
                _listener.OnProductsQuerySuccess();
                return;
            }

            #if HMS_BUILD && !UNITY_EDITOR
                var builder = ConfigurationBuilder.Instance(HuaweiPurchasingModule.Instance());
            #else
                var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            #endif

            builder.AddProduct("remove_ads", ProductType.NonConsumable);
            UnityPurchasing.Initialize(this, builder);
        }
        
        public void Buy()
        {
            if (IsInitialized())
            {
                var myProduct = _unityStoreController.products.WithID("remove_ads");
                
                if (myProduct != null && myProduct.availableToPurchase)
                {
                    _unityStoreController.InitiatePurchase(myProduct);
                }
                else
                {
                    Debug.Log(TAG + "Buy(): FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            else
            {
                Debug.Log(TAG + "Buy() FAIL. Not initialized.");
            }
        }
        
        private void FetchedProducts(Product[] remoteProducts)
        {

            if(remoteProducts != null && remoteProducts.Length > 0)
            {
                Product remoteProduct = remoteProducts[0];

                Debug.Log(TAG + "Fetched Product : " + remoteProduct.metadata.localizedTitle);
                var productName = Regex.Replace(remoteProduct.metadata.localizedTitle, "(?> \\(.+?\\))$", "");
                _localProduct.name = productName;
                _localProduct.desc = remoteProduct.metadata.localizedDescription;
                _localProduct.price = remoteProduct.metadata.localizedPriceString;
                _localProduct.isLoaded = true;
            }
        }
        
        private static bool IsInitialized()
        {
            return _unityStoreController != null && _unityStoreExtension != null;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("UnityIapProvider - OnInitializeFailed InitializationFailureReason:" + error);
            _listener.OnProductsQueryFailed();
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            _listener.OnPurchaseSuccess(args.purchasedProduct.definition.id);
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason err)
        {
            Debug.Log($"OnPurchaseFailed: FAIL. Product: '{product.definition.storeSpecificId}', PurchaseFailureReason: {err}");
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("UnityIapProvider - OnInitialized is succeed!");
            
            _unityStoreController = controller;
            _unityStoreExtension = extensions;

            FetchedProducts(_unityStoreController.products.all);
            _listener.OnProductsQuerySuccess();
        }
        
        public LocalProduct GetFetchedProducts()
        {
            return _localProduct;
        }
    }
}