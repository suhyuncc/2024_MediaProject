using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//gh
public class CSVParsingD : MonoBehaviour
{
    public static CSVParsingD instance;

    [SerializeField]
    private TextAsset csvFile = null;
    private static Dictionary<string, DialogueData[]> dialogueDict = new Dictionary<string, DialogueData[]>();
    private static Dictionary<string, string[]> selection = new Dictionary<string, string[]>();
    static bool isFirstOn = true;
    public static DialogueData[] GetDialogue(string eventName)
    {
        return dialogueDict[eventName];
    }

    public void Setcsv(TextAsset newFile)
    {
        // 파싱을 진행해야할 파일 교체
        csvFile = newFile;
        // 딕셔너리 리셋
        dialogueDict = new Dictionary<string, DialogueData[]>();
        // 파싱 진행
        SetDict();
    }

    public void SetDict()
    {
        string csvText = csvFile.text.Substring(0, csvFile.text.Length - 1); //csv파일 전체를 string타입으로 변환
        string[] row = csvText.Split(new char[] { '\n' }); //개행을 기준으로 행마다 분할

        for(int i = 1; i < row.Length; i++)
        {
            string[] data = row[i].Split(new char[] { ',' }); //(,)기준으로 데이터 분할

            string eventName = data[0];

            List<DialogueData> dataList = new List<DialogueData>();

            //EventName별로 DialogueData 구조체 생성
            while (data[0].Trim() != "end")
            {
                List<string> BGM_SFX_List = new List<string>();
                List<string> contextList = new List<string>();
                List<string> select_context_List = new List<string>();
                List<string> selectList = new List<string>();
                List<string> Next_event_List = new List<string>();
                List<string> is_dice_List = new List<string>();
                List<string> Dice_Context_List = new List<string>();
                List<string> Dice_name_List = new List<string>();
                List<string> Dice_stat_List = new List<string>();
                List<string> Dice_Next_event_List = new List<string>();
                List<string> is_reset_List = new List<string>();

                DialogueData dialogueData;
                
                dialogueData.speakerType = Int32.Parse(data[1]);

                // 공백이라면
                if (data[7].Trim().Equals(""))
                {
                    dialogueData.image_serialNum = 0; // 0을 할당(0은 검은 화면)
                }
                else
                {
                    dialogueData.image_serialNum = Int32.Parse(data[7].Trim()); // 아니라면 CSV에 있는 값을 할당
                }

                // 공백이라면
                if (data[14].Trim().Equals(""))
                {
                    dialogueData.item_serialNum = 0; // 0을 할당
                }
                else
                {
                    dialogueData.item_serialNum = Int32.Parse(data[14].Trim()); // 아니라면 CSV에 있는 값을 할당
                }

                // 공백이라면
                if (data[15].Trim().Equals(""))
                {
                    dialogueData.stage_serialNum = 0; // 0을 할당
                }
                else
                {
                    dialogueData.stage_serialNum = Int32.Parse(data[15].Trim()); // 아니라면 CSV에 있는 값을 할당
                }

                // 공백이라면
                if (data[16].Trim().Equals(""))
                {
                    dialogueData.sub_serialNum = 0; // 0을 할당
                }
                else
                {
                    dialogueData.sub_serialNum = Int32.Parse(data[16].Trim()); // 아니라면 CSV에 있는 값을 할당
                }

                do
                {
                    BGM_SFX_List.Add(data[2].ToString());

                    data[3] = data[3].Replace("@", ","); // @를 ,로 변환(CSV파일 규칙) - CSV파일의 "대사"열
                    contextList.Add(data[3].ToString());

                    selectList.Add(data[4].ToString()); // CSV파일의 "선택지가 있는가?"열

                    data[5] = data[5].Replace("@", ","); // @를 ,로 변환(CSV파일 규칙) - CSV파일의 "선택지 대사"열
                    select_context_List.Add(data[5].ToString());

                    Next_event_List.Add(data[6].Trim().ToString()); // CSV파일의 "선택후 대사"열

                    is_dice_List.Add(data[8].Trim().ToString()); // CSV파일의 "주사위를 굴리는가?"열

                    Dice_Context_List.Add(data[9].Trim().ToString()); // CSV파일의 "주사위 대사"열

                    Dice_name_List.Add(data[10].Trim().ToString()); // CSV파일의 "주사위 종류"열

                    Dice_stat_List.Add(data[11].Trim().ToString()); // CSV파일의 "대상 스탯"열

                    Dice_Next_event_List.Add(data[12].Trim().ToString()); // CSV파일의 "이벤트 후 대사"열

                    is_reset_List.Add(data[13].Trim().ToString()); // CSV파일의 "어떤 리셋인가?"열 ( 1: deadend / 2: hard reset)

                    if (++i < row.Length)
                    {
                        data = row[i].Split(new char[] { ',' });
                    }
                    else break;
                } while (i < row.Length && data[1] == "" && data[0] != "end");

                dialogueData.BGM_SFX_Num = BGM_SFX_List.ToArray();
                dialogueData.dialogue_Context = contextList.ToArray();
                dialogueData.is_select = selectList.ToArray();
                dialogueData.Secletion_Context = select_context_List.ToArray();
                dialogueData.Next_event = Next_event_List.ToArray(); // - CSV파일의 "선택후 대사"열
                dialogueData.is_dice = is_dice_List.ToArray();
                dialogueData.Dice_Context = Dice_Context_List.ToArray();
                dialogueData.Dice_name = Dice_name_List.ToArray();
                dialogueData.Dice_stat = Dice_stat_List.ToArray();
                dialogueData.Dice_Next_event = Dice_Next_event_List.ToArray();
                dialogueData.is_reset = is_reset_List.ToArray();

                dataList.Add(dialogueData);
            }

            dialogueDict.Add(eventName, dataList.ToArray());
        }

    }

    private void Awake()
    {
        instance = this;

        Debug.Log(Application.dataPath);
        Debug.Log(Application.persistentDataPath);
        if (isFirstOn == true)
        {
            isFirstOn = false;
            SetDict();
        }
    }
}
