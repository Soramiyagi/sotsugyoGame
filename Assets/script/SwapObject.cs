using UnityEngine;

public class SwapObjectFinder2D : MonoBehaviour
{
    public float radius = 1.0f;
    private Transform savedSwapTransform = null;

    void Update()
    {
        // Fキーでswapオブジェクトの位置を保存
        if (Input.GetKeyDown(KeyCode.F))
        {
            Vector2 center = (Vector2)transform.position + Vector2.up;
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);

            GameObject nearestSwap = null;
            float nearestDistance = float.MaxValue;

            foreach (Collider2D collider in hitColliders)
            {
                if (collider.CompareTag("swap"))
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
                savedSwapTransform = nearestSwap.transform;
                Debug.Log("保存されたswapオブジェクト: " + nearestSwap.name);
            }
            else
            {
                Debug.Log("swapタグのオブジェクトが見つかりませんでした。");
            }
        }

        // Eキーで位置を入れ替え & 保存破棄
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (savedSwapTransform != null)
            {
                Vector3 temp = transform.position;
                transform.position = savedSwapTransform.position;
                savedSwapTransform.position = temp;

                Debug.Log("プレイヤーとswapオブジェクトの位置を入れ替えました。");

                // 保存を破棄
                savedSwapTransform = null;
            }
            else
            {
                Debug.Log("保存されたswapオブジェクトがありません。Fキーで先に保存してください。");
            }
        }
    }

    // 可視化（シーンビュー用）
    private void OnDrawGizmosSelected()
    {
        Vector2 center = (Vector2)transform.position + Vector2.up;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center, radius);
    }
}



