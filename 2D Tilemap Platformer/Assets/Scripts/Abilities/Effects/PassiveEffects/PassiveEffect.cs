using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveEffect", menuName = "ScriptableObjects/Effects/PassiveEffects/PassiveEffect")]
public class PassiveEffect : Effect
{

    public Entity effectedEntity;

    public ParticleSystem effectPrefab;
    ParticleSystem activeSystem;


}
