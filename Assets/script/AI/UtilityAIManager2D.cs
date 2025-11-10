using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class UtilityAIManager2D : MonoBehaviour
{
    public EnemyContext2D context;

    private List<UtilityAction2D> actions = new List<UtilityAction2D>();
    private UtilityAction2D currentAction;
    private float currentScore;

    void Awake()
    {
        context = GetComponent<EnemyContext2D>();

        // 行動を登録
        actions.Add(new ChaseAction2D());
        actions.Add(new BoomerangAttackAction2D());
    }

    void Update()
    {
        if (context.player == null) return;

        // 各行動のスコアを計算
        Dictionary<UtilityAction2D, float> scores = new Dictionary<UtilityAction2D, float>();

        foreach (var action in actions)
        {
            float score = action.GetScore(context);
            scores[action] = score;

#if UNITY_EDITOR
            // ★ 各行動のスコアをログ出力（小数点2桁）
            Debug.Log($"{action.GetType().Name} のスコア: {score:F2}");
#endif
        }

        // 最もスコアが高い行動を選択
        var bestPair = scores.OrderByDescending(s => s.Value).FirstOrDefault();
        UtilityAction2D best = bestPair.Key;
        float bestScore = bestPair.Value;

        // 行動が変わったときだけ表示
        if (best != currentAction)
        {
            currentAction = best;
            currentScore = bestScore;
            Debug.Log($" 現在の行動: {currentAction.GetType().Name}（スコア: {currentScore:F2}）");
        }

        // 行動を実行
        currentAction?.Execute(context);
    }
}

