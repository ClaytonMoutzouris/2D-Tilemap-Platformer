using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    //List of different player attack animations
    public PlayerController player;
    public List<AttackData> attacks = new List<AttackData>();
    public bool attacking = false;
    public Attack activeAttack;

    private void Start()
    {
        player = GetComponent<PlayerController>();
    }

    public bool ActivateAttack(int index)
    {
        if(!attacking)
        {
            attacking = true;
            Attack temp = Instantiate(attacks[index].baseAttack, transform);
            temp.attackData = attacks[index];
            StartCoroutine(temp.Activate(this));
            activeAttack = temp;
            return true;
        }

        if(!activeAttack)
        {
            attacking = false;
        }

        return false;
    }

}
