using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gadget", menuName = "ScriptableObjects/Items/Equipment/Gadget")]
public class Gadget : Equipment
{
    public Effect activatedEffect;
    public float cooldown;
    float lastActiveTimestamp = 0;
    Effect activeEffect;

    public override string GetTooltip()
    {
        return base.GetTooltip();
    }

    public override void OnEquipped(PlayerController entity)
    {
        base.OnEquipped(entity);
    }

    public override void OnUnequipped(CharacterEntity entity)
    {
        if(activeEffect)
        {
            activeEffect.RemoveEffect();
        }

        base.OnUnequipped(entity);

    }

    public virtual void Activate()
    {
        if(activatedEffect && Time.time > lastActiveTimestamp + cooldown)
        {
            Effect temp = Instantiate(activatedEffect);
            temp.ApplyEffect(owner);
            activeEffect = temp;
            lastActiveTimestamp = Time.time;
        }
    }
}
