using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDependencies : MonoBehaviour
{
    public SceneReference[] Dependencies;

    private void Awake()
    {
        foreach (SceneReference sceneRef in Dependencies)
        {
            SceneManager.LoadScene(sceneRef.ScenePath, LoadSceneMode.Additive);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Reload current scene")]
    public void ReleadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
#endif
}
