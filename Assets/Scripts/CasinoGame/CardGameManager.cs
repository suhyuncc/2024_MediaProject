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

    // 초기 배열 (0이 8개, 1이 6개, 2가 6개)
    public List<int> numbers = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0,
                                            1, 1, 1, 1, 1, 1,
                                            2, 2, 2, 2, 2, 2 };

    // 플레이어와 딜러의 숫자 리스트
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

        //테스트용 코드
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

    //fight버튼으로 사용
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

        // A의 첫 번째 숫자가 2가 아닐 때까지 섞기
        do
        {
            for (int i = numbers.Count - 1; i > 0; i--)
            {
                int swapIndex = Random.Range(0, numbers.Count);
                (numbers[i], numbers[swapIndex]) = (numbers[swapIndex], numbers[i]);
            }
        } while (numbers[0] == 2); // 첫 번째 숫자가 2이면 다시 섞음

        // 번갈아 가면서 숫자를 배정
        bool isATurn = true;

        for (int i = 0; i < 12; i++) // 총 12개의 숫자를 나눠줌 (6개씩)
        {
            if (isATurn)
                Dealer.Add(numbers[i]);
            else
                Player.Add(numbers[i]);

            isATurn = !isATurn; // A → B → A → B 순서 유지
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

        //멀리건 후 결과 적용
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

        Debug.Log($"멀리건 후 Player: [{string.Join(", ", Player)}]");
        Debug.Log($"멀리건 후 Deck: [{string.Join(", ", Deck)}]");
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

        //주사위 버튼 활성화
        dice_btn.gameObject.SetActive(true);

        //phase = Phase.main;

        StopCoroutine("hand");
    }

    public void Roll_the_dice()
    {
        // 아이콘 세팅
        _dice_Panel.Set_icon("casino");
        // 화면전환과 동시에 주사위 세팅
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
            //방어
            case 0:
                player_guard += 1;
                Debug.Log("플레이어의 방어!!");
                GameManager.Instance.Change_SFX(sounds[2]);
                break;
            //기 모으기
            case 1:
                player_charge += 1;
                Debug.Log("플레이어의 기 모으기!!");
                GameManager.Instance.Change_SFX(sounds[1]);
                p_energy.SetActive(true);
                break;
            //공격
            case 2:
                //원기옥
                if(player_charge >= 3)
                {
                    player_charge = 0;
                    dealer_guard -= 20;
                    Debug.Log("플레이어의 원!!!기!!옥!!!");
                }
                else if (player_charge > 0)
                {
                    player_charge -= 1;
                    dealer_guard -= 1;
                    Debug.Log("플레이어의 공격!!");
                    GameManager.Instance.Change_SFX(sounds[3]);
                    p_energy.SetActive(false);
                }
                else
                {
                    Debug.Log("플레이어는 기가 부족하여 공격할 수 없었다!!");
                    GameManager.Instance.Change_SFX(sounds[0]);
                }
                break;
        }
    }

    private void Dealer_action(int num)
    {
        switch (num)
        {
            //방어
            case 0:
                dealer_guard += 1;
                Debug.Log("딜러의 방어!!");
                GameManager.Instance.Change_SFX(sounds[2]);
                break;
            //기 모으기
            case 1:
                dealer_charge += 1;
                Debug.Log("딜러의 기 모으기!!");
                GameManager.Instance.Change_SFX(sounds[1]);
                d_energy.SetActive(true);
                break;
            //공격
            case 2:
                //원기옥
                if (dealer_charge >= 3)
                {
                    dealer_charge = 0;
                    player_guard -= 20;
                    Debug.Log("딜러의 원!!!기!!옥!!!");
                }
                else if (dealer_charge > 0)
                {
                    dealer_charge -= 1;
                    player_guard -= 1;
                    Debug.Log("딜러의 공격!!");
                    GameManager.Instance.Change_SFX(sounds[3]);
                    d_energy.SetActive(false);
                }
                else
                {
                    Debug.Log("딜러는 기가 부족하여 공격할 수 없었다!!");
                    GameManager.Instance.Change_SFX(sounds[0]);
                }
                break;
        }
    }

    IEnumerator fight()
    {
        int index = 0;

        //전투 종료 조건
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

        Debug.Log("재시작!!");
        p_energy.SetActive(false);
        d_energy.SetActive(false);
        Set();

        StopCoroutine("fight");
    }

    private void show_result()
    {
        if(dealer_guard < 0)
        {
            Debug.Log("플레이어 승리");
            player_life += 1;
            p_lifes[player_life - 1].SetActive(true);
        }
        else if (player_guard < 0)
        {
            Debug.Log("딜러 승리");
            dealer_life += 1;
            d_lifes[dealer_life - 1].SetActive(true);
        }
        else
        {
            Debug.Log("무승부");
        }

        player_guard = 0;
        player_charge = 0;

        dealer_guard = 0;
        dealer_charge = 0;
    }
}
