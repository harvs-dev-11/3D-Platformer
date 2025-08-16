using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float defaultFadeDuration = 1f;

    private SceneFader Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(FadeIn(defaultFadeDuration));
    }

    public void RestartScene(float fadeDuration)
    {
        StartCoroutine(FadeOutAndLoadByBuildIndex(SceneManager.GetActiveScene().buildIndex, fadeDuration));
    }

    private IEnumerator FadeIn(float fadeDuration)
    {
        fadeCanvasGroup.alpha = 1;
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = 1 - (t / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 0;
    }

    private IEnumerator FadeOutAndLoadByBuildIndex(int buildIndex, float fadeDuration)
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = t / fadeDuration;
            yield return null;
        }

        fadeCanvasGroup.alpha = 1;
        
        yield return null;

        yield return SceneManager.LoadSceneAsync(buildIndex);

        StartCoroutine(FadeIn(fadeDuration));
    }
}