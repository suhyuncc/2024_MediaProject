using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public enum Phase
{
    standby,
    main,
    fight,
    result,
    reroll
}

public class CardGameManager : MonoBehaviour
{
    public static CardGameManager Instance;

    [SerializeField]
    private Dice_panel _dice_Panel;

    // �ʱ� �迭 (0�� 8��, 1�� 6��, 2�� 6��)
    public List<int> numbers = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0,
                                            1, 1, 1, 1, 1, 1,
                                            2, 2, 2, 2, 2, 2 };

    // �÷��̾�� ������ ���� ����Ʈ
    public List<int> Dealer = new List<int>();
    public List<int> Player = new List<int>();
    public List<int> Deck = new List<int>();

    [SerializeField]
    private Card[] player_cards;
    [SerializeField]
    private Card[] dealer_cards;
    [SerializeField]
    private Card[] M_cards;
    [SerializeField]
    private GameObject[] card_behinds;
    [SerializeField]
    private GameObject[] H_hands;
    [SerializeField]
    private GameObject shuffle;
    [SerializeField]
    private GameObject[] p_lifes;
    [SerializeField]
    private GameObject[] d_lifes;
    [SerializeField]
    private GameObject p_energy;
    [SerializeField]
    private GameObject d_energy;
    [SerializeField]
    private Button dice_btn;
    [SerializeField]
    private Button fight_btn;
    [Header("Reroll")]
    [SerializeField]
    private GameObject M_panel;
    [SerializeField]
    private Button reroll_btn;
    [SerializeField]
    private Button cancel_btn;

    [Header("Audio")]
    [SerializeField]
    private AudioClip[] sounds;

    public Phase phase;

    private int player_guard;
    private int dealer_guard;
    private int player_charge;
    private int dealer_charge;
    private int player_life;
    private int dealer_life;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        

        player_life = 0;
        dealer_life = 0;

        phase = Phase.standby;
        Set();
    }

    // Update is called once per frame
    void Update()
    {
        if (phase == Phase.standby)
        {
            
            
        }

        if(phase == Phase.reroll)
        {
            for (int i = 0; i < M_cards.Length; i++)
            {
                if (M_cards[i].is_back)
                {
                    reroll_btn.gameObject.SetActive(true);
                    break;
                }
                else
                {
                    reroll_btn.gameObject.SetActive(false);
                }

            }
        }

        if (phase == Phase.main)
        {
            bool btn_check = true;

            for (int i = 0; i < player_cards.Length; i++)
            {
                if (player_cards[i].card_id == 3)
                {
                    btn_check = false;
                }
            }

            if (btn_check)
            {
                fight_btn.gameObject.SetActive(true);
            }
        }

        //�׽�Ʈ�� �ڵ�
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            dealer_cards[0].Flip();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            dealer_cards[1].Flip();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            dealer_cards[2].Flip();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            dealer_cards[3].Flip();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            dealer_cards[4].Flip();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            dealer_cards[5].Flip();
        }
    }

    private void Set()
    {
        StartCoroutine("card_set");
    }

    public void Reroll()
    {
        StartCoroutine("roll");
        phase = Phase.main;
    }

    public void Hand()
    {
        StartCoroutine("hand");
        phase = Phase.main;
    }

    //fight��ư���� ���
    public void Fight()
    {
        phase = Phase.fight;

        StartCoroutine("fight");

        for (int i = 0; i < player_cards.Length; i++)
        {
            player_cards[i].OFF_collider();
        }
    }

    IEnumerator card_set()
    {
        dice_btn.gameObject.SetActive(false);
        fight_btn.gameObject.SetActive(false);

        for (int i = 0; i < player_cards.Length; i++)
        {
            player_cards[i].is_empty = true;
            player_cards[i].Card_reset();
            player_cards[i].OFF_collider();
            if (player_cards[i].gameObject.activeSelf)
            {
                //player_cards[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < dealer_cards.Length; i++)
        {
            dealer_cards[i].Card_reset();
            if (dealer_cards[i].gameObject.activeSelf)
            {
                dealer_cards[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < M_cards.Length; i++)
        {
            M_cards[i].Card_reset();
            if (M_cards[i].gameObject.activeSelf)
            {
                M_cards[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < card_behinds.Length; i++)
        {
            card_behinds[i].SetActive(false);
        }

        Dealer = new List<int>();
        Player = new List<int>();
        Deck = new List<int>();

        // A�� ù ��° ���ڰ� 2�� �ƴ� ������ ����
        do
        {
            for (int i = numbers.Count - 1; i > 0; i--)
            {
                int swapIndex = Random.Range(0, numbers.Count);
                (numbers[i], numbers[swapIndex]) = (numbers[swapIndex], numbers[i]);
            }
        } while (numbers[0] == 2); // ù ��° ���ڰ� 2�̸� �ٽ� ����

        // ������ ���鼭 ���ڸ� ����
        bool isATurn = true;

        for (int i = 0; i < 12; i++) // �� 12���� ���ڸ� ������ (6����)
        {
            if (isATurn)
                Dealer.Add(numbers[i]);
            else
                Player.Add(numbers[i]);

            isATurn = !isATurn; // A �� B �� A �� B ���� ����
        }

        for (int i = 12; i < numbers.Count; i++)
        {
            Deck.Add(numbers[i]);
        }

        Debug.Log($"Dealer: [{string.Join(", ", Dealer)}]");
        Debug.Log($"Player: [{string.Join(", ", Player)}]");
        Debug.Log($"Deck: [{string.Join(", ", Deck)}]");

        p_card_set();

        m_card_set();

        d_card_set();

        shuffle.SetActive(true);

        yield return new WaitForSecondsRealtime(2.0f);

        for (int i = card_behinds.Length - 1; i >= 0; i--)
        {
            card_behinds[i].SetActive(true);
            card_behinds[i].GetComponent<Behind_setting>().Set();
            yield return new WaitForSecondsRealtime(0.3f);

            if(i == 3)
            {
                M_panel.SetActive(true);
            }
        }

        yield return new WaitForSecondsRealtime(1.5f);

        if (!cancel_btn.gameObject.activeSelf)
        {
            cancel_btn.gameObject.SetActive(true);
        }
        phase = Phase.reroll;

        StopCoroutine("card_set");
    }

    private void p_card_set()
    {
        for(int i = 0; i < player_cards.Length; i++)
        {
            player_cards[i].card_id = 3;
        }
    }

    private void d_card_set()
    {
        for (int i = 0; i < dealer_cards.Length; i++)
        {
            dealer_cards[i].card_id = Dealer[i];
        }
    }

    private void m_card_set()
    {
        for (int i = 0; i < M_cards.Length; i++)
        {
            M_cards[i].card_id = Player[i];
        }
    }

    IEnumerator roll()
    {
        for (int i = 0; i < M_cards.Length; i++)
        {
            if (M_cards[i].is_back)
            {
                M_cards[i].gameObject.SetActive(false);
            }

        }

        //�ָ��� �� ��� ����
        card_reroll();

        for (int i = 0; i < M_cards.Length; i++)
        {
            if (M_cards[i].is_back)
            {
                card_behinds[i].SetActive(true);
                card_behinds[i].GetComponent<Behind_setting>().Set();
                yield return new WaitForSecondsRealtime(0.3f);
            }

        }

        yield return new WaitForSecondsRealtime(2.6f);

        

        Hand();

        StopCoroutine("roll");
    }

    private void card_reroll()
    {
        for (int i = 0; i < M_cards.Length; i++)
        {
            if (M_cards[i].is_back)
            {
                Deck.Add(M_cards[i].card_id);
                M_cards[i].card_id = Deck[0];
                Player[i] = M_cards[i].card_id;
                Deck.RemoveAt(0);
            }
        }

        Debug.Log($"�ָ��� �� Player: [{string.Join(", ", Player)}]");
        Debug.Log($"�ָ��� �� Deck: [{string.Join(", ", Deck)}]");
    }

    IEnumerator hand()
    {
        M_panel.SetActive(false);

        for (int i = 0; i < M_cards.Length; i++)
        {
            M_cards[i].Hand();
        }

        yield return new WaitForSecondsRealtime(1.0f);

        for (int i = 0; i < H_hands.Length; i++)
        {
            H_hands[i].GetComponent<Card>().Change_Image(M_cards[i].card_id);
            H_hands[i].SetActive(true);
        }

        for (int i = 0; i < player_cards.Length; i++)
        {
            //player_cards[i].gameObject.GetComponent<Image>().color = new UnityEngine.Color(0,1,1);
            player_cards[i].gameObject.SetActive(true);
            player_cards[i].ON_collider();
        }

        //�ֻ��� ��ư Ȱ��ȭ
        dice_btn.gameObject.SetActive(true);

        //phase = Phase.main;

        StopCoroutine("hand");
    }

    public void Roll_the_dice()
    {
        // ������ ����
        _dice_Panel.Set_icon("casino");
        // ȭ����ȯ�� ���ÿ� �ֻ��� ����
        GameManager.Instance.Dice_On("1d6");
    }

    public void Dealer_Flip(int num)
    {
        dealer_cards[num-1].Flip();
    }

    public void Change_PList(int index1, int index2)
    {
        int temp;
        temp = Player[index2];
        Player[index2] = Player[index1];
        Player[index1] = Player[index2];

        Debug.Log($"Player: [{string.Join(", ", Player)}]");
    }

    private void Player_action(int num)
    {
        switch (num)
        {
            //���
            case 0:
                player_guard += 1;
                Debug.Log("�÷��̾��� ���!!");
                GameManager.Instance.Change_SFX(sounds[2]);
                break;
            //�� ������
            case 1:
                player_charge += 1;
                Debug.Log("�÷��̾��� �� ������!!");
                GameManager.Instance.Change_SFX(sounds[1]);
                p_energy.SetActive(true);
                break;
            //����
            case 2:
                //�����
                if(player_charge >= 3)
                {
                    player_charge = 0;
                    dealer_guard -= 20;
                    Debug.Log("�÷��̾��� ��!!!��!!��!!!");
                }
                else if (player_charge > 0)
                {
                    player_charge -= 1;
                    dealer_guard -= 1;
                    Debug.Log("�÷��̾��� ����!!");
                    GameManager.Instance.Change_SFX(sounds[3]);
                    p_energy.SetActive(false);
                }
                else
                {
                    Debug.Log("�÷��̾�� �Ⱑ �����Ͽ� ������ �� ������!!");
                    GameManager.Instance.Change_SFX(sounds[0]);
                }
                break;
        }
    }

    private void Dealer_action(int num)
    {
        switch (num)
        {
            //���
            case 0:
                dealer_guard += 1;
                Debug.Log("������ ���!!");
                GameManager.Instance.Change_SFX(sounds[2]);
                break;
            //�� ������
            case 1:
                dealer_charge += 1;
                Debug.Log("������ �� ������!!");
                GameManager.Instance.Change_SFX(sounds[1]);
                d_energy.SetActive(true);
                break;
            //����
            case 2:
                //�����
                if (dealer_charge >= 3)
                {
                    dealer_charge = 0;
                    player_guard -= 20;
                    Debug.Log("������ ��!!!��!!��!!!");
                }
                else if (dealer_charge > 0)
                {
                    dealer_charge -= 1;
                    player_guard -= 1;
                    Debug.Log("������ ����!!");
                    GameManager.Instance.Change_SFX(sounds[3]);
                    d_energy.SetActive(false);
                }
                else
                {
                    Debug.Log("������ �Ⱑ �����Ͽ� ������ �� ������!!");
                    GameManager.Instance.Change_SFX(sounds[0]);
                }
                break;
        }
    }

    IEnumerator fight()
    {
        int index = 0;

        //���� ���� ����
        while((index < 6) && (player_guard >= 0) && (dealer_guard >= 0))
        {
            if (dealer_cards[index].is_back)
            {
                dealer_cards[index].Flip();
            }
            

            player_guard = 0;

            dealer_guard = 0;

            yield return new WaitForSecondsRealtime(0.8f);

            dealer_cards[index].Battle(0.5f);

            yield return new WaitForSecondsRealtime(0.5f);

            Dealer_action(dealer_cards[index].card_id);

            yield return new WaitForSecondsRealtime(1.8f);

            player_cards[index].Battle(0.5f);

            yield return new WaitForSecondsRealtime(0.5f);

            Player_action(player_cards[index].card_id);

            yield return new WaitForSecondsRealtime(1.8f);

            index++;
        }

        show_result();

        yield return new WaitForSecondsRealtime(1.5f);

        Debug.Log("�����!!");
        p_energy.SetActive(false);
        d_energy.SetActive(false);
        Set();

        StopCoroutine("fight");
    }

    private void show_result()
    {
        if(dealer_guard < 0)
        {
            Debug.Log("�÷��̾� �¸�");
            player_life += 1;
            p_lifes[player_life - 1].SetActive(true);
        }
        else if (player_guard < 0)
        {
            Debug.Log("���� �¸�");
            dealer_life += 1;
            d_lifes[dealer_life - 1].SetActive(true);
        }
        else
        {
            Debug.Log("���º�");
        }

        player_guard = 0;
        player_charge = 0;

        dealer_guard = 0;
        dealer_charge = 0;
    }
}
