using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B2_btn : Event_button
{
    [SerializeField]
    private Player_stat P_stat;     //�÷��̾� ����

    public override void Play_Event()
    {
        //������ �Ǵ�
        for (int i = 0; i < P_stat.Item_list.Length; i++)
        {
            //������
            if (P_stat.Item_list[i] == 7)
            {
                //�̺�Ʈ ���̾�α� �ε�
                CSVParsingD.instance.Setcsv(Get_events(0));
                //���̾�α� ����
                Dialogue_Manage.Instance.GetEventName("Example");
                CountFull();
                return;
            }
            //������
            else if (P_stat.Item_list[i] == 8)
            {
                //�̺�Ʈ ���̾�α� �ε�
                CSVParsingD.instance.Setcsv(Get_events(1));
                //���̾�α� ����
                Dialogue_Manage.Instance.GetEventName("Example");
                CountFull();
                return;
            }
        }

        //�̺�Ʈ ���̾�α� �ε�
        CSVParsingD.instance.Setcsv(Get_events(2));
        //���̾�α� ����
        Dialogue_Manage.Instance.GetEventName("Example");
        CountFull();

    }
}
