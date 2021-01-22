﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevatorMovement : MonoBehaviour
{

    public playerManager playerManager;
    public levelManager levelManager;

    public float elevatorYUp, elevatorYDown;
    public bool isUpOnStart;
    private bool isUp = false;
    public int downX, downY, upX, upY;
    private int elevatorPlatformUpX;

    // Start is called before the first frame update
    void Start()
    {
        elevatorPlatformUpX = upX + 1;
        if (isUpOnStart)
        {
            isUp = true;
            transform.localPosition = new Vector3(0, elevatorYUp, 0);
            levelManager.onElevatorToggle(elevatorPlatformUpX, downY, isUp);
        }
        else
        {
            isUp = false;
            transform.localPosition = new Vector3(0, elevatorYDown, 0);
        }
    }

    public void Reset()
    {
        elevatorPlatformUpX = upX + 1;
        if (isUpOnStart)
        {
            isUp = true;
            transform.localPosition = new Vector3(0, elevatorYUp, 0);
            levelManager.onElevatorToggle(elevatorPlatformUpX, downY, isUp);
        }
        else
        {
            isUp = false;
            transform.localPosition = new Vector3(0, elevatorYDown, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerManager.allowInput)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isUp)
            {
                isUp = false;
                transform.localPosition = new Vector3(0,elevatorYDown,0);
                playerManager.onElevatorToggle(upX, upY, downX, downY, isUp);
                levelManager.onElevatorToggle(elevatorPlatformUpX, downY, isUp);
                playerManager.onElevatorMovementComplete();
            }
            
            else
            {
                isUp = true;
                transform.localPosition = new Vector3(0, elevatorYUp, 0);
                playerManager.onElevatorToggle(upX, upY, downX, downY, isUp);
                levelManager.onElevatorToggle(elevatorPlatformUpX, downY, isUp);
                playerManager.onElevatorMovementComplete();
            }
        }
    }

    
}
