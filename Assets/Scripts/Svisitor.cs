using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Svisitor : Event_button
{
    [SerializeField]
    private Player_stat P_stat;
    [SerializeField]
    private GameObject _npc;
    [SerializeField]
    private GameObject _bell;

    private bool is_juice;
    private bool is_flug = true;
    public override void Play_Event()
    {
        //�͸��� �Ǵ�
        for (int i = 0; i < P_stat.Item_list.Length; i++)
        {
            if (P_stat.Item_list[i] == 6)
            {
                is_flug = false;
            }
        }

        if (is_flug && _npc.GetComponent<Event_button>().Get_count() == 2)
        {
            Set_count(2);
        }

        if (Get_count() == 1)
        {
            //�ֽ� �Ǵ�
            for(int i = 0; i < P_stat.Item_list.Length; i++)
            {
                if (P_stat.Item_list[i] == 4)
                {
                    is_juice = true;
                }
            }
            //���ᰡ ���� ��
            if (is_juice)
            {
                //�̺�Ʈ ���̾�α� �ε�
                CSVParsingD.instance.Setcsv(Get_events(1));
                //���̾�α� ����
                Dialogue_Manage.Instance.GetEventName("Example"); 
                Up_count(1);
            }
            //���ᰡ ���� ��
            else
            {
                //�̺�Ʈ ���̾�α� �ε�
                CSVParsingD.instance.Setcsv(Get_events(2));
                //���̾�α� ����
                Dialogue_Manage.Instance.GetEventName("Example");
                Up_count(1);
            }

            is_juice = false;
        }
        else if (Get_count() == 2)
        {
            //�̺�Ʈ ���̾�α� �ε�
            CSVParsingD.instance.Setcsv(Get_events(3));
            //���̾�α� ����
            Dialogue_Manage.Instance.GetEventName("Example");
        }
        else
        {
            //count�� 1�� �ƴ϶�� 1����
            if (_npc.GetComponent<Event_button>().Get_count() != 1)
            {
                _npc.GetComponent<Event_button>().Up_count(1);
            }

            if (!_npc.activeSelf)
            {
                _npc.SetActive(true);
            }

            //count�� 1�� �ƴ϶�� 1����
            if (_bell.GetComponent<Event_button>().Get_count() != 1)
            {
                _bell.GetComponent<Event_button>().Up_count(1);
            }

            if (!_bell.activeSelf)
            {
                _bell.SetActive(true);
            }

            base.Play_Event();
        }
        
    }
}
