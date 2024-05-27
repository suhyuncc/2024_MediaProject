using UnityEngine;
//gh
[System.Serializable]
public struct DialogueData
{
    public int speakerType; // ���ϴ� ĳ����
    public string[] dialogue_Context; // ��ȭ ����
    public string[] is_select; // �������� �ִ°�?
    public string[] Secletion_Context; // ������ ����
    public string[] Next_event; // ������ ���� event_name
    public int image_serialNum; // ��� �ø��� �ѹ�
    public string[] is_dice; // �ֻ����� �����°�?
    public string[] Dice_Context; // �ֻ��� ������ ����
    public string[] Dice_name; // �ֻ��� ����
    public string[] Dice_stat; // ��� ����
    public string[] Dice_Next_event; // �ֻ��� ���� event_name
    public string[] is_reset; // � �����ΰ�?
    public int item_serialNum; // ������ �ø��� �ѹ�
    public int stage_serialNum; // �������� �ø��� �ѹ�
}
public class Dialogue : MonoBehaviour
{
    [SerializeField] string eventName; //���� �������� ��ȭ ����

    [SerializeField] string[] dialogue_Data;
}
