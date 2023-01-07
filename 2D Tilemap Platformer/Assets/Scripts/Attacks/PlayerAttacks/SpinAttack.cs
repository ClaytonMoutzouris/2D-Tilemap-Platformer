using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpinAttack", menuName = "ScriptableObjects/Attacks/SpinAttack")]
public class SpinAttack : Attack
{
    public float rotationSpeed = 25;
    //A basic attack.
    public override IEnumerator Activate(PlayerController user, ButtonInput button = ButtonInput.LightAttack)
    {
        entity = user;
        StartUp();

        entity.movementState = PlayerMovementState.Attacking;

        //entity.overrideController["PlayerAttack1"] = ownerAnimation;
        entity._animator.Play(attackAnimation.name);
        entity._animator.speed = attackSpeed;
        //Quaternion startRotation = entity._attackManager.meleeWeaponObject.transform.rotation;

        float rotSpeed = rotationSpeed * entity.GetDirection() * -1;
        float rotReset = 0;

        if (!entity._animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimation.name))
        {
            yield return null;
        }

        //This checks if the animation has completed one cycle, and won't progress until it has
        //This allows for the animator speed to be adjusted by the "attack speed"
        while (entity._animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
        {
            //entity.transform.Rotate(new Vector3(0, 0, rotSpeed));
            //Debug.Log("weaponObject rotation Before: " + entity._attackManager.meleeWeaponObject.transform.rotation);
            //wepRotation.Set(wepRotation.x, wepRotation.y, wepRotation.z, wepRotation.w);

            entity._attackManager.RotationObject.transform.Rotate(Vector3.forward, rotSpeed);
            //rotSpeed += rotSpeed;
            //Debug.Log("weaponObject rotation: " + entity._attackManager.meleeWeaponObject.transform.rotation);
            //user._attackManager.meleeWeaponObject.transform.Rotate(new Vector3(0, 0, rotSpeed));

            //weapon.eulerAngles = new Vector3(0, 0, -180);

            rotReset += Mathf.Abs(rotSpeed);
            if(rotReset > 360)
            {
                rotReset = 0;
                user._attackManager.meleeWeaponObject.ClearHits();
            }

            yield return null;
        }

        //float waitTime = attackAnimation.length * (1 / entity._animator.speed);
        //yield return new WaitForSeconds(waitTime);
        entity._animator.speed = 1;
        //entity.transform.rotation = Quaternion.Euler(Vector3.zero);
        entity._attackManager.RotationObject.transform.rotation = Quaternion.Euler(Vector3.zero);


        if (entity.movementState != PlayerMovementState.Dead)
        {
            entity.movementState = PlayerMovementState.Idle;
        }
        entity._animator.Play(Animator.StringToHash("Idle"));
        CleanUp();
    }

}
