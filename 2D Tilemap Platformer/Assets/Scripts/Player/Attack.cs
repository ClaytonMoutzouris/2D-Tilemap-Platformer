using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public string ownerAnimState = "DEFAULT_ATTACK";
    public Collider2D hitbox;
    public float delay = 0f;
    public AttackData attackData;

    public IEnumerator Activate(AttackManager manager)
    {
        //player._animator.Play();
        animator.Play("Attack");
        animator.speed = attackData.attackSpeed;
        spriteRenderer.color = attackData.color;
        Debug.Log("duration: " + animator.GetCurrentAnimatorStateInfo(0).length + delay);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length*(1/animator.speed) + delay);
        manager.attacking = false;
        Destroy(gameObject);
        //Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length + delay);
    }


}
