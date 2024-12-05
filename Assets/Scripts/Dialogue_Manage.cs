using System;
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

    [SerializeField]
    private Player_stat P_stat;     //플레이어 정보
    public string eventName; // eventName 수령 받을 곳
    private DialogueData[] dialogueData; //대화 데이터
    public bool isDialogue = false; // recieve event
    private bool currentDialogue = false; // is current dialogue working


    [Header("UI")]
    [SerializeField]
    private float _typeSpeed;
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
    private GameObject EVBoxes; //주사위 이벤트시 선택지 박스들
    [SerializeField]
    private GameObject _softReset; //deadend 버튼
    [SerializeField]
    private GameObject _hardReset; //처음부터 시작 버튼
    [SerializeField]
    private GameObject _ending; //엔딩 버튼
    [SerializeField]
    private GameObject _realending; //진엔딩 버튼
    [SerializeField]
    private Text contextText; //대화
    [SerializeField]
    private Toggle _normal;
    [SerializeField]
    private Toggle _fast;
    public bool is_clicked = false; // 텍스트 창이 클릭되었는지 체크

    [Header("Background")]
    [SerializeField]
    private RawImage _background;
    [SerializeField]
    private Texture[] backgrounds;

    [Header("SubBackground")]
    [SerializeField]
    private GameObject _subBack;
    [SerializeField]
    private RawImage _subBackImge;
    [SerializeField]
    private Texture[] Sub_backgrounds;

    [Header("Scripts")]
    [SerializeField]
    private LogManager _logManager;

    [Header("Shop")]
    [SerializeField]
    private GameObject _shopPanel;
    private bool _shopOn = false;

    [Header("Keypad")]
    [SerializeField]
    private GameObject _keypadPanel;

    private void Awake()
    {
        Instance = this;
    }

    public void GetEventName(string _eventName) //eventName 수령받는 함수
    {
        eventName = _eventName;
        isDialogue = true;
        _subBack.SetActive(false);//sub_eventPanel 끄기
        if (dialoguePanel.activeSelf == false)
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
        if (_fast.isOn)
        {
            _typeSpeed = 0.05f;
        }
        else
        {
            _typeSpeed = 0.1f;
        }

        //EventName을 받아왔을 때
        if (isDialogue)
        {
            dataIndex = 0;
            contextIndex = 0;
            currentTypeEnd = false;
            is_clicked = false;
            isDialogue = false;
            currentDialogue = true;

            _logManager.Init_Log(); //로그창 초기화

            dialogueData = CSVParsingD.GetDialogue(eventName);  // 화자 타입, 화자 이름, 대사를 원하는 이벤트에 있는 내용을 가져옴
            previousImage = dialogueData[0].speakerType;        // 캐릭터 시리얼 번호
            //nameText.text = dialogueData[0].name;             // 캐릭터 이름(필요시 사용)

            if (previousImage >= 0)
            {
                Character_off();

                if (_characters.transform.GetChild(dialogueData[0].speakerType).gameObject.activeSelf == false &&
                previousImage > 0)
                {
                    _characters.transform.GetChild(previousImage - 1).gameObject.SetActive(true);
                    GameObject _prevImage = _characters.transform.GetChild(previousImage - 1).gameObject;
                    Color _prevColor = _prevImage.GetComponent<RawImage>().color;
                    _prevColor.a = 1.0f;
                    _prevImage.GetComponent<RawImage>().color = _prevColor;
                }
            }

            endTriangle.SetActive(false);
            
            //타이핑 코루틴
            typingText = TypingMotion();

            //타이핑 될 문장
            toType = dialogueData[dataIndex].dialogue_Context[contextIndex];
            
            StartCoroutine(typingText);
        }


        //다이알로그 진행중이고 클릭이 되면
        if(currentDialogue && (is_clicked || Input.GetKeyDown(KeyCode.Space)))
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
                    //typingText = TypingMotion();
                    endTriangle.SetActive(false);
                    toType = dialogueData[dataIndex].dialogue_Context[contextIndex];
                    //StartCoroutine(typingText);
                }
                //지금 출력중인 문장이 다 출력 되어있으면
                else if (contextIndex >= dialogueData[dataIndex].dialogue_Context.Length - 1)
                {
                    //시리얼 넘버가 0이 아닐 때 배경 바꾸기
                    if (dialogueData[dataIndex].image_serialNum != 0)
                    {
                        Change_back(dialogueData[dataIndex].image_serialNum);
                    }

                    //음수일 경우 서브화면 끄기
                    if (dialogueData[dataIndex].sub_serialNum > 0)
                    {
                        Change_Subback(dialogueData[dataIndex].sub_serialNum - 1);
                    }
                    else if (dialogueData[dataIndex].sub_serialNum < 0)
                    {
                        Subback_off();
                    }

                    //공백이 아닐때
                    if (!dialogueData[dataIndex].BGM_SFX_Num[contextIndex].Trim().Equals(""))
                    {
                        int num = Int32.Parse(dialogueData[dataIndex].BGM_SFX_Num[contextIndex]);
                        if(num < 0)
                        {
                            num = num * -1;
                            GameManager.Instance.Set_SFX(num - 1);
                        }
                        else if (num > 0)
                        {
                            GameManager.Instance.Set_BGM(num - 1);
                        }
                        else
                        {
                            GameManager.Instance.Stop_SFX();
                        }
                    }

                    

                    dataIndex++;

                    //아이템 획득
                    if (dialogueData[dataIndex - 1].item_serialNum != 0)
                    {
                        if (dialogueData[dataIndex - 1].item_serialNum < 0)
                        {
                            //칩 개수 추가
                            switch(dialogueData[dataIndex - 1].item_serialNum)
                            {
                                case -1:
                                    P_stat.Coin_num += 10;
                                    break;
                                case -2:
                                    P_stat.Coin_num += 15;
                                    break;
                                case -3:
                                    P_stat.Coin_num += 20;
                                    break;
                                case -4:
                                    P_stat.Coin_num -= 100;
                                    break;
                                case -5:
                                    Increase_san(10);
                                    break;
                            }
                        }
                        else 
                        {
                            //아이템 리스트에 아이템 추가
                            GameManager.Instance.Add_Item(dialogueData[dataIndex - 1].item_serialNum - 1);
                        }
                        
                    }



                    //다음 출력할 문장 있으면
                    if (dataIndex < dialogueData.Length)
                    {
                        contextIndex = 0;
                        //nameText.text = dialogueData[dataIndex].name;
                        currentTypeEnd = false;
                        typingText = TypingMotion();
                        endTriangle.SetActive(false);
                        toType = dialogueData[dataIndex].dialogue_Context[contextIndex];


                        if (dialogueData[dataIndex].speakerType < 0)
                        {
                            //케릭터 이미지 끄기
                            Character_off();
                        }
                        else if(dialogueData[dataIndex].speakerType > 0)
                        {
                            GameObject _prevImage;
                            Color _prevColor;

                            //나머지 색 다운
                            for (int i = 0; i < _characters.transform.childCount; i++)
                            {
                                if (_characters.transform.GetChild(i).gameObject.activeSelf)
                                {
                                    _prevImage = _characters.transform.GetChild(i).gameObject;
                                    _prevColor = _prevImage.GetComponent<RawImage>().color;
                                    _prevColor.a = 0.3f;
                                    _prevImage.GetComponent<RawImage>().color = _prevColor;
                                }
                            }

                            previousImage = dialogueData[dataIndex].speakerType;

                            if (_characters.transform.GetChild(previousImage - 1).gameObject.activeSelf == false)
                            {
                                _prevImage = _characters.transform.GetChild(previousImage - 1).gameObject;
                                _prevColor = _prevImage.GetComponent<RawImage>().color;
                                _prevColor.a = 1.0f;
                                _prevImage.GetComponent<RawImage>().color = _prevColor;
                                _characters.transform.GetChild(previousImage - 1).gameObject.SetActive(true);
                            }
                            else
                            {
                                _prevImage = _characters.transform.GetChild(previousImage - 1).gameObject;
                                _prevColor = _prevImage.GetComponent<RawImage>().color;
                                _prevColor.a = 1.0f;
                                _prevImage.GetComponent<RawImage>().color = _prevColor;
                            }
                        }
                        else
                        {
                            if(previousImage > 0)
                            {
                                GameObject _prevImage = _characters.transform.GetChild(previousImage - 1).gameObject;
                                Color _prevColor = _prevImage.GetComponent<RawImage>().color;
                                _prevColor.a = 0.3f;
                                _prevImage.GetComponent<RawImage>().color = _prevColor;
                            }
                            
                        }
                        


                        StartCoroutine(typingText); ;
                    }
                    //다음 출력할 문장 없으면
                    else
                    {
                        currentDialogue = false;

                        if (previousImage > 0)
                        {
                            GameObject _prevImage = _characters.transform.GetChild(previousImage - 1).gameObject;
                            Color _prevColor = _prevImage.GetComponent<RawImage>().color;
                            _prevColor.a = 0.3f;
                            _prevImage.GetComponent<RawImage>().color = _prevColor;
                        }

                        if (dialogueData[dataIndex - 1].is_select[contextIndex] == "-1")
                        {
                            contextIndex = 0;
                            //엘리베이터 선택지 창 열기
                            EVbox();
                        }
                        else if (dialogueData[dataIndex - 1].is_select[contextIndex] == "-2")
                        {
                            contextIndex = 0;
                            //키패드 창 열기
                            _keypadPanel.SetActive(true);
                        }
                        else if (dialogueData[dataIndex - 1].is_select[contextIndex] == "1")
                        {
                            contextIndex = 0;
                            //선택지 보여주기
                            selectbox();
                        }
                        else if (dialogueData[dataIndex - 1].is_select[contextIndex] != "0" &&
                            dialogueData[dataIndex - 1].is_select[contextIndex] != "1" &&
                            !dialogueData[dataIndex - 1].is_select[contextIndex].Trim().Equals(""))
                        {
                            contextIndex = 0;
                            //2번째 선택지 창 열기
                            selectbox2(dialogueData[dataIndex - 1].is_select[contextIndex]);
                        }
                        else if (dialogueData[dataIndex - 1].is_dice[contextIndex] != "0" && 
                            !dialogueData[dataIndex - 1].is_dice[contextIndex].Trim().Equals(""))
                        {
                            contextIndex = 0;
                            //주사위 선택지 보여주기
                            checkbox(dialogueData[dataIndex - 1].is_dice[contextIndex]);
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
                        else if (dialogueData[dataIndex - 1].is_reset[contextIndex] == "4")
                        {
                            // 진엔딩을 위한 버튼
                            _realending.SetActive(true);
                        }
                        else if (_shopOn)
                        {
                            _shopOn = false;
                            _shopPanel.SetActive(true);
                        }
                        //세이렌 이벤트
                        //다음 화면 전환이 예정되어있다면 세이렌 무시
                        else if (GameManager.Instance.Siren_On && dialogueData[dataIndex - 1].stage_serialNum == 0)
                        {
                            GameManager.Instance.Siren_event();
                            GameManager.Instance.Siren_On = false;
                        }
                        //매니저 이벤트
                        else if (GameManager.Instance.Manager_On && GameManager.Instance.Manager_count != 0 
                            && GameManager.Instance.Manager_Start)
                        {
                            GameManager.Instance.Manager_Event();
                            GameManager.Instance.Manager_Start = false;
                        }
                        //축사 이벤트
                        else if (GameManager.Instance.Owner_count == 4)
                        {
                            GameManager.Instance.Owner_Event();
                            //GameManager.Instance.Owner_count = 0;
                        }
                        //쉐프 이벤트
                        else if (GameManager.Instance.Chef_count == 5)
                        {
                            GameManager.Instance.Chef_Event();
                        }
                        else
                        {
                            //스테이지 변경
                            if (dialogueData[dataIndex - 1].stage_serialNum != 0)
                            {
                                P_stat.Current_stage_num = dialogueData[dataIndex - 1].stage_serialNum;
                                StopCoroutine(typingText);
                                GameManager.Instance.Stage_start(P_stat.Current_stage_num);
                                //dialoguePanel.SetActive(false);//Dialogue UI 끄기
                            }
                            else
                            {
                                dialoguePanel.SetActive(false);//Dialogue UI 끄기
                                _subBack.SetActive(false);//sub_eventPanel 끄기
                                _logManager.Init_Log(); //로그 초기화
                            }
                        }

                        
                    }
                }
            }
            //코루틴이 끝나지 않았다면
            else
            {
                StopCoroutine(typingText);
                contextText.text = toType;
                currentTypeEnd = true;
                endTriangle.SetActive(true);
                //로그 메세지 추가
                _logManager.Add_Log(dialogueData[dataIndex].dialogue_Context);
            }

            is_clicked = false;
        }
    }
    IEnumerator TypingMotion()
    {
        contextText.text = null;
        for (int i = 0; i < toType.Length; i++)
        {
            contextText.text += toType[i];
            yield return new WaitForSecondsRealtime(_typeSpeed);
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

    private void selectbox2(string num)
    {
        dialogueData = CSVParsingD.GetDialogue("select" + num);

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

    private void EVbox()
    {
        GameManager.Instance.EV_ON(GameManager.Instance.EV_index);
    }

    private void checkbox(string num)
    {
        dialogueData = CSVParsingD.GetDialogue("check" + num);

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

    //서브 배경 변경을 시리얼넘버에 따라 실행
    public void Change_Subback(int image_num)
    {
        if (!_subBack.gameObject.activeSelf)
        {
            _subBack.gameObject.SetActive(true);
        }

        _subBackImge.texture = Sub_backgrounds[image_num];
    }

    public void Subback_off()
    {
        _subBack.gameObject.SetActive(false);
    }

    public void Dialouge_off()
    {
        dialoguePanel.gameObject.SetActive(false);
    }

    private void Character_off()
    {
        for (int i = 0; i < _characters.transform.childCount; i++)
        {
            if (_characters.transform.GetChild(i).gameObject.activeSelf)
            {
                _characters.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    // 상점 열기
    public void Shop()
    {
        _shopOn = true;
    }

    private void Increase_san(int i)
    {
        P_stat.C_san += i;

        if(P_stat.C_san > 90)
        {
            P_stat.C_san = 90;
        }
    }
}
