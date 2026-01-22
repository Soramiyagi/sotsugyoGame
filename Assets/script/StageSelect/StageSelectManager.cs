using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
    public Transform[] stageItems;
    public string[] sceneNames;

    [Header("Scale Multiplier")]
    public float selectedMultiplier = 1.333f; // 0.3 Å® 0.4

    private Vector3[] baseScales;
    private int currentIndex = 0;

    private float inputCooldown = 0.2f;
    private float lastInputTime = 0f;

    void Awake()
    {
        baseScales = new Vector3[stageItems.Length];
        for (int i = 0; i < stageItems.Length; i++)
        {
            baseScales[i] = stageItems[i].localScale;
        }
    }

    void Start()
    {
        UpdateVisual();
    }

    void Update()
    {
        HandleMoveInput();
        HandleSubmitInput();
    }

    void HandleMoveInput()
    {
        if (Time.time - lastInputTime < inputCooldown)
            return;

        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal > 0)
        {
            currentIndex++;
            lastInputTime = Time.time;
        }
        else if (horizontal < 0)
        {
            currentIndex--;
            lastInputTime = Time.time;
        }

        currentIndex = Mathf.Clamp(currentIndex, 0, stageItems.Length - 1);
        UpdateVisual();
    }

    void HandleSubmitInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene(sceneNames[currentIndex]);
        }
    }

    void UpdateVisual()
    {
        for (int i = 0; i < stageItems.Length; i++)
        {
            if (i == currentIndex)
                stageItems[i].localScale = baseScales[i] * selectedMultiplier;
            else
                stageItems[i].localScale = baseScales[i];
        }
    }
}
