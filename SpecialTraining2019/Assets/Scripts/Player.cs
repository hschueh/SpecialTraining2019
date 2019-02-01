﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    //Vector2 speed = new Vector2(0, 0);
    const float speedValue = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.getInstance().IsGameStart() == false)
        {
            return;
        }

        Vector2 newPos = rb.position;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            newPos.y += speedValue;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            newPos.y -= speedValue;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newPos.x -= speedValue;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            newPos.x += speedValue;
        }
        rb.MovePosition(newPos);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Triggered!");
        GameController.getInstance().GameOver();
    }
}