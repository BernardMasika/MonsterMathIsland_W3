using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] Transform healthBarUI;

    public int maxHealth = 150;

    public UnityEvent onDeath;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBarUI();
    }


    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            health = 0;
            onDeath.Invoke();
        }

        UpdateHealthBarUI();
    }

    public void SetHealthBar(Transform _healthBarUI)
    {
        healthBarUI = _healthBarUI;
    }

    private void UpdateHealthBarUI()
    {
        if (!healthBarUI) return;

        healthBarUI.GetChild(1).GetComponent<Image>().fillAmount = (float)health / maxHealth;
        healthBarUI.GetChild(2).GetComponent<TMP_Text>().text = $"{health} / {maxHealth} ";
    }
}
