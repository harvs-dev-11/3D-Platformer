using UnityEngine;
using UnityEngine.Playables;

public class Door : MonoBehaviour
{
    [SerializeField] private int requiredKeys;
    [SerializeField] private PlayableDirector timeline;
    [HideInInspector] public int keysCollected;

    public void KeyCollected()
    {
        keysCollected++;
        if (keysCollected >= requiredKeys)
            timeline.Play();
    }
}