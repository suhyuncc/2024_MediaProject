using UnityEngine;
//gh
[System.Serializable]
public struct DialogueData
{
    public int speakerType; // ���� ��ȭ���� ��� ... ���� ���� �ʿ����� �Ƹ�, ������Ʈ 
    public string[] dialogue_Context; // ��ȭ ����
    public string[] is_select; //ȭ��
    public string[] Secletion_Context; // ��ȭ ����
    public string[] Next_event;
}
public class Dialogue : MonoBehaviour
{
    [SerializeField] string eventName; //���� �������� ��ȭ ����

    [SerializeField] string[] dialogue_Data;
}
