using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHurtable
{
    Hurtbox GetHurtbox();
    Health GetHealth();
    Entity GetEntity();
    void GetHurt(ref AttackHitData hitData);
    bool CheckHit(AttackObject attackObject);
    bool CheckFriendly(Entity entity);

}

public interface IAttacker
{

}