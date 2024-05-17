using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBox : MonoBehaviour
{
    [SerializeField]
    private string next_Event; // ���� ���


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEventName(string _eventName)
    {
        next_Event = _eventName;
    }

    public void Next_Dialouge()
    {
        Dialogue_Manage.Instance.GetEventName(next_Event.ToString());

        for(int i = 0; i < this.transform.parent.childCount; i++)
        {
            //��� ����â ���߱�
            this.transform.parent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
