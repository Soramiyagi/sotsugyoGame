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
        if (context.player == null) return;

        var best = actions
            .Select(a => new { action = a, score = a.GetScore(context) })
            .OrderByDescending(x => x.score)
            .First().action;

        if (best != currentAction)
        {
            currentAction = best;
            UpdateAnimation(best);
        }

        currentAction.Execute(context);
    }

    void UpdateAnimation(AdvancedUtilityAction2D action)
    {
        if (animator == null) return;

        animator.SetBool("IsMoving", action is AdvancedChaseAction2D);
    }
}
