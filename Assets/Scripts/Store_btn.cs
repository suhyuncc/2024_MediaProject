using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store_btn : Event_button
{
    [SerializeField]
    private Player_stat P_stat;     //�÷��̾� ����

    public override void Play_Event()
    {
        bool is_fire = false;
        //������ ���� �Ǵ�
        for (int i = 0; i < P_stat.Item_list.Length; i++)
        {
            if (P_stat.Item_list[i] == 8)
            {
                is_fire = true;
            }
        }

        //������ �Ǵ�
        if (is_fire)
        {
            //�̺�Ʈ ���̾�α� �ε�
            CSVParsingD.instance.Setcsv(Get_events(0));
            //���̾�α� ����
            Dialogue_Manage.Instance.GetEventName("Example");
            CountFull();
        }
        else
        {
            //�̺�Ʈ ���̾�α� �ε�
            CSVParsingD.instance.Setcsv(Get_events(1));
            //���̾�α� ����
            Dialogue_Manage.Instance.GetEventName("Example");
            CountFull();
        }

    }
}
