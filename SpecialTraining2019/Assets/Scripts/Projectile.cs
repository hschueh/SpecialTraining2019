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
    public const int TYPE_NUMBER = 3;

    int projectileType = TYPE_NORMAL;

    int life_counter;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        life_counter = 0;

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
        life_counter++;
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
        life_counter = 0;
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
                    float scale = 0.1f - ((float)life_counter / 1500.0f);
                    if (scale < 0) scale = 0.0f;
                    speed = direction * scale + speed * (1.0f - scale);
                    speed *= 0.06f;
                }
                rb.transform.Translate(new Vector3(speed.x, speed.y, 0));
                break;
        }

    }
}
