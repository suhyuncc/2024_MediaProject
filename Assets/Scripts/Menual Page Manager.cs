using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenualPageManager : MonoBehaviour
{
    [Header("Contents")]
    [SerializeField]
    private string title;
    [SerializeField]
    private Content[] contents; //�޴��� �����

    [Header("UI")]
    [SerializeField]
    private Button back;                                //���� ��ư
    [SerializeField]
    private Button next;                                //���� ��ư
    [SerializeField]
    private Text page_num;
    [SerializeField]
    private Text page_content;
    private int current_page_index = 0;                //���� ������

    private void Start()
    {
        set_contents(current_page_index);
    }

    // Update is called once per frame
    void Update()
    {
        if (current_page_index == 0)
        {
            back.gameObject.SetActive(false);
        }
        else 
        {
            back.gameObject.SetActive(true);
        }

        if (current_page_index == contents.Length - 1)
        {
            next.gameObject.SetActive(false);
        }
        else
        {
            next.gameObject.SetActive(true);
        }
    }

    private void set_contents(int page)
    {
        page_content.text = title + '\n';
        for (int i = 0; i < contents[page].menual.Length; i++)
        {
            page_content.text += '\n' + contents[page].menual[i];
        }
        //page_content.text = title + '\n' + '\n' + strings[page];
    }

    public void Push_back()
    {
        current_page_index--;
        page_num.text = (current_page_index + 1).ToString();
        set_contents(current_page_index);
    }

    public void Push_next()
    {
        current_page_index++;
        page_num.text = (current_page_index + 1).ToString();
        set_contents(current_page_index);
    }

    [System.Serializable]
    public struct Content
    {
        public string[] menual;
    }
}
