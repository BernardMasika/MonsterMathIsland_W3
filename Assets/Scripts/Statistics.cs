using UnityEngine;

public class Statistics : MonoBehaviour
{
    public int level = 1;
    public int experience;

    public int baseMaxHealth = 54;
    public int baseAttack = 10;
    public int baseDefense = 10;

    public int maxHealth;
    public int attack;
    public int defense;

    private int prevLvUp;
    private int nextLvUp;

    // Start is called before the first frame update
    void Start()
    {
        CalculateStats();
        experience = prevLvUp;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CalculateStats()
    {
        prevLvUp = (int)Mathf.Pow(level, 4f);
        nextLvUp = (int)Mathf.Pow(level + 1, 4f);
        maxHealth = baseMaxHealth + (int)Mathf.Pow(level, 1.2f) * 10 + (int)Random.Range(0, 10);
        attack = baseAttack + (int)Mathf.Pow(level, 1.15f);
        defense = baseDefense + (int)Mathf.Pow(level, 1.15f);
    }

    public void GainExperience(int amount)
    {
        experience += amount;
        if (experience >= nextLvUp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        CalculateStats();
    }
}
