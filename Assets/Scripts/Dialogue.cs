using UnityEngine;
//gh
[System.Serializable]
public struct DialogueData
{
    public string is_select; //ȭ��
    public string[] dialogue_Context; // ��ȭ ����
    public int speakerType; // ���� ��ȭ���� ��� ... ���� ���� �ʿ����� �Ƹ�, ������Ʈ 
}
public class Dialogue : MonoBehaviour
{
    [SerializeField] string eventName; //���� �������� ��ȭ ����

    [SerializeField] string[] dialogue_Data;
}
