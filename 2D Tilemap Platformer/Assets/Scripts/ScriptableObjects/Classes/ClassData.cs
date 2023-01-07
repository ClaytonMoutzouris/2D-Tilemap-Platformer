using UnityEngine;

public enum ClassType { Engineer, Soldier, Medic, BountyHunter };

[CreateAssetMenu(fileName = "ClassData", menuName = "ScriptableObjects/Classes/ClassData")]
public class ClassData : ScriptableObject
{
    [Header("Class Info")]
    public string className;

    [Header("Talents")]
    public TalentTree talents;
}
