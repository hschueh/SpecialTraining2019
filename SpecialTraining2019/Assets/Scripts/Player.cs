using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    const float speedValue = 0.1f;

    private bool prevState;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        prevState = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.getInstance().IsGameStart() == false)
        {
            prevState = true;
            return;
        }

        if (prevState == true)
        {
            prevState = false;
            MoveToInitialPosition();
            return;
        }

        Vector2 posDiff = new Vector2();
        if (Input.GetKey(KeyCode.UpArrow))
        {
            posDiff.y += speedValue;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            posDiff.y -= speedValue;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            posDiff.x -= speedValue;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            posDiff.x += speedValue;
        }

        if (posDiff.x > 0.05f)
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 45, 0));
        else if (posDiff.x < -0.05f)
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, -45, 0));
        else
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        rb.MovePosition(rb.position + posDiff);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (GameController.getInstance().IsGameStart() == false)
        {
            return;
        }

        Debug.Log("Triggered!");
        GameController.getInstance().GameOver();
    }

    void MoveToInitialPosition()
    {
         rb.MovePosition(new Vector2(0, -2.5f));
    }
}
