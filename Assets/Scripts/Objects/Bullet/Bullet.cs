using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * Player(OnCollision)  : �Ѿ� ���� X, isTrigger X(���ۿ� ���� ��ֹ��� ħ������ �ʱ� ����)
 * Enemy(OnTrigger)     : �Ѿ� ���� O, isTrigger O(NavMesh�� ���� ��ֹ��� ���Ѱ�)
*/

public class Bullet : MonoBehaviour, IObserver<GameManager.GameState>
{
    // Bullet options
    [System.Serializable]
    /// <summary>Bullet�� ������Ÿ��, ƨ�迩��, ���뿩��, ƨ��� Ƚ��(Private)�� ������ ����ü</summary>
    public struct Option
    {
        public List<string> damageType; // Damage type(���� �ִ� 2��)
        public float damage;            // Damage value
        public bool bounce, penetrate;  // Bounce, Penetrate
        public int bounceCount;         // Bounce count
        public float speed;             // Bullet speed

        // Create Functions
        public Option(List<string> damageType)
        {
            this.damageType = damageType;
            damage = 10f;
            bounce = false;
            penetrate = false;
            bounceCount = 3;
            speed = 1000f;
        }
        public Option(float damage)
        {
            damageType      = new List<string>();
            this.damage     = damage;
            bounce          = false;
            penetrate       = false;
            bounceCount     = 3;
            speed           = 1000f;
        }
        public Option(List<string> damageType, float damage, bool bounce, bool penetrate)
        {
            this.damageType = damageType;
            this.damage     = damage;
            this.bounce     = bounce;
            this.penetrate  = penetrate;
            bounceCount     = 3;
            speed           = 1000f;
        }
    }
    public Option option;

    float lifeTime = 5f;
    float lifeTimer = 0f;

    // Unity Component
    private Rigidbody rb;

    // Unity Functions
    void Awake()
    {
        // Get Rigidbody
        rb = GetComponent<Rigidbody>();
        
        // Observer Pattern
        Subscribe();
    }
    void Start()
    {
        // Move forward with rigidbody
        rb.AddForce(transform.forward * option.speed);
    }
    void Update()
    {
        // Life Timer
        lifeTimer += Time.deltaTime;
        if (lifeTimer > lifeTime)
            Destroy();  // Destroy this bullet
    }
    void FixedUpdate()
    {
        // Look at forward
        if (option.bounce && transform.forward != rb.velocity.normalized)
            transform.forward = rb.velocity.normalized;

        // Velocity
        if (rb.velocity.magnitude < 0.8)
        {
            Vector3 v = rb.velocity.normalized;
            v *= option.speed;
            v = v.normalized;
            rb.velocity.Set(v.x, 0, v.z);
        }
    }

    // Player(OnCollision)  : �Ѿ� ���� X, isTrigger X(�÷��̾ ��ֹ��� �������� �ʰ� �ϱ� ����)
    void OnCollisionEnter(Collision collision)
    {
        Hit(collision.gameObject.GetComponent<Character>());
    }
    // Enemy(OnTrigger)     : �Ѿ� ���� O, isTrigger O(NavMesh�� ���� ��ֹ��� ���Ѱ�)
    void OnTriggerEnter(Collider other)
    {
        Hit(other.GetComponent<Character>());
    }
    // Hit Judgment Function
    void Hit(Character target)
    {
        // Wall or Obstract hit
        if (target == null)
        {
            // Bounce
            if (option.bounce && --option.bounceCount > 0)
                return;
        }
        // Character hit
        else
        {
            // Get DamageType's in bullet option
            foreach (var option in option.damageType)
            {
                IDamaged dmgType = (IDamaged)Activator.CreateInstance(Type.GetType(option));
                dmgType.Damaged(target, this.option.damage);
            }
            // Penetrate
            if (option.penetrate)
                return;
        }

        // Destroy this bullet
        Destroy();
    }

    // Destroy
    void Destroy()
    {
        // ObserverPattern
        UnSubscribe();
        // GameObject Destroy
        Destroy(gameObject);
    }

    // Observer Pattern
    void Subscribe()
    {
        GameManager.instance.Subscribe(this);   // GameManager
    }
    void UnSubscribe()
    {
        GameManager.instance.UnSubscribe(this); // GameManager
    }
    public void OnCompleted()
    {
        throw new NotImplementedException();
    }
    public void OnError(Exception error)
    {
        Debug.LogError(error.ToString());
    }
    public void OnNext(GameManager.GameState value)
    {
        // 게임의 결과가 정해지는 순간 Bullet Object 파괴
        switch(value)
        {
            case GameManager.GameState.StageClear:
            case GameManager.GameState.Win:
            case GameManager.GameState.Lose:
                // Position.y -1000
                transform.Translate(0, -1000, 0);
                break;
        }
    }
}
