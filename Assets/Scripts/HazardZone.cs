using UnityEngine;

public class HazardZone : MonoBehaviour
{
    [SerializeField] private float fadeDuration;
    private void OnTriggerEnter(Collider other)
    {
        FindFirstObjectByType<SceneFader>().RestartScene(fadeDuration);
    } 
}