using UnityEngine;
using UnityEngine.UI;

public class CircularLoadingIndicator : MonoBehaviour
{
    public int segments = 360;
    public float radius = 50f;
    public float lineWidth = 5f;
    public Color color = Color.white;

    private RectTransform rectTransform;
    private Image[] images;
    private float currentFillAmount = 0f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        images = new Image[segments];
        float stepLength = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            Image image = new GameObject("Segment " + i).AddComponent<Image>();
            image.transform.SetParent(transform, false);
            image.color = color;
            image.rectTransform.anchorMin = image.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            image.rectTransform.sizeDelta = new Vector2(lineWidth, radius);
            image.rectTransform.anchoredPosition = GetPosition(i * stepLength);
            image.rectTransform.localRotation = Quaternion.Euler(0, 0, i * stepLength);
            images[i] = image;
        }
    }

    Vector2 GetPosition(float angle)
    {
        float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
        float y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
        return new Vector2(x, y);
    }

    public void SetFillAmount(float fillAmount)
    {
        currentFillAmount = Mathf.Clamp01(fillAmount);
        int fillSegments = Mathf.RoundToInt(segments * currentFillAmount);

        for (int i = 0; i < segments; i++)
        {
            images[i].enabled = i < fillSegments;
        }
    }
}