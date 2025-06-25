using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Image actorImage;
    public Text actorName;
    public Text messageText;
    public RectTransform backgroundBox;

    IMessage currentMessage;
    Actor[] currentActors;
    public static bool isActive = false;

    public InventoryManager inventoryManager;
    public Item[] itemsToRemove;

    public DeerStateMachine deerStateMachine;
    private DialogueTrigger currentTrigger;
    private NPC currentNPC;

    public GameObject DialogueBox;
    public GameObject DialogueButton;
    public GameObject DialogueFriendship;
    public GameObject dialogueFirstChoice;
    public GameObject dialogueSecondChoice;

    public Notification goodNotification;
    public Notification badNotification;

    private HashSet<IMessage> MessageList = new HashSet<IMessage>();

    public GameObject gameJournal;

    public void OpenDialogue(List<IMessage> messages, Actor[] actors,DialogueTrigger newTrigger,NPC newNPC)
    {
        foreach(IMessage message in messages)
        {
            MessageList.Add(message);
        }

        currentMessage = messages[0];
        
        currentActors = actors;
        isActive= true;
        currentTrigger = newTrigger;
        currentNPC = newNPC;
        Debug.Log("Started conversation!Loaded messages:" + messages.Count);
       
        DisplayMessage();
        DialogueBox.SetActive(true);
        DialogueButton.SetActive(true);
        DialogueFriendship.SetActive(true);
        DialogueFriendship.GetComponent<ProgressBar>().CurrentValue = currentNPC.levelOfFriendship;

    }
    void DisplayMessage()
    {
        IMessage messageToDisplay = currentMessage;
        if (messageToDisplay is Message)
        {
            dialogueFirstChoice.SetActive(false);
            dialogueSecondChoice.SetActive(false);

            Message simpleMessage= messageToDisplay as Message;
            messageText.text = simpleMessage.message;

            Actor actorToDisplay = currentActors[simpleMessage.actorId];
            actorName.text = actorToDisplay.name;
            actorImage.sprite = actorToDisplay.sprite;
        }
        else
        {
            dialogueFirstChoice.SetActive(true);
            dialogueSecondChoice.SetActive(true);

            MessageWithChoices choiceMessage =messageToDisplay as MessageWithChoices;
            messageText.text = choiceMessage.message;

            Actor actorToDisplay = currentActors[choiceMessage.actorId];
            actorName.text = actorToDisplay.name;
            actorImage.sprite = actorToDisplay.sprite;

            dialogueFirstChoice.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text= choiceMessage.text1;
            dialogueSecondChoice.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = choiceMessage.text2
                ;
        }

    }

    public void NextMessage()
    {
        currentMessage=currentMessage.NextMessage;
        if (currentMessage!=null)
        {   
            
            DisplayMessage();

        }
        else
        {
            Debug.Log("Conversation Ended");
            currentTrigger.enabled = false;
            // Remove items after the dialogue ends
            //deerController.StartFollowing(); 
            DialogueBox.SetActive(false);
            DialogueButton.SetActive(false);
            currentTrigger.isEnded= true;
            DialogueFriendship.SetActive(false);
            gameJournal.SetActive(true);
            isActive= false;

            if (currentNPC.levelOfFriendship < 10)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
           
        }
        
    }
    
    public void ChooseOption(int option)
    {
        MessageWithChoices choiceMessage=currentMessage as MessageWithChoices;
        choiceMessage.selectedChoice = option;
        if (option == choiceMessage.rightChoice)
        { 
            currentNPC.levelOfFriendship += 20;
            goodNotification.Enabled = true;
        }
        else
        { 
            currentNPC.levelOfFriendship -= 20;
            badNotification.Enabled = true;
        }

        DialogueFriendship.GetComponent<ProgressBar>().CurrentValue = currentNPC.levelOfFriendship;
        NextMessage();
    }
  
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) &&
           isActive == true &&
           currentMessage is Message) {
            NextMessage();
        }
    }
}
