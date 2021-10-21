using RPG.Shops;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI shopName;
        [SerializeField] Transform listRoot;
        [SerializeField] RowUI rowPrefab;
        [SerializeField] TextMeshProUGUI totalCost;

        Shopper shopper = null;
        Shop currentShop = null;

        void Start()
        {
            shopper = GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();
            if (shopper == null) return;

            shopper.activeShopChange += ShopChanged;

            ShopChanged();
        }

        private void ShopChanged()
        {
            currentShop = shopper.GetActiveShop();
            gameObject.SetActive(currentShop != null);

            if (currentShop != null)
            {
                currentShop.onChange -= RefreshUI;
                shopName.text = currentShop.GetShopName();
                currentShop.onChange += RefreshUI;
            }

            RefreshUI();
        }

        private void RefreshUI()
        {
            if (currentShop == null) return;

            foreach (Transform child in listRoot)
                Destroy(child.gameObject);

            foreach (var item in currentShop.GetFilteredItems())
            {
                var row = Instantiate(rowPrefab, listRoot);
                row.Setup(item, currentShop);
            }
        }

        public void Confirm()
        {
            currentShop.ConfirmTransaction();
        }

        public void Close()
        {
            shopper.SetActiveShop(null);
        }
    }
}