using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObject/MonsterLibrary", fileName = "MonsterLibrary", order = 0)]
public class MonsterLibrary : ScriptableObject
{
    public List<Monster> EasyMonsters;
    public List<Monster> MediumMonsters;
    public List<Monster> HardMonsters;
    public List<Monster> BossMonsters;

    public Monster GetRandomMonsterByDifficulty(MonsterDifficulty difficulty)
    {
        List<Monster> monsters = GetMonsterListByDifficulty(difficulty);
        int randomMonster = Random.Range(0, monsters.Count);
        return monsters[randomMonster];
    }

    public List<Monster> GetMonsterListByDifficulty(MonsterDifficulty difficulty)
    {
        switch (difficulty)
        {
        case MonsterDifficulty.Easy:
            {
                return EasyMonsters;
            }
        case MonsterDifficulty.Medium:
            {
                return MediumMonsters;
            }
        case MonsterDifficulty.Hard:
            {
                return HardMonsters;
            }
        case MonsterDifficulty.Boss:
            {
                return BossMonsters;
            }
        default:
            return null;
        }
    }
}
