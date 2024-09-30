using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Player : Character, IObserver<GameManager.GameState>, IObservable<Player>, IDisposable
{
    // Singleton
    public static Player instance { get; private set; }
    /// <summary> Succeeding instance data </summary>
    void InstanceSuccession()
    {
        //Stat
        stat = instance.stat;

        //Strategy
        bulletOption = instance.bulletOption;
        bulletSpawnStrategy = instance.bulletSpawnStrategy;

        //Destroy
        Destroy(instance.gameObject);
    }

    /// <summary> Layer Mask for raycast to target </summary>
    [Header("Player")][Tooltip("Raycast Layer Mask")]
    public LayerMask targetLayer;

    // Sound
    private AudioSource audioSource;
    [Header("Audio")]
    public AudioClip fireSound;

    // Unity Functions
    protected override void Awake()
    {
        // Singleton
        if (instance != null)
            InstanceSuccession();
        else
            base.Awake();
        instance = this;

        // AudioSource
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = fireSound;

        // StateMachine
        ChangeState(_StateMachine.Attack);
    }
    protected void Start()
    {
        // Observer Pattern
        Subscribe();

        // One game, One player
        if(GameManager.instance != null)
            GameManager.instance.AddDontDestroyObjects(gameObject);
    }

    // StateMachine
    public override void StateMachine()
    {
        switch (state)
        {
            case _StateMachine.Idle:
                if(!WaveManager.WaveEnd())
                    ChangeState(_StateMachine.Attack);
                break;
            case _StateMachine.Attack:
                Attack();
                break;
        }
    }
    // Actions
    public override void Idle()
    {
        
    }
    /// <summary>Empty Function. Not working. Use function the Move(Vector3 direction)</summary>
    public override bool Move()
    {
        throw new NotImplementedException();
    }
    /// <summary>Move Function with direction vector</summary>
    public bool Move(Vector3 direction)
    {
        //Observer Pattern : Update destination of enemy navmeshagent
        NotifyObserver();

        //Player Move
        Vector3 inputVector = new Vector3(direction.x, 0, direction.y);
        transform.position += inputVector * stat.speed * Time.deltaTime;  //Translate
        transform.LookAt(transform.position + inputVector); //Rotate
        return inputVector.magnitude > 0f;
    }
    public override void Attack()
    {
        //No Enemys
        if (WaveManager.eachEnemyDistanceFromPlayer == null || WaveManager.eachEnemyDistanceFromPlayer.Count <= 0)
            return;

        //Auto Targeting
        RaycastHit raycastHit;
        bool isHit = false;
        int minDistanceIdx = 0;
        for (int i = 0; i < WaveManager.eachEnemyDistanceFromPlayer.Count; i++)
        {
            //Raycast
            Physics.Raycast(transform.position, (WaveManager.cur_wave[i].position - transform.position).normalized, out raycastHit, 100, targetLayer);
            Character target = raycastHit.collider.GetComponent<Character>();

            //First time a raycast hit an enemy
            if (!isHit && target != null)
            {
                isHit = true;
                minDistanceIdx = i;
                continue;
            }

            //�������� ���ų� �ѹ� �̻� �¾Ұ� �̹��� Ÿ���̸� ��ϵ� ���� �̹� ���� �Ÿ���
            if(!isHit || target != null)
                minDistanceIdx = WaveManager.eachEnemyDistanceFromPlayer[minDistanceIdx] >= WaveManager.eachEnemyDistanceFromPlayer[i] ? i : minDistanceIdx;
        }

        //Look at target
        transform.LookAt(WaveManager.cur_wave[minDistanceIdx]);
        transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
        
        //Aim to target
        if(gunPos != null)
        {
            gunPos.LookAt(WaveManager.cur_wave[minDistanceIdx]);
            gunPos.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
        }
    }
    public override void Dead()
    {
        // Never die when win
        if(GameManager.instance.gameState == GameManager.GameState.Win)
            return;
        
        // Game state -> lose
        GameManager.instance.SetGameState(GameManager.GameState.Lose);
    }

    // Set default strategy
    protected override void SetDefaultStrategy()
    {
        //Bullet option
        AddBulletDamageType(new DamageType_Single());

        //Bullet spawn
        AddBulletSpawnStrategy(new BulletSpawn_Front());
    }

    // Event Functions
    public override void BulletSpawn()
    {
        // No Enemy
        if (WaveManager.WaveEnd())
            return;

        // Fire sound play
        audioSource.Play();

        // Shooting
        base.BulletSpawn();
    }

    // Systems Functions
    public void ResetPlayer()
    {
        //StateMachine
        ChangeState(_StateMachine.Idle);

        //Transform
        transform.localPosition = new Vector3(0f, 0.5f, 0f);
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }
    public void Stat_DamageUp(float damage)
    {
        // Stat
        stat.damage += damage;

        // Synchronize Bullet.Option
        bulletOption.damage += damage;
    }

    // Observer Pattern Observe : Game Result
    public override void OnNext(GameManager.GameState value)
    {
        // Animator speed
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

        // State change
        switch (value)
        {
            case GameManager.GameState.Win:
            case GameManager.GameState.Lose:
                ChangeState(_StateMachine.Idle);
                break;
        }
    }
    
    /// <summary> Subject : Player is Move </summary>
    List<IObserver<Player>> observers_PlayerMove = new List<IObserver<Player>>();
    /// <summary> Subject : Player is Move </summary>
    public IDisposable Subscribe(IObserver<Player> observer)
    {
        observers_PlayerMove.Add(observer);
        return this;
    }
    public void UnSubscribe(IObserver<Player> observer)
    {
        if (observers_PlayerMove.Contains(observer))
            observers_PlayerMove.Remove(observer);
    }
    public void NotifyObserver()
    {
        foreach (IObserver<Player> observer in observers_PlayerMove)
                observer.OnNext(this);
    }
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
