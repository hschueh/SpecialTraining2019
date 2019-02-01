using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 speed = new Vector2(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.getInstance().GetGameState() == GameController.STATE_PAUSE)
        {
            // when pause, do nothing.
            return;
        }

        rb.transform.Translate(new Vector3(speed.x, speed.y, 0));
    }

    public void SetSpeed(float x, float y)
    {
        speed.x = x;
        speed.y = y;
        speed.Normalize();
        speed *= 0.06f;
    }

    public void SetPosition(float x, float y)
    {
        gameObject.transform.position = new Vector3(x, y, 0);
    }
   

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
