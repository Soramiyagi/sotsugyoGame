using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenuManager : MonoBehaviour
{
    [Header("Menu Items（上→下の順）")]
    public Transform[] menuItems;

    [Header("Scene Names（menuItemsと同じ順）")]
    public string[] sceneNames;

    private int currentIndex = 0;

    private float inputCooldown = 0.2f;
    private float lastInputTime = 0f;

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

        float vertical =
            Input.GetAxisRaw("Vertical"); // W/S & コントローラー上下

        if (vertical > 0)
        {
            currentIndex--;
            lastInputTime = Time.time;
        }
        else if (vertical < 0)
        {
            currentIndex++;
            lastInputTime = Time.time;
        }

        currentIndex = Mathf.Clamp(currentIndex, 0, menuItems.Length - 1);

        UpdateVisual();
    }

    void HandleSubmitInput()
    {
        // Space or Controller A（Submit）
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene(sceneNames[currentIndex]);
        }
    }

    void UpdateVisual()
    {
        for (int i = 0; i < menuItems.Length; i++)
        {
            if (i == currentIndex)
                menuItems[i].localScale = Vector3.one * 1.2f;
            else
                menuItems[i].localScale = Vector3.one;
        }
    }
}
