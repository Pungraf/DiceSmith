using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private int _healt;
    private int _incomingDamage;

    public List<Status> statusesList;

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
    }

    // Update is called once per frame
    void Update()
    {
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
                    IncomingDamage = Mathf.Abs(overDamage);
                    statusesList.RemoveAt(i);
                }
            }
        }

        /*foreach(Status status in statusesList)
        {
            if(status is IShieldEffect)
            {
                ShieldStatus currentShield = (ShieldStatus)status;
                int overDamage = currentShield.UseShield(IncomingDamage);
                if(overDamage > 0)
                {
                    IncomingDamage = 0;
                }
                else
                {
                    IncomingDamage = Mathf.Abs(overDamage);
                    statusesList.Remove(status);
                }
            }
        }*/
    }

    public void GetDamage(int damage)
    {
        IncomingDamage = damage;
        UseShields();
        Health -= IncomingDamage;
        IncomingDamage = 0;
    }
}
