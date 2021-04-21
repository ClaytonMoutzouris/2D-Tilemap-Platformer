using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "ScriptableObjects/Attacks/AttackData")]
public class AttackData : ScriptableObject
{
    //This essentially just holds a prefab in it, not sure what other info can be here.
    public Attack attack;

}
