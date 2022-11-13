using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarierEfect : Effect
{
    public BarierEfect(int shield)
    {
        _shield = shield;
    }

    public int _shield;

    public override void Execute(Entity enemy, Entity ally)
    {
        foreach (Status status in ally.statusesList)
        {
            if (status.GetType() == typeof(BarierStatus))
            {
                BarierStatus existingBoneShield = (BarierStatus)status;
                existingBoneShield._shieldValue = _shield;
                existingBoneShield.statusUi.Counters.text = existingBoneShield._shieldValue.ToString();
                return;

            }
        }
        BarierStatus newBoneShield = new BarierStatus(_shield);
        ally.statusesList.Add(newBoneShield);
        GameObject newBoneShieldUi = (GameObject)Instantiate(Resources.Load("StatusUi/BarierUi"), ally.statusPanel, false);
        newBoneShield.statusUi = newBoneShieldUi.GetComponent<BarierUi>();
        newBoneShield.statusUi.Counters.text = _shield.ToString();
        newBoneShield.host = ally;
    }
}
