using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerController : MonoBehaviour
{
    //referencing gameobject components
    public levelManager levelManager;
    public playerManager playerManager;

  
    private Vector3 startingPosition;

    public int currentX, currentY;
    public int homeX, homeY;
    public bool isHome;
    public char homeID;
    private int startingX, startingY;
    public TextMeshProUGUI debugText;

    private void Awake()
    {
        startingPosition = transform.position;
        startingX = currentX;
        startingY = currentY;
    }
    private void OnDestroy()
    {
     
    }
    // Start is called before the first frame update
    void Start()
    {
        levelManager.avatarMapPosUpdate(startingX, startingY, false);
        debugPrintPos();
    }
    public void Reset()
    {
        transform.position = startingPosition;
        currentX = startingX;
        currentY = startingY;
        isHome = false;
    }

    public void callFallingCheck()
    {
        StartCoroutine(falling());
    }

    //using coroutine to show cube falling
    private IEnumerator falling()
    {
        yield return new WaitForSeconds(0.1f);
        while (levelManager.groundCheck(currentX, currentY))
        {
            currentX++;
            transform.position += Vector3.down;
            //show update every loop of the while
            yield return null;
            yield return new WaitForSeconds(0.1f);
        }
        movementComplete();
        yield return null;
        debugPrintPos();
    }

    public void moveRight()
    {
        //if this player is home, block movement
        if (isHome)
        {
            movementComplete();
            //Debug.Log(this.name + "! isHome: X: " + currentX + ", Y: " + currentY);
            return;
        }
        
        //checking if player can move from current position to direction right
        if (levelManager.movementCheck(currentX, currentY, 1))
        {   
            //cleanup for map update to remove 'P' from current position (soon to be old position)
            levelManager.avatarMapPosUpdate(currentX, currentY, true);
            //updating current position 
            currentY++;
            //updating the objects visuals on the screen
            transform.position += Vector3.right;
            //checking this avatar's home and current coordinates match
            if (homeX == currentX && homeY == currentY)
            {
                isHome = true;
                levelManager.levelCompleteCheck();
            }
            //updating 'P' on the map
            levelManager.avatarMapPosUpdate(currentX, currentY, false);
        }
        //call coroutine to check/start falling
        StartCoroutine(falling());
        debugPrintPos();
    }

    public void moveLeft()
    {
        if (isHome)
        {
            //Debug.Log(this.name + "! isHome: X: " + currentX + ", Y: " + currentY);
            movementComplete();
            return;
        }

        if (levelManager.movementCheck(currentX, currentY, -1))
        {
            levelManager.avatarMapPosUpdate(currentX, currentY, true);
            currentY--;
            transform.position += Vector3.left;
            if (homeX == currentX && homeY == currentY)
            {
                isHome = true;
                levelManager.levelCompleteCheck();
            }
            //Debug.Log(this.name + "! moveCheck: X: " + currentX + ", Y: " + currentY + ", isHome: " + isHome.ToString());
            levelManager.avatarMapPosUpdate(currentX, currentY, false);
        }
        StartCoroutine(falling());
        debugPrintPos();
    }

    private void movementComplete()
    {
        playerManager.onControllerMovementComplete();
    }

    public void moveUp()
    {
        levelManager.avatarMapPosUpdate(currentX, currentY, true);
        transform.position += Vector3.up * 2;
        currentX -= 2;
        levelManager.avatarMapPosUpdate(currentX, currentY, false);
        debugPrintPos();
    } 

    public void moveDown()
    {
        levelManager.avatarMapPosUpdate(currentX, currentY, true);
        transform.position -= Vector3.up * 2;
        currentX += 2;
        levelManager.avatarMapPosUpdate(currentX, currentY, false);
        debugPrintPos();
    }

    private void debugPrintPos()
    {
        debugText.text = "x: " + currentX + "\ny: " + currentY;
    }
}


