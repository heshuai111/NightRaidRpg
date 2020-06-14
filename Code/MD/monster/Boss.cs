using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum bossState//定义枚举类型，保存野怪的五种状态
{
    Idle,//静立状态
    Walk,//行走状态
    Attack,//攻击状态
    Gethit,//收到攻击状态
    Death//死亡状态
}

public class Boss : MonoBehaviour
{
    public bossState state;//状态量
    public string animall_idle;//静立动画
    public string animall_walk;//行走动画
    public string animall_gethit;//收到伤害动画
    public string animall_death;//死亡动画
    public string animall_now;//当前动画

    public string aniname_attack_now;//当前攻击动画
    public string aniname_normalattack;//普攻动画
    public float time_normalattack;//普攻时间
    public string aniname_crazyattack;//疯狂攻击动画
    public float time_crazyattack;//疯狂攻击时间
    public int attack = 10;//每次攻击力
    public float minDistance = 3;//最小攻击距离
    public float maxDistance = 50;//最大攻击距离
    public int attack_rate = 1;//攻击速率
    private float attack_timer = 0;//计时器
    public float speed = 1;//运动速度
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
    Renderer render;//渲染器
    private CharacterController cc;//角色控制器
    //public CharacterAttackSystem c;

    /* private void Awake()
     {       
         //normal = render.material.color;
     }*/

    // Start is called before the first frame update
    void Start()
    {

        state = bossState.Idle;//初始化初始状态
        animall_now = animall_idle;//初始化初始动画
        aniname_attack_now = aniname_normalattack;//初始化攻击动画
        cc = this.GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("monster");//获取玩家对象
        render = GetComponent<Renderer>();
        target = player.transform;//获取玩家的位置
        animation = GetComponent<Animation>();
        
    }

    // Update is called once per frame
    void Update()
    {
        animation.CrossFade(animall_now);
        float distance = Vector3.Distance(target.position, transform.position);//计算玩家与野怪的距离
        if (distance <= maxDistance)//判断玩家是否在野怪的攻击范围内
        {
            state = bossState.Attack;
        }
        if (state == bossState.Attack)
        {
            AutoAttack();//野怪执行攻击
        }
        else if (state == bossState.Death)//如果野怪的状态是死亡，则播放死亡动画
        {
            animation.CrossFade(animall_death);
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
                RandomState();//随机产生行走和静立状态
            }
        }
    }

    
    void RandomState()//随机产生行走和静立状态
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
            //c.TakeDamage(GetAttack());
            if (aniname_attack_now == aniname_normalattack)
            {
                if (attack_timer > time_normalattack)//超过普攻时间
                {
                    aniname_attack_now = animall_idle;//修改当前动画为静立动画
                }
            }
            else if (aniname_attack_now == aniname_crazyattack)
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
            cc.SimpleMove(transform.forward * speed);
            animation.CrossFade(animall_walk);
        }
    }

    void RandomAttack()//随机产生攻击方式
    {
        float value = Random.Range(0, 3);
        if (value == 0)
        {
            aniname_attack_now = aniname_crazyattack;
        }
        else
        {
            aniname_attack_now = aniname_normalattack;
        }
    }


    public void TakeDamage(int attack)
    {
        if (state == bossState.Death) return;
        float value = Random.Range(0f, 1f);
        if (value < miss_rate)//Miss效果
        {
            //AudioSource.PlayClipAtPoint(miss_sound, transform.position);
        }
        else
        {
            this.hp -= attack;
            Slider_c6.instance.SetValue(hp);
            //StartCoroutine(ShowBodyRed());
            if (hp <= 0)
            {
                state = bossState.Death;
                Destroy(this.gameObject, 2);
            }
        }
    }

    IEnumerator ShowBodyRed()
    {
        render.material.color = Color.red;
        yield return new WaitForSeconds(1f);
        render.material.color = normal;
    }
}





