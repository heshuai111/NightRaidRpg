using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HudText : MonoBehaviour
{
    //文字预设体
    public GameObject hudText;
    //public GameObject hudprefab;

    //产生伤害文字
    public void HUD(int damage)
    {
        
        GameObject hud = Instantiate(hudText,transform) as GameObject;
        Debug.Log(transform+"**8***");
        hud.GetComponent<Text>().text= "-" + damage.ToString();
        //hud.GetComponent<Text>().fontSize = 24;
        Debug.Log(hud.GetComponent<Text>().text+"______");
        Destroy(hud, 0.3f);

    }

}
