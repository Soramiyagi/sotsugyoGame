using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AdvancedUtilityAI2D : MonoBehaviour
{
    public AdvancedEnemyContext2D context;
    public Animator animator;

    private List<AdvancedUtilityAction2D> actions = new List<AdvancedUtilityAction2D>();
    private AdvancedUtilityAction2D currentAction;

    void Awake()
    {
        context = GetComponent<AdvancedEnemyContext2D>();
        animator = GetComponent<Animator>();

        actions.Add(new AdvancedChaseAction2D());
        actions.Add(new AdvancedMeleeAction2D());
        actions.Add(new AdvancedRangedAction2D());
    }

    void Update()
    {
        if (context.player == null) return;

        var scores = new Dictionary<AdvancedUtilityAction2D, float>();
        foreach (var action in actions)
            scores[action] = action.GetScore(context);

        var bestPair = scores.OrderByDescending(s => s.Value).First();
        AdvancedUtilityAction2D best = bestPair.Key;

        if (best != currentAction)
        {
            currentAction = best;
            UpdateAnimationState(currentAction);
        }

        currentAction?.Execute(context);
    }

    void UpdateAnimationState(AdvancedUtilityAction2D action)
    {
        animator.SetBool("IsMoving", false);
        if (action is AdvancedChaseAction2D)
            animator.SetBool("IsMoving", true);
    }
}
