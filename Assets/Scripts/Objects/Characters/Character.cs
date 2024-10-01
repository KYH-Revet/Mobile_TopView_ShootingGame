using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Character : MonoBehaviour, IObserver<GameManager.GameState>
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

    /// <summary>Character�� ���� Stat ���� ��Ƶ� ����ü</summary>
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
    /// <summary>Character�� Stat.hp�� �����ϴ� �Լ�</summary>
    /// <param name="value">Character.stat.hp += value</param>

    public virtual void HPControll(float value)
    {
        switch (GameManager.instance.gameState)
        {
            case GameManager.GameState.Win:
            case GameManager.GameState.Lose:
                break;
            default:
                // Alive
                if (_state != _StateMachine.Dead)
                {
                    // Change hp
                    stat.hp += value;

                    // Over healing, hp = max hp
                    if (stat.maxHp < stat.hp)
                        stat.hp = stat.maxHp;

                    // HP UI synchronization
                    if (hpBar != null)
                        hpBar.HPSynchronization();

                    // Dead
                    if (stat.hp <= 0)
                        ChangeState(_StateMachine.Dead);
                }
                break;
        }
    }

    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////

    // Gun & Bullet //

    [Header("Gun & bullet")]
    /// <summary>�Ѿ� ������</summary>
    public GameObject prefab_bullet;
    /// <summary>�� ��ġ</summary>
    public Transform gunPos;
    /// <summary>�ѱ� ��ġ</summary>
    public Transform bulletPos;

    // Bullet Option
    /// <summary> Character�� �߻��� �Ѿ˿� �ο��� ���� �ɼ� ����</summary>
    public Bullet.Option bulletOption = new Bullet.Option(new List<string>());
    /// <summary> �Ѿ˿� ������ ���� �߰� �Լ�</summary>
    /// <param name="type">������ ����</param>
    public void AddBulletDamageType(IDamaged type)
    {
        //Add DamageType
        string dmgType = type.GetType().Name;

        // New DamageType
        if (!bulletOption.damageType.Contains(dmgType))
            bulletOption.damageType.Add(dmgType);
    }
    
    // Bullet Spawn Strategy
    /// <summary>Character�� �Ѿ� �߻� ������ ������ Dictionary. [string = IBulletSpawn �������̽��� ��ӹ��� Class], [int = ������ Level]</summary>
    public Dictionary<string, int> bulletSpawnStrategy = new Dictionary<string, int>();
    /// <summary>�Ѿ� �߻� ���� �߰� �Լ�</summary>
    /// <param name="strategy">IBulletSpawn �߻� ����</param>
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
    
    /// <summary> �Ѿ� ��ȯ(Event �Լ�) </summary>
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

    /// <summary>Character�� StateMachine</summary>
    public enum _StateMachine
    {
        Idle,
        Move,
        Attack,
        Dead
    }
    [SerializeField]
    protected _StateMachine _state;
    public _StateMachine state { get { return _state; } }
    /// <summary>StateMachine�� state�� Animator�� state ������ ���� �Լ�</summary>
    /// <param name="nextState">������ ����</param>
    public void ChangeState(_StateMachine nextState)
    {
        // Same state or Already dead
        if (_state == nextState && _state == _StateMachine.Dead)
            return;

        // State Change
        _state = nextState;
        animator.SetInteger("State", (int)nextState);

        if (_state == _StateMachine.Dead)
            animator.SetTrigger("Dead");
    }

    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////

    // UI //

    /// <summary>Character�� ���� ü�� ǥ�� UI</summary>
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
    public virtual void UnSubscribe()
    {
        // GameManager Game result
        GameManager.instance.UnSubscribe(this);
    }
    public virtual void OnCompleted()
    {
        throw new NotImplementedException();
    }
    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }
    public virtual void OnNext(GameManager.GameState value)
    {
        // Character animation
        switch (value)
        {
            case GameManager.GameState.Processing:
            case GameManager.GameState.StageClear:
                animator.speed = 1f;
                break;
            default:
                animator.speed = 0f;
                break;
        }
    }
}
