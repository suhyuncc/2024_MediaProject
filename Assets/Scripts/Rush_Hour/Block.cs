using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Vector3 mousePosition;
    private Vector2 originalPosition;
    private RectTransform R_transform;

    public bool is_vertical;
    public bool is_collision;
    private bool is_drag;

    private int x_direct;
    public int x_verdict;
    private float x_float;

    public int y_direct;
    private float y_float;
    // Start is called before the first frame update
    void Start()
    {
        R_transform = this.GetComponent<RectTransform>();

        if (R_transform.eulerAngles.z != 0)
        {
            is_vertical = true;
        }
        else
        {
            is_vertical = false;
        }

        is_collision = false;

        originalPosition = R_transform.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private Vector3 GetmousePos()
    {
        Vector3 re = Camera.main.WorldToScreenPoint(transform.position);
        return new Vector3(re.x, re.y, 0);
    }

    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - GetmousePos();
    }

    private void OnMouseDrag()
    {
        is_drag = true;

        //이미지 레이어 최상단으로 이동
        //transform.SetAsLastSibling();

        //Debug.Log($"x: {((Input.mousePosition.x - 960.0f) - mousePosition.x) - R_transform.anchoredPosition.x}");

        if (is_vertical)
        {
            y_float = ((Input.mousePosition.y - 540.0f) - mousePosition.y - R_transform.anchoredPosition.y);
        }
        else
        {
            x_float = ((Input.mousePosition.x - 960.0f) - mousePosition.x) - R_transform.anchoredPosition.x;
            x_direct = (x_float > 0) ? 1 : (x_float < 0) ? -1 : 0;
            Debug.Log(x_direct);
        }

        if (is_collision)
        {
            if (-x_verdict == x_direct || -y_direct == (int)Mathf.Sign(y_float))
            {
                is_collision = false;
            }
        }

        //이미지 이동
        if (is_drag && !is_collision)
        {
            if (is_vertical)
            {
                //수직일때는 세로로만 이동
                this.GetComponent<RectTransform>().anchoredPosition = new Vector2(R_transform.anchoredPosition.x,
                (Input.mousePosition.y - 540.0f) - mousePosition.y);

                
            }
            else
            {
                this.GetComponent<RectTransform>().anchoredPosition = new Vector2((Input.mousePosition.x - 960.0f) - mousePosition.x,
                R_transform.anchoredPosition.y);
            }

        }

    }

    private void OnMouseUp()
    {
        is_drag = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("block")) && is_drag)
        {
            if (is_vertical)
            {
                y_direct = (int)Mathf.Sign(y_float);
            }
            else
            {
                if(x_direct != 0)
                {
                    x_verdict = x_direct;
                }
                
            }

            is_collision = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if((collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("block")) && is_drag)
        {
            if (is_vertical)
            {
                //y_direct = (int)Mathf.Sign(y_float);
            }
            else
            {
                //x_direct = (int)Mathf.Sign(x_float);
            }

            is_collision = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("block")) && is_collision)
        {
            is_collision = false;
        }
    }
}
