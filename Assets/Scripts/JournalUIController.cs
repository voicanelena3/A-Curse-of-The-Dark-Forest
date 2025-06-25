using UnityEngine;
using UnityEngine.UI; // Still needed for Button, Image, and Panel
using TMPro;        // Required for TextMeshProUGUI
using System.Collections.Generic; // Required for List

public class JournalUIController : MonoBehaviour
{
    [Header("Journal References")]
    [Tooltip("Drag the GameObject with the GameJournal script here (e.g., GameJournalManager).")]
    public GameJournal gameJournal; 

    [Tooltip("Drag the UI Panel GameObject that represents your journal screen here.")]
    public GameObject journalPanel; 

    [Header("Journal Control Buttons")]
    [Tooltip("Drag the Button GameObject that will open/close the journal here.")]
    public Button openJournalButton; 

    [Tooltip("Drag the Button GameObject for closing the journal (if separate) here. Optional.")]
    public Button closeJournalButton; 



    [Header("Display Elements")] 
    [Tooltip("UI TextMeshProUGUI element to display the deer's name.")]
    public TextMeshProUGUI deerNameText;
    [Tooltip("UI TextMeshProUGUI element to display the deer's characteristics.")]
    public TextMeshProUGUI deerCharacteristicsText;
    [Tooltip("UI Image element to display the deer's photo.")]
    public Image deerPhotoDisplay;



    private bool isJournalOpen = false; 

    void Start()
    {
      
        if (journalPanel != null)
        {
            journalPanel.SetActive(false);
        }


        if (openJournalButton != null)
        {
            openJournalButton.onClick.AddListener(ToggleJournal);
        }
        if (closeJournalButton != null)
        {
            closeJournalButton.onClick.AddListener(ToggleJournal); 
        }

        
        if (gameJournal == null)
        {
            Debug.LogError("JournalUIController: GameJournal reference is not set. Please assign it in the Inspector.");
        }
    }

    
    public void ToggleJournal()
    {
        if (journalPanel == null)
        {
            Debug.LogError("JournalUIController: Journal Panel is not assigned! Cannot toggle.");
            return;
        }

        isJournalOpen = !isJournalOpen;
        journalPanel.SetActive(isJournalOpen);
        Debug.Log($"Journal Toggled: {(isJournalOpen ? "Open" : "Closed")}");

        if (isJournalOpen)
        {
            RefreshJournalDisplay();
        }
    }

   
    private void RefreshJournalDisplay()
    {
        if (gameJournal == null) return;

        List<DeerEntry> allDeer = gameJournal.GetAllDeerEntries();

        if (allDeer.Count == 0)
        {
            Debug.Log("No deer befriended yet!");
           
            if (deerNameText != null) deerNameText.text = "No Deer Befriended";
            if (deerCharacteristicsText != null) deerCharacteristicsText.text = "";
            if (deerPhotoDisplay != null) deerPhotoDisplay.sprite = null;
        }
        else
        {
            if (deerNameText != null && deerCharacteristicsText != null && deerPhotoDisplay != null)
            {
                DeerEntry firstDeer = allDeer[0]; 
                deerNameText.text = firstDeer.deerName;
                deerCharacteristicsText.text = firstDeer.characteristics;

                
                Sprite deerSprite = Resources.Load<Sprite>(firstDeer.photoPath);
                if (deerSprite != null)
                {
                    deerPhotoDisplay.sprite = deerSprite;
                }
                else
                {
                    Debug.LogWarning($"Could not load sprite from path: {firstDeer.photoPath}. Ensure it's in a Resources folder and path is correct. Using default sprite.");
                   
                    deerPhotoDisplay.sprite = null;
                }
            }
        }
        
    }
}
