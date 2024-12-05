using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keypad_Number : MonoBehaviour
{
    public int Index;
    [SerializeField]
    private Keypad_Head _head;
    [SerializeField]
    private Texture[] _textures;
    [SerializeField]
    private RawImage _image;
    [SerializeField]
    private GameObject _object;


    public void SetNumber()
    {
        if (_head.numbers[Index] < 10)
        {
            _image.texture = _textures[_head.numbers[Index]];
            _object.SetActive(true);
        }
        else
        {
            _object.SetActive(false);
        }
    }
}
