using UnityEngine;

public class SwapObject : MonoBehaviour
{
    public float radius = 1.0f;
    private bool swapReady = false;
    private Vector3 swapObjPos;                 //入れ替え対象の位置
    private GameObject nearestSwap = null;      //入れ替え対象そのもの
    public Vector3 offset;

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
                if (collider.CompareTag("swap")|| collider.CompareTag("SwapEnemy"))
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
            if (swapReady)
            {
                Vector3 temp = transform.position + offset;
                transform.position = swapObjPos;
                nearestSwap.transform.position = temp;

                Debug.Log("プレイヤーとswapオブジェクトの位置を入れ替えました。");

                // 保存を破棄
                nearestSwap = null;
                swapReady = false;
            }
            else
            {
                Debug.Log("保存されたswapオブジェクトがありません。Fキーで先に保存してください。");
            }
        }
    }

    // 可視化（シーンビュー用）
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position + offset;
        Gizmos.DrawWireSphere(center, radius);
    }
}



