using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Event_button : MonoBehaviour
{
    [SerializeField]
    private string _filename; // �� ��ư���� �ϳ��ϳ� �̺�Ʈ �̸� ����� ��

    public void Play_Event()
    {
        //�̺�Ʈ ���̾�α� �ε�
        CSVParsingD.instance.Setcsv(Resources.Load<TextAsset>(_filename));
        //���̾�α� ����
        Dialogue_Manage.Instance.GetEventName("Example");
    }
}
