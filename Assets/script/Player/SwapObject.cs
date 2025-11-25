using UnityEngine;

public class SwapObject : MonoBehaviour
{
    public float radius = 1.0f;
    public Vector3 offset;
    public GameObject indicatorObject;

    private bool swapReady = false;
    private Vector3 swapObjPos;
    private GameObject nearestSwap = null;

    private bool isSlow = false;

    void Update()
    {
        // Fキーでswapオブジェクトの位置を保存
        if (Input.GetKeyDown(KeyCode.F))
        {
            Vector3 center = transform.position + offset;
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);

            nearestSwap = null;
            float nearestDistance = float.MaxValue;

            foreach (Collider2D collider in hitColliders)
            {
                if (collider.CompareTag("swap") || collider.CompareTag("SwapEnemy"))
                {
                    float distance = Vector2.Distance(transform.position, collider.transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestSwap = collider.gameObject;
                    }
                }
            }

            if (nearestSwap != null)
            {
                swapObjPos = nearestSwap.transform.position;
                swapReady = true;
            }
            else
            {
                swapReady = false;
            }
        }

        // Eキーで位置を入れ替え
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (swapReady && nearestSwap != null)
            {
                Vector3 temp = transform.position + offset;
                transform.position = swapObjPos;
                nearestSwap.transform.position = temp;

                // ★ 実時間でスロー
                StartCoroutine(SlowTimeFor(0.5f));  // ← 好きな時間（秒）

                nearestSwap = null;
                swapReady = false;
            }
        }

        if (indicatorObject != null)
        {
            indicatorObject.SetActive(swapReady);
        }
    }

    // 実時間でスローを戻すコルーチン
    private System.Collections.IEnumerator SlowTimeFor(float duration)
    {
        Time.timeScale = 0.1f;  // 超スロー
        float start = Time.unscaledTime;

        while (Time.unscaledTime < start + duration)
        {
            yield return null;
        }

        Time.timeScale = 1.0f;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position + offset;
        Gizmos.DrawWireSphere(center, radius);
    }
}



