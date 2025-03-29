using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 5f;
    public int baseDamage = 5; // Base damage of the projectile
    public GameObject impactEffect; // Optional: Impact effect when hitting the player

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the right tag
        {
            PlayerStats playerHealth = other.GetComponent<PlayerStats>(); // Get player health component

            if (playerHealth != null)
            {
                int actualDamage = Mathf.Max(1, baseDamage); // Ensure at least 1 damage
                playerHealth.TakeDamage(actualDamage);
                Debug.Log("Projectile hit player! Damage: " + actualDamage);
            }

            if (impactEffect != null)
            {
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Instantiate(impactEffect, hitPoint, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}

