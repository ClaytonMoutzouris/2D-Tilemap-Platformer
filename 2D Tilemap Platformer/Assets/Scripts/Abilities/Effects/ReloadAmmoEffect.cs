using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReloadAmmoEffect", menuName = "ScriptableObjects/Effects/TriggeredEffects/ReloadAmmoEffect")]
public class ReloadAmmoEffect : TriggeredEffect
{
    [Header("Reload Effect")] 
    public AmmoType ammoType = AmmoType.None;
    //If the ammo type is set to None, it will reload any type

    public override void ApplyEffect(Entity owner, Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(owner, effected, data);

        if(effectedEntity && effectedEntity is PlayerController player)
        {
            Weapon weapon = player._equipmentManager.GetEquippedWeapon(WeaponSlot.Ranged);

            if(weapon)
            {
                weapon.Reload(ammoType);
            }
        }
    }

    public override void Trigger(Entity owner, AttackHitData data = null)
    {
        if (owner && owner is PlayerController player)
        {
            Weapon weapon = player._equipmentManager.GetEquippedWeapon(WeaponSlot.Ranged);

            if (weapon)
            {
                weapon.Reload(ammoType);
            }
        }
    }

}
