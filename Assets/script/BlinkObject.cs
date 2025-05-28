using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkObject : MonoBehaviour
{
    public float interval = 0.5f; // ì_ñ≈ÇÃä‘äuÅiïbÅj

    private SpriteRenderer spriteRenderer;
    private float timer = 0f;
    private bool isVisible = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            isVisible = !isVisible;
            spriteRenderer.enabled = isVisible;
            timer = 0f;
        }
    }
}
