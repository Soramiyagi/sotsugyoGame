using UnityEngine;

public abstract class UtilityConsideration : ScriptableObject
{
    // 0..1 に正規化されたスコアを返す
    public abstract float Score(EnemyContext ctx);

    // カーブでマッピングしたい場合はここでAnimationCurveを使う派生を作る
}

