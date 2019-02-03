using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Vector2 speed = new Vector2(0, 0);

    ITileCallback callback = null;

    public const int TYPE_NORMAL = 0;
    public const int TYPE_FAST = 1;
    public const int TYPE_GUIDED = 2;

    int projectileType = TYPE_NORMAL;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        switch (projectileType)
        {
            case TYPE_NORMAL:
                sr.color = Color.white;
                break;
            case TYPE_FAST:
                sr.color = Color.yellow;
                break;
            case TYPE_GUIDED:
                sr.color = Color.red;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.getInstance().GetGameState() == GameController.STATE_PAUSE)
        {
            // when pause, do nothing.
            return;
        }

        //rb.transform.Translate(new Vector3(speed.x, speed.y, 0));
        UpdatePos();
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

        if (callback != null)
        {
            callback.OnDestroy();
        }
    }

    public void SetCallback(ITileCallback callback)
    {
        this.callback = callback;
    }


    public void SetType(int type)
    {
        projectileType = type;
    }

    private void UpdatePos()
    {
        switch (projectileType)
        {
            case TYPE_NORMAL:
                rb.transform.Translate(new Vector3(speed.x, speed.y, 0));
                break;
            case TYPE_FAST:
                rb.transform.Translate(new Vector3(2 * speed.x, 2 * speed.y, 0));
                break;
            case TYPE_GUIDED:
                double ts = new LocationInfo().timestamp;
                Vector3 pPos = GameController.getInstance().getPlayerPos();
                Vector2 direction = new Vector2(pPos.x - transform.position.x, pPos.y - transform.position.y).normalized;
                if (speed.x * direction.x + speed.y * direction.y > 0)
                {
                    speed /= 0.06f;
                    speed = direction * 0.04f + speed * 0.96f;
                    speed *= 0.06f;
                }
                rb.transform.Translate(new Vector3(speed.x, speed.y, 0));
                break;
        }

    }
}
