using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inven_Panel : MonoBehaviour
{
    [SerializeField]
    private Player_stat P_stat;
    [SerializeField]
    private GameObject[] _itemSlot;
    [SerializeField]
    private Texture[] _itemImages;
    [SerializeField]
    private Text[] _itemNum;

    private void OnEnable()
    {
        for(int i = 0; i < P_stat.Item_list.Length; i++)
        {
            _itemNum[i].gameObject.SetActive(false);

            if (P_stat.Item_list[i] == 0)
            {
                _itemSlot[i].SetActive(false);
            }
            else if (P_stat.Item_list[i] == 2)
            {
                _itemSlot[i].GetComponent<RawImage>().texture = _itemImages[P_stat.Item_list[i]];
                _itemSlot[i].SetActive(true);
                _itemNum[i].text = $"{P_stat.Coin_num}";
                _itemNum[i].gameObject.SetActive(true);
            }
            else
            {
                _itemSlot[i].GetComponent<RawImage>().texture = _itemImages[P_stat.Item_list[i]];
                _itemSlot[i].SetActive(true);
            }
        }
    }
}
