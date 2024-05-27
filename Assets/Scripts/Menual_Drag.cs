using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menual_Drag : MonoBehaviour
{
    [SerializeField]
    private GameObject backPanel;
    [SerializeField]
    private bool is_On;

    private Vector3 start_pos = new Vector3(-1400.0f, 0f, 0f);
    private Vector3 arrive_pos = new Vector3(-420.0f, 0f, 0f);
    private Vector3 relative_pos = new Vector3(0f,0f,0f);


    private void OnMouseDown()
    {
        //활성화 상태 및 뒷 패널 조작
        if (is_On)
        {
            is_On = false;
            backPanel.SetActive(false);
        }
        else
        {
            is_On = true;
            backPanel.SetActive(true);
        }

        //마우스 포인터와 메뉴얼창의 상대적 거리계산
        relative_pos.x = Input.mousePosition.x - this.GetComponent<RectTransform>().anchoredPosition.x - 960.0f;
    }

    private void OnMouseDrag()
    {
        

        //도착위치보다 넘어갔다면
        if (this.GetComponent<RectTransform>().anchoredPosition.x >= arrive_pos.x && is_On)
        {
            this.GetComponent<RectTransform>().anchoredPosition = new Vector3(arrive_pos.x,
            this.transform.position.y, this.transform.position.z);
        }
        else
        {
            //이미지 이동
            this.GetComponent<RectTransform>().anchoredPosition = new Vector3((Input.mousePosition.x - 960.0f) - relative_pos.x,
                this.transform.position.y, this.transform.position.z);
        }
    }

    private void OnMouseUp()
    {
        //도착위치까지
        if(this.GetComponent<RectTransform>().anchoredPosition.x < arrive_pos.x && is_On)
        {
            this.GetComponent<RectTransform>().anchoredPosition = new Vector3(arrive_pos.x,
            this.transform.position.y, this.transform.position.z);
        }

        //출발위치까지
        if (this.GetComponent<RectTransform>().anchoredPosition.x > start_pos.x && !is_On)
        {
            this.GetComponent<RectTransform>().anchoredPosition = new Vector3(start_pos.x,
            this.transform.position.y, this.transform.position.z);
        }
    }
}
