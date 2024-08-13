using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : Enemy
{
    // Unity Functions
    protected override void Awake()
    {
        // Base
        base.Awake();

        // Start animation
        ChangeState(_StateMachine.Move);
    }

    // Multiple attack pattern
    enum AttackPattern
    {
        NormalShot,
        SpreadShot,
        BounceShot
    }
    AttackPattern attackPattern;

    // List for multiple pattern //
    public List<GameObject> prefab_bullets;

    // Bullet Option
    public List<Bullet.Option> bulletOptions = new List<Bullet.Option>();
    /// <summary>다수의 공격패턴을 위한 Bullet.Option 추가 함수</summary>
    /// <param name="type">데미지 유형</param><param name="idx">패턴 번호</param>
    public void AddBulletDamageType(IDamaged type, int idx)
    {
        //List Add (배열의 크기가 idx보다 작으면. 공간마련, 주의 : 부족한만큼 생성된 항목은 빈 데이터를 가짐)
        if (bulletOptions.Count <= idx)
            for (int i = 0; i < idx - bulletOptions.Count; i++)
                bulletOptions.Add(new Bullet.Option(new List<string>()));

        //Add DamageType
        string dmgType = type.GetType().Name;
        
        // New DamageType
        if (!bulletOptions[idx].damageType.Contains(dmgType))
            bulletOptions[idx].damageType.Add(dmgType);
    }

    // Bullet Spawn Strategy
    public List<Dictionary<string, int>> bulletSpawnStrategys = new List<Dictionary<string, int>>();
    /// <summary>공격 패턴이 여러개인 보스들을 위한 새로운 BulletSpawn strategy 추가 함수</summary>
    /// <param name="strategy">추가할 전략</param><param name="idx">패턴 번호</param>
    public void AddBulletSpawnStrategy(IBulletSpawn strategy, int idx)
    {
        //List Add
        if (bulletOptions.Count <= idx)
            for (int i = 0; i < idx - bulletOptions.Count; i++)
                bulletOptions.Add(new Bullet.Option(0f));

        //Add Spawn Strategy
        string s = strategy.GetType().Name;
        try
        {
            if (++bulletSpawnStrategys[idx][s] > 3)
                bulletSpawnStrategys[idx][s] = 3;
        }
        catch (KeyNotFoundException)
        {
            bulletSpawnStrategys[idx].Add(s, 1);
        }
    }

    // Override //
    public override void HPControll(float value)
    {
        // Alive
        if (state != _StateMachine.Dead)
        {
            // Change hp
            stat.hp += value;

            // OverHeal
            if (stat.maxHp < stat.hp)
                stat.hp = stat.maxHp;

            // HP UI synchronization
            if (hpBar != null)
                hpBar.HPSynchronization();

            // Dead
            if (stat.hp <= 0)
            {
                ChangeState(_StateMachine.Dead);
                animator.SetTrigger("Dead");
            }
        }
    }
    /// <summary> attackPattern 값에 맞춰 총알 소환 </summary>
    public override void BulletSpawn()
    {
        // Pattern number(index)
        int idx = (int)attackPattern;

        // Select bullet prefab object
        GameObject bulletObj = prefab_bullets.Count > idx ? prefab_bullets[idx] : prefab_bullets[0];

        // Spawn
        foreach (var spawnStrategy in bulletSpawnStrategys[idx])
        {
            IBulletSpawn strategy = (IBulletSpawn)Activator.CreateInstance(Type.GetType(spawnStrategy.Key));
            strategy.Spawn(bulletObj, bulletPos, bulletOptions[idx], spawnStrategy.Value);
        }
    }

    /// <summary> 다양한 패턴의 공격을 위한 전략 설정 </summary>
    protected override void SetDefaultStrategy()
    {
        // New Values
        Bullet.Option newOption = new Bullet.Option(stat.damage);
        List<IDamaged> newDamageType = new List<IDamaged>();
        List<IBulletSpawn> newBulletSpawn = new List<IBulletSpawn>();

        // Lambda Functions
        Action<int> AddStrategy = idx =>
        {
            // Set memory
            bulletOptions.Add(newOption);
            bulletSpawnStrategys.Add(new Dictionary<string, int>());

            // Add bullet option & strategys
            foreach (IDamaged type in newDamageType)
                AddBulletDamageType(type, idx);
            foreach (IBulletSpawn spawn in newBulletSpawn)
                AddBulletSpawnStrategy(spawn, idx);

            // Reset
            newOption = new Bullet.Option(stat.damage);
            newDamageType.Clear();
            newBulletSpawn.Clear();
        };

        int patternIdx = 0;
        // Pattern 1 : Normal Shoot (Animation으로 여러번 발사)
        {
            // Bullet option
            newOption.speed = 1500;

            // Damage Type
            newDamageType.Add(new DamageType_Single());

            // Bullet Spawn
            newBulletSpawn.Add(new BulletSpawn_Front());

            // Add
            AddStrategy(patternIdx++);
        }
        // Pattern 2 : Spread Shot (Animation으로 여러번 발사
        {
            // Bullet option
            newOption.speed = 1000;

            // Damage Type
            newDamageType.Add(new DamageType_Single());

            // Bullet Spawn
            newBulletSpawn.Add(new BulletSpawn_Front());
            newBulletSpawn.Add(new BulletSpawn_Angle_45());

            // Add
            AddStrategy(patternIdx++);
        }
        // Pattern 3 : Bounce Shot
        {
            // Bullet option
            newOption.bounce = true;
            newOption.speed = 500f;

            // Damage Type
            newDamageType.Add(new DamageType_Single());

            // Bullet Spawn
            newBulletSpawn.Add(new BulletSpawn_Front());
            newBulletSpawn.Add(new BulletSpawn_Angle_45());

            // Add
            AddStrategy(patternIdx++);
        }
    }

    // StateMachine //
    public override void StateMachine()
    {
        switch(state)
        {
            case _StateMachine.Idle:
                Idle();
                break;
            case _StateMachine.Move:
                Move();
                break;
            case _StateMachine.Attack:
                Attack();
                break;
        }
    }
    
    // Actions //
    public override void Idle()
    {
        // Look at target(Player)
        AimToTarget();

        // Wait for delay
        if (DelayTimer())
            ChangeState(_StateMachine.Attack);
    }
    public override bool Move()
    {
        // No more moving
        if (positioning)
            return false;

        // Reach to first destination
        if (navMeshAgent.remainingDistance <= 1f)
        {
            positioning = true;
            navMeshAgent.isStopped= true;
            ChangeState(_StateMachine.Idle);
            return false;
        }
        return true;
    }
    public override void Attack()
    {
        // Look at target(Player)
        AimToTarget();
    }
    public override void Dead()
    {
        // 승리한 상태에서는 죽지 않음
        if (GameManager.instance.gameState == GameManager.GameState.Win)
            return;

        // GameManager에 승리 알림
        GameManager.instance.SetGameState(GameManager.GameState.Win);
        base.Dead();
    }

    // Event funciton(Attack is end) //
    public override void AttackEnd()
    {
        // Next pattern
        attackPattern = (AttackPattern)UnityEngine.Random.Range(0, (int)AttackPattern.BounceShot + 1);
        animator.SetInteger("AttackPattern", (int)attackPattern);

        // Reset timer, Change state
        base.AttackEnd();
    }

}
