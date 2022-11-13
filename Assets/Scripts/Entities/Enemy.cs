using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Entity
{

    private List<Item> dropPool = new List<Item>();
    private List<Item> loot = new List<Item>();

    public List<EnemyAbility> abilities;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    public void Start()
    {
        base.Start();
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
        statusPanel = GameObject.Find("EnemyStatusPanel").GetComponent<RectTransform>();

        foreach(EnemyAbility ability in abilities)
        {
            ability.Start();
        }
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
            Item lootedBase = dropPool[Random.Range(0, dropPool.Count)];
            int itemQuantity = Random.Range(1, 4);
            loot.Add(new Item { itemType = lootedBase.itemType, amount = itemQuantity, isStackable = lootedBase.isStackable });
        }
    }

    public void DealDamage()
    {
        int index = Random.Range(0, abilities.Count);
        GameController.Instance.ExecuteAbility(abilities[index], this, enemy, this);
    }

    public void AssigneLoot()
    {
        foreach(Item item in loot)
        {
            GameController.Instance.lootToText += item.itemType + " x" + item.amount + "\n";
            enemy.GetComponent<Player>().inventory.AddItem(item);
        }
    }
}
