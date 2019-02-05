using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    // Moving speed also affect by InputWrapper
    const float speedValue = 0.45f;

    private bool shouldInit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // only when gameState == STATE_START, player can move
        // when gameState become STATE_START, call MoveToInitPosition
        if (GameController.getInstance().GetGameState() != GameController.STATE_START)
        {
            // START, STOP, PAUSE
            if (GameController.getInstance().GetGameState() == GameController.STATE_STOP)
            {
                shouldInit = true;
            }
            return;
        }

        if (shouldInit)
        {
            shouldInit = false;
            MoveToInitPosition();
            return;
        }

        Vector2 posDiff = InputWrapper.getInstance().getMovement();
        posDiff *= speedValue;

        if (posDiff.x > 0.05f)
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 45, 0));
        else if (posDiff.x < -0.05f)
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, -45, 0));
        else
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        Vector2 final_pos = rb.position + posDiff;

        Vector3 posBotLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 posTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0));

        if (final_pos.x > posTopRight.x) 
        {
            final_pos.x = posTopRight.x - 0.1f;
        }

        if (final_pos.x < posBotLeft.x)
        {
            final_pos.x = posBotLeft.x + 0.1f;
        }

        if (final_pos.y > posTopRight.y)
        {
            final_pos.y = posTopRight.y - 0.1f;
        }

        if (final_pos.y < posBotLeft.y)
        {
            final_pos.y = posBotLeft.y + 0.1f;
        }

        rb.MovePosition(final_pos);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (GameController.getInstance().GetGameState() != GameController.STATE_START)
        {
            return;
        }

        Debug.Log("Triggered!");
        GameController.getInstance().GameOver();
    }

    void MoveToInitPosition()
    {
         rb.MovePosition(new Vector2(0, 0));
    }
}
