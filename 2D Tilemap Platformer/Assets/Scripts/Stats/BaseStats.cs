using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseStats", menuName = "ScriptableObjects/Stats/BaseStats")]

public class BaseStats : ScriptableObject
{
    [Header("Primary Stats")]
    public Stat[] startingStats = new Stat[(int)StatType.Luck + 1] {
        new Stat(StatType.Attack, 1),
        new Stat(StatType.Defense, 1),
        new Stat(StatType.Constitution, 1),
        new Stat(StatType.Speed, 1),
        new Stat(StatType.Luck, 1)

    };

    [Header("Secondary Stats and Dependencies")]
    public SecondaryStat[] startingSecondaryStats = new SecondaryStat[(int)SecondaryStatType.StatusDurationReduction + 1] {
        new SecondaryStat(SecondaryStatType.MoveSpeed,
            null,
            new List<StatDependancy> {
                new StatDependancy(StatType.Speed, .2f)
            },
            3
        ),
        new SecondaryStat(SecondaryStatType.BaseHealth,
            null,
            new List<StatDependancy> {
                new StatDependancy(StatType.Constitution, 5f)
            },
            10
        ),
        new SecondaryStat(SecondaryStatType.DamageBonus,
            null,
            new List<StatDependancy> {
                new StatDependancy(StatType.Attack, .5f)
            },
            0
        ),
        new SecondaryStat(SecondaryStatType.CritChance,
            null,
            new List<StatDependancy> {
                new StatDependancy(StatType.Luck, 2.5f)
            },
            5
        ),       
        new SecondaryStat(SecondaryStatType.CritDamage,
            null,
            new List<StatDependancy> {
            },
            200
        ),
        new SecondaryStat(SecondaryStatType.AttackSpeedBonus,
            null,
            new List<StatDependancy> {
                new StatDependancy(StatType.Speed, 1f)
            },
            0
        ),
        new SecondaryStat(SecondaryStatType.DamageReduction,
            null,
            new List<StatDependancy> {
                new StatDependancy(StatType.Defense, 1f)
            },
            0
        ),
        new SecondaryStat(SecondaryStatType.DodgeChance,
            null,
            new List<StatDependancy> {
                new StatDependancy(StatType.Luck, 1f)
            },
            5
        ),
        new SecondaryStat(SecondaryStatType.StatusDurationReduction,
            null,
            new List<StatDependancy> {
                new StatDependancy(StatType.Defense, 2.5f)
            },
            0
        ),
        new SecondaryStat(SecondaryStatType.JumpHeight,
            null,
            new List<StatDependancy> {
            },
            4
        ),
        new SecondaryStat(SecondaryStatType.ExtraJumps,
            null,
            new List<StatDependancy> {
                new StatDependancy(StatType.Speed, .1f)
            },
            0
        )
    };
}
