using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    public int damage = 15;
    public float attackRange = 3f;
    public DeerCombat deerCompanion;
    public Button specialAbilityButton;

    private float abilityCooldown = 30f;
    private float lastAbilityUse;

    void Start()
    {
        if (specialAbilityButton != null)
        {
            specialAbilityButton.onClick.AddListener(UseDeerSpecialAbility);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AttackEnemy();
        }

        // Enable button after cooldown
        if (specialAbilityButton != null)
        {
            specialAbilityButton.interactable = Time.time > lastAbilityUse + abilityCooldown;
        }
    }

    void AttackEnemy()
    {
        GameObject enemy = GameObject.FindWithTag("Enemy");
        if (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) <= attackRange)
        {
            Debug.Log("Player attacked enemy!");
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(damage);
        }
    }

    void UseDeerSpecialAbility()
    {
        if (Time.time > lastAbilityUse + abilityCooldown)
        {
            Debug.Log("Used deer ability!");
            lastAbilityUse = Time.time;
            // Trigger some special logic in the deer
        }
    }
}
