using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AdvancedUtilityAI2D : MonoBehaviour
{
    public AdvancedEnemyContext2D context;
    public Animator animator;

    private List<AdvancedUtilityAction2D> actions = new();
    private AdvancedUtilityAction2D currentAction;

    void Awake()
    {
        context = GetComponent<AdvancedEnemyContext2D>();
        animator = GetComponentInChildren<Animator>();

        actions.Add(new AdvancedChaseAction2D());
        actions.Add(new AdvancedMeleeAction2D());
        actions.Add(new AdvancedRangedAction2D());
    }

    void Update()
    {
        if (context == null || context.player == null) return;

        var scores = new Dictionary<AdvancedUtilityAction2D, float>();
        foreach (var a in actions)
            scores[a] = a.GetScore(context);

#if UNITY_EDITOR
        foreach (var s in scores)
            Debug.Log($"[AI SCORE] {s.Key.GetType().Name} : {s.Value:F2}");
#endif

        var best = scores.OrderByDescending(s => s.Value).First().Key;

        if (best != currentAction)
        {
            currentAction = best;
            UpdateAnimationState(best);
        }

        currentAction.Execute(context);
    }

    void UpdateAnimationState(AdvancedUtilityAction2D action)
    {
        if (animator == null) return;

        animator.SetBool("IsMoving", action is AdvancedChaseAction2D);
    }
}
