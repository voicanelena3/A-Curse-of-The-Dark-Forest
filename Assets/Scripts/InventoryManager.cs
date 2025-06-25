using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item>Items=new List<Item>();
    public Transform ItemContent;
    public GameObject InventoryItem;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void Add(Item item)
    {
        Items.Add(item);
        ListItems();


    }

    public void Remove(Item item)
    {
        Items.Remove(item);
        ListItems();


    }


    public void ListItems()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        
        foreach (var item in Items)
        {
                GameObject obj = Instantiate(InventoryItem, ItemContent);
                var itemName = obj.transform.Find("Text (TMP)").GetComponent<TMPro.TextMeshProUGUI>();


                var itemIcon = obj.transform.Find("ItemIcon").GetComponent<UnityEngine.UI.Image>();


                itemName.text = item.itemName;
                itemIcon.sprite = item.icon;
        }

       
    }
    

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
}
