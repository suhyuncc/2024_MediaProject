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
    private Player_stat P_stat;     //�÷��̾� ����
    public string eventName; // eventName ���� ���� ��
    private DialogueData[] dialogueData; //��ȭ ������
    public bool isDialogue = false; // recieve event
    private bool currentDialogue = false; // is current dialogue working


    [Header("UI")]
    [SerializeField]
    private float _typeSpeed;
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
    private GameObject EVBoxes; //�ֻ��� �̺�Ʈ�� ������ �ڽ���
    [SerializeField]
    private GameObject _softReset; //deadend ��ư
    [SerializeField]
    private GameObject _hardReset; //ó������ ���� ��ư
    [SerializeField]
    private GameObject _ending; //���� ��ư
    [SerializeField]
    private GameObject _realending; //������ ��ư
    [SerializeField]
    private Text contextText; //��ȭ
    [SerializeField]
    private Toggle _normal;
    [SerializeField]
    private Toggle _fast;
    public bool is_clicked = false; // �ؽ�Ʈ â�� Ŭ���Ǿ����� üũ

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

    public void GetEventName(string _eventName) //eventName ���ɹ޴� �Լ�
    {
        eventName = _eventName;
        isDialogue = true;
        _subBack.SetActive(false);//sub_eventPanel ����
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

        //EventName�� �޾ƿ��� ��
        if (isDialogue)
        {
            dataIndex = 0;
            contextIndex = 0;
            currentTypeEnd = false;
            is_clicked = false;
            isDialogue = false;
            currentDialogue = true;

            _logManager.Init_Log(); //�α�â �ʱ�ȭ

            dialogueData = CSVParsingD.GetDialogue(eventName);  // ȭ�� Ÿ��, ȭ�� �̸�, ��縦 ���ϴ� �̺�Ʈ�� �ִ� ������ ������
            previousImage = dialogueData[0].speakerType;        // ĳ���� �ø��� ��ȣ
            //nameText.text = dialogueData[0].name;             // ĳ���� �̸�(�ʿ�� ���)

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
            
            //Ÿ���� �ڷ�ƾ
            typingText = TypingMotion();

            //Ÿ���� �� ����
            toType = dialogueData[dataIndex].dialogue_Context[contextIndex];
            
            StartCoroutine(typingText);
        }


        //���̾˷α� �������̰� Ŭ���� �Ǹ�
        if(currentDialogue && (is_clicked || Input.GetKeyDown(KeyCode.Space)))
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
                    //typingText = TypingMotion();
                    endTriangle.SetActive(false);
                    toType = dialogueData[dataIndex].dialogue_Context[contextIndex];
                    //StartCoroutine(typingText);
                }
                //���� ������� ������ �� ��� �Ǿ�������
                else if (contextIndex >= dialogueData[dataIndex].dialogue_Context.Length - 1)
                {
                    //�ø��� �ѹ��� 0�� �ƴ� �� ��� �ٲٱ�
                    if (dialogueData[dataIndex].image_serialNum != 0)
                    {
                        Change_back(dialogueData[dataIndex].image_serialNum);
                    }

                    //������ ��� ����ȭ�� ����
                    if (dialogueData[dataIndex].sub_serialNum > 0)
                    {
                        Change_Subback(dialogueData[dataIndex].sub_serialNum - 1);
                    }
                    else if (dialogueData[dataIndex].sub_serialNum < 0)
                    {
                        Subback_off();
                    }

                    //������ �ƴҶ�
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

                    //������ ȹ��
                    if (dialogueData[dataIndex - 1].item_serialNum != 0)
                    {
                        if (dialogueData[dataIndex - 1].item_serialNum < 0)
                        {
                            //Ĩ ���� �߰�
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
                            //������ ����Ʈ�� ������ �߰�
                            GameManager.Instance.Add_Item(dialogueData[dataIndex - 1].item_serialNum - 1);
                        }
                        
                    }



                    //���� ����� ���� ������
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
                            //�ɸ��� �̹��� ����
                            Character_off();
                        }
                        else if(dialogueData[dataIndex].speakerType > 0)
                        {
                            GameObject _prevImage;
                            Color _prevColor;

                            //������ �� �ٿ�
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
                    //���� ����� ���� ������
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
                            //���������� ������ â ����
                            EVbox();
                        }
                        else if (dialogueData[dataIndex - 1].is_select[contextIndex] == "-2")
                        {
                            contextIndex = 0;
                            //Ű�е� â ����
                            _keypadPanel.SetActive(true);
                        }
                        else if (dialogueData[dataIndex - 1].is_select[contextIndex] == "1")
                        {
                            contextIndex = 0;
                            //������ �����ֱ�
                            selectbox();
                        }
                        else if (dialogueData[dataIndex - 1].is_select[contextIndex] != "0" &&
                            dialogueData[dataIndex - 1].is_select[contextIndex] != "1" &&
                            !dialogueData[dataIndex - 1].is_select[contextIndex].Trim().Equals(""))
                        {
                            contextIndex = 0;
                            //2��° ������ â ����
                            selectbox2(dialogueData[dataIndex - 1].is_select[contextIndex]);
                        }
                        else if (dialogueData[dataIndex - 1].is_dice[contextIndex] != "0" && 
                            !dialogueData[dataIndex - 1].is_dice[contextIndex].Trim().Equals(""))
                        {
                            contextIndex = 0;
                            //�ֻ��� ������ �����ֱ�
                            checkbox(dialogueData[dataIndex - 1].is_dice[contextIndex]);
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
                        else if (dialogueData[dataIndex - 1].is_reset[contextIndex] == "4")
                        {
                            // �������� ���� ��ư
                            _realending.SetActive(true);
                        }
                        else if (_shopOn)
                        {
                            _shopOn = false;
                            _shopPanel.SetActive(true);
                        }
                        //���̷� �̺�Ʈ
                        //���� ȭ�� ��ȯ�� �����Ǿ��ִٸ� ���̷� ����
                        else if (GameManager.Instance.Siren_On && dialogueData[dataIndex - 1].stage_serialNum == 0)
                        {
                            GameManager.Instance.Siren_event();
                            GameManager.Instance.Siren_On = false;
                        }
                        //�Ŵ��� �̺�Ʈ
                        else if (GameManager.Instance.Manager_On && GameManager.Instance.Manager_count != 0 
                            && GameManager.Instance.Manager_Start)
                        {
                            GameManager.Instance.Manager_Event();
                            GameManager.Instance.Manager_Start = false;
                        }
                        //��� �̺�Ʈ
                        else if (GameManager.Instance.Owner_count == 4)
                        {
                            GameManager.Instance.Owner_Event();
                            //GameManager.Instance.Owner_count = 0;
                        }
                        //���� �̺�Ʈ
                        else if (GameManager.Instance.Chef_count == 5)
                        {
                            GameManager.Instance.Chef_Event();
                        }
                        else
                        {
                            //�������� ����
                            if (dialogueData[dataIndex - 1].stage_serialNum != 0)
                            {
                                P_stat.Current_stage_num = dialogueData[dataIndex - 1].stage_serialNum;
                                StopCoroutine(typingText);
                                GameManager.Instance.Stage_start(P_stat.Current_stage_num);
                                //dialoguePanel.SetActive(false);//Dialogue UI ����
                            }
                            else
                            {
                                dialoguePanel.SetActive(false);//Dialogue UI ����
                                _subBack.SetActive(false);//sub_eventPanel ����
                                _logManager.Init_Log(); //�α� �ʱ�ȭ
                            }
                        }

                        
                    }
                }
            }
            //�ڷ�ƾ�� ������ �ʾҴٸ�
            else
            {
                StopCoroutine(typingText);
                contextText.text = toType;
                currentTypeEnd = true;
                endTriangle.SetActive(true);
                //�α� �޼��� �߰�
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
            //�ڽ�Ű��
            SelectBoxes.transform.GetChild(i).gameObject.SetActive(true);

            //���� �ڰ�
            SelectBoxes.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<Text>().text
                = dialogueData[0].Secletion_Context[i];

            //���� ��� �˷��ְ�
            SelectBoxes.transform.GetChild(i).GetComponent<SelectBox>().SetEventName(dialogueData[0].Next_event[i]);

        }
    }

    private void selectbox2(string num)
    {
        dialogueData = CSVParsingD.GetDialogue("select" + num);

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

    //���� ��� ������ �ø���ѹ��� ���� ����
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

    // ���� ����
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
