using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HUD : MonoBehaviour
{
    //滚动速度
    private float speed = 0.5f;
    //计时器
    private float timer = 0f;
    //销毁时间
    private float time = 1.8f;

    // Update is called once per frame
   
    public void makeNew()
    {
        Scroll();
    }
    //血量减少的显示控制
    public void Scroll()
    {
        //字体滚动
        this.transform.Translate(Vector3.up * speed * Time.deltaTime);
        timer += Time.deltaTime;
        //字体缩小
        this.GetComponent<Text>().fontSize--;
        this.GetComponent<Text>().color = new Color(1, 0, 0, 1 - timer);
    }
}
