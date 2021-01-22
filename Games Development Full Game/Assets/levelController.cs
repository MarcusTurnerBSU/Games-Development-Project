using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class levelController : MonoBehaviour
{
    public Button returnMainMenu;
    public string mainMenu;
    // Start is called before the first frame update
    void Start()
    {
        returnMainMenu.onClick.AddListener(onClickReturnMainMenu);
    }

    private void OnDestroy()
    {
        returnMainMenu.onClick.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onClickReturnMainMenu()
    {
        Debug.LogWarning("returnMainMenu");
        SceneManager.LoadScene(mainMenu, LoadSceneMode.Single);
    }
}
