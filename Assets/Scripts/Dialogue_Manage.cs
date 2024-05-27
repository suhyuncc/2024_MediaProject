using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//gh
public class Dialogue_Manage : MonoBehaviour
{
    public static Dialogue_Manage Instance;

    public string eventName; // eventName 수령 받을 곳
    public DialogueData[] dialogueData; //대화 데이터
    public bool isDialogue = false; // recieve event
    private bool currentDialogue = false; // is current dialogue working
    

    [Header("UI")]
    [SerializeField]
    private GameObject endTriangle; //끝나면 깜빡거리는 삼각형
    [SerializeField]
    private GameObject dialoguePanel; //대화panel
    /// <summary>
    /// <para>
    /// 0번: BarMaster
    /// 1번: BarVisitor
    /// </para>
    /// <para>
    /// 2번: HotelEmployee
    /// 3번: PoolVisitor
    /// </para>
    /// <para>
    /// 4번: HotelOwner
    /// 5번: NPC1
    /// </para>
    /// <para>
    /// 6번: StoreMaster
    /// 7번: SweetroomVisitor
    /// </para>
    /// </summary>
    [SerializeField]
    private GameObject _characters; //캐릭터 Image들이 저장되어있는 패널
    [SerializeField]
    private GameObject SelectBoxes; //선택지 박스들
    [SerializeField]
    private GameObject CheckBoxes; //주사위 이벤트시 선택지 박스들
    [SerializeField]
    private GameObject _softReset; //deadend 버튼
    [SerializeField]
    private GameObject _hardReset; //처음부터 시작 버튼
    [SerializeField]
    private GameObject _ending; //처음부터 시작 버튼
    [SerializeField]
    private Text contextText; //대화

    [Header("Background")]
    [SerializeField]
    private RawImage _background;
    [SerializeField]
    private Texture[] backgrounds;

    private void Awake()
    {
        Instance = this;
    }

    public void GetEventName(string _eventName) //eventName 수령받는 함수
    {
        eventName = _eventName;
        isDialogue = true;
        if(dialoguePanel.activeSelf == false)
            dialoguePanel.SetActive(true);
    }
    
    private int dataIndex = 0;
    private int contextIndex = 0;

    private int previousImage = 0; 

    private bool currentTypeEnd = false;
    private string toType = null;

    private IEnumerator typingText;

    private GameObject gm;
    private void Update()
    {

        if(isDialogue)
        {
            for(int t = 0; t< _characters.transform.childCount; t++)
            {
                if (_characters.transform.GetChild(t).gameObject.activeSelf)
                {
                    _characters.transform.GetChild(t).gameObject.SetActive(false);
                }
            }
            isDialogue = false;
            currentDialogue= true;
            gm = GameObject.Find("GameManager");
            //background.transform.GetChild(gm.GetComponent<GameManager>().GetStageNumber()).gameObject.SetActive(true);
            dialogueData = CSVParsingD.GetDialogue(eventName); // 화자 타입, 화자 이름, 대사를 원하는 이벤트에 있는 내용을 가져옴
            endTriangle.SetActive(false);
            //nameText.text = dialogueData[0].name;
            previousImage = dialogueData[0].speakerType;    // 케릭터 시리얼 번호
            if (_characters.transform.GetChild(dialogueData[0].speakerType).gameObject.activeSelf == false &&
                previousImage != 0)
            {
                
                _characters.transform.GetChild(previousImage - 1).gameObject.SetActive(true);
            }
            dataIndex = 0;
            contextIndex = 0;
            currentTypeEnd = false;
            //타이핑 코루틴
            typingText = TypingMotion();
            //타이핑 될 문장
            toType = dialogueData[dataIndex].dialogue_Context[contextIndex];
            StartCoroutine(typingText);
        }//EventName을 받아왔을 때

        //다이알로그 진행중이고 클릭이 되면
        if(currentDialogue && Input.GetMouseButtonDown(0))
        {
            //코루틴이 끝났는지
            if (currentTypeEnd)
            {
                //지금 출력중인 문장이 다 출력 안되어있으면
                if (contextIndex < dialogueData[dataIndex].dialogue_Context.Length - 1)
                {
                    //나머지 싹다 출력
                    contextIndex++;
                    currentTypeEnd = false;
                    typingText = TypingMotion();
                    endTriangle.SetActive(false);
                    toType = dialogueData[dataIndex].dialogue_Context[contextIndex];
                    StartCoroutine(typingText);
                }
                //지금 출력중인 문장이 다 출력 되어있으면
                else if (contextIndex >= dialogueData[dataIndex].dialogue_Context.Length - 1)
                {
                    //시리얼 넘버가 0이 아닐 때 배경 바꾸기
                    if (dialogueData[dataIndex].image_serialNum != 0)
                    {
                        Change_back(dialogueData[dataIndex].image_serialNum);
                    }

                    dataIndex++;                    

                    //다음 출력할 문장 있으면
                    if (dataIndex < dialogueData.Length)
                    {
                        contextIndex = 0;
                        //nameText.text = dialogueData[dataIndex].name;
                        currentTypeEnd = false;
                        typingText = TypingMotion();
                        endTriangle.SetActive(false);
                        toType = dialogueData[dataIndex].dialogue_Context[contextIndex];

                        if(dialogueData[dataIndex].speakerType != 0)
                        {
                            if (_characters.transform.GetChild(dialogueData[dataIndex].speakerType).gameObject.activeSelf == false)
                            {
                                /*
                                //Debug.Log(dialogueData[dataIndex].speakerType);
                                GameObject _prevImage = _characters.transform.GetChild(previousImage - 1).gameObject;
                                Color _prevColor = _prevImage.GetComponent<RawImage>().color;
                                _prevColor.a = 0.3f;
                                _prevImage.GetComponent<RawImage>().color = _prevColor; //여기까지 알파값 바까주고---------*/
                                previousImage = dialogueData[dataIndex].speakerType;
                                _characters.transform.GetChild(previousImage - 1).gameObject.SetActive(true);
                            }
                            else
                            {
                                /*
                                GameObject _prevImage = _characters.transform.GetChild(previousImage - 1).gameObject;
                                Color _prevColor = _prevImage.GetComponent<RawImage>().color;
                                _prevColor.a = 0.3f;
                                _prevImage.GetComponent<RawImage>().color = _prevColor;
                                */
                                previousImage = dialogueData[dataIndex].speakerType;
                                GameObject _prevImage = _characters.transform.GetChild(previousImage - 1).gameObject;
                                Color _prevColor = _prevImage.GetComponent<RawImage>().color;
                                _prevColor.a = 1.0f;
                                _prevImage.GetComponent<RawImage>().color = _prevColor;
                            }
                        }
                        


                        StartCoroutine(typingText); ;
                    }
                    //다음 출력할 문장 없으면
                    else
                    {
                        currentDialogue = false;

                        if(dialogueData[dataIndex - 1].item_serialNum != 0)
                        {
                            //아이템 리스트에 아이템 추가
                            GameManager.Instance.Add_Item(dialogueData[dataIndex - 1].item_serialNum - 1);
                        }

                        if (dialogueData[dataIndex - 1].is_select[contextIndex] == "1")
                        {
                            contextIndex = 0;
                            //선택지 보여주기
                            selectbox();
                        }
                        else if (dialogueData[dataIndex - 1].is_select[contextIndex] == "2")
                        {
                            contextIndex = 0;
                            //2번째 선택지 창 열기
                            selectbox2();
                        }
                        else if (dialogueData[dataIndex - 1].is_dice[contextIndex] == "1")
                        {
                            contextIndex = 0;
                            //주사위 선택지 보여주기
                            checkbox();
                        }
                        else if (dialogueData[dataIndex - 1].is_dice[contextIndex] == "2")
                        {
                            contextIndex = 0;
                            //2번쨰 주사위 던지기 주사위 선택지 보여주기
                            checkbox2();
                        }
                        else if (dialogueData[dataIndex - 1].is_reset[contextIndex] == "1")
                        {
                            // deadend를 위한 버튼
                            _softReset.SetActive(true);
                        }
                        else if (dialogueData[dataIndex - 1].is_reset[contextIndex] == "2")
                        {
                            // hardreset을 위한 버튼
                            _hardReset.SetActive(true);
                        }
                        else if (dialogueData[dataIndex - 1].is_reset[contextIndex] == "3")
                        {
                            // 일반 엔딩을 위한 버튼
                            _ending.SetActive(true);
                        }
                        //세이렌 이벤트
                        //다음 화면 전환이 예정되어있다면 세이렌 무시
                        else if (GameManager.Instance.Siren_On && dialogueData[dataIndex - 1].stage_serialNum == 0)
                        {
                            GameManager.Instance.Siren_event();
                            GameManager.Instance.Siren_On = false;
                        }
                        //축사 이벤트
                        else if (GameManager.Instance.Owner_count == 3)
                        {
                            GameManager.Instance.Owner_Event();
                            GameManager.Instance.Owner_count = 0;
                        }
                        else
                        {
                            //스테이지 변경
                            if (dialogueData[dataIndex - 1].stage_serialNum != 0)
                            {
                                GameManager.Instance.Stage_start(dialogueData[dataIndex - 1].stage_serialNum);
                                //dialoguePanel.SetActive(false);//Dialogue UI 끄기
                            }
                            else
                            {
                                dialoguePanel.SetActive(false);//Dialogue UI 끄기
                            }
                            
                        }

                        
                    }
                }
            }
            else
            {
                StopCoroutine(typingText);
                contextText.text = toType;
                currentTypeEnd = true;
                endTriangle.SetActive(true);
            }
        }
    }
    IEnumerator TypingMotion()
    {
        contextText.text = null;
        for (int i = 0; i < toType.Length; i++)
        {
            contextText.text += toType[i];
            yield return new WaitForSeconds(0.15f);
        }
        currentTypeEnd = true;
        endTriangle.SetActive(true);
        yield break;
    }

    private void selectbox()
    {
        dialogueData = CSVParsingD.GetDialogue("select");
        
        for (int i = 0; i < dialogueData[0].Secletion_Context.Length; i++)
        {
            //박스키고
            SelectBoxes.transform.GetChild(i).gameObject.SetActive(true);

            //글자 박고
            SelectBoxes.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<Text>().text
                = dialogueData[0].Secletion_Context[i];

            //다음 대사 알려주고
            SelectBoxes.transform.GetChild(i).GetComponent<SelectBox>().SetEventName(dialogueData[0].Next_event[i]);

        }
    }

    private void selectbox2()
    {
        dialogueData = CSVParsingD.GetDialogue("select2");

        for (int i = 0; i < dialogueData[0].Secletion_Context.Length; i++)
        {
            //박스키고
            SelectBoxes.transform.GetChild(i).gameObject.SetActive(true);

            //글자 박고
            SelectBoxes.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<Text>().text
                = dialogueData[0].Secletion_Context[i];

            //다음 대사 알려주고
            SelectBoxes.transform.GetChild(i).GetComponent<SelectBox>().SetEventName(dialogueData[0].Next_event[i]);

        }
    }

    private void checkbox()
    {
        dialogueData = CSVParsingD.GetDialogue("check");

        for (int i = 0; i < dialogueData[0].Dice_Context.Length; i++)
        {
            if (dialogueData[0].Dice_Context[i] != "")
            {
                //박스키고
                CheckBoxes.transform.GetChild(i).gameObject.SetActive(true);

                //글자 박고
                CheckBoxes.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<Text>().text
                    = dialogueData[0].Dice_Context[i];

                //던질 주사위 대사 알려주고
                CheckBoxes.transform.GetChild(i).GetComponent<CheckBox>().SetDiceName(dialogueData[0].Dice_name[i]);

                //대상 스탯 알려주고
                CheckBoxes.transform.GetChild(i).GetComponent<CheckBox>().SetCheckStat(dialogueData[0].Dice_stat[i]);

                //주사위를 던진 후 대사 알려주고
                CheckBoxes.transform.GetChild(i).GetComponent<CheckBox>().SetEventName(dialogueData[0].Dice_Next_event);
            }
        }
        
    }

    private void checkbox2()
    {
        dialogueData = CSVParsingD.GetDialogue("check2");

        for (int i = 0; i < dialogueData[0].Dice_Context.Length; i++)
        {
            if (dialogueData[0].Dice_Context[i] != "")
            {
                //박스키고
                CheckBoxes.transform.GetChild(i).gameObject.SetActive(true);

                //글자 박고
                CheckBoxes.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<Text>().text
                    = dialogueData[0].Dice_Context[i];

                //던질 주사위 대사 알려주고
                CheckBoxes.transform.GetChild(i).GetComponent<CheckBox>().SetDiceName(dialogueData[0].Dice_name[i]);

                //대상 스탯 알려주고
                CheckBoxes.transform.GetChild(i).GetComponent<CheckBox>().SetCheckStat(dialogueData[0].Dice_stat[i]);

                //주사위를 던진 후 대사 알려주고
                CheckBoxes.transform.GetChild(i).GetComponent<CheckBox>().SetEventName(dialogueData[0].Dice_Next_event);
            }
        }

    }

    //배경 변경을 시리얼넘버에 따라 실행
    public void Change_back(int image_num)
    {
        _background.texture = backgrounds[image_num];
    }

    public void Dialouge_off()
    {
        dialoguePanel.gameObject.SetActive(false);
    }
}
