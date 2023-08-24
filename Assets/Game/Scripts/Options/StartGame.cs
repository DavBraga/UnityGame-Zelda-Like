using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField]GameObject graphicsSettings; 
    public void FireGame()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenu");
    }
    public void OpenGraphicsSettings()
    {

    }
    private void Start() {
        StartCoroutine(WaitAndShow());
    }
    IEnumerator WaitAndShow()
    {
        yield return new WaitForEndOfFrame();
        graphicsSettings.SetActive(true);

    }
}
