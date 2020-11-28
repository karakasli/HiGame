using Codes.ServiceModules.IapService.Entities;
using Codes.ServiceModules.IapService.Providers;
using Codes.ServiceModules.Utils;
using System;
using UnityEngine;

namespace Codes.ServiceModules.IapService
{
    public class IapManager: Singleton<IapManager>, IIapListener
    {
        private IIapProvider _iapProvider;        
        private Action<bool> _initCallback;

        public void Init(LocalProduct localProduc, Action<bool> initCallback)
        {
            _iapProvider = IapFactory.GetAvailableIapProvider();
            _initCallback = initCallback;
            
            _iapProvider.Init(localProduc, this);
        }
        
        public void BuyProduct()
        {
            _iapProvider.Buy();
        }
        
        public void OnProductsQuerySuccess()
        {
            _initCallback(true);
        }

        public void OnProductsQueryFailed()
        {
            _initCallback(false);
        }

        public void OnPurchaseSuccess(string productId)
        {
            Debug.Log($"ProcessPurchase: is succeed!. Product: '{productId}'");
            PlayerPrefs.SetInt("remove_ads", 1);
        }
    }
}