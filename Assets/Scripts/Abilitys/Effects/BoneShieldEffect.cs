using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoneShieldEffect : Effect
{
    public BoneShieldEffect(int shield)
    {
        _shield = shield;
    }

    public int _shield;

    public override void Execute(Entity enemy, Entity ally)
    {
        foreach(Status status in ally.statusesList)
        {
            if(status.GetType() == typeof(BoneShieldStatus))
            {
                BoneShieldStatus existingBoneShield = (BoneShieldStatus)status;
                existingBoneShield._shieldValue += _shield;
                existingBoneShield.statusUi.Counters.text = existingBoneShield._shieldValue.ToString();
                return;
            }
        }
        BoneShieldStatus newBoneShield = new BoneShieldStatus(_shield);
        ally.statusesList.Add(newBoneShield);
        GameObject newBoneShieldUi = (GameObject)Instantiate(Resources.Load("StatusUi/BoneShieldUi"), ally.statusPanel, false);
        newBoneShield.statusUi = newBoneShieldUi.GetComponent<BoneShieldUi>();
        newBoneShield.statusUi.Counters.text = _shield.ToString();
        newBoneShield.host = ally;
    }
}
