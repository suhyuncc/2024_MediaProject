using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager instance;

    [SerializeField]
    private Player_stat P_stat;     //플레이어 정보
    [SerializeField]
    private string _scenename;
    [SerializeField]
    private GameObject[] _panels;
    [SerializeField]
    private GameObject _endPanel;
    [SerializeField]
    private GameObject _winBtn;
    [SerializeField]
    private GameObject _loseBtn;
    [SerializeField]
    private Text _timertxt;
    [SerializeField]
    private RawImage _background;
    [SerializeField]
    private RawImage _light;
    [SerializeField]
    private Texture[] _images;
    [SerializeField]
    private Texture _lightImage;
    [SerializeField]
    private float _time;
    public float _inittime;
    private int[] _randomArray = new int[4] { 0, 90, 180, 270 };
    private bool _endCheck;
    private int _imageindex;
    private bool _startCheck;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        _startCheck = false;
        _endCheck = false;

        _imageindex = 0;

        _background.texture = _images[_imageindex];

        _inittime = Check_time();
        _time = _inittime;

        for (int i = 0; i < _panels.Length; i++)
        {
            int ran = Random.Range(0, 4);
            _panels[i].transform.Rotate(0, 0, _randomArray[ran]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_startCheck)
        {
            _time -= Time.deltaTime;
        }
        
        _timertxt.text = $"{_time.ToString("0.00")}";

        if(_time < _inittime / 2.0f && _imageindex == 0)
        {
            _imageindex++;
            StartCoroutine(Blink());
        }

        if (_time < _inittime / 4.0f && _imageindex == 1)
        {
            _imageindex++;
            StartCoroutine(Blink());
        }

        // 성공시
        if (_endCheck)
        {
            Time.timeScale = 0;
            _endPanel.SetActive(true);
            _winBtn.SetActive(true);
            _light.texture = _lightImage;
        }

        // 시간초과시
        if (_time < 0.0f)
        {
            Time.timeScale = 0;
            _endPanel.SetActive(true);
            _loseBtn.SetActive(true);
        }
    }

    public void Start_Checking()
    {
        _startCheck = true;
    }

    public void End_Checking()
    {
        _endCheck = true;

        for (int i = 0; i < _panels.Length; i++)
        {
            if((int)_panels[i].transform.rotation.eulerAngles.z != 0)
            {
                _endCheck = false;
            }
            
        }
    }

    public void Success_btn()
    {
        //성공시 실행하는 스테이지
        P_stat.Current_stage_num = -4;
        StopAllCoroutines();
        Time.timeScale = 1;
        SceneManager.LoadScene(_scenename);
    }

    public void Fail_btn()
    {
        //실패시 실행하는 스테이지
        P_stat.Current_stage_num = -5;
        StopAllCoroutines();
        Time.timeScale = 1;
        SceneManager.LoadScene(_scenename);
        
    }

    IEnumerator Blink()
    {
        float r = _background.color.r;
        float g = _background.color.g;
        float b = _background.color.b;
        float a = _background.color.a;

        int count = 0;
        int speed = 3;

        while(count < 4)
        {
            if(r >= 1 && g >= 1 && b >= 1)
            {
                while(r > 0.0f && g > 0.0f && b > 0.0f)
                {
                    r -= Time.deltaTime * speed;
                    g -= Time.deltaTime * speed;
                    b -= Time.deltaTime * speed;

                    _background.color = new Color(r, g, b, a);
                    yield return null;
                }
            }
            else if (r <= 0 && g <= 0 && b <= 0)
            {
                if(count == 3)
                {
                    _background.texture = _images[_imageindex];
                }

                while (r < 1.0f && g < 1.0f && b < 1.0f)
                {
                    r += Time.deltaTime * speed;
                    g += Time.deltaTime * speed;
                    b += Time.deltaTime * speed;

                    _background.color = new Color(r, g, b, a);
                    yield return null;
                }
            }
            count++;
        }

    }

    private float Check_time()
    {
        float time = 30.0f;

        //아이템 판단
        for (int i = 0; i < P_stat.Item_list.Length; i++)
        {
            //손전등
            if (P_stat.Item_list[i] == 7)
            {
                time = 60.0f;
                return time;
            }
            //라이터
            else if (P_stat.Item_list[i] == 8)
            {
                time = 45.0f;
                return time;
            }
        }

        return time;
    }
}
