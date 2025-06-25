using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Notification : MonoBehaviour
{
    public bool Enabled
    {
        get { return this.gameObject.activeSelf; }
        set
        {
            this.gameObject.SetActive(value);
            if (value == true)
            {
                StartCoroutine(StayVisible());
            }
        }
    }

    IEnumerator StayVisible()
    {
        yield return new WaitForSeconds(2);
        this.gameObject.SetActive(false);
    }
}
