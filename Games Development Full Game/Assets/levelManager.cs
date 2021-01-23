using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class levelManager : MonoBehaviour
{
    public playerManager playerManager;
    public elevatorMovement[] elevatorMovement;
    public GameObject canvasLevelComplete;
    public GameObject canvasHud;
    public Button resetButton;
    //empty 2d array to setup grid for level
    char[,] map;
    //how many homes reachable
    public int homeCountMax;
    //how many homes have been reached
    int homeCountCurrent;

    private void Awake()
    {
        canvasHud.SetActive(true);
        canvasLevelComplete.SetActive(false);
        resetButton.onClick.AddListener(onClickReset);
        generateMap();
        homeCountCurrent = 0;

    }

    void Update()
    {
        if (!playerManager.allowInput)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            playerManager.increaseMoveCount();

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
        canvasLevelComplete.SetActive(false);
        canvasHud.SetActive(true);
        playerManager.Reset();
        for (int i = 0; i < elevatorMovement.Length; i++)
        {
            elevatorMovement[i].Reset();
        }
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
            canvasLevelComplete.SetActive(true);
            canvasHud.SetActive(false);
        }
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
}
