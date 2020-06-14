using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn1 : MonoBehaviour
{
    public int maxnum = 1;//控制野怪的总数量
    private int currentnum = 0;//计数
    //计时器
    public float time = 3;
    private float timer = 0;
    public GameObject prefab;

    private void Update()
    {
        MakeObject();
    }

    public void MakeObject()
    {
        if (currentnum > maxnum)
        {
            Debug.Log("野怪达到上限");
        }
        if (currentnum < maxnum)
        {
            Debug.Log(currentnum+"******");
            timer += Time.deltaTime;
            if (timer > time)
            {
                Vector3 pos = transform.position;
                pos.x += Random.Range(-5, 5);
                pos.z += Random.Range(-5, 5);
                GameObject.Instantiate(prefab, pos, Quaternion.identity);//实例化一个对象
                timer = 0;
                currentnum++;
                Debug.Log(currentnum + "&&&&&&");
            }
        }
    }


}
