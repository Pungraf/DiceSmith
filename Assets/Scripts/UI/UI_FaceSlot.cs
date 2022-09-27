using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_FaceSlot : MonoBehaviour, IDropHandler
{
    private RectTransform rectTransform;
    private Item item;
    private Image image;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        image = transform.Find("image").GetComponent<Image>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            item = new Item { itemType = UI_ItemDrag.Instance.GetItem().itemType, amount = 1, isStackable = UI_ItemDrag.Instance.GetItem().IsStackable()};
            image.sprite = Item.GetSprite(item.itemType);
            CraftController.Instance.inventory.RemoveItem(item);
            UI_ItemDrag.Instance.Hide();
        }
    }
}
