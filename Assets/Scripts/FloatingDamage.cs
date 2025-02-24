using UnityEngine;
using TMPro;

public class FloatingDamage : MonoBehaviour
{
    // Reference to the TextMeshProUGUI component that displays the damage number.
    public TextMeshProUGUI damageText;

    // Speed at which the text floats upward.
    public float floatSpeed = 2f;
    // Total time before the text fades out.
    public float fadeDuration = 1f;

    private float _elapsedTime = 0f;
    private CanvasGroup _canvasGroup;
    private Color _originalColor;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        _originalColor = damageText.color;
    }

    // Set the damage value to display
    public void SetDamage(int damage)
    {
        damageText.text = damage.ToString();
    }

    private void Update()
    {
        // Move the text upward
        transform.Translate(Vector3.up * (floatSpeed * Time.deltaTime), Space.World);

        // Fade out the text over time
        _elapsedTime += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, _elapsedTime / fadeDuration);
        damageText.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, alpha);

        if (_elapsedTime >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }
}