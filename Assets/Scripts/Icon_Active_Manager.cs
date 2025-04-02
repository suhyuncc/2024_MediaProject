using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon_Active_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _panels;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Off_Panel()
    {
        for(int i = 0; i < _panels.Length; i++)
        {
            if (_panels[i].activeSelf)
            {
                _panels[i].SetActive(false);
            }
        }
    }
}
