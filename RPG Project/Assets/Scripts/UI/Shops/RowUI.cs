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

        public void Setup(ShopItem item)
        {
            itemName.text = item.GetName();
            icon.sprite = item.GetIcon();
            availability.text = $"{item.GetAvailability()}";
            price.text = $"{item.GetPrice()}";
        }
    }
}