using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private ParticleSystem particlePrefab;

    private void OnTriggerEnter(Collider other)
    {
        FindFirstObjectByType<Door>().KeyCollected();
        
        ParticleSystem ps = Instantiate(particlePrefab, transform.position, Quaternion.identity);
        ps.Play();
        ps.Stop();
        Destroy(gameObject);
    }
}