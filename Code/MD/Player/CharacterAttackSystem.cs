using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAttackState
{
    ctrlWalk,
     NormalAttack,
    SkillAttack,
    other,
    death
}
public  enum AttackState
{
    moving,
    Idle,
    attack,
   
}

public class CharacterAttackSystem : MonoBehaviour
{
    public PlayerAttackState state = PlayerAttackState.ctrlWalk;
    public AttackState attackState = AttackState.Idle;
    public string aniname_normalattack;
    public string aniname_idle;
    public string aniname_now;
    public float time_normalattack;
    public float rateNormalAttack = 1;
    public float timer = 0;
    public float min_distance = 1;//攻击最小距离
    public float normalAttack = 5;//普攻伤害值
    private fps_playerParameter1 parameter;
    private Transform target_normalattack;
    public Player_contrller move_contrller;
    public Animation animation;
    public bool showEffect = false;
    public GameObject effect;
    private PlayerStatus playstaus;

    private Dictionary<string, GameObject> efxDict = new Dictionary<string, GameObject>();
    private bool isLockingTarget = false;
    private SkillInfo info = null;
    private void Awake()
    {
        move_contrller = this.GetComponent<Player_contrller>();
       
    }
    // Start is called before the first frame update
    void Start()
    {
        playstaus = this.GetComponent<PlayerStatus>();
        parameter = this.GetComponent<fps_playerParameter1>();
        animation = this.GetComponent<Animation>();
        addAnimationEvent("n2017_skil1_2", "controlHP1");
    }

    // Update is called once per frame
    void Update()
    {
        if (parameter.NormalAttack)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
             RaycastHit hitinfo;
             bool isCollider = Physics.Raycast(ray,out hitinfo);
             if (isCollider && hitinfo.collider.tag == Tags.enemy)
             {
                 target_normalattack = hitinfo.collider.transform;
                 state = PlayerAttackState.NormalAttack;

             }
             else
             {
                 state = PlayerAttackState.ctrlWalk;
                 target_normalattack = null;
             }
             
  
            
        }
     
        if (state == PlayerAttackState.NormalAttack)
        {
            if (target_normalattack == null)
            {
                state = PlayerAttackState.ctrlWalk;
            }
            else
            {
                float distance = Vector3.Distance(transform.position, target_normalattack.position);
                if (distance <= min_distance)
                {
                    //攻击
                    attackState = AttackState.attack;
                   

                   playattack();
                    timer += Time.deltaTime;
                    //  GameObject.Instantiate(effect, target_normalattack.position, Quaternion.identity);
                    if (showEffect == false)
                    {
                        showEffect = true;
                        //   GameObject.Instantiate(effect, target_normalattack.position, Quaternion.identity);
                        target_normalattack.GetComponent<StoreMan>().TakeDamage(GetAttack());
                    }
                    if (timer >= time_normalattack)
                    {
                        aniname_now = aniname_idle;
                        if (showEffect == false)
                        {
                            showEffect = true;
                            GameObject.Instantiate(effect, target_normalattack.position, Quaternion.identity);
                            //  target_normalattack.GetComponent<>().TakeDamage(GetAttack());
                        }

                    }
                    if (timer >= (1f / rateNormalAttack))
                    {
                        timer = 0;

                        playattack();
                        showEffect = false;

                    }




                }

                else
                {
                    attackState = AttackState.moving;
                    move_contrller.SimpleMove(target_normalattack.position);

                }
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnLockSingleTarget();
        }

        //if (isLockingTarget && Input.GetMouseButton(0))
        //{
           
        //}

             
    }


    public int GetAttack()
    {
        return (int)playstaus.attack;


    }
   void   playattack()
    {

        animation.CrossFade(aniname_now);
      
       

    }

    public void TakeDamage(int attack)
    {
        if (state == PlayerAttackState.death) return;
       
        else
        {
            playstaus.hp -= attack;
            //StartCoroutine(ShowBodyRed());
            if (playstaus.hp <= 0)
            {
                state = PlayerAttackState.death;
            //    Destroy(this.gameObject, 2);
            }
        }
    }
    public void UseSkill(SkillInfo info)
    {
        if (playstaus.heroType == HeroType.Magician)
        {
            if (info.applicableRole == ApplicableRole.Swordman)
            {
                return;
            }
        }

        if (playstaus.heroType == HeroType.Swordman)
        {
            if (info.applicableRole == ApplicableRole.Magician)
            {
                return;
            }
        }
        switch (info.applyType)
        {
            case ApplyType.Passive:
                OnPassliveSkillUse(info);
                break;
        }
    }

  void OnPassliveSkillUse(SkillInfo info)
    {
       // state = PlayerAttackState.SkillAttack;
        //animation.CrossFade("n2017_skil_6");
        //yield return new WaitForSeconds(1.8f);
        //state = PlayerAttackState.ctrlWalk;
        //GameObject prefab = null;
        //efxDict.TryGetValue("Effect1", out prefab);
        //GameObject.Instantiate(prefab, transform.position, Quaternion.identity);
        isLockingTarget = true;
        this.info = info;

        

    }
    void OnLockTarget()
    {
        isLockingTarget = false;
       // switch (info.applyType)
      //  {
          //  case ApplyType.SingleTarget:
            
            //    break;
            
      //  }

    }
  IEnumerator OnLockSingleTarget()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;
        bool isCollider = Physics.Raycast(ray, out hitinfo);
        //if (isCollider && hitinfo.collider.tag == Tags.enemy)
        {
            animation.CrossFade("n2017_skil_2");
            yield return new WaitForSeconds(1.8f);
            state = PlayerAttackState.ctrlWalk;
            GameObject prefab = null;
            //efxDict.TryGetValue("Effect1", out prefab);
            GameObject.Instantiate(effect, hitinfo.collider.transform.position, Quaternion.identity);
            hitinfo.collider.GetComponent<gebulin>().TakeDamage(GetAttack() * info.applyValue / 100);
        }
        
        {
            state = PlayerAttackState.NormalAttack;
        }

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
        target_normalattack.GetComponent<StoreMan>().TakeDamage(GetAttack());
        GameObject.Instantiate(effect, target_normalattack.position, Quaternion.identity);
       // Destroy(effect, 2);

    }
}
