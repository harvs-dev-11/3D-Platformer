using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private float fadeDuration;

    private int numberOfScenes;

    private void Start()
    {
        numberOfScenes = SceneManager.sceneCountInBuildSettings;
    }

    private void OnTriggerEnter(Collider other)
    {
        SceneFader sceneFader = FindFirstObjectByType<SceneFader>();
        float currentScene = SceneManager.GetActiveScene().buildIndex;
        if (numberOfScenes != currentScene + 1)
            sceneFader.LoadNextScene(fadeDuration);
        else
            sceneFader.RestartGame(fadeDuration);
    } 
}