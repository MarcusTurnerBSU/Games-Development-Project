using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class levelManager : MonoBehaviour
{
    public playerManager playerManager;
    public elevatorMovement elevatorMovement;
    public GameObject canvasLevelComplete;
    public GameObject canvasHud;
    public Button resetButton;
    //empty 2d array to setup grid for level
    char[,] map;
    //how many homes reachable
    public int homeCountMax;
    //how many homes have been reached
    int homeCountCurrent;    

    private void onClickReset()
    {
        homeCountCurrent = 0;
        canvasLevelComplete.SetActive(false);
        canvasHud.SetActive(true);
        playerManager.Reset();
        elevatorMovement.Reset();
        generateMap();
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
    {
        
        int tempPosY = currentY;
        tempPosY += direction;
        char tempChar = map[currentX, tempPosY];
        switch (tempChar)
        {
            case 'W':
                Debug.Log("Wall hit" + currentX + tempPosY);
                //Debug.Log("Wall");
                return false;
            case 'P':
                Debug.LogWarning("Player hit" + currentX + tempPosY);
                return false;
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
    private void Awake()
    {
        canvasHud.SetActive(true);
        canvasLevelComplete.SetActive(false);
        resetButton.onClick.AddListener(onClickReset);
        generateMap();
        homeCountCurrent = 0;

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
            {'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W'},
            {'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W'},
            {'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W'},
            {'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W'},
            {'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W'},
            {'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W'},
            {'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W'},
            {'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W'},
            {'W', '/', '/', '/', '/', '/', '/', '/', '/', '/', '/', '1', 'W'},
            {'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', '/', '/', 'W', 'W', 'W'},
            {'W', '/', '/', '/', '/', '/', '/', '/', '/', '/', '/', '2', 'W'},
            {'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W'},
        };
    }
}
