using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Keypad_Head : MonoBehaviour
{
    public List<int> numbers = new List<int>() { 10, 10, 10, 10, 10, 10};
    [SerializeField]
    private Texture[] _textures;
    [SerializeField]
    private RawImage[] _images;
    [SerializeField]
    private GameObject[] _objects;

    private void OnEnable()
    {
        numbers = new List<int>() { 10, 10, 10, 10, 10, 10 };
    }

    private void Update()
    {
        int verdict = 1000 * numbers[3] + 100 * numbers[2] + 10 * numbers[1] + numbers[0];
        //히든엔딩을 위한 번호
        if(verdict == 1410)
        {
            numbers = new List<int>() { 10, 10, 10, 10, 10, 10 };
            GameManager.Instance.Stage_start(32);
        }
    }

    public void Insert(int i)
    {
        numbers.Insert(0, i);
    }

    public void Back()
    {
        numbers.RemoveAt(0);
    }

    public void Reset_List()
    {
        numbers = new List<int>() { 10, 10, 10, 10, 10, 10 };
    }

    public void SetNumber()
    {
        for (int i = 0; i < _objects.Length; i++)
        {
            if (numbers[i] < 10)
            {
                _images[i].texture = _textures[numbers[i]];
                _objects[i].SetActive(true);
            }
            else
            {
                _objects[i].SetActive(false);
            }
        }
        
    }
}
