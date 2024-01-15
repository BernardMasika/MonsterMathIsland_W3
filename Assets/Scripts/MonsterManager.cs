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
    [SerializeField] private GameObject deadMonsterPoint;
    [SerializeField] TMP_Text waveNoUI;
    public List<GameObject> _monsters;

    int monsterWaveCount = 0;
    int _originalAmountOfMonsters;



    // Start is called before the first frame update
    private void Awake()
    {
        _originalAmountOfMonsters = _amountOfMonsters;
        SpawnMonsterWave();
    }

    public void SpawnMonsterWave()
    {
        monsterWaveCount++;
        _amountOfMonsters = _originalAmountOfMonsters + Mathf.FloorToInt(monsterWaveCount / 2f);

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
        int max = _monsterPrefabs.Length;
        if (monsterWaveCount < 2) max = 1;
        else if (monsterWaveCount > 1 && monsterWaveCount < 4) max = 2;
        else if (monsterWaveCount > 3) max = 3;
        int monsterIndex = Mathf.FloorToInt(Random.Range(0, max));
        var monster = Instantiate(_monsterPrefabs[monsterIndex], _spawnPoint.position, Quaternion.identity).GetComponent<Statistics>();
        monster.level = Random.Range(0, monsterWaveCount + 1);
        monster.LevelUp();

        _monsters.Add(monster.gameObject);
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
            if (t != null)
            {
                t.position = Vector3.Lerp(t.position, position, timer * speed);
                t.rotation = rotation;
            }

            timer += Time.deltaTime;

            yield return null;
        }

    }

    public void KillMonster(int monsterIndex)
    {
        GameObject monster = _monsters[monsterIndex];
        StopAllCoroutines();
        StartCoroutine(LearpToPosition(monster.transform, deadMonsterPoint.transform.position, monster.transform.rotation, 0.6f));
        StartCoroutine(DestroyMonster(monster, 1.5f));


        _monsters.RemoveAt(monsterIndex);
    }

    IEnumerator DestroyMonster(GameObject monster, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (monster.transform != null)
            Destroy(monster);
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
