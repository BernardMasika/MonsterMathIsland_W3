using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    public int level = 1;
    public int experience;
    public Image expBarUI;

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
        int prevExp = experience;
        experience += amount;

        StartCoroutine(LerpExpBar(prevExp));

        if (experience >= nextLvUp)
        {
            LevelUp();
        }
    }

    private IEnumerator LerpExpBar(int prevExp)
    {
        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime;
            float currentLevelExp = experience - prevLvUp;
            float prevLevelExp = prevExp - prevLvUp;

            float totalLevelExp = nextLvUp - prevLvUp;

            float mappedExp = currentLevelExp / totalLevelExp;

            float mappedPrevExp = prevLevelExp / totalLevelExp;


            expBarUI.fillAmount = Mathf.Lerp(mappedPrevExp, mappedExp, t);
            yield return null;
        }

    }

    private void LevelUp()
    {
        level++;
        CalculateStats();
    }
}
