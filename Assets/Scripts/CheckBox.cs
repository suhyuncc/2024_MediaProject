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
    private string[] _next_Events; // ���� ���


    public void SetCheckStat(string checkstat)
    {
        _checkStat = checkstat;
    }

    public void SetDiceName(string diceName)
    {
        _diceName = diceName;
    }

    public void SetEventName(string[] _eventName)
    {
        _next_Events = _eventName;
    }

    

    public void Go_dice()
    {
        if (_dice_Panel.Player_set)
        {
            // ���� ��� �ѱ��
            _dice_Panel.SetEventName(_next_Events);

            // ȭ����ȯ�� ���ÿ� �ֻ��� ����
            GameManager.Instance.Dice_On(_diceName);
        }
        else if (_checkStat == "poker")
        {
            GameManager.Instance.Play_Casino();

            CardGameManager.Instance.SetEventName(_next_Events[0]);
        }
        else if (_checkStat == "rush")
        {
            GameManager.Instance.Play_Rush();

            R_H_Manager.instance.SetEventName(_next_Events);
        }
        else
        {
            // ������ ����
            _dice_Panel.Set_icon(_checkStat);

            // ���� ��� �ѱ��
            _dice_Panel.SetEventName(_next_Events);

            // ȭ����ȯ�� ���ÿ� �ֻ��� ����
            GameManager.Instance.Dice_On(_diceName);
        }

        for (int i = 0; i < this.transform.parent.childCount; i++)
        {
            //��� ����â ���߱�
            this.transform.parent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
