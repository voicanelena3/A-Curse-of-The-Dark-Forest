/*using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 60;

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log("Enemy took damage! Remaining: " + health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    public int health = 60;

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log("Enemy took damage! Remaining: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(4);
       
    }
}
