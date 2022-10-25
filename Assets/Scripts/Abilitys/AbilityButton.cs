using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    [SerializeField]
    private GameObject abilityGo;

    public Ability ability;
    // Start is called before the first frame update
    public void Start()
    {
        ability = abilityGo.GetComponent<Ability>();
        ability.Start();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CastAbility()
    {
        ability.Cast();
    }
}
