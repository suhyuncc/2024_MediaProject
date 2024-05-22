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

    public string eventName; // eventName ���� ���� ��
    public DialogueData[] dialogueData; //��ȭ ������
    [SerializeField]
    private bool isPrevDialogue = false;
    public bool isDialogue = false; // recieve event
    private bool currentDialogue = false; // is current dialogue working

    [Header("UI")]
    [SerializeField]
    private GameObject endTriangle; //������ �����Ÿ��� �ﰢ��
    [SerializeField]
    private GameObject dialoguePanel; //��ȭpanel
    /// <summary>
    /// <para>
    /// 0��: BarMaster
    /// 1��: BarVisitor
    /// </para>
    /// <para>
    /// 2��: HotelEmployee
    /// 3��: PoolVisitor
    /// </para>
    /// <para>
    /// 4��: HotelOwner
    /// 5��: NPC1
    /// </para>
    /// <para>
    /// 6��: StoreMaster
    /// 7��: SweetroomVisitor
    /// </para>
    /// </summary>
    [SerializeField]
    private GameObject _characters; //Image���� ����Ǿ��ִ� �г�
    [SerializeField]
    private GameObject SelectBoxes; //Image���� ����Ǿ��ִ� �г�
    [SerializeField]
    private Text contextText; //��ȭ

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


    public void GetEventName(string _eventName) //eventName ���ɹ޴� �Լ�
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
            dialogueData = CSVParsingD.GetDialogue(eventName); // ȭ�� Ÿ��, ȭ�� �̸�, ��縦 ���ϴ� �̺�Ʈ�� �ִ� ������ ������
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
            //Ÿ���� �ڷ�ƾ
            typingText = TypingMotion();
            //Ÿ���� �� ����
            toType = dialogueData[dataIndex].dialogue_Context[contextIndex];
            StartCoroutine(typingText);
        }//EventName�� �޾ƿ��� ��

        //���̾˷α� �������̰� Ŭ���� �Ǹ�
        if(currentDialogue && Input.GetMouseButtonDown(0))
        {
            //�ڷ�ƾ�� ��������
            if (currentTypeEnd)
            {
                //���� ������� ������ �� ��� �ȵǾ�������
                if (contextIndex < dialogueData[dataIndex].dialogue_Context.Length - 1)
                {
                    //������ �ϴ� ���
                    contextIndex++;
                    currentTypeEnd = false;
                    typingText = TypingMotion();
                    endTriangle.SetActive(false);
                    toType = dialogueData[dataIndex].dialogue_Context[contextIndex];
                    StartCoroutine(typingText);
                }
                //���� ������� ������ �� ��� �Ǿ�������
                else if (contextIndex >= dialogueData[dataIndex].dialogue_Context.Length - 1)
                {
                    //�ø��� �ѹ��� 0�� �ƴ� �� ��� �ٲٱ�
                    if (dialogueData[dataIndex].image_serialNum != 0)
                    {
                        Change_back(dialogueData[dataIndex].image_serialNum);
                    }

                    dataIndex++;                    

                    //���� ����� ���� ������
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
                            _prevImage.GetComponent<RawImage>().color = _prevColor; //������� ���İ� �ٱ��ְ�---------
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
                    //���� ����� ���� ������
                    else
                    {
                        currentDialogue = false;

                        if (dialogueData[dataIndex - 1].is_select[contextIndex] == "1")
                        {
                            contextIndex = 0;
                            //������ �����ֱ�
                            selectbox();
                        }
                        else
                        {
                            dialoguePanel.SetActive(false);//Dialogue UI ����
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
            //�ڽ�Ű��
            SelectBoxes.transform.GetChild(i).gameObject.SetActive(true);

            //���� �ڰ�
            SelectBoxes.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<Text>().text
                = dialogueData[0].Secletion_Context[i];

            //���� ��� �˷��ְ�
            SelectBoxes.transform.GetChild(i).GetComponent<SelectBox>().SetEventName(dialogueData[0].Next_event[i]);

        }
    }

    //��� ������ �ø���ѹ��� ���� ����
    public void Change_back(int image_num)
    {
        _background.texture = backgrounds[image_num];
    }
}
