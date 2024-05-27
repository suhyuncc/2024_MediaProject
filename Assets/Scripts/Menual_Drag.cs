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
        //Ȱ��ȭ ���� �� �� �г� ����
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

        //���콺 �����Ϳ� �޴���â�� ����� �Ÿ����
        relative_pos.x = Input.mousePosition.x - this.GetComponent<RectTransform>().anchoredPosition.x - 960.0f;
    }

    private void OnMouseDrag()
    {
        

        //������ġ���� �Ѿ�ٸ�
        if (this.GetComponent<RectTransform>().anchoredPosition.x >= arrive_pos.x && is_On)
        {
            this.GetComponent<RectTransform>().anchoredPosition = new Vector3(arrive_pos.x,
            this.transform.position.y, this.transform.position.z);
        }
        else
        {
            //�̹��� �̵�
            this.GetComponent<RectTransform>().anchoredPosition = new Vector3((Input.mousePosition.x - 960.0f) - relative_pos.x,
                this.transform.position.y, this.transform.position.z);
        }
    }

    private void OnMouseUp()
    {
        //������ġ����
        if(this.GetComponent<RectTransform>().anchoredPosition.x < arrive_pos.x && is_On)
        {
            this.GetComponent<RectTransform>().anchoredPosition = new Vector3(arrive_pos.x,
            this.transform.position.y, this.transform.position.z);
        }

        //�����ġ����
        if (this.GetComponent<RectTransform>().anchoredPosition.x > start_pos.x && !is_On)
        {
            this.GetComponent<RectTransform>().anchoredPosition = new Vector3(start_pos.x,
            this.transform.position.y, this.transform.position.z);
        }
    }
}
