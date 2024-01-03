using System.Collections.Generic;
using TutorialAssets.Scripts;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _queuePoint;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private int _amountOfMonsters = 20;
    [SerializeField] private GameObject[] _monsterPrefabs;
    [SerializeField] private float waveDifficulty;
    public List<GameObject> _monsters;
    
    // Start is called before the first frame update
    private void Awake()
    {
        for (var i = 0; i < _amountOfMonsters; i++)
        {
            InstantiateMonster();
        }

        MonsterAttacks(0);
        MoveNextMonsterToQueue();
        
        CalculateWaveDifficulty(ref waveDifficulty);
    }

    //Returns a value between 0 to 1 for the difficulty of this monster wave
    private float CalculateWaveDifficulty(ref float difficulty)
    {
        foreach (var monster in _monsters)
        {
            difficulty += monster.GetComponent<MonsterController>().points;
        }

        difficulty /= (_amountOfMonsters * 3); //use 3 as 3 is the maximum points a single monster can yield

        return difficulty;
    }

    private void InstantiateMonster()
    {
        int monsterIndex = Mathf.FloorToInt(Random.Range(0,_monsterPrefabs.Length));
        var monster = Instantiate(_monsterPrefabs[monsterIndex],_spawnPoint.position,Quaternion.identity);
        _monsters.Add(monster);
    }

    private void MoveMonsterToQueuePoint(int monsterIndex)
    {
        if (_monsters.Count <= monsterIndex) return;
        
        Transform monster = _monsters[monsterIndex].transform;
        monster.GetComponent<MonsterController>().state = MonsterState.Queue;
        monster.position = _queuePoint.position;
        monster.rotation = _queuePoint.rotation;
    }

    public void KillMonster(int monsterIndex)
    {
        Destroy(_monsters[monsterIndex]);
        _monsters.RemoveAt(monsterIndex);
    }

    public void MonsterAttacks(int monsterIndex)
    {
        if (_monsters.Count <= monsterIndex) return;
        
        Transform monster = _monsters[monsterIndex].transform;
        monster.GetComponent<MonsterController>().state = MonsterState.Attack;
        monster.position = _attackPoint.position;
        monster.rotation = _attackPoint.rotation;
    }

    public void MoveNextMonsterToQueue()
    {
        if (_monsters.Count <= 1) return;
        
        MoveMonsterToQueuePoint(1);
    }

    public bool IsMonsterListEmpty()
    {
        return _monsters.Count == 0;
    }

    public MonsterType GetMonsterType(int monsterIndex)
    {
        return _monsters[monsterIndex].GetComponent<MonsterController>().type;
    }
}
