using UnityEngine;

public class UtilityAI : MonoBehaviour
{
    public EnemyContext context;
    public UtilityAction[] actions;
    public float evaluateInterval = 0.2f;
    public float lastEvaluateTime = 0f;

    private UtilityAction currentAction = null;

    void Reset()
    {
        context = GetComponent<EnemyContext>();
    }

    void Update()
    {
        if (Time.time - lastEvaluateTime >= evaluateInterval)
        {
            EvaluateAndRun();
            lastEvaluateTime = Time.time;
        }
    }

    void EvaluateAndRun()
    {
        if (actions == null || actions.Length == 0) return;

        float bestScore = -1f;
        UtilityAction best = null;

        foreach (var a in actions)
        {
            if (a == null) continue;
            float score = a.Evaluate(context);
            // 少しのランダム性を入れて行動のバタつきを防止（オプション）
            // score *= Random.Range(0.95f, 1.05f);

            if (score > bestScore)
            {
                bestScore = score;
                best = a;
            }
        }

        if (best != null)
        {
            // 切り替えや継続のポリシーもここで作れる
            currentAction = best;
            currentAction.Execute(context);
        }
    }
}

