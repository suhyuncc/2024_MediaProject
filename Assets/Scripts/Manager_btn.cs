using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_btn : Event_button
{
    [SerializeField]
    private Player_stat P_stat;     //�÷��̾� ����

    public override void Play_Event()
    {
        //������ �Ǵ�
        for (int i = 0; i < P_stat.Item_list.Length; i++)
        {
            //ī��Ű
            if (P_stat.Item_list[i] == 13)
            {
                //�̺�Ʈ ���̾�α� �ε�
                CSVParsingD.instance.Setcsv(Get_events(0));
                //���̾�α� ����
                Dialogue_Manage.Instance.GetEventName("Example");
                CountFull();
                return;
            }
        }

        //�̺�Ʈ ���̾�α� �ε�
        CSVParsingD.instance.Setcsv(Get_events(1));
        //���̾�α� ����
        Dialogue_Manage.Instance.GetEventName("Example");
        CountFull();


    }
}
