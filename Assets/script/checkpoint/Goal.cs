using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Goal : MonoBehaviour
{
    [SerializeField] private FadeScript fadeScript;
    [SerializeField] private GameObject stageClearText;
    [SerializeField] private string nextSceneName;

    private bool isCleared = false;

    private void Start()
    {
        // 文字は最初OFF
        stageClearText.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCleared) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            isCleared = true;
            StartCoroutine(GoalSequence());
        }
    }

    private IEnumerator GoalSequence()
    {
        // フェードアウト
        yield return StartCoroutine(fadeScript.FadeOut());

        // 文字表示
        stageClearText.SetActive(true);

        // 2秒待つ
        yield return new WaitForSeconds(2f);

        // シーン遷移
        SceneManager.LoadScene(nextSceneName);
    }
}
