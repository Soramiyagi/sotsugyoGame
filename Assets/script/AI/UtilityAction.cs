using UnityEngine;

[CreateAssetMenu(menuName = "UtilityAI/Action")]
public class UtilityAction : ScriptableObject
{
    public string actionName = "Action";
    [Tooltip("�eConsideration�̏d�݁i�D��x�����j")]
    public UtilityConsideration[] considerations;
    public float[] weights; // considerations �ƒ��������킹�邱��

    [Range(0f, 10f)]
    public float priorityMultiplier = 1f; // �s���ŗL�̊�{�{��

    // �⏕�F�X�R�A�����̍ۂɗp����i��Z����d��Z�Ȃǁj
    public float Evaluate(EnemyContext ctx)
    {
        if (considerations == null || considerations.Length == 0) return 0f;
        float product = 1f;
        for (int i = 0; i < considerations.Length; i++)
        {
            float w = (weights != null && i < weights.Length) ? weights[i] : 1f;
            float s = Mathf.Clamp01(considerations[i].Score(ctx)) * w;
            // ��Z�x�[�X�i��Z�́u�S�Ă������ƑI�΂�₷���v�����j
            product *= s;
        }
        // product��0..1�̑z��BpriorityMultiplier�ōs���ŗL�̗D��x�𒲐�
        return product * priorityMultiplier;
    }

    // ���s�i�I�[�o�[���C�h���ċ�̓I�ɂ��邩�AInspector����GameObject�Ƀt�b�N�j
    public virtual void Execute(EnemyContext ctx)
    {
        Debug.Log($"Execute action: {actionName} on {ctx.gameObject.name}");
    }
}

