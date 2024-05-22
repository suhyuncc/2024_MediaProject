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
    [SerializeField]
    private bool isPrevDialogue = false;
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
    private GameObject _characters; //Image들이 저장되어있는 패널
    [SerializeField]
    private GameObject SelectBoxes; //Image들이 저장되어있는 패널
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

    public void ItIsPreviousDialogue(int num)
    {
        isPrevDialogue= true;
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
            if (_characters.transform.GetChild(dialogueData[0].speakerType).gameObject.activeSelf == false)
            {
                //Debug.Log(dialogueData[0].speakerType);
                previousImage = dialogueData[0].speakerType;
                _characters.transform.GetChild(previousImage).gameObject.SetActive(true);
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

                        if (_characters.transform.GetChild(dialogueData[dataIndex].speakerType).gameObject.activeSelf == false)
                        {
                            //Debug.Log(dialogueData[dataIndex].speakerType);
                            GameObject _prevImage = _characters.transform.GetChild(previousImage).gameObject;
                            Color _prevColor = _prevImage.GetComponent<RawImage>().color;
                            _prevColor.a = 0.3f;
                            _prevImage.GetComponent<RawImage>().color = _prevColor; //여기까지 알파값 바까주고---------
                            previousImage = dialogueData[dataIndex].speakerType;
                            _characters.transform.GetChild(previousImage).gameObject.SetActive(true);
                        }
                        else
                        {
                            GameObject _prevImage = _characters.transform.GetChild(previousImage).gameObject;
                            Color _prevColor = _prevImage.GetComponent<RawImage>().color;
                            _prevColor.a = 0.3f;
                            _prevImage.GetComponent<RawImage>().color = _prevColor;
                            previousImage = dialogueData[dataIndex].speakerType;
                            _prevImage = _characters.transform.GetChild(previousImage).gameObject;
                            _prevColor = _prevImage.GetComponent<RawImage>().color;
                            _prevColor.a = 1.0f;
                            _prevImage.GetComponent<RawImage>().color = _prevColor;
                        }

                        StartCoroutine(typingText); ;
                    }
                    //다음 출력할 문장 없으면
                    else
                    {
                        currentDialogue = false;

                        if (dialogueData[dataIndex - 1].is_select[contextIndex] == "1")
                        {
                            contextIndex = 0;
                            //선택지 보여주기
                            selectbox();
                        }
                        else
                        {
                            dialoguePanel.SetActive(false);//Dialogue UI 끄기
                        }

                        /*
                        if (isBothDialoguein1Time)
                        {
                            isBothDialoguein1Time = false;
                            GetEventName(eventNameIf2Event);

                        }
                        else if (!isPrevDialogue)
                        {
                            //gm.GetComponent<GameManager>().currentState = state.idle;
                            //gm.GetComponent<GameManager>().MapManager.GetComponent<MapManagement>().ReturnScene();
                            //dialoguePanel.SetActive(false);//Dialogue UI
                        }
                        else
                        {
                            if (isBothDialogue)
                            {
                                //gm.GetComponent<GameManager>().SetEventName(eventNameIf2Event);
                            }
                            //gm.GetComponent<GameManager>().StartBattle(isStageNumber); //StartBattle!
                        }*/
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

    //배경 변경을 시리얼넘버에 따라 실행
    public void Change_back(int image_num)
    {
        _background.texture = backgrounds[image_num];
    }
}
