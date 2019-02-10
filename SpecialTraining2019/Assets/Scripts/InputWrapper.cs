using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputWrapper : MonoBehaviour
{

    public static InputWrapper instance;

    public const int TYPE_DESKTOP = 0;
    public const int TYPE_MOBILE_FIX_ON_TOUCH = 1;
    public const int TYPE_MOBILE_FIXED_PIVOT = 2;
    public const int TYPE_MOBILE_FLOATING_PIVOT = 3;

    public int type = -1;

    Vector2 movement = new Vector2(float.NaN, float.NaN);
    Vector2 nanVec2 = new Vector2(float.NaN, float.NaN);
    Vector2 pivot = new Vector2(0.0f, -2.0f);
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
        stick = GameObject.Find("Joystick");

        switch (Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                type = TYPE_MOBILE_FIXED_PIVOT;
                break;
            default:
                type = TYPE_DESKTOP;
                break;

        }

        switch (type)
        {
            case TYPE_MOBILE_FIXED_PIVOT:
                transform.position = new Vector3(pivot.x, pivot.y, 0);
                break;
            case TYPE_MOBILE_FLOATING_PIVOT:
            case TYPE_MOBILE_FIX_ON_TOUCH:
            case TYPE_DESKTOP:
                stick.GetComponent<SpriteRenderer>().enabled = false;
                joystickBase.GetComponent<SpriteRenderer>().enabled = false;
                break;
        }


    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case TYPE_MOBILE_FIX_ON_TOUCH:
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (pivot.Equals(nanVec2))
                    {
                        joystickBase.GetComponent<SpriteRenderer>().enabled = true;
                        stick.GetComponent<SpriteRenderer>().enabled = true;
                        Vector3 pos = Camera.main.ScreenToWorldPoint(touch.position);
                        pivot = pos;
                        transform.position = new Vector3(pivot.x, pivot.y, 0);

                        stick.transform.localPosition = new Vector3(0, 0, 0);
                    }
                    else
                    {
                        Vector3 tPos = Camera.main.ScreenToWorldPoint(touch.position);
                        movement.x = tPos.x - pivot.x;
                        movement.y = tPos.y - pivot.y;
                        if (movement.magnitude > 0.2f)
                        {
                            movement = movement.normalized * 0.2f;
                        }
                        stick.transform.localPosition = new Vector3(movement.x, movement.y, 0);
                    }
                }
                else
                {
                    pivot.x = float.NaN;
                    pivot.y = float.NaN;
                    joystickBase.GetComponent<SpriteRenderer>().enabled = false;
                    stick.GetComponent<SpriteRenderer>().enabled = false;

                    transform.position = new Vector3(pivot.x, pivot.y, 0);
                    movement.x = 0;
                    movement.y = 0;
                }
                break;
            case TYPE_MOBILE_FIXED_PIVOT:
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    Vector3 tPos = Camera.main.ScreenToWorldPoint(touch.position);
                    movement.x = tPos.x - pivot.x;
                    movement.y = tPos.y - pivot.y;
                    if (movement.magnitude > 0.2f)
                    {
                        movement = movement.normalized * 0.2f;
                    }
                    stick.transform.localPosition = new Vector3(movement.x, movement.y, 0);
                }
                else
                {
                    movement.x = 0;
                    movement.y = 0;
                }
                break;
            case TYPE_MOBILE_FLOATING_PIVOT:
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (pivot.Equals(nanVec2))
                    {
                        joystickBase.GetComponent<SpriteRenderer>().enabled = true;
                        stick.GetComponent<SpriteRenderer>().enabled = true;
                        Vector3 pos = Camera.main.ScreenToWorldPoint(touch.position);
                        pivot = pos;
                        transform.position = new Vector3(pivot.x, pivot.y, 0);

                        stick.transform.localPosition = new Vector3(0, 0, 0);
                    }
                    else
                    {
                        Vector3 tPos = Camera.main.ScreenToWorldPoint(touch.position);
                        movement.x = tPos.x - pivot.x;
                        movement.y = tPos.y - pivot.y;
                        if (movement.magnitude > 0.2f)
                        {
                            movement = movement.normalized * 0.2f;
                        }
                        stick.transform.localPosition = new Vector3(movement.x, movement.y, 0);
                        // If drag too far, the pivot will move relatively.
                        pivot.x = tPos.x - movement.x;
                        pivot.y = tPos.y - movement.y;
                        transform.position = new Vector3(pivot.x, pivot.y, 0);
                    }
                }
                else
                {
                    pivot.x = float.NaN;
                    pivot.y = float.NaN;
                    joystickBase.GetComponent<SpriteRenderer>().enabled = false;
                    stick.GetComponent<SpriteRenderer>().enabled = false;

                    transform.position = new Vector3(pivot.x, pivot.y, 0);
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
                movement = movement.normalized * 0.2f;
                break;
        }
    }

    public Vector2 getMovement()
    {
        return movement;
    }
}
