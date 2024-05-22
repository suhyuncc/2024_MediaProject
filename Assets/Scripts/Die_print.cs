using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InnerDriveStudios.DiceCreator;

public class Die_print : MonoBehaviour
{
    [SerializeField]
    private ARollable[] dies;

    public Text R_txt;

    public int resultCount;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Print_result();
    }

    void Print_result()
    {
        string resultColor;

        int resultValue = 0;

        

        for (int i = 0; i < dies.Length; i++)
        {
            IRollResult result = dies[i].GetRollResult();

            resultColor = (dies[i].HasEndResult() && result.isExact) ? "green" : "blue";

            //only show values if we are NOT rolling OR updating every frame
            resultValue +=
                (!dies[i].isRolling) ? int.Parse(result.valuesAsString) : int.Parse("0");
        }

        for (int i = 0; i < dies.Length; i++)
        {
            if (!dies[i].isRolling)
            {
                resultCount++;
            }
            else
            {
                resultCount = 0;
            }

            
        }

        if (resultCount == dies.Length)
        {
            R_txt.text = string.Format("결과 = {0}", resultValue);
            resultCount = 0;
        }
        else
        {

            R_txt.text = string.Format("결과 = ??");
            resultCount = 0;
        }

    }
}
