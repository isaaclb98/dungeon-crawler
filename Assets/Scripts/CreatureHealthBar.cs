using UnityEngine;
using UnityEngine.UI;


public class CreatureHealthBar : MonoBehaviour
{
    public Slider healthBar;
    public Transform creatureTransform;
    public Camera mainCamera;
    public float maxHealth = 100f;
    private float currentHealth;

    public Vector3 offset = new Vector3(0, 2, 0);
    public float visibilityDistance = 10f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        float healthPercentage = currentHealth / maxHealth;
        healthBar.value = healthPercentage;

        if(healthPercentage > 0.5f)
        {
            healthBar.fillRect.GetComponent<Image>().color = Color.green;
        }
        else if(healthPercentage > 0.2f)
        {
            healthBar.fillRect.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            healthBar.fillRect.GetComponent<Image>().color = Color.red;
        }

        Collider creatureCollider = creatureTransform.GetComponent<Collider>();
        if (creatureCollider == null) return;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        if(GeometryUtility.TestPlanesAABB(planes,creatureCollider.bounds))
        {
            float distance = Vector3.Distance(creatureTransform.position, mainCamera.transform.position);
            if(distance < visibilityDistance)
            {
                healthBar.gameObject.SetActive(true);

                Vector3 screenPos = mainCamera.WorldToScreenPoint(creatureTransform.position + offset);
                healthBar.transform.position = screenPos;
            }
            else
            {
                healthBar.gameObject.SetActive(false);
            }
        }
        else
        {
            healthBar.gameObject.SetActive(false);
        }
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
}
