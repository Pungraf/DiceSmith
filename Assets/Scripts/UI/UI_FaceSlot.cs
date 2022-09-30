using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_FaceSlot : MonoBehaviour, IDropHandler
{

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Item item;
    private Image image;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        image = transform.Find("image").GetComponent<Image>();
    }

    private void Start()
    {
        rectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () =>
        {
            if (item != null && item.itemType != Item.ItemType.Empty)
            {
                CraftController.Instance.inventory.AddItem(item);
                item = new Item { itemType = Item.ItemType.Empty, amount = 1, isStackable = true };
                image.sprite = item.GetSprite();
                CraftController.Instance.GenerateDiceFaces();

            }
        };
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if(item != null && item.itemType != Item.ItemType.Empty)
            {
                Debug.Log(item.itemType.ToString());
                CraftController.Instance.inventory.AddItem(item);
            }
            item = new Item { itemType = UI_ItemDrag.Instance.GetItem().itemType, amount = 1, isStackable = UI_ItemDrag.Instance.GetItem().IsStackable()};
            image.sprite = Item.GetSprite(item.itemType);
            CraftController.Instance.inventory.RemoveItem(item);
            CraftController.Instance.GenerateDiceFaces();
            UI_ItemDrag.Instance.Hide();
        }
    }

    public Item GetItem()
    {
        return item;
    }

    public void SetItem(Item item)
    {
        this.item = item;
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
