using RPG.Shops;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class RowUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI itemName;
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI availability;
        [SerializeField] TextMeshProUGUI price;
        [SerializeField] TextMeshProUGUI quantity;

        ShopItem shopItem;
        Shop currentShop;

        public void Setup(ShopItem item, Shop shop)
        {
            itemName.text = item.GetName();
            icon.sprite = item.GetIcon();
            availability.text = $"{item.GetAvailability()}";
            price.text = $"${item.GetPrice():N2}";
            quantity.text = $"{item.GetQuantityInTransaction()}";

            this.currentShop = shop;
            this.shopItem = item;
        }

        public void Add()
        {
            currentShop.AddToTransaction(shopItem.GetItem(), 1);            
        }

        public void Remove()
        {
            currentShop.AddToTransaction(shopItem.GetItem(), -1);            
        }
    }
}