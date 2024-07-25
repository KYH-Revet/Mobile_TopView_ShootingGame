using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Character : MonoBehaviour, IObserver<GameManager.GameResult>
{

    // Unity //

    // Animator
    [Header("Animation")][SerializeField]
    protected Animator animator;

    // Functions
    protected virtual void Awake()
    {
        //Animator
        animator = GetComponent<Animator>() != null ? GetComponent<Animator>() : GetComponentInChildren<Animator>();
        
        //Set stat.hp
        stat.hp = stat.maxHp;

        //Set default strategy
        SetDefaultStrategy();
    }
    protected virtual void Update()
    {
        //StateMachine
        StateMachine();
    }

    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////
    
    // Character Stat

    /// <summary>Character의 여러 Stat 값을 모아둔 구조체</summary>
    [System.Serializable]
    public struct Stat
    {
        public float maxHp;
        public float hp;
        public float speed;
        public float damage;
        public Stat(float maxHp, float speed, float damage)
        {
            this.maxHp = maxHp;
            hp = maxHp;
            this.speed = speed;
            this.damage = damage;
        }
    }
    [SerializeField][Header("Stat")]
    public Stat stat = new Stat();
    /// <summary>Character의 Stat.hp를 조작하는 함수</summary>
    /// <param name="value">Character.stat.hp += value</param>
    public virtual void HPControll(float value)
    {
        //Alive
        if (state != _StateMachine.Dead)
        {
            //Change hp
            stat.hp += value;

            //OverHeal
            if (stat.maxHp < stat.hp)
                stat.hp = stat.maxHp;

            //HP UI synchronization
            if (hpBar != null)
                hpBar.HPSynchronization();

            //Dead
            if (stat.hp <= 0)
            {
                ChangeState(_StateMachine.Dead);
                Dead();
            }
        }
    }

    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////

    // Gun & Bullet //

    [Header("Gun & bullet")]
    /// <summary>총알 프리팹</summary>
    public GameObject prefab_bullet;
    /// <summary>총 위치</summary>
    public Transform gunPos;
    /// <summary>총구 위치</summary>
    public Transform bulletPos;

    // Bullet Option
    /// <summary> Character가 발사할 총알에 부여된 여러 옵션 저장</summary>
    public Bullet.Option bulletOption = new Bullet.Option(new List<string>());
    /// <summary> 총알에 데미지 유형 추가 함수</summary>
    /// <param name="type">데미지 유형</param>
    public void AddBulletDamageType(IDamaged type)
    {
        //Add DamageType
        string dmgType = type.GetType().Name;

        // New DamageType
        if (!bulletOption.damageType.Contains(dmgType))
            bulletOption.damageType.Add(dmgType);
    }
    
    // Bullet Spawn Strategy
    /// <summary>Character의 총알 발사 전략을 저장한 Dictionary. [string = IBulletSpawn 인터페이스를 상속받은 Class], [int = 전략의 Level]</summary>
    public Dictionary<string, int> bulletSpawnStrategy = new Dictionary<string, int>();
    /// <summary>총알 발사 전략 추가 함수</summary>
    /// <param name="strategy">IBulletSpawn 발사 전략</param>
    public void AddBulletSpawnStrategy(IBulletSpawn strategy)
    {
        //Add Spawn Strategy
        string s = strategy.GetType().Name;
        try
        {
            if (++bulletSpawnStrategy[s] > 3)
                bulletSpawnStrategy[s] = 3;
        }
        catch(KeyNotFoundException)
        {
            bulletSpawnStrategy.Add(s, 1);
        }
    }
    
    /// <summary> 총알 소환(Event 함수) </summary>
    public virtual void BulletSpawn()
    {
        //Spawn
        foreach (var spawnStrategy in bulletSpawnStrategy)
        {
            IBulletSpawn strategy = (IBulletSpawn)Activator.CreateInstance(Type.GetType(spawnStrategy.Key));
            strategy.Spawn(prefab_bullet, bulletPos, bulletOption, spawnStrategy.Value);
        }
    }
    
    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////

    // StateMachine //

    /// <summary>Character의 StateMachine</summary>
    public enum _StateMachine
    {
        Idle,
        Move,
        Attack,
        Dead
    }
    [SerializeField]
    protected _StateMachine state;
    /// <summary>StateMachine의 state와 Animator의 state 변수값 변경 함수</summary>
    /// <param name="nextState">변경할 상태</param>
    public void ChangeState(_StateMachine nextState)
    {
        if (state == nextState && state == _StateMachine.Dead)
            return;
        state = nextState;
        animator.SetInteger("State", (int)nextState);
    }

    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////

    // UI //

    /// <summary>Character의 현재 체력 표시 UI</summary>
    [SerializeField] [Header("UI")]
    protected HPBar hpBar;

    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////

    // Abstract Functions //

    //Statemachine
    /// <summary>StateMahcine : Idle, Move, Attack, Dead</summary>
    public abstract void StateMachine();
    /// <summary>State Idle</summary>
    public abstract void Idle();
    /// <summary>State Move</summary><returns>true = Moving / false = Standing</returns>
    public abstract bool Move();
    /// <summary>State Attack</summary>
    public abstract void Attack();
    /// <summary>State Dead</summary>
    public abstract void Dead();
    /// <summary>Default Strategy</summary>
    protected abstract void SetDefaultStrategy();

    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////

    // Observer Pattern //
    
    public virtual void Subscribe()
    {
        // GameManager Game result
        GameManager.instance.Subscribe(this);
    }
    public void OnCompleted()
    {
        throw new NotImplementedException();
    }
    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }
    public virtual void OnNext(GameManager.GameResult value)
    {
        switch (value)
        {
            case GameManager.GameResult.Processing:
                animator.speed = 1f;
                break;
            case GameManager.GameResult.Pause:
                animator.speed = 0f;
                break;

        }
    }
}
