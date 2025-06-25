using UnityEngine;
using System.Collections.Generic;
using System;
[Serializable]
public class DeerEntry
{
    public string deerName;             
    [TextArea(3, 10)] 
    public string characteristics;     
    public string photoPath;          


    public DeerEntry(string name, string traits, string path)
    {
        deerName = name;
        characteristics = traits;
        photoPath = path;
    }
}

public class GameJournal : MonoBehaviour
{
 
    public List<DeerEntry> befriendedDeer = new List<DeerEntry>();

 
    [ContextMenu("Add Sample Deer")]
    void AddSampleDeer()
    {
       
        AddDeerEntry("Sample Deer", "This is a sample deer for testing, gentle and curious.", "DeerPhotos/SampleDeer");
    }

    /// <summary>
    /// Adds a new deer entry to the journal.
    /// Call this method from your game logic when a deer is befriended.
    /// </summary>
    /// <param name="name">The name of the deer.</param>
    /// <param name="traits">The characteristics/description of the deer.</param>
    /// <param name="imagePath">The path to the deer's image asset (e.g., "Assets/Resources/DeerPhotos/MyDeer.png").
    /// Note: For Resources.Load, the path should be relative to the "Resources" folder and without the file extension.</param>
    public void AddDeerEntry(string name, string traits, string imagePath)
    {
        // Basic validation
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(traits) || string.IsNullOrWhiteSpace(imagePath))
        {
            Debug.LogWarning("Cannot add deer entry: Name, characteristics, or image path cannot be empty.");
            return;
        }

        DeerEntry newDeer = new DeerEntry(name, traits, imagePath);
        befriendedDeer.Add(newDeer);
        Debug.Log($"Added {name} to the journal!");
    }

    /// <summary>
    /// Retrieves a specific deer entry from the journal by its name.
    /// </summary>
    /// <param name="name">The name of the deer to find.</param>
    /// <returns>The DeerEntry object if found, otherwise null.</returns>
    public DeerEntry GetDeerEntry(string name)
    {
        foreach (DeerEntry deer in befriendedDeer)
        {
            if (deer.deerName == name)
            {
                return deer;
            }
        }
        Debug.LogWarning($"Deer '{name}' not found in the journal.");
        return null;
    }

   
    public List<DeerEntry> GetAllDeerEntries()
    {
        return befriendedDeer;
    }
}
