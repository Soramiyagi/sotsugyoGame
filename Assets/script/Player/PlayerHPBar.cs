using UnityEngine;

public class PlayerHPBar : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;

    private RectTransform rect;

    private float initialY;  // インスペクターで設定されたYスケールを保持

    void Start()
    {
        rect = GetComponent<RectTransform>();
        initialY = rect.localScale.y;  // 最初のYスケールを保存
    }

    void Update()
    {
        float hpPercent = Mathf.Clamp01(player.PerHP);      // 0〜1の範囲に制限
        float scaleX = hpPercent * 0.4f;                     // Xは最大0.4まで
        rect.localScale = new Vector3(scaleX, initialY, 1f); // Yは初期値を使用
    }
}
