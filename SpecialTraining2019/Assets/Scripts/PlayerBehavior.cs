using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
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
}
