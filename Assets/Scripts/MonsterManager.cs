using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private Transform monsterHealthBarUI;
    [SerializeField] TMP_Text waveNoUI;
    public List<GameObject> _monsters;
    
    int monsterWaveCount = 0;



    // Start is called before the first frame update
    private void Awake()
    {
        SpawnMonsterWave();
    }

    public void SpawnMonsterWave()
    {
        monsterWaveCount++;
        
        for (var i = 0; i < _amountOfMonsters; i++)
        {
            InstantiateMonster();
        }

        MonsterAttacks(0);
        MoveNextMonsterToQueue();

        CalculateWaveDifficulty(ref waveDifficulty);

        if (!waveNoUI) return;
        waveNoUI.text = $"Wave {monsterWaveCount}";
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
        int monsterIndex = Mathf.FloorToInt(Random.Range(0, _monsterPrefabs.Length));
        var monster = Instantiate(_monsterPrefabs[monsterIndex], _spawnPoint.position, Quaternion.identity);
        _monsters.Add(monster);
    }

    private void MoveMonsterToQueuePoint(int monsterIndex)
    {
        if (_monsters.Count <= monsterIndex) return;

        Transform monster = _monsters[monsterIndex].transform;
        monster.GetComponent<MonsterController>().state = MonsterState.Queue;
        StartCoroutine(LearpToPosition(monster, _queuePoint.position, _queuePoint.rotation, 0.3f));

    }

    private IEnumerator LearpToPosition(Transform t, Vector3 position, Quaternion rotation, float speed)
    {
        float distToPos = Vector3.Distance(t.position, position);
        float timer = 0;

        while (distToPos > 0.1f)
        {
            t.position = Vector3.Lerp(t.position, position, timer * speed);
            t.rotation = rotation;
            timer += Time.deltaTime;

            yield return null;
        }

    }

    public void KillMonster(int monsterIndex)
    {
        StopAllCoroutines();
        Destroy(_monsters[monsterIndex]);
        _monsters.RemoveAt(monsterIndex);
    }

    public void MonsterAttacks(int monsterIndex)
    {
        if (_monsters.Count <= monsterIndex) return;

        Transform monster = _monsters[monsterIndex].transform;
        monster.GetComponent<MonsterController>().state = MonsterState.Attack;
        StartCoroutine(LearpToPosition(monster, _attackPoint.position, _attackPoint.rotation, 0.3f));


        var monsterHealth = monster.GetComponent<Health>();
        monsterHealth.SetHealthBar(monsterHealthBarUI);
        monsterHealth.onDeath.AddListener(MonsterDeath);
    }

    private void MonsterDeath()
    {
        KillMonster(0);
        MonsterAttacks(0);
        MoveNextMonsterToQueue();
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
