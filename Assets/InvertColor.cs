using UnityEngine;

public class InvertColor : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = new Color(1.0f - originalColor.r, 1.0f - originalColor.g, 1.0f - originalColor.b, originalColor.a);
        }
    }
}