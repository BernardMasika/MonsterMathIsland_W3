using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    public int level = 1;
    public int experience;
    public Image expBarUI;
    public GameObject levelUpUI;

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

        if (!expBarUI) yield break;

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

    public void LevelUp()
    {
        int prevHealth = maxHealth;
        int prevAttack = attack;
        int prevDefense = defense;
        level++;
        CalculateStats();

        if (!levelUpUI) return;

        levelUpUI.SetActive(true);
        levelUpUI.transform.GetChild(1).GetComponent<TMP_Text>().text = $"Level: {level}\r\n" +
            $"HP: {maxHealth} (+{maxHealth - prevHealth})\r\n" +
            $"Attack: {attack} (+{attack - prevAttack})\r\n" +
            $"Defense: {defense} (+{defense - prevDefense})";

        Invoke(nameof(CloseLevelUpUI), 3f);
    }

    void CloseLevelUpUI() { levelUpUI.SetActive(false); }
}
