using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CompanionAbility", menuName = "ScriptableObjects/Abilities/CompanionAbility")]
public class CompanionAbility : Ability
{
    public Companion prefab;

    Companion currentCompanion;

    public override void OnGainedAbility(Entity entity)
    {
        base.OnGainedAbility(entity);

        if(owner is PlayerController player)
        {
            Companion newCompanion = Instantiate(prefab, owner.transform.position, Quaternion.identity);
            newCompanion.SetOwner(player);
            currentCompanion = newCompanion;
        }
        
    }

    public override void OnAbilityLost()
    {
        currentCompanion.owner._companionManager.RemoveCompanion(currentCompanion);
        currentCompanion.owner = null;

        Destroy(currentCompanion.gameObject);
        currentCompanion = null;

        base.OnAbilityLost();
    }
}
