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
        

        R_txt.text = string.Format("°á°ú = {0}", resultValue);
    }
}
