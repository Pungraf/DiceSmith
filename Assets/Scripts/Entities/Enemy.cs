using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    private List<Item> dropPool = new List<Item>();

    private List<Item> loot = new List<Item>();

    private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        dropPool.Add(new Item { itemType = Item.ItemType.Eclipse, amount = 1, isStackable = true });
        dropPool.Add(new Item { itemType = Item.ItemType.Crescent, amount = 1, isStackable = true });
        dropPool.Add(new Item { itemType = Item.ItemType.FullMoon, amount = 1, isStackable = true });

        dropPool.Add(new Item { itemType = Item.ItemType.Blood, amount = 1, isStackable = true });
        dropPool.Add(new Item { itemType = Item.ItemType.Veins, amount = 1, isStackable = true });
        dropPool.Add(new Item { itemType = Item.ItemType.Heart, amount = 1, isStackable = true });

        dropPool.Add(new Item { itemType = Item.ItemType.Claw, amount = 1, isStackable = true });
        dropPool.Add(new Item { itemType = Item.ItemType.Bone, amount = 1, isStackable = true });
        dropPool.Add(new Item { itemType = Item.ItemType.Skull, amount = 1, isStackable = true });

        GenerateDrop();

        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateDrop()
    {
        int lootedNumber = Random.Range(1, 4);
        for(int i = 0; i < lootedNumber; i++)
        {
            Item lootedBase = dropPool[Random.Range(0, loot.Count + 1)];
            int itemQuantity = Random.Range(1, 4);
            loot.Add(new Item { itemType = lootedBase.itemType, amount = itemQuantity, isStackable = lootedBase.isStackable });
        }
    }

    public void DealDamage()
    {
        target.GetComponent<Entity>().Health -= 1;
    }

    public void AssigneLoot()
    {
        foreach(Item item in loot)
        {
            Debug.Log("Looted: " + item.itemType + " x" + item.amount);
            target.GetComponent<Player>().inventory.AddItem(item);
        }
    }
}
