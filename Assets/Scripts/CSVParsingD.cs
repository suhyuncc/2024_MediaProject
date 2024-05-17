using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//gh
public class CSVParsingD : MonoBehaviour
{
    [SerializeField]
    private TextAsset csvFile = null;
    private static Dictionary<string, DialogueData[]> dialogueDict = new Dictionary<string, DialogueData[]>();
    private static Dictionary<string, string[]> selection = new Dictionary<string, string[]>();
    static bool isFirstOn = true;
    public static DialogueData[] GetDialogue(string eventName)
    {
        return dialogueDict[eventName];
    }

    public void Setcsv()
    {
        return;
    }

    public void SetDict()
    {
        string csvText = csvFile.text.Substring(0, csvFile.text.Length - 1); //get csv file as string type, except last line(empty)
        string[] row = csvText.Split(new char[] { '\n' }); //split by enter sign

        for(int i = 1; i < row.Length; i++)
        {
            string[] data = row[i].Split(new char[] { ',' }); //split by (,)

            if (data[0].Trim() == "" || data[0].Trim() == "end")
            {
                continue; //no event -> continue
            }

            List<DialogueData> dataList = new List<DialogueData>();
            string eventName = data[0];

            while (data[0].Trim() != "end")
            {
                List<string> contextList = new List<string>();
                List<string> select_context_List = new List<string>();
                List<string> selectList = new List<string>();
                List<string> Next_event_List = new List<string>();

                DialogueData dialogueData;
                
                dialogueData.speakerType = Int32.Parse(data[1]);
                
                do
                {
                    data[3] = data[3].Replace("@", ","); // @를 ,로 변환(CSV파일 규칙) - CSV파일의 "대사"열
                    data[5] = data[5].Replace("@", ","); // @를 ,로 변환(CSV파일 규칙) - CSV파일의 "선택지 대사"열
                    contextList.Add(data[3].ToString());
                    selectList.Add(data[4].ToString());
                    select_context_List.Add(data[5].ToString());
                    Next_event_List.Add(data[6].Trim().ToString());
                    if (++i < row.Length)
                    {
                        data = row[i].Split(new char[] { ',' });
                    }
                    else break;
                } while (i < row.Length && data[1] == "" && data[0] != "end");

                dialogueData.dialogue_Context= contextList.ToArray();
                dialogueData.is_select = selectList.ToArray();
                dialogueData.Secletion_Context = select_context_List.ToArray();
                dialogueData.Next_event = Next_event_List.ToArray(); // - CSV파일의 "선택후 대사"열
                dataList.Add(dialogueData);
                
                
            }
            dialogueDict.Add(eventName, dataList.ToArray());
        }
        foreach(KeyValuePair<string, DialogueData[]> j in dialogueDict)
        {
            Debug.Log(j.Key);
        }
    }

    private void Awake()
    {
        Debug.Log(Application.dataPath);
        Debug.Log(Application.persistentDataPath);
        if (isFirstOn == true)
        {
            isFirstOn = false;
            SetDict();
        }
    }
}
