using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap_Drag : MonoBehaviour
{
    [SerializeField]
    private GameObject backPanel;
    [SerializeField]
    private bool is_On;

    private Vector3 start_pos = new Vector3(1490.0f, 30.0f, 0f);
    private Vector3 arrive_pos = new Vector3(340.0f, 30.0f, 0f);
    private Vector3 relative_pos = new Vector3(0f,0f,0f);


    private void OnMouseDown()
    {
        //활성화 상태 및 뒷 패널 조작
        if (!is_On)
        {
            is_On = true;
            backPanel.SetActive(true);
            
        }
        else
        {
            is_On = false;
        }

        
        //마우스 포인터와 메뉴얼창의 상대적 거리계산
        relative_pos.x = (Input.mousePosition.x - 960.0f) - this.GetComponent<RectTransform>().anchoredPosition.x;

        Debug.Log(relative_pos.x);
    }

    private void OnMouseDrag()
    {
        

        //도착위치보다 넘어갔다면
        if (this.GetComponent<RectTransform>().anchoredPosition.x <= arrive_pos.x && is_On)
        {
            this.GetComponent<RectTransform>().anchoredPosition = arrive_pos;
        }
        else
        {
            //이미지 이동
            this.GetComponent<RectTransform>().anchoredPosition = new Vector3((Input.mousePosition.x - 960.0f) - relative_pos.x,
                start_pos.y, start_pos.z);
        }
    }

    private void OnMouseUp()
    {

        //도착위치까지
        if(this.GetComponent<RectTransform>().anchoredPosition.x > arrive_pos.x && is_On)
        {
            this.GetComponent<RectTransform>().anchoredPosition = arrive_pos;
        }

        //출발위치까지
        if (this.GetComponent<RectTransform>().anchoredPosition.x < start_pos.x && !is_On)
        {
            this.GetComponent<RectTransform>().anchoredPosition = start_pos;

            backPanel.SetActive(false);
        }
    }
}
