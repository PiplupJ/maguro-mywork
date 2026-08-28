using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneString = "TitleScene"; //デフォルトはタイトルシーン
    [SerializeField] private float waitTime = 1f;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneString);
    }

    public void LoadScene(string targetScene)
    {
        SceneManager.LoadScene(targetScene);
    }

    public void LoadSceneWithDelay()
    {
        StartCoroutine(ExecuteAfterDelay(sceneString));
    }

    public void LoadSceneWithDelay(string targetScene)
    {
        StartCoroutine(ExecuteAfterDelay(targetScene));
    }

    IEnumerator ExecuteAfterDelay(string targetScene)
    {
        yield return new WaitForSeconds(waitTime);

        LoadScene(targetScene);
    }
}
