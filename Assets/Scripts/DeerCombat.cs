using UnityEngine;
using System.Collections; 
using UnityEngine.UI;     

public class DeerCombat : MonoBehaviour
{
    public int health = 100;
    public GameObject projectilePrefab;
    public Transform shootPoint;

    [Header("Damage Flash Settings")]
    public Image damageFlashImage; 
    public float flashDuration = 0.2f;
    public float fadeOutTime = 0.1f;  


    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("Deer took damage! Remaining health: " + health);

       
        if (damageFlashImage != null)
        {
         
            StopAllCoroutines(); 
            StartCoroutine(FlashScreenDamage());
        }

        if (health <= 0)
        {
            Debug.Log("Deer is defeated!");
           
            Destroy(gameObject);
        }
    }

    private IEnumerator FlashScreenDamage()
    {
      
        if (damageFlashImage.gameObject.activeSelf == false)
        {
            damageFlashImage.gameObject.SetActive(true);
        }

        
      
        damageFlashImage.enabled = true;

       
        Color currentColor = damageFlashImage.color;
        currentColor.a = 1f; 
        damageFlashImage.color = currentColor;

       
        yield return new WaitForSeconds(flashDuration);

        
        float startAlpha = damageFlashImage.color.a;
        for (float t = 0; t < fadeOutTime; t += Time.deltaTime)
        {
            Color newColor = damageFlashImage.color;
            newColor.a = Mathf.Lerp(startAlpha, 0f, t / fadeOutTime);
            damageFlashImage.color = newColor;
            yield return null; 
        }

      
        currentColor = damageFlashImage.color;
        currentColor.a = 0f;
        damageFlashImage.color = currentColor;

        damageFlashImage.enabled = false;
    }
}