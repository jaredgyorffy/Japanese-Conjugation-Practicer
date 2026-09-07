using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "ScriptableObject/MonsterLibrary", fileName = "MonsterLibrary", order = 0)]
public class MonsterLibrary : ScriptableObject
{
    [field:SerializeField] public List<Monster> EasyMonsters;
    [field: SerializeField] public List<Monster> MediumMonsters;
    [field: SerializeField] public List<Monster> HardMonsters;
    [field: SerializeField] public List<Monster> BossMonsters;

    public Monster GetRandomMonsterByDifficulty(MonsterDifficulty difficulty)
    {
        List<Monster> monsters = GetMonsterListByDifficulty(difficulty);
        int randomMonster = Random.Range(0, monsters.Count);
        return monsters[randomMonster];
    }

    public Monster GetRandomMonsterFromList(List<Monster> monsterList)
    {
        int randomMonster = Random.Range(0, monsterList.Count);
        return monsterList[randomMonster];
    }

    public List<Monster> GetMonsterListByDifficulty(MonsterDifficulty difficulty)
    {
        switch (difficulty)
        {
        case MonsterDifficulty.Easy:
            {
                return EasyMonsters.ToList();
            }
        case MonsterDifficulty.Medium:
            {
                return MediumMonsters.ToList();
            }
        case MonsterDifficulty.Hard:
            {
                return HardMonsters.ToList();
            }
        case MonsterDifficulty.Boss:
            {
                return BossMonsters.ToList();
            }
        default:
            return null;
        }
    }
}
