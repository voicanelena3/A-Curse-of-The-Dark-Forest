using UnityEngine;

public class DelayedReminderTrigger : MonoBehaviour
{
    [Tooltip("Drag your ReminderImage GameObject (with ReminderDisplay script) here.")]
    public ReminderDisplay reminderDisplay;

    [Tooltip("Delay in seconds before the reminder appears (1.5 minutes = 90 seconds).")]
    public float delayInSeconds = 90f; // 1.5 minutes

    void Start()
    {
        if (reminderDisplay == null)
        {
            Debug.LogError("ReminderDisplay reference is missing on DelayedReminderTrigger. The reminder cannot be shown.", this);
            return;
        }

        // Schedule the ShowReminder method to be called after the specified delay
        Invoke("TriggerTheReminder", delayInSeconds);
        Debug.Log($"Reminder scheduled to appear in {delayInSeconds} seconds.");
    }

    private void TriggerTheReminder()
    {
        // This method is called by Invoke
        if (reminderDisplay != null)
        {
            reminderDisplay.ShowReminder();
        }
    }
}