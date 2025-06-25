using UnityEngine;
using System.Collections; 
using UnityEngine.UI;     

public class ReminderDisplay : MonoBehaviour
{
    [Header("Reminder Settings")]
    [Tooltip("The duration (in seconds) the reminder image will be visible.")]
    public float displayDuration = 60f;

    private Image reminderImage; 
    private Coroutine activeReminderCoroutine; 

    void Awake()
    {
      
        reminderImage = GetComponent<Image>();

        
        if (reminderImage != null)
        {
            reminderImage.enabled = false; 
        }
    }

 
    public void ShowReminder()
    {
        if (reminderImage == null)
        {
            Debug.LogError("ReminderImage component not found on this GameObject.", this);
            return;
        }

       
        if (activeReminderCoroutine != null)
        {
            StopCoroutine(activeReminderCoroutine);
        }

        reminderImage.enabled = true;
      
        Debug.Log($"Reminder will be visible for {displayDuration} seconds.");

   
        activeReminderCoroutine = StartCoroutine(HideReminderAfterDelay());
    }

    
    private IEnumerator HideReminderAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);

        // Hide the image after the delay
        if (reminderImage != null)
        {
            reminderImage.enabled = false;
            
        }
        Debug.Log("Reminder hidden.");
        activeReminderCoroutine = null; // Clear the reference
    }


    public void HideReminderImmediately()
    {
        if (activeReminderCoroutine != null)
        {
            StopCoroutine(activeReminderCoroutine);
            activeReminderCoroutine = null;
        }
        if (reminderImage != null)
        {
            reminderImage.enabled = false;
       
        }
        Debug.Log("Reminder hidden immediately.");
    }
}