﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraPos;
    public Transform p1Pos;
    public Transform p2Pos;
    public Camera cam;
    private float margin = 0.5f;
    public float xL;
    public float xR;
    public Transform midPoint;

    [SerializeField]
    private BoxCollider2D leftWall;
    [SerializeField]
    private BoxCollider2D rightWall;

    // Start is called before the first frame update
    void Start()
    {
        CalcScreen(p1Pos, p2Pos);
    }

    // Update is called once per frame
    void Update()
    {
        CalcScreen(p1Pos, p2Pos);
        SetMidPoint();
    }

    public void ResetMidPoint()
    {
        DisableWalls();
        midPoint.position = new Vector2(0, 0);
    }

    private void SetMidPoint()
    {
        float midPosX = midPoint.position.x;
        float midPosY;

        midPosX = (xR + xL) / 2;
        midPosY = ((p1Pos.position.y + p2Pos.position.y) / 2) + 3;
        midPoint.position = new Vector2(midPosX, midPosY);
    }

    private void CalcScreen(Transform p1, Transform p2)
    {
        if(p1.position.x < p2.position.x)
        {
            xL = p1.position.x - margin;
            xR = p2.position.x + margin;
        }
        else
        {
            xL = p2.position.x - margin;
            xR = p1.position.x + margin;
        }
    }

    private void DisableWalls()
    {
        leftWall.enabled = false;
        rightWall.enabled = false;
    }

    public void EnableWalls()
    {
        leftWall.enabled = true;
        rightWall.enabled = true;
    }
}
