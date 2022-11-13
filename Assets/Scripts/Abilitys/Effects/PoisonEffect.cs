using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : Effect
{
    public PoisonEffect(int poison)
    {
        _poison = poison;
    }

    public int _poison;

    public override void Execute(Entity enemy, Entity ally)
    {
        foreach (Status status in enemy.statusesList)
        {
            if (status.GetType() == typeof(PoisonStatus))
            {
                PoisonStatus existingPoison = (PoisonStatus)status;
                if (existingPoison._damage > 2)
                {
                    return;
                }
                existingPoison._damage += _poison;
                existingPoison.statusUi.Counters.text = existingPoison._damage.ToString();
                return;

            }
        }
        PoisonStatus newPoison = new PoisonStatus(_poison);
        enemy.statusesList.Add(newPoison);
        GameObject newBoneShieldUi = (GameObject)Instantiate(Resources.Load("StatusUi/PoisonUi"), enemy.statusPanel, false);
        newPoison.statusUi = newBoneShieldUi.GetComponent<PoisonUi>();
        newPoison.statusUi.Counters.text = _poison.ToString();
        newPoison.host = enemy;
    }
}
