using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private int _healt;
    private int _incomingDamage;

    public List<Status> statusesList;
    public Animator animator;
    public AbilityEffects abilityEffects;
    public Entity enemy;
    public RectTransform statusPanel;


    private string actualAbilityVisualName;
    private Transform actualAbilityVisualTarget;
    private Transform actualAbilityVisualSpawn;

    public int IncomingDamage
    {
        get => _incomingDamage;
        set
        {
            _incomingDamage = value;
        }
    }


    public int Health
    {
        get => _healt;
        set
        {
            _healt = value;
        }
    }

    // Start is called before the first frame update
    public void Start()
    {
        statusesList = new List<Status>();
        animator = GetComponent<Animator>();
        abilityEffects = GetComponent<AbilityEffects>();
    }

    // Update is called once per frame
    void Update()
    {
    }


    //Set player enemy
    public void SetEnemy(Entity playerEnemy)
    {
        enemy = playerEnemy;
    }

    //Get player Enemy
    public Entity GetEnemy()
    {
        return enemy;
    }

    private void UseShields()
    {
        for(int i = statusesList.Count -1; i >= 0; i--)
        {
            if (statusesList[i] is IShieldEffect)
            {
                ShieldStatus currentShield = (ShieldStatus)statusesList[i];
                int overDamage = currentShield.UseShield(IncomingDamage);
                if (overDamage > 0)
                {
                    IncomingDamage = 0;
                }
                else
                {
                    Destroy(statusesList[i].statusUi.gameObject);
                    IncomingDamage = Mathf.Abs(overDamage);
                    statusesList.RemoveAt(i);
                }
            }
        }
    }

    public void GetDamage(int damage)
    {
        IncomingDamage = damage;
        UseShields();
        Health -= IncomingDamage;
        IncomingDamage = 0;
    }

    public void PlayAbilityVisuals(string animationToPlay, string abilityVisualsName, bool spawnAtTarget, bool target)
    {
        animator.SetTrigger(animationToPlay);
        actualAbilityVisualName = abilityVisualsName;

        if (spawnAtTarget)
        {
            if (target)
            {
                actualAbilityVisualTarget = abilityEffects.hand;
            }
            else
            {
                actualAbilityVisualTarget = null;
            }
            actualAbilityVisualSpawn = GameController.Instance.enemy.transform;
        }
        else
        {
            if (target)
            {
                actualAbilityVisualTarget = GameController.Instance.enemy.transform;
            }
            else
            {
                actualAbilityVisualTarget = null;
            }
            actualAbilityVisualSpawn = abilityEffects.hand;
        }
    }

    public void SpawnVisualEffect()
    {
        abilityEffects.SpawnVisuals(actualAbilityVisualName, actualAbilityVisualSpawn, actualAbilityVisualTarget);
    }
}
