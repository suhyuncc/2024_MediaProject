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
        // �Ľ��� �����ؾ��� ���� ��ü
        csvFile = newFile;
        // ��ųʸ� ����
        dialogueDict = new Dictionary<string, DialogueData[]>();
        // �Ľ� ����
        SetDict();
    }

    public void SetDict()
    {
        string csvText = csvFile.text.Substring(0, csvFile.text.Length - 1); //csv���� ��ü�� stringŸ������ ��ȯ
        string[] row = csvText.Split(new char[] { '\n' }); //������ �������� �ึ�� ����

        for(int i = 1; i < row.Length; i++)
        {
            string[] data = row[i].Split(new char[] { ',' }); //(,)�������� ������ ����

            string eventName = data[0];

            List<DialogueData> dataList = new List<DialogueData>();

            //EventName���� DialogueData ����ü ����
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

                // �����̶��
                if (data[7].Trim().Equals(""))
                {
                    dialogueData.image_serialNum = 0; // 0�� �Ҵ�(0�� ���� ȭ��)
                }
                else
                {
                    dialogueData.image_serialNum = Int32.Parse(data[7].Trim()); // �ƴ϶�� CSV�� �ִ� ���� �Ҵ�
                }

                // �����̶��
                if (data[14].Trim().Equals(""))
                {
                    dialogueData.item_serialNum = 0; // 0�� �Ҵ�
                }
                else
                {
                    dialogueData.item_serialNum = Int32.Parse(data[14].Trim()); // �ƴ϶�� CSV�� �ִ� ���� �Ҵ�
                }

                // �����̶��
                if (data[15].Trim().Equals(""))
                {
                    dialogueData.stage_serialNum = 0; // 0�� �Ҵ�
                }
                else
                {
                    dialogueData.stage_serialNum = Int32.Parse(data[15].Trim()); // �ƴ϶�� CSV�� �ִ� ���� �Ҵ�
                }

                // �����̶��
                if (data[16].Trim().Equals(""))
                {
                    dialogueData.sub_serialNum = 0; // 0�� �Ҵ�
                }
                else
                {
                    dialogueData.sub_serialNum = Int32.Parse(data[16].Trim()); // �ƴ϶�� CSV�� �ִ� ���� �Ҵ�
                }

                do
                {
                    BGM_SFX_List.Add(data[2].ToString());

                    data[3] = data[3].Replace("@", ","); // @�� ,�� ��ȯ(CSV���� ��Ģ) - CSV������ "���"��
                    contextList.Add(data[3].ToString());

                    selectList.Add(data[4].ToString()); // CSV������ "�������� �ִ°�?"��

                    data[5] = data[5].Replace("@", ","); // @�� ,�� ��ȯ(CSV���� ��Ģ) - CSV������ "������ ���"��
                    select_context_List.Add(data[5].ToString());

                    Next_event_List.Add(data[6].Trim().ToString()); // CSV������ "������ ���"��

                    is_dice_List.Add(data[8].Trim().ToString()); // CSV������ "�ֻ����� �����°�?"��

                    Dice_Context_List.Add(data[9].Trim().ToString()); // CSV������ "�ֻ��� ���"��

                    Dice_name_List.Add(data[10].Trim().ToString()); // CSV������ "�ֻ��� ����"��

                    Dice_stat_List.Add(data[11].Trim().ToString()); // CSV������ "��� ����"��

                    Dice_Next_event_List.Add(data[12].Trim().ToString()); // CSV������ "�̺�Ʈ �� ���"��

                    is_reset_List.Add(data[13].Trim().ToString()); // CSV������ "� �����ΰ�?"�� ( 1: deadend / 2: hard reset)

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
                dialogueData.Next_event = Next_event_List.ToArray(); // - CSV������ "������ ���"��
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
