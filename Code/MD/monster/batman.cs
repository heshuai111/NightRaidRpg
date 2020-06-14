using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum batmanState//定义枚举类型，保存野怪的五种状态
{
    Idle,//静立状态
    Walk,//行走状态
    Attack,//攻击状态
    Gethit,//收到攻击状态
    Death//死亡状态
}

public class batman : MonoBehaviour
{
    public batmanState state1;//状态量
    public string animall_idle1;//静立动画
    public string animall_walk1;//行走动画
    public string animall_run1;//跑状态动画
    public string animall_gethit1;//收到伤害动画
    public string animall_death1;//死亡动画
    public string animall_now1;//当前动画

    public string aniname_attack_now1;//当前攻击动画
    public string aniname_normal1attack1_1; //普攻动画
    public string aniname_normal1attack2_1;
    public string aniname_normal1attack3_1;
    public string aniname_normal1attack4_1;
    public float time1_normal1attack1;//普攻时间
    public string aniname_crazyattack_1;//疯狂攻击动画
    public float time1_crazyattack1;//疯狂攻击时间
    public int attack = 10;//每次攻击力
    public float minDistance1 = 3;//最小攻击距离
    public float maxDistance1 = 50;//最大攻击距离
    public int attack_rate1 = 1;//攻击速率
    private float attack_time1r11 = 0;//计时器
    public float speed1 = 1;//走的速度
    public float runspeed1 = 3;//跑的速度
    public int hp1 = 100;//初始血量
    public float miss_rate1 = 0.2f;//攻击Miss的概率

    private Color normalColor;//野怪正常颜色
    public float red_time1 = 1;//显示被击中的时间

    public AudioClip miss_sound;//攻击Miss的音效
    //计时器
    public float time1 = 1;
    public float time1r1 = 0;

    GameObject player1;//玩家
    public Transform target1;//玩家的位置
    Animation animation1;//播放动画
    Renderer render1;//渲染器
    private CharacterController cc1;//角色控制器
  //public CharacterAttackSystem c;

    private void Awake()
    {
        render1 = GetComponent<Renderer>();
       // normalColor = render1.material.color;
    }

    // Start is called before the first frame update
    void Start()
    {
        state1 = batmanState.Idle;//初始化初始状态
        animall_now1 = animall_idle1;//初始化初始动画
        aniname_attack_now1 = aniname_normal1attack1_1;//初始化攻击动画
        cc1 = this.GetComponent<CharacterController>();
        player1 = GameObject.FindGameObjectWithTag("monster");//获取玩家对象
        target1 = player1.transform;//获取玩家的位置
        animation1 = GetComponent<Animation>();

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target1.position, transform.position);//计算玩家与野怪的距离
        if (distance <= maxDistance1)//判断玩家是否在野怪的攻击范围内
        {
            state1 = batmanState.Attack;
        }
        if (state1 == batmanState.Attack)
        {
            AutoAttack1();//野怪执行攻击
        }
        else if (state1 == batmanState.Death)//如果野怪的状态是死亡，则播放死亡动画
        {
            animation1.CrossFade(animall_death1);
        }
        else//野怪处于其他状态
        {
            animation1.CrossFade(animall_now1);//播放野怪当前动画
            if (animall_now1 == animall_walk1)
            {
                cc1.SimpleMove(transform.forward * speed1);
            }
            time1r1 += Time.deltaTime;//计时器
            if (time1r1 >= time1)
            {
                time1r1 = 0;
                Randomstate1();//随机产生行走和静立状态
            }
        }
    }

    void Randomstate1()//随机产生行走和静立状态
    {
        int value = Random.Range(0, 2);
        if (value == 0)
        {
            animall_now1 = animall_idle1;
        }
        else
        {
            if (animall_now1 != animall_walk1)
            {
                transform.Rotate(transform.up * Random.Range(0, 360));//随机转向
            }
            animall_now1 = animall_walk1;
        }
    }

    void AutoAttack1()//野怪自动攻击
    {
        float distance = Vector3.Distance(target1.position, transform.position);
        if (distance <= minDistance1)//如果玩家在最小攻击范围内直接攻击
        {
            attack_time1r11 += Time.deltaTime;
            animation1.CrossFade(aniname_attack_now1);
            //c.TakeDamage(GetAttack());
            if (aniname_attack_now1 == aniname_normal1attack1_1)
            {
                if (attack_time1r11 > time1_normal1attack1)//超过普攻时间
                {
                    aniname_attack_now1 = animall_idle1;//修改当前动画为静立动画
                }
            }
            if (aniname_attack_now1 == aniname_crazyattack_1)
            {
                if (attack_time1r11 > time1_crazyattack1)//超过疯狂攻击时间
                {
                    aniname_attack_now1 = animall_idle1;//修改当前动画为静立动画
                }
            }
            if (attack_time1r11 > (1f / attack_rate1))//修改攻击方式
            {
                RandomAttack1();//随机产生攻击方式
                attack_time1r11 = 0;
            }

        }
        else//如果玩家不在最小攻击范围内，野怪向玩家移动
        {
            transform.LookAt(target1);
            cc1.SimpleMove(transform.forward * runspeed1);
            animation1.CrossFade(animall_run1);
        }
    }

    void RandomAttack1()//随机产生攻击方式
    {

        float value = Random.Range(0, 4);
        if (value == 0)
        {
            aniname_attack_now1 = aniname_normal1attack1_1;

        }
        else if (value == 1)
        {
            aniname_attack_now1 = aniname_normal1attack2_1;

        }
        else if (value == 2)
        {
            aniname_attack_now1 = aniname_normal1attack3_1;

        }
        else if (value == 3)
        {
            aniname_attack_now1 = aniname_normal1attack4_1;

        }
        else if (value == 4)
        {
            aniname_attack_now1 = aniname_crazyattack_1;
        }
    }



public void BeDamage(int attack)
{
    if (state1 == batmanState.Death) return;
    float value = Random.Range(0f, 1f);
    if (value < miss_rate1)//Miss效果
    {
        //AudioSource.PlayClipAtPoint(miss_sound, transform.position);
    }
    else
    {
        this.hp1 -= attack;
        Slider_c5.instance.SetValue(hp1);
        Debug.Log("当前血量："+hp1);
        //StartCoroutine(ShowBodyRed1());
        if (hp1 <= 0)
        {
            state1 = batmanState.Death;
            Destroy(this.gameObject, 2);
        }
    }
}

IEnumerator ShowBodyRed1()
{
    render1.material.color = Color.red;
    yield return new WaitForSeconds(1f);
    render1.material.color = normalColor;
}
    private void OnDestroy()
    {
        //GameObject.Destroy();
    }

}





