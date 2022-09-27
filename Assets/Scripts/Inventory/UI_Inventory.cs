using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    [SerializeField]
    private Transform itemSlotTemplate;

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        inventory.OnItemListChange += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void Awake()
    {
        itemSlotContainer = transform.Find("ItemSlotContainer");
    }

    private void RefreshInventoryItems()
    {
        foreach(Transform child in itemSlotContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () =>
            {
                //On left click action
            };
            itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () =>
            {
                //On right click action
                //Remove from inventory
                inventory.RemoveItem(item);
            };

            Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
            UI_Item ui_Item = itemSlotRectTransform.GetComponent<UI_Item>();
            ui_Item.SetItem(item);
            //image.sprite = item.GetSprite();

            TextMeshProUGUI uiAmount = itemSlotRectTransform.Find("amount").GetComponent<TextMeshProUGUI>();
            if(item.amount > 1)
            {
                uiAmount.SetText(item.amount.ToString());
            }
            else
            {
                uiAmount.SetText("");
            }
        }
    }
}
