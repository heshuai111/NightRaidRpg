using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum scorpionState//定义枚举类型，保存野怪的五种状态
{
    Idle,//静立状态
    Walk,//行走状态
    Attack,//攻击状态
    Gethit,//收到攻击状态
    Death//死亡状态
}

public class scorpion : MonoBehaviour
{
    public scorpionState state;//状态量
    public string animall_idle;//静立动画
    public string animall_walk;//行走动画
    public string animall_gethit;//收到伤害动画
    public string animall_death;//死亡动画
    public string animall_now;//当前动画

    public string aniname_attack_now;//当前攻击动画
    public string aniname_normalattack1; //普攻动画
    public string aniname_normalattack2;
    public float time_normalattack;//普攻时间
    public string aniname_crazyattack;//疯狂攻击动画
    public float time_crazyattack;//疯狂攻击时间
    public int attack = 10;//每次攻击力
    public float minDistance = 3;//最小攻击距离
    public float maxDistance = 50;//最大攻击距离
    public int attack_rate = 1;//攻击速率
    private float attack_timer = 0;//计时器
    public float speed = 1;//走的速度
    public float runspeed = 3;//跑的速度
    public int hp = 100;//初始血量
    public float miss_rate = 0.2f;//攻击Miss的概率
    private Color normal;//野怪正常颜色
    public float red_time = 1;//显示被击中的时间
    public AudioClip miss_sound;//攻击Miss的音效
    //计时器
    public float time = 1;
    public float timer = 0;

    GameObject player;//玩家
    public Transform target;//玩家的位置
    Animation animation;//播放动画
    private CharacterController cc;//角色控制器
    GameObject g;

    public GameObject body;

    public CharacterAttackSystem c;//访问游戏角色的方法


    private void Awake()
    {
        body = GameObject.Find("GIANT_SCORPION_BODY");
        normal = body.GetComponent<SkinnedMeshRenderer>().material.color;
    }

    // Start is called before the first frame update
    void Start()
    {
        state = scorpionState.Idle;//初始化初始状态
        animall_now = animall_idle;//初始化初始动画
        aniname_attack_now = aniname_normalattack1;//初始化攻击动画
        cc = this.GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player");//获取玩家对象
        target = player.transform;//获取玩家的位置
        animation = GetComponent<Animation>();

        addAnimationEvent(aniname_normalattack1, "controlHP1");
        addAnimationEvent(aniname_normalattack2, "controlHP2");
        addAnimationEvent(aniname_crazyattack, "controlHP3");
        c = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterAttackSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);//计算玩家与野怪的距离
        if (distance <= maxDistance)//判断玩家是否在野怪的攻击范围内
        {
            state = scorpionState.Attack;
        }
        if (distance <= minDistance)
        {
            //始终朝向玩家
            transform.LookAt(target);
        }
        if (state == scorpionState.Attack)
        {
            AutoAttack();//野怪执行攻击
        }
        else if (state == scorpionState.Death)//如果野怪的状态是死亡，则播放死亡动画
        {
            animation.CrossFade(animall_death);
        }
        else if (state == scorpionState.Gethit)//如果野怪的状态是受到伤害状态，则播放受到伤害动画
        {
            animation.CrossFade(animall_gethit);
        }
        else//野怪处于其他状态
        {
            animation.CrossFade(animall_now);//播放野怪当前动画
            if (animall_now == animall_walk)
            {
                cc.SimpleMove(transform.forward * speed);
            }
            timer += Time.deltaTime;//计时器
            if (timer >= time)
            {
                timer = 0;
                Randomstate();//随机产生行走和静立状态
            }
        }
        if (hp <= 0)
        {
            state = scorpionState.Death;
            animation.CrossFade(animall_death);
            Destroy(this.gameObject, 1.5f);
        }
    }

    void Randomstate()//随机产生行走和静立状态
    {
        int value = Random.Range(0, 2);
        if (value == 0)
        {
            animall_now = animall_idle;
        }
        else
        {
            if (animall_now != animall_walk)
            {
                transform.Rotate(transform.up * Random.Range(0, 360));//随机转向
            }
            animall_now = animall_walk;
        }
    }

    void AutoAttack()//野怪自动攻击
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= minDistance)//如果玩家在最小攻击范围内直接攻击
        {
            attack_timer += Time.deltaTime;
            animation.CrossFade(aniname_attack_now);
            if (aniname_attack_now == aniname_normalattack1)
            {
                if (attack_timer > time_normalattack)//超过普攻时间
                {
                    aniname_attack_now = animall_idle;//修改当前动画为静立动画
                }
            }
            if (aniname_attack_now == aniname_crazyattack)
            {
                if (attack_timer > time_crazyattack)//超过疯狂攻击时间
                {
                    aniname_attack_now = animall_idle;//修改当前动画为静立动画
                }
            }
            if (attack_timer > (1f / attack_rate))//修改攻击方式
            {
                RandomAttack();//随机产生攻击方式
                attack_timer = 0;
            }

        }
        else//如果玩家不在最小攻击范围内，野怪向玩家移动
        {
            transform.LookAt(target);
            cc.SimpleMove(transform.forward * runspeed);
            animation.CrossFade(animall_walk);
        }
    }

    void RandomAttack()//随机产生攻击方式
    {

        float value = Random.Range(0, 4);
        if (value == 0)
        {
            aniname_attack_now = aniname_crazyattack;

        }
        else if (value == 1)
        {
            aniname_attack_now = aniname_normalattack2;

        }
        else
        {
            aniname_attack_now = aniname_normalattack1;

        }
        
    }



  public void TakeDamage(int attack)
 {
     if (state == scorpionState.Death) return;
     float value = Random.Range(0f, 1f);
     if (value < miss_rate)//Miss效果
     {
         //AudioSource.PlayClipAtPoint(miss_sound, transform.position);
     }
     else
     {
         state = scorpionState.Gethit;
         //血量减少的同时，血条也减少
         this.hp -= attack;
         Slider_c2.instance.SetValue(hp);
         //显示受到的伤害数值
         GameObject.Find("Canvas (3)").GetComponent<HudText>().HUD(attack);
         //野怪受到伤害身体变红
         StartCoroutine(ShowBodyRed());
         if (hp <= 0)
         {
             state = scorpionState.Death;
         }
     }
 }

 IEnumerator ShowBodyRed()
 {
        body.GetComponent<SkinnedMeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(1f);
        body.GetComponent<SkinnedMeshRenderer>().material.color = normal;
 }
    public int GetAttack()//传递伤害值
    {
        return attack;
    }

    public void addAnimationEvent(string aname, string function)//为每个动画添加事件处理
    {
        AnimationClip animationClip = animation.GetClip(aname);
        AnimationEvent animationEvent = new AnimationEvent();
        animationEvent.functionName = function;
        animationEvent.time = 0.5f;
        animationClip.AddEvent(animationEvent);
    }

    void controlHP1()//攻击一的动画触发
    {
        Debug.Log("攻击一的动画触发");
        //c.TakeDamage(GetAttack());
    }

    void controlHP2()//攻击二的动画触发
    {
        Debug.Log("攻击二的动画触发");
        //c.TakeDamage(15);
    }
    void controlHP3()//攻击三的动画触发
    {

        Debug.Log("攻击三的动画触发");
        //c.TakeDamage(30);
    }


}





