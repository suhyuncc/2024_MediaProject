using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casino_changer : Event_button
{
    [SerializeField]
    private Player_stat P_stat;

    public override void Play_Event()
    {
        //������ �Ǵ�
        for (int i = 0; i < P_stat.Item_list.Length; i++)
        {
            //Ƽ�� O
            if (P_stat.Item_list[i] == 14)
            {
                //�̺�Ʈ ���̾�α� �ε�
                CSVParsingD.instance.Setcsv(Get_events(2));
                //���̾�α� ����
                Dialogue_Manage.Instance.GetEventName("Example");
                CountFull();
                return;
            }
        }

        if (P_stat.Coin_num >= 100)
        {
            //�̺�Ʈ ���̾�α� �ε�
            CSVParsingD.instance.Setcsv(Get_events(1));
            //���̾�α� ����
            Dialogue_Manage.Instance.GetEventName("Example");
        }
        else
        {
            //�̺�Ʈ ���̾�α� �ε�
            CSVParsingD.instance.Setcsv(Get_events(0));
            //���̾�α� ����
            Dialogue_Manage.Instance.GetEventName("Example");
        }

    }

}
