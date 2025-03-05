using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 5f;
    public int baseDamage = 5; // Base damage of the projectile
    public GameObject impactEffect; // Optional: Impact effect when hitting the player

    public PlayerHealth playerHealth; // Reference to the PlayerHealth script

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
            ApplyDamage();
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    void ApplyDamage()
    {
        int actualDamage = Mathf.Max(1, baseDamage - playerHealth.playerData.startingDefense); // Prevent negative damage
        playerHealth.ApplyDamage(actualDamage); // Use the PlayerHealth script to apply damage
    }
}

