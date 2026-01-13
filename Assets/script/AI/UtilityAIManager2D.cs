using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class UtilityAIManager2D : MonoBehaviour
{
    public EnemyContext2D context;
    public Animator animator;

    private List<UtilityAction2D> actions = new List<UtilityAction2D>();
    private UtilityAction2D currentAction;
    private float currentScore;

    void Awake()
    {
        context = GetComponent<EnemyContext2D>();
        animator = GetComponent<Animator>();

        actions.Add(new ChaseAction2D());
        actions.Add(new BoomerangAttackAction2D());
    }

    void Update()
    {
        if (context.player == null) return;

        Dictionary<UtilityAction2D, float> scores = new Dictionary<UtilityAction2D, float>();

        foreach (var action in actions)
        {
            float score = action.GetScore(context);
            scores[action] = score;

#if UNITY_EDITOR
            Debug.Log($"{action.GetType().Name} のスコア: {score:F2}");
#endif
        }

        var bestPair = scores.OrderByDescending(s => s.Value).FirstOrDefault();
        UtilityAction2D best = bestPair.Key;
        float bestScore = bestPair.Value;

        if (best != currentAction)
        {
            currentAction = best;
            currentScore = bestScore;

            Debug.Log($"現在の行動: {currentAction.GetType().Name}（スコア: {currentScore:F2}）");

            UpdateAnimationState(currentAction);
        }

        currentAction?.Execute(context);
    }
    void UpdateAnimationState(UtilityAction2D action)
    {
        // 移動アニメのみ制御（攻撃は Shooter 側）
        animator.SetBool("IsMoving", false);

        if (action is ChaseAction2D)
        {
            animator.SetBool("IsMoving", true);
        }
    }
}

