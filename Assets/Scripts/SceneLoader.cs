using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class SceneLoader : MonoBehaviour
{
    // Public method to load a scene by name
    public void LoadSceneByName(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name is empty! Cannot load scene.");
            return;
        }

        // Check if the scene exists in the build settings before trying to load
        int sceneIndex = SceneUtility.GetBuildIndexByScenePath(sceneName);
        if (sceneIndex == -1) // -1 means scene not found by path/name
        {
            Debug.LogError($"Scene '{sceneName}' is not found in Build Settings or its name is incorrect. Please add it to File > Build Settings.");
            return;
        }

        SceneManager.LoadScene(sceneName);
        Debug.Log($"Loading scene: {sceneName}");
    }

    // Public method to load a scene by build index
    public void LoadSceneByIndex(int sceneIndex)
    {
        if (sceneIndex < 0 || sceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError($"Scene index {sceneIndex} is out of build settings range. Max index: {SceneManager.sceneCountInBuildSettings - 1}");
            return;
        }
        SceneManager.LoadScene(sceneIndex);
        Debug.Log($"Loading scene with index: {sceneIndex}");
    }

    
    public void LoadNextSpecificScene()
    {
    
        LoadSceneByName("YourNextSceneName"); 
    }
}