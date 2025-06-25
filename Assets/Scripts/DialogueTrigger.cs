using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Message[] simpleMessages;
    public MessageWithChoices[] choiceMessages;
    public Actor[] actors;
    public bool isEnded=false;
    public NPC owner;
    public List <IMessage> messages=new List<IMessage>();
    public ITrigger trigger;
    public TriggerType triggerType;
    public string itemTriggerName;
    public DeerStateMachine deerStateMachine;
    public IFollowUp followUp;
    public FollowUpType followUpType;

    private void Start()
    {

        Dictionary<int,IMessage>messageDictionary= new Dictionary<int,IMessage>();
        foreach(Message message in simpleMessages)
        {
            messageDictionary.Add(message.messageId, message);
        }
        foreach(MessageWithChoices choice in choiceMessages)
        {
            messageDictionary.Add(choice.messageId, choice);
        }
        foreach(Message message in simpleMessages)
        {
            if (messageDictionary.ContainsKey(message.nextMessageId))
            {
                message.nextMessage = messageDictionary[message.nextMessageId];
            }
            else
            {
                message.nextMessage=null;
            }
            messages.Add(message);
        }
        foreach(MessageWithChoices choice in choiceMessages)
        {
            choice.firstChoice = messageDictionary[choice.firstChoiceId];
            choice.secondChoice= messageDictionary[choice.secondChoiceId];
            messages.Add(choice);
        }

        if (triggerType == TriggerType.Basic)
        {
            trigger=new BasicTrigger();
        }
        else
        {
            trigger=new ItemTrigger(itemTriggerName,Object.FindFirstObjectByType<InventoryManager>());
        }
        if (followUpType == FollowUpType.Basic)
        {
            followUp=new BasicFollowUp();
        }
        else
        {
            followUp = new FollowPlayer(deerStateMachine);
        }
    }

    public void StartDialogue()
    {
        trigger.StartDialogue(messages, actors, this, owner);
        followUp.StartFollowUp();
    }
}


public interface ITrigger
{
    public void StartDialogue(List<IMessage> messages, Actor[] actors, DialogueTrigger dialogueTrigger, NPC owner);
}

public enum TriggerType
{
    Basic,Item
}
public class BasicTrigger : ITrigger
{
   public void StartDialogue(List<IMessage> messages, Actor[] actors, DialogueTrigger dialogueTrigger, NPC owner)
    {
        Object.FindFirstObjectByType<DialogueManager>().OpenDialogue(messages, actors, dialogueTrigger, owner);
        dialogueTrigger.enabled = false;
    }
}

public class ItemTrigger : ITrigger
{
    public string itemName;
    public InventoryManager inventoryManager;

    public ItemTrigger(string itemName, InventoryManager inventoryManager)
    {
        this.itemName = itemName;
        this.inventoryManager = inventoryManager;
    }

    public void StartDialogue(List<IMessage> messages, Actor[] actors, DialogueTrigger dialogueTrigger, NPC owner)
    {
        var items =inventoryManager.Items;
        var wantedItem=items.Find(item => item.itemName==itemName);
        if (wantedItem != null)
        {
            inventoryManager.Remove(wantedItem);
            owner.levelOfFriendship += 10;

            BefriendDeer befriendDeerUIScript = Object.FindFirstObjectByType<BefriendDeer>();

            if (befriendDeerUIScript != null)
            {
                befriendDeerUIScript.OnDeerBefriended();
                Debug.Log("Deer has been befriended! UI should update now."); // For debugging
            }
            else
            {
                Debug.LogWarning("BefriendDeer UI script not found in scene! Make sure it's attached to a GameObject and its UI references are set.");
            }
            


            Object.FindFirstObjectByType<DialogueManager>().OpenDialogue(messages, actors, dialogueTrigger, owner);
            dialogueTrigger.enabled=false;
        }
    }



}

public interface IFollowUp
{
    public void StartFollowUp();
}

public enum FollowUpType
{
    Basic,FollowPlayer
}

public class BasicFollowUp : IFollowUp
{
    public void StartFollowUp()
    {

    }
}

public class FollowPlayer : IFollowUp
{   private DeerStateMachine stateMachine;

    public FollowPlayer(DeerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void StartFollowUp()
    {
      stateMachine.enabled = true;
    }
}
public interface IMessage
{
    IMessage NextMessage {  get; }
}

[System.Serializable]
public class Message:IMessage
{
    public int actorId;
    public string message;
    public IMessage nextMessage;
    public int nextMessageId;
    public IMessage NextMessage
    {
        get { return nextMessage; }
    }
    public int messageId;

}

[System.Serializable]
public class MessageWithChoices:IMessage
{
    public int actorId;
    public string message;
    public IMessage firstChoice;
    public int firstChoiceId;
    public IMessage secondChoice;
    public int secondChoiceId;
    public IMessage NextMessage
    {
        get
        {
            switch (selectedChoice)
            {
                case 0:return firstChoice;
                    
                case 1:return secondChoice;

                default:return null;
            }
        }
    }
    public string text1;
    public string text2;
    public int messageId;
    public int selectedChoice=0;
    public int rightChoice; 
}

[System.Serializable]
public class Actor
{
    public string name;
    public Sprite sprite;
}
