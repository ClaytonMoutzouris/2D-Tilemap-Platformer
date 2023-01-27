using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpinAttack", menuName = "ScriptableObjects/Attacks/WeaponAttacks/PlayerAttacks/WeaponAttack")]
public class WeaponAttack : PlayerAttack
{
    [HideInInspector]
    public Weapon weapon;
    
    public virtual void SetWeapon(Weapon wep)
    {
        weapon = wep;
        attackSpeed = wep.attackSpeed + (wep.attackSpeed*(player.stats.GetSecondaryStat(SecondaryStatType.AttackSpeedBonus).GetValue()/100));
    }

}
