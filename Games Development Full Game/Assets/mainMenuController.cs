using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainMenuController : MonoBehaviour
{

    public Button buttonLevel01;
    public string levelName01;

    public Button buttonLevel02;
    public string levelName02;

    public Button exit;
    // Start is called before the first frame update
    void Start()
    {
        buttonLevel01.onClick.AddListener(onClickLevel01);
        buttonLevel02.onClick.AddListener(onClickLevel02);
        exit.onClick.AddListener(onClickExit);
    }
    private void OnDestroy()
    {
        buttonLevel01.onClick.RemoveAllListeners();
        buttonLevel02.onClick.RemoveAllListeners();
        exit.onClick.RemoveAllListeners();
    }

    void onClickLevel01 ()
    {
        Debug.LogWarning("onClickLevel01");
        SceneManager.LoadScene(levelName01, LoadSceneMode.Single);
    }

    void onClickLevel02()
    {
        Debug.LogWarning("onClickLevel02");
        SceneManager.LoadScene(levelName02, LoadSceneMode.Single);
    }

    void onClickExit()
    {
        Debug.LogWarning("Exit");
        if (Application.isPlaying)
        {
            Application.Quit();
        }
    }
}
