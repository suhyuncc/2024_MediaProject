using UnityEngine;
//gh
[System.Serializable]
public struct DialogueData
{
    public int speakerType; // 말하는 캐릭터
    public string[] dialogue_Context; // 대화 내용
    public string[] is_select; // 선택지가 있는가?
    public string[] Secletion_Context; // 선택지 내용
    public string[] Next_event; // 선택지 다음 event_name
    public int image_serialNum; // 배경 시리얼 넘버
    public string[] is_dice; // 주사위를 굴리는가?
    public string[] Dice_Context; // 주사위 선택지 내용
    public string[] Dice_name; // 주사위 종류
    public string[] Dice_stat; // 대상 스탯
    public string[] Dice_Next_event; // 주사위 다음 event_name
    public string[] is_reset; // 어떤 리셋인가?
    public int item_serialNum; // 아이템 시리얼 넘버
    public int stage_serialNum; // 스테이지 시리얼 넘버
}
public class Dialogue : MonoBehaviour
{
    [SerializeField] string eventName; //현재 진행중인 대화 종류

    [SerializeField] string[] dialogue_Data;
}
