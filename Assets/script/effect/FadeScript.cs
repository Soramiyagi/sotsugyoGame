using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    private Image image;
    private float red, green, blue;

    private void Awake()
    {
        image = GetComponent<Image>();
        red = image.color.r;
        green = image.color.g;
        blue = image.color.b;
        SetAlpha(0f);
    }

    private void SetAlpha(float alpha)
    {
        image.color = new Color(red, green, blue, alpha);
    }

    // フェードイン（透明化）
    public IEnumerator FadeIn(float speed = 0.02f)
    {
        float alpha = image.color.a;
        while (alpha > 0f)
        {
            alpha -= speed;
            alpha = Mathf.Max(alpha, 0f);
            SetAlpha(alpha);
            yield return null;
        }
    }

    // フェードアウト（黒くなる）
    public IEnumerator FadeOut(float speed = 0.01f)
    {
        float alpha = image.color.a;
        while (alpha < 1f)
        {
            alpha += speed;
            alpha = Mathf.Min(alpha, 1f);
            SetAlpha(alpha);
            yield return null;
        }
    }
}

