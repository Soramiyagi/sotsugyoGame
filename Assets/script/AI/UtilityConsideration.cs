using UnityEngine;

public abstract class UtilityConsideration : ScriptableObject
{
    // 0..1 �ɐ��K�����ꂽ�X�R�A��Ԃ�
    public abstract float Score(EnemyContext ctx);

    // �J�[�u�Ń}�b�s���O�������ꍇ�͂�����AnimationCurve���g���h�������
}

