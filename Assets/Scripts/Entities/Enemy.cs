using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField]
    ParticleSystem attackParticle;

    private List<Item> dropPool = new List<Item>();
    private List<Item> loot = new List<Item>();
    public Animator animator; 
 
    public int damage = 5;
    public GameObject target;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void DealDamage()
    {
        target.GetComponent<Entity>().Health -= damage;
        GameController.Instance.UpdateUI();
        attackParticle.Play();
        statusesList.Add(new BoneShieldStatus(2));

    }

    public void AssigneLoot()
    {
        foreach(Item item in loot)
        {
            GameController.Instance.lootToText += item.itemType + " x" + item.amount + "\n";
            target.GetComponent<Player>().inventory.AddItem(item);
        }
    }
}
