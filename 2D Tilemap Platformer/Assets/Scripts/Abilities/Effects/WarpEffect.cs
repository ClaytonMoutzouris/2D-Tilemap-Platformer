using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WarpEffect", menuName = "ScriptableObjects/Effects/TriggeredEffects/WarpEffect")]
public class WarpEffect : Effect
{
    [Header("Warp Effect")]
    public float warpDistance = 5;
    public float warpDelay = .5f;
    public Vector3 warpDirection = Vector2.right;
    public bool useAim = true;
    public ParticleSystem warpOutVisual;
    public ParticleSystem warpInVisual;

    public override void ApplyEffect(Entity owner, Entity effected, AttackHitData data = null)
    {
        base.ApplyEffect(owner, effected, data);

        if (effectedEntity)
        {
            effectedEntity.StartCoroutine(Warp());
        }
    }

    public IEnumerator Warp()
    {
        ParticleSystem origin = Instantiate(warpOutVisual, effectedEntity.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(warpDelay);

        if (useAim && effectedEntity is PlayerController player)
        {
            warpDirection = player._input.GetLeftStickAim().normalized;
        }
        else
        {
            warpDirection.x *= effectOwner.GetDirection();
        }

        effectedEntity.transform.position = effectedEntity.transform.position + warpDirection * warpDistance;
        ParticleSystem destination = Instantiate(warpInVisual, effectedEntity.transform.position, Quaternion.identity);

    }

}
