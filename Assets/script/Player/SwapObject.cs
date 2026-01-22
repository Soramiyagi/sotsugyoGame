using UnityEngine;

public class SwapObject : MonoBehaviour
{
    public float radius = 1.0f;
    public Vector3 offset;
    public GameObject indicatorObject;

    public GameObject preSwapEffectPrefab;  // 選択中エフェクト（A）
    public GameObject swapEffectPrefab;     // 入れ替え瞬間エフェクト（B）

    public GameObject activeSwapEffect;     // A の実体（Inspectorは空）

    [Header("Swap Cooldown")]
    public float swapCooldown = 2.0f;       // 入れ替えクールタイム（秒）

    private bool swapReady = false;
    private Vector3 swapObjPos;
    private GameObject nearestSwap = null;

    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;

    void Update()
    {
        // ===== クールタイム処理 =====
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }
        }

        // ===== 入力判定（キーボード＋コントローラー）=====
        bool targetInput =
            Input.GetKeyDown(KeyCode.F) ||
            Input.GetButtonDown("SwapTarget");

        bool executeInput =
            Input.GetKeyDown(KeyCode.E) ||
            Input.GetButtonDown("SwapExecute");

        // 入れ替え対象選択（※クールタイム中も選択できる仕様）
        if (targetInput)
        {
            SelectSwapTarget();
        }

        // 入れ替え実行
        if (executeInput)
        {
            ExecuteSwap();
        }

        // 選択中エフェクト更新
        UpdatePreSwapEffect();
    }

    // ===== 入れ替え対象選択 =====
    void SelectSwapTarget()
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

        swapReady = nearestSwap != null;

        if (swapReady)
        {
            swapObjPos = nearestSwap.transform.position;
        }
    }

    // ===== 入れ替え実行 =====
    void ExecuteSwap()
    {
        if (isOnCooldown) return;                 // ★ クールタイム中は不可
        if (!swapReady || nearestSwap == null) return;

        // 選択中エフェクト削除
        if (activeSwapEffect != null)
        {
            Destroy(activeSwapEffect);
            activeSwapEffect = null;
        }

        // 入れ替え瞬間エフェクト
        if (swapEffectPrefab != null)
        {
            Vector3 effectPos = nearestSwap.transform.position;
            effectPos.y += 1f;
            Instantiate(swapEffectPrefab, effectPos, Quaternion.identity);
        }

        // 入れ替え処理
        Vector3 temp = transform.position + offset;
        transform.position = nearestSwap.transform.position;
        nearestSwap.transform.position = temp;

        // スロー演出
        StartCoroutine(SlowTimeFor(0.5f));

        // ===== クールタイム開始 =====
        isOnCooldown = true;
        cooldownTimer = swapCooldown;

        nearestSwap = null;
        swapReady = false;
    }

    // ===== 選択中エフェクト追従 =====
    void UpdatePreSwapEffect()
    {
        if (swapReady && nearestSwap != null)
        {
            Vector3 pos = nearestSwap.transform.position;

            if (activeSwapEffect == null && preSwapEffectPrefab != null)
            {
                activeSwapEffect = Instantiate(preSwapEffectPrefab, pos, Quaternion.identity);
            }

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

    // ===== スロー演出 =====
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

    // ===== デバッグ表示 =====
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position + offset;
        Gizmos.DrawWireSphere(center, radius);
    }
}


