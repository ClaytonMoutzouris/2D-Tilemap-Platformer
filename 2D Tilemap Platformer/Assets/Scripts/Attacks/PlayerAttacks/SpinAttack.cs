using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpinAttack", menuName = "ScriptableObjects/Attacks/PlayerAttacks/WeaponAttacks/SpinAttack")]
public class SpinAttack : WeaponAttack
{
    public float rotationSpeed = 25;
    //A basic attack.
    public override IEnumerator Activate(ButtonInput button = ButtonInput.LightAttack)
    {
        StartUp();

        player.movementState = PlayerMovementState.Attacking;

        //entity.overrideController["PlayerAttack1"] = ownerAnimation;
        player._animator.Play(attackAnimation.name);
        player._animator.speed = attackSpeed;
        //Quaternion startRotation = entity._attackManager.meleeWeaponObject.transform.rotation;

        float rotSpeed = rotationSpeed * player.GetDirection() * -1;
        float rotReset = 0;

        if (!player._animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimation.name))
        {
            yield return null;
        }

        //This checks if the animation has completed one cycle, and won't progress until it has
        //This allows for the animator speed to be adjusted by the "attack speed"
        while (player._animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
        {
            //entity.transform.Rotate(new Vector3(0, 0, rotSpeed));
            //Debug.Log("weaponObject rotation Before: " + entity._attackManager.meleeWeaponObject.transform.rotation);
            //wepRotation.Set(wepRotation.x, wepRotation.y, wepRotation.z, wepRotation.w);

            player._attackManager.RotationObject.transform.Rotate(Vector3.forward, rotSpeed);
            //rotSpeed += rotSpeed;
            //Debug.Log("weaponObject rotation: " + entity._attackManager.meleeWeaponObject.transform.rotation);
            //user._attackManager.meleeWeaponObject.transform.Rotate(new Vector3(0, 0, rotSpeed));

            //weapon.eulerAngles = new Vector3(0, 0, -180);

            rotReset += Mathf.Abs(rotSpeed);
            if(rotReset > 360)
            {
                rotReset = 0;
                player._attackManager.meleeWeaponObject.ClearHits();
            }

            yield return null;
        }

        //float waitTime = attackAnimation.length * (1 / entity._animator.speed);
        //yield return new WaitForSeconds(waitTime);
        player._animator.speed = 1;
        //entity.transform.rotation = Quaternion.Euler(Vector3.zero);
        player._attackManager.RotationObject.transform.rotation = Quaternion.Euler(Vector3.zero);


        if (player.movementState != PlayerMovementState.Dead)
        {
            player.movementState = PlayerMovementState.Idle;
        }
        player._animator.Play(Animator.StringToHash("Idle"));
        CleanUp();
    }

}
