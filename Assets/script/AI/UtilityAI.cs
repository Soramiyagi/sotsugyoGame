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
            // �����̃����_���������čs���̃o�^����h�~�i�I�v�V�����j
            // score *= Random.Range(0.95f, 1.05f);

            if (score > bestScore)
            {
                bestScore = score;
                best = a;
            }
        }

        if (best != null)
        {
            // �؂�ւ���p���̃|���V�[�������ō���
            currentAction = best;
            currentAction.Execute(context);
        }
    }
}

