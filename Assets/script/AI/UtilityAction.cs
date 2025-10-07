using UnityEngine;

[CreateAssetMenu(menuName = "UtilityAI/Action")]
public class UtilityAction : ScriptableObject
{
    public string actionName = "Action";
    [Tooltip("各Considerationの重み（優先度調整）")]
    public UtilityConsideration[] considerations;
    public float[] weights; // considerations と長さを合わせること

    [Range(0f, 10f)]
    public float priorityMultiplier = 1f; // 行動固有の基本倍率

    // 補助：スコア合成の際に用いる（乗算や加重乗算など）
    public float Evaluate(EnemyContext ctx)
    {
        if (considerations == null || considerations.Length == 0) return 0f;
        float product = 1f;
        for (int i = 0; i < considerations.Length; i++)
        {
            float w = (weights != null && i < weights.Length) ? weights[i] : 1f;
            float s = Mathf.Clamp01(considerations[i].Score(ctx)) * w;
            // 乗算ベース（乗算は「全てが高いと選ばれやすい」挙動）
            product *= s;
        }
        // productは0..1の想定。priorityMultiplierで行動固有の優先度を調節
        return product * priorityMultiplier;
    }

    // 実行（オーバーライドして具体的にするか、InspectorからGameObjectにフック）
    public virtual void Execute(EnemyContext ctx)
    {
        Debug.Log($"Execute action: {actionName} on {ctx.gameObject.name}");
    }
}

