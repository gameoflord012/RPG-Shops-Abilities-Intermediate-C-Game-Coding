using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Control;
using UnityEngine;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour, IRaycastable
    {
        [SerializeField] string shopName;
        [SerializeField] StockItemConfig[] stockConfigs;

        Shopper currentShopper;

        [System.Serializable]
        class StockItemConfig
        {
            public InventoryItem item;
            public int initialStock;
            [Range(0, 100)]
            public float buyingDiscount;
        }

        Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int>();

        public event Action onChange;

        public IEnumerable<ShopItem> GetFilteredItems() 
        {
            foreach (var stockConfig in stockConfigs)
            {
                var calculatedPrice = (1 - stockConfig.buyingDiscount / 100) * stockConfig.item.GetPrice();
                int transactionQuantity = 0;
                transaction.TryGetValue(stockConfig.item, out transactionQuantity);
                yield return new ShopItem(stockConfig.item, stockConfig.initialStock, calculatedPrice, transactionQuantity);
            }
        }

        public void SelectFilter(ItemCategory category) {}
        public ItemCategory GetFilter() { return ItemCategory.None; }
        public void SelectMode(bool isBuying) {}
        public bool IsBuyingMode() { return true; }
        public bool CanTransact() { return true; }

        public void SetShopper(Shopper shopper)
        {
            currentShopper = shopper;
        }

        public void ConfirmTransaction() 
        {
            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
            if (shopperInventory == null) return;

            var transatctionSnapshot = new Dictionary<InventoryItem, int>(transaction);
            foreach(var item in transatctionSnapshot.Keys)
            {
                var quantity = transatctionSnapshot[item];

                for(int i = 0; i < quantity; i++)
                {
                    if (shopperInventory.AddToFirstEmptySlot(item, 1))
                    {
                        AddToTransaction(item, -1);
                    }
                }
            }

            onChange?.Invoke();
        }

        public float TransactionTotal() { return 0; }

        public void AddToTransaction(InventoryItem item, int quantity) 
        {
            // Debug.Log($"Added to transaction: {item.GetDisplayName()} x {quantity}");
            if(!transaction.ContainsKey(item))
            {
                transaction.Add(item, 0);
            }

            transaction[item] += quantity;

            if(transaction[item] <= 0)
            {
                transaction.Remove(item);
            }

            onChange?.Invoke();
        }

        public string GetShopName()
        {
            return shopName;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Shop;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if(Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<Shopper>().SetActiveShop(this);                
            }

            return true;
        }
    }
}