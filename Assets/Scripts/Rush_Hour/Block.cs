using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    private RectTransform points;
    [SerializeField]
    private int child_num;

    private Vector3 mousePosition;
    private Vector2 originalPosition;
    private RectTransform R_transform;

    [SerializeField]
    private Vector2 finish_point;
    [SerializeField]
    private Vector2 prev_point;
    private Vector2 origin_point;

    public bool is_main;
    public bool is_vertical;
    public bool is_collision;
    private bool is_drag;

    private int x_direct;
    public int x_verdict;
    private float x_float;

    public int y_direct;
    public int y_verdict;
    private float y_float;
    // Start is called before the first frame update
    void Start()
    {

        R_transform = this.GetComponent<RectTransform>();

        //prev_point.localPosition = new Vector3(init_x, init_y);

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
        prev_point = originalPosition - points.anchoredPosition;
        origin_point = prev_point;
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

    private Transform neariest_point()
    {
        Transform arrive_point = points.GetChild(0);
        Vector2 end_pos = this.GetComponent<RectTransform>().anchoredPosition;

        float small_y = 999999.9f;
        float small_x = 999999.9f;

        for (int i = 0; i < points.childCount; i++)
        {
            if (is_vertical)
            {
                float y = Mathf.Abs(end_pos.y - (points.anchoredPosition.y + points.GetChild(i).localPosition.y));

                if(small_y > y)
                {
                    small_y = y;
                    arrive_point = points.GetChild(i);
                }
            }
            else
            {
                float x = Mathf.Abs(end_pos.x - (points.anchoredPosition.x + points.GetChild(i).localPosition.x));

                if (small_x > x)
                {
                    small_x = x;
                    arrive_point = points.GetChild(i);
                }
            }
            //Debug.Log($"{points.GetChild(i).localPosition}");
        }

        //Debug.Log($"{arrive_point.localPosition}");
        return arrive_point;
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

            if (y_float > 0)
            {
                y_direct = 1;
            }
            else if (y_float < 0)
            {
                y_direct = -1;
            }
        }
        else
        {
            x_float = ((Input.mousePosition.x - 960.0f) - mousePosition.x) - R_transform.anchoredPosition.x;

            if(x_float > 0)
            {
                x_direct = 1;
            }
            else if(x_float < 0)
            {
                x_direct = -1;
            }
            //x_direct = (x_float > 0) ? 1 : -1;
            //Debug.Log(x_direct);
        }

        if (is_collision)
        {
            if(is_vertical && (-y_verdict == y_direct))
            {
                is_collision = false;
            }
            else if (!is_vertical && (-x_verdict == x_direct))
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

        Transform end = neariest_point();

        if((is_vertical && MathF.Abs(end.localPosition.y - prev_point.y) > 10) ||
            (!is_vertical && MathF.Abs(end.localPosition.x - prev_point.x) > 10))
        {
            R_H_Manager.instance.RunBlockCommand(this.gameObject, prev_point);
        }

        Vector2 end_pos = end.localPosition;

        prev_point = end_pos;

        Move_to_point(end_pos);

        if(is_main && end_pos == finish_point)
        {
            R_H_Manager.instance.On_Finish(true);
        }

    }

    public void Reset_point()
    {
        prev_point = origin_point;
    }

    public void Move_to_point(Vector2 end)
    {
        float new_x = points.anchoredPosition.x + end.x;
        float new_y = points.anchoredPosition.y + end.y;

        this.GetComponent<RectTransform>().anchoredPosition = new Vector2(new_x, new_y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("block")) && is_drag)
        {
            if (is_vertical)
            {
                if (y_direct != 0)
                {
                    y_verdict = y_direct;
                }
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
