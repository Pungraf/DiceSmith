using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public bool isStackable;
    public enum ItemType
    {
        Bone1,
        Bone2,
        Bone3,
        Blood1,
        Blood2,
        Blood3,
        Night1,
        Night2,
        Night3
    }

    public ItemType itemType;
    public int amount;

    public static Sprite GetSprite(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.Bone1:   return ItemAssets.Instance.Bone1Sprite;
            case ItemType.Bone2:   return ItemAssets.Instance.Bone2Sprite;
            case ItemType.Bone3:   return ItemAssets.Instance.Bone3Sprite;
            case ItemType.Blood1:  return ItemAssets.Instance.Blood1Sprite;
            case ItemType.Blood2:  return ItemAssets.Instance.Blood2Sprite;
            case ItemType.Blood3:  return ItemAssets.Instance.Blood3Sprite;
            case ItemType.Night1:  return ItemAssets.Instance.Night1Sprite;
            case ItemType.Night2:  return ItemAssets.Instance.Night2Sprite;
            case ItemType.Night3:  return ItemAssets.Instance.Night3Sprite;
        }
    }

    public bool IsStackable()
    {
        if (isStackable)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

     public Sprite GetSprite() {
        return GetSprite(itemType);
    }
}
