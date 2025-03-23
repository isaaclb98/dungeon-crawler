using TMPro;
using UnityEngine;

public class TextPopupUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float lifetime = 1.5f;

    private float timer;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }


    public void SetText(string message, Color color)
    {
        text.text = message;
        text.color = color;
    }

    void Update()
    {
        timer += Time.deltaTime;
        canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / lifetime);

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}