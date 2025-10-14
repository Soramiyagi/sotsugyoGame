using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UtilityAIManager2D : MonoBehaviour
{
    public EnemyContext2D context;
    public List<UtilityAction2D> actions = new List<UtilityAction2D>();

    private UtilityAction2D currentAction;

    void Awake()
    {
        context = GetComponent<EnemyContext2D>();

        // 行動リストを自動登録
        actions.Add(new ChaseAction2D());
        actions.Add(new AttackAction2D());
    }

    void Update()
    {
        if (context.player == null) return;

        // スコアが最も高い行動を選ぶ
        UtilityAction2D bestAction = actions
            .OrderByDescending(a => a.GetScore(context))
            .FirstOrDefault();

        if (bestAction != currentAction)
        {
            currentAction = bestAction;
        }

        currentAction?.Execute(context);
    }
}
