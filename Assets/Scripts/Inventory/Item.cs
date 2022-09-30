using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public bool isStackable;
    public enum ItemType
    {
        Empty,
        Luck,
        Missfortune,
        Claw,
        Bone,
        Skull,
        Blood,
        Veins,
        Heart,
        Eclipse,
        Crescent,
        FullMoon
    }

    public ItemType itemType;
    public int amount;

    public static Sprite GetSprite(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.Empty:     return ItemAssets.Instance.EmptySprite;
            case ItemType.Claw:      return ItemAssets.Instance.Bone1Sprite;
            case ItemType.Bone:      return ItemAssets.Instance.Bone2Sprite;
            case ItemType.Skull:     return ItemAssets.Instance.Bone3Sprite;
            case ItemType.Blood:     return ItemAssets.Instance.Blood1Sprite;
            case ItemType.Veins:     return ItemAssets.Instance.Blood2Sprite;
            case ItemType.Heart:     return ItemAssets.Instance.Blood3Sprite;
            case ItemType.Eclipse:   return ItemAssets.Instance.Night1Sprite;
            case ItemType.Crescent:  return ItemAssets.Instance.Night2Sprite;
            case ItemType.FullMoon:  return ItemAssets.Instance.Night3Sprite;
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
