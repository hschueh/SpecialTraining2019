using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputWrapper : MonoBehaviour
{

    public static InputWrapper instance;

    public const int TYPE_DESKTOP = 0;
    public const int TYPE_MOBILE = 1;

    public int type = -1;

    Vector2 movement = new Vector2(float.NaN, float.NaN);
    Vector2 nanVec2 = new Vector2(float.NaN, float.NaN);
    Vector2 pivot;
    GameObject joystickBase;
    GameObject stick;

    public static InputWrapper getInstance()
    {
        return instance;
    }

    void Awake()
    {
        //If we don't currently have a game control...
        if (instance == null)
            //...set this one to be it...
            instance = this;
        //...otherwise...
        else if (instance != this)
            //...destroy this one because it is a duplicate.
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        joystickBase = GameObject.Find("JoystickBase");
        //joystickBase.GetComponent<SpriteRenderer>().enabled = false;
        stick = GameObject.Find("Joystick");
        //stick.GetComponent<SpriteRenderer>().enabled = false;

        switch (Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                type = TYPE_MOBILE;
                break;
            default:
                type = TYPE_DESKTOP;
                break;

        }


    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case TYPE_MOBILE:
                if(Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (pivot.Equals(nanVec2))
                    {
                        pivot = touch.position;

                        joystickBase.GetComponent<SpriteRenderer>().enabled = true;
                        stick.GetComponent<SpriteRenderer>().enabled = true;
                        Vector3 pos = Camera.main.ScreenToWorldPoint(touch.position);
                        transform.position = new Vector3(pos.x, pos.y, 0);

                        stick.transform.localPosition = new Vector3(0, 0, 0);
                    }
                    else
                    {
                        movement = touch.position - pivot;
                        var mNormalize = movement.normalized * 0.2f;
                        stick.transform.localPosition = new Vector3(mNormalize.x, mNormalize.y, 0);
                    }
                }
                else
                {
                    pivot.x = float.NaN;
                    pivot.y = float.NaN;
                    joystickBase.GetComponent<SpriteRenderer>().enabled = false;
                    stick.GetComponent<SpriteRenderer>().enabled = false;
                    movement.x = 0;
                    movement.y = 0;
                }
                break;
            case TYPE_DESKTOP:
                movement = new Vector2();
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    movement.y += 1;
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    movement.y -= 1;
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    movement.x -= 1;
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    movement.x += 1;
                }
                break;
        }
    }

    public Vector2 getMovement()
    {
        return movement;
    }
}
