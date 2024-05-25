using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBox : MonoBehaviour
{
    [SerializeField]
    private Dice_panel _dice_Panel;
    [SerializeField]
    private string _checkStat; // ��� ����
    [SerializeField]
    private string _diceName; // �ֻ��� ����
    [SerializeField]
    private string _next_Event; // ���� ���


    public void SetCheckStat(string checkstat)
    {
        _checkStat = checkstat;
    }

    public void SetDiceName(string diceName)
    {
        _diceName = diceName;
    }

    public void SetEventName(string _eventName)
    {
        _next_Event = _eventName;
    }

    

    public void Go_dice()
    {
        // ������ ����
        _dice_Panel.Set_icon(_checkStat);

        // ���� ��� �ѱ��
        _dice_Panel.SetEventName(_next_Event);

        // ȭ����ȯ�� ���ÿ� �ֻ��� ����
        GameManager.Instance.Dice_On(_diceName);

        for (int i = 0; i < this.transform.parent.childCount; i++)
        {
            //��� ����â ���߱�
            this.transform.parent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
