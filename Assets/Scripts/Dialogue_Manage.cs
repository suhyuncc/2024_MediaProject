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
    private GameObject _characters; //ĳ���� Image���� ����Ǿ��ִ� �г�
    [SerializeField]
    private GameObject SelectBoxes; //������ �ڽ���
    [SerializeField]
    private GameObject CheckBoxes; //�ֻ��� �̺�Ʈ�� ������ �ڽ���
    [SerializeField]
    private GameObject _softReset; //deadend ��ư
    [SerializeField]
    private GameObject _hardReset; //ó������ ���� ��ư
    [SerializeField]
    private GameObject _ending; //ó������ ���� ��ư
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
            previousImage = dialogueData[0].speakerType;    // �ɸ��� �ø��� ��ȣ
            if (_characters.transform.GetChild(dialogueData[0].speakerType).gameObject.activeSelf == false &&
                previousImage != 0)
            {
                
                _characters.transform.GetChild(previousImage - 1).gameObject.SetActive(true);
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

                        if(dialogueData[dataIndex].speakerType != 0)
                        {
                            if (_characters.transform.GetChild(dialogueData[dataIndex].speakerType).gameObject.activeSelf == false)
                            {
                                /*
                                //Debug.Log(dialogueData[dataIndex].speakerType);
                                GameObject _prevImage = _characters.transform.GetChild(previousImage - 1).gameObject;
                                Color _prevColor = _prevImage.GetComponent<RawImage>().color;
                                _prevColor.a = 0.3f;
                                _prevImage.GetComponent<RawImage>().color = _prevColor; //������� ���İ� �ٱ��ְ�---------*/
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
                    //���� ����� ���� ������
                    else
                    {
                        currentDialogue = false;

                        if(dialogueData[dataIndex - 1].item_serialNum != 0)
                        {
                            //������ ����Ʈ�� ������ �߰�
                            GameManager.Instance.Add_Item(dialogueData[dataIndex - 1].item_serialNum - 1);
                        }

                        if (dialogueData[dataIndex - 1].is_select[contextIndex] == "1")
                        {
                            contextIndex = 0;
                            //������ �����ֱ�
                            selectbox();
                        }
                        else if (dialogueData[dataIndex - 1].is_select[contextIndex] == "2")
                        {
                            contextIndex = 0;
                            //2��° ������ â ����
                            selectbox2();
                        }
                        else if (dialogueData[dataIndex - 1].is_dice[contextIndex] == "1")
                        {
                            contextIndex = 0;
                            //�ֻ��� ������ �����ֱ�
                            checkbox();
                        }
                        else if (dialogueData[dataIndex - 1].is_dice[contextIndex] == "2")
                        {
                            contextIndex = 0;
                            //2���� �ֻ��� ������ �ֻ��� ������ �����ֱ�
                            checkbox2();
                        }
                        else if (dialogueData[dataIndex - 1].is_reset[contextIndex] == "1")
                        {
                            // deadend�� ���� ��ư
                            _softReset.SetActive(true);
                        }
                        else if (dialogueData[dataIndex - 1].is_reset[contextIndex] == "2")
                        {
                            // hardreset�� ���� ��ư
                            _hardReset.SetActive(true);
                        }
                        else if (dialogueData[dataIndex - 1].is_reset[contextIndex] == "3")
                        {
                            // �Ϲ� ������ ���� ��ư
                            _ending.SetActive(true);
                        }
                        //���̷� �̺�Ʈ
                        //���� ȭ�� ��ȯ�� �����Ǿ��ִٸ� ���̷� ����
                        else if (GameManager.Instance.Siren_On && dialogueData[dataIndex - 1].stage_serialNum == 0)
                        {
                            GameManager.Instance.Siren_event();
                            GameManager.Instance.Siren_On = false;
                        }
                        //��� �̺�Ʈ
                        else if (GameManager.Instance.Owner_count == 3)
                        {
                            GameManager.Instance.Owner_Event();
                            GameManager.Instance.Owner_count = 0;
                        }
                        else
                        {
                            //�������� ����
                            if (dialogueData[dataIndex - 1].stage_serialNum != 0)
                            {
                                GameManager.Instance.Stage_start(dialogueData[dataIndex - 1].stage_serialNum);
                                //dialoguePanel.SetActive(false);//Dialogue UI ����
                            }
                            else
                            {
                                dialoguePanel.SetActive(false);//Dialogue UI ����
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
            //�ڽ�Ű��
            SelectBoxes.transform.GetChild(i).gameObject.SetActive(true);

            //���� �ڰ�
            SelectBoxes.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<Text>().text
                = dialogueData[0].Secletion_Context[i];

            //���� ��� �˷��ְ�
            SelectBoxes.transform.GetChild(i).GetComponent<SelectBox>().SetEventName(dialogueData[0].Next_event[i]);

        }
    }

    private void selectbox2()
    {
        dialogueData = CSVParsingD.GetDialogue("select2");

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

    private void checkbox()
    {
        dialogueData = CSVParsingD.GetDialogue("check");

        for (int i = 0; i < dialogueData[0].Dice_Context.Length; i++)
        {
            if (dialogueData[0].Dice_Context[i] != "")
            {
                //�ڽ�Ű��
                CheckBoxes.transform.GetChild(i).gameObject.SetActive(true);

                //���� �ڰ�
                CheckBoxes.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<Text>().text
                    = dialogueData[0].Dice_Context[i];

                //���� �ֻ��� ��� �˷��ְ�
                CheckBoxes.transform.GetChild(i).GetComponent<CheckBox>().SetDiceName(dialogueData[0].Dice_name[i]);

                //��� ���� �˷��ְ�
                CheckBoxes.transform.GetChild(i).GetComponent<CheckBox>().SetCheckStat(dialogueData[0].Dice_stat[i]);

                //�ֻ����� ���� �� ��� �˷��ְ�
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
                //�ڽ�Ű��
                CheckBoxes.transform.GetChild(i).gameObject.SetActive(true);

                //���� �ڰ�
                CheckBoxes.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<Text>().text
                    = dialogueData[0].Dice_Context[i];

                //���� �ֻ��� ��� �˷��ְ�
                CheckBoxes.transform.GetChild(i).GetComponent<CheckBox>().SetDiceName(dialogueData[0].Dice_name[i]);

                //��� ���� �˷��ְ�
                CheckBoxes.transform.GetChild(i).GetComponent<CheckBox>().SetCheckStat(dialogueData[0].Dice_stat[i]);

                //�ֻ����� ���� �� ��� �˷��ְ�
                CheckBoxes.transform.GetChild(i).GetComponent<CheckBox>().SetEventName(dialogueData[0].Dice_Next_event);
            }
        }

    }

    //��� ������ �ø���ѹ��� ���� ����
    public void Change_back(int image_num)
    {
        _background.texture = backgrounds[image_num];
    }

    public void Dialouge_off()
    {
        dialoguePanel.gameObject.SetActive(false);
    }
}
