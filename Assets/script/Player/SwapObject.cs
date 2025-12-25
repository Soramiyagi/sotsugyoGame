using UnityEngine;

public class SwapObject : MonoBehaviour
{
    public float radius = 1.0f;
    public Vector3 offset;
    public GameObject indicatorObject;

    public GameObject preSwapEffectPrefab;  // ← 選択中に出すエフェクト（A）
    public GameObject swapEffectPrefab;     // ← 入れ替え瞬間のエフェクト（B）

    public GameObject activeSwapEffect;     // ← A の実体（Inspectorは空にして）

    private bool swapReady = false;
    private Vector3 swapObjPos;
    private GameObject nearestSwap = null;

    void Update()
    {
        // Fキー：入れ替え対象を選択
        if (Input.GetKeyDown(KeyCode.F))
        {
            Vector3 center = transform.position + offset;
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);

            nearestSwap = null;
            float nearestDistance = float.MaxValue;

            foreach (Collider2D col in hitColliders)
            {
                if (col.CompareTag("swap") || col.CompareTag("SwapEnemy"))
                {
                    float dist = Vector2.Distance(transform.position, col.transform.position);
                    if (dist < nearestDistance)
                    {
                        nearestDistance = dist;
                        nearestSwap = col.gameObject;
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

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (swapReady && nearestSwap != null)
            {
                // ★ 選択中エフェクト(A)を削除
                if (activeSwapEffect != null)
                {
                    Destroy(activeSwapEffect);
                    activeSwapEffect = null;
                }

                // ★ 入れ替え瞬間エフェクト(B)を出す
                if (swapEffectPrefab != null)
                {
                    Vector3 effectPos = nearestSwap.transform.position; // 現在位置
                    effectPos.y += 1f;
                    Instantiate(swapEffectPrefab, effectPos, Quaternion.identity);
                }

                // ★ 入れ替え処理
                Vector3 temp = transform.position + offset;
                transform.position = nearestSwap.transform.position; // 現在位置に入れ替え
                nearestSwap.transform.position = temp;

                StartCoroutine(SlowTimeFor(0.5f));

                nearestSwap = null;
                swapReady = false;
            }
        }

        // ★選択中（swapReady = true）の間だけ A を表示
        if (swapReady && nearestSwap != null)
        {
            // 常に対象の現在位置を取得して y +1 にする
            Vector3 pos = nearestSwap.transform.position;


            // エフェクトがまだ生成されていなければ生成
            if (activeSwapEffect == null && preSwapEffectPrefab != null)
            {
                activeSwapEffect = Instantiate(preSwapEffectPrefab, pos, Quaternion.identity);
            }

            // 生成済みなら毎フレーム位置更新して追従
            if (activeSwapEffect != null)
            {
                activeSwapEffect.transform.position = pos;
            }
        }
        else
        {
            if (activeSwapEffect != null)
            {
                Destroy(activeSwapEffect);
                activeSwapEffect = null;
            }
        }
    }

    // スロー戻す
    private System.Collections.IEnumerator SlowTimeFor(float duration)
    {
        Time.timeScale = 0.4f;
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
