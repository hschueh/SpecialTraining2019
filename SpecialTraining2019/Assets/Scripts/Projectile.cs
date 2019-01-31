using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    bool idle = true;
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
        if(!idle)
            rb.MovePosition(rb.position + speed);
    }

    public void SetSpeed(float x, float y)
    {
        speed.x = x;
        speed.y = y;
        speed.Normalize();
        speed *= 0.1f;
    }

    public void SetPosition(float x, float y)
    {
        gameObject.transform.TransformVector(new Vector3(x, y, 0));
    }

    public void Shot()
    {
        idle = false;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
