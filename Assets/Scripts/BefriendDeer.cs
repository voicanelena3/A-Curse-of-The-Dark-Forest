using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class BefriendDeer : MonoBehaviour
{
    public Image deerPhotoImage;    
    public TMP_Text deerText;      

    public Sprite befriendedDeerSprite; 

    
    void Start()
    {
      
        if (deerPhotoImage != null)
        {
            deerPhotoImage.enabled = false;
        }

        if (deerText != null)
        {
           
            deerText.gameObject.SetActive(false);
           
        }
    }

    public void OnDeerBefriended()
    {
        if (deerPhotoImage != null && befriendedDeerSprite != null)
        {
            deerPhotoImage.sprite = befriendedDeerSprite; 
            deerPhotoImage.enabled = true; 
        }

        if (deerText != null)
        {
            deerText.text = "Friend: Deer God of The Forest"; 
            deerText.gameObject.SetActive(true); 
        }
    }
}