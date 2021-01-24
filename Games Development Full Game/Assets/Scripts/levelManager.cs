using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class levelManager : MonoBehaviour
{
    public playerManager playerManager;
    public elevatorMovement[] elevatorMovement;
    public GameObject canvasHud;
    public GameObject levelComplete;
    public GameObject instructions;
    public Button resetButton;
    public Button exitButton;
    public Button helpButton;
    public TextMeshProUGUI totalCountText;
    public AudioSource audioSource;
    public AudioClip audioClipLevelComplete;
    public AudioClip audioClipButtonPress;
    public AudioClip[] audioClips;
    //empty 2d array to setup grid for level
    char[,] map;
    //how many homes reachable
    public int homeCountMax;
    //how many homes have been reached
    int homeCountCurrent;

    private void Awake()
    {
        canvasHud.SetActive(true);
        levelComplete.SetActive(false);
        instructions.SetActive(false);
        resetButton.onClick.AddListener(onClickReset);
        exitButton.onClick.AddListener(onClickExit);
        helpButton.onClick.AddListener(onClickHelp);
        generateMap();
        homeCountCurrent = 0;

    }

    void Update()
    {
        if (!playerManager.allowInput)
            return;

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            playerManager.increaseMoveCount();
            playAudioElevator();

            for (int i = 0; i < elevatorMovement.Length; i++)
            {
                elevatorMovement[i].toggle();
            }
        }
    }

    private void onClickReset()
    {
        generateMap();
        homeCountCurrent = 0;
        levelComplete.SetActive(false);
        instructions.SetActive(false);
        playerManager.Reset();
        for (int i = 0; i < elevatorMovement.Length; i++)
        {
            elevatorMovement[i].Reset();
        }
        playerManager.moveCountText.transform.gameObject.SetActive(true);
        playAudioButtonPress();
    }

    private void onClickExit()
    {
        playAudioButtonPress();
        if (Application.isPlaying)
        {
            Application.Quit();
        }
       
    }

    private void onClickHelp()
    {
        if (instructions.activeSelf)
        {
            instructions.SetActive(false);
        }
        else
        {
            instructions.SetActive(true);
        }
        playAudioButtonPress();
    }

    public void avatarMapPosUpdate(int x, int y, bool isCleanup)
    {
        if (isCleanup)
        {
            map[x, y] = '/';
        }
        else
        {
            map[x, y] = 'P';
        }
    }

    public bool movementCheck(int currentX, int currentY, int direction)
    {//if next movement is a player, check the next space along, keep doing this until a wall or empty space has been found
        
        int tempPosY = currentY;
        tempPosY += direction;
        char tempChar = map[currentX, tempPosY];
        //checking if movement will result in the following cases
        switch (tempChar)
        {
            case 'W':
                return false;
            case 'P':
                tempPosY += direction;
                tempChar = map[currentX, tempPosY];
                //if 'P' check again to see if the next avatar will move
                switch (tempChar)
                {
                    case 'W':
                        return false;
                    case 'P':
                        tempPosY += direction;
                        tempChar = map[currentX, tempPosY];
                        //if 'P' again, also check again to see if the avatar after the previous avatar will move
                        switch (tempChar)
                        {
                            case 'W':
                                return false;
                            case 'P':
                                return false;
                            case '/':
                                return true;
                        }
                        return false;
                    case '/':
                        return true;
                }
                return false;
            case '/':
                return true;
        }

        return true;
    }

    public bool groundCheck(int currentX, int currentY)
    {
        char tempChar = map[currentX + 1, currentY];
        switch (tempChar)
        {
            case 'W':
                return false;
            case 'P':
                return false;
            case 'E':
                return false;
        }
        avatarMapPosUpdate(currentX, currentY, true);
        avatarMapPosUpdate(currentX + 1, currentY, false);
        return true;
    }
    public void levelCompleteCheck()
    {
        homeCountCurrent++;
        if (homeCountCurrent >= homeCountMax)
        {
            homeCountCurrent = 0;
            StartCoroutine(levelCompleteDisplayDelay());
            playerManager.moveCountText.transform.gameObject.SetActive(false);
            totalCountText.text = "" + playerManager.moveCount;
            playAudioLevelComplete();
        }
    }

    private IEnumerator levelCompleteDisplayDelay()
    {
        yield return new WaitForSeconds(0.2f);
        levelComplete.SetActive(true);
    }

    public void onElevatorToggle(int x, int y, bool isUp)
    {
        if (isUp)
        {
            map[x, y] = 'E';
        }
        else
        {
            map[x, y] = '/';
        }
    }

    public void generateMap()
    {
        map = new char[,]
        {
            //visual look of grid positions for map
            //avatars starting locations, home locations and platform locations
            {'W', '/', '/', '/', 'W', '/', '/', 'W', 'W', '/', '/', '/', 'W'},
            {'W', '/', 'W', '/', 'W', '/', 'W', 'W', 'W', '/', 'W', '/', 'W'},
            {'W', '/', '/', '/', 'W', '/', '/', '/', 'W', '/', '/', '/', 'W'},
            {'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W'},
            {'W', '/', '/', '/', 'W', 'W', '/', '/', 'W', '/', '/', '/', 'W'},
            {'W', 'W', '/', 'W', 'W', 'W', '/', 'W', 'W', 'W', '/', 'W', 'W'},
            {'W', '/', '/', '/', 'W', '/', '/', '/', 'W', '/', '/', '/', 'W'},
            {'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W'},
            {'W', '/', '/', '/', 'W', '/', '/', '/', 'W', '/', '/', '/', 'W'},
            {'W', 'W', 'W', '/', 'W', 'W', '/', 'W', 'W', 'W', '/', 'W', 'W'},
            {'W', '/', '/', '/', 'W', '/', '/', '/', 'W', '/', '/', 'W', 'W'},
            {'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W'},

        };
    }

    private void playAudioElevator()
    {
        int rand = Mathf.RoundToInt(Random.Range(0, 2));
        audioSource.PlayOneShot(audioClips[rand], 0.2f);
    }

    private void playAudioLevelComplete()
    {
        audioSource.PlayOneShot(audioClipLevelComplete, 1f);
    }

    private void playAudioButtonPress()
    {
        audioSource.PlayOneShot(audioClipButtonPress, 0.4f);
    }
}
