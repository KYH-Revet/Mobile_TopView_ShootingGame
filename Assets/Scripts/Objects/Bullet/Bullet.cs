using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * Player(OnCollision)  : 총알 관통 X, isTrigger X(조작에 의해 장애물을 침범하지 않기 위함)
 * Enemy(OnTrigger)     : 총알 관통 O, isTrigger O(NavMesh에 의해 장애물을 비켜감)
*/

public class Bullet : MonoBehaviour, IObserver<GameManager.GameState>
{
    // Bullet options
    [System.Serializable]
    /// <summary>Bullet의 데미지타입, 튕김여부, 관통여부, 튕기는 횟수(Private)를 저장한 구조체</summary>
    public struct Option
    {
        public List<string> damageType; // Damage type(현재 최대 2개)
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
        //Move forward with rigidbody
        rb.AddForce(transform.forward * option.speed);
    }
    void Update()
    {
        // Life Timer
        lifeTimer += Time.deltaTime;
        if (lifeTimer > lifeTime)
            Destroy(gameObject);

        // Velocity
        if (rb.velocity.magnitude < 0.8)
        {
            Vector3 v = rb.velocity.normalized;
            v *= option.speed;
            v = v.normalized;
            rb.velocity.Set(v.x, 0, v.z);
        }
    }
    
    void FixedUpdate()
    {
        // Look at forward
        if (option.bounce && transform.forward != rb.velocity.normalized)
            transform.forward = rb.velocity.normalized;
    }
    // Player(OnCollision)  : 총알 관통 X, isTrigger X(플레이어가 장애물을 관통하지 않게 하기 위함)
    void OnCollisionEnter(Collision collision)
    {
        Hit(collision.gameObject.GetComponent<Character>());
    }
    // Enemy(OnTrigger)     : 총알 관통 O, isTrigger O(NavMesh에 의해 장애물을 비켜감)
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

        // Observer Pattern
        UnSubscribe();

        // Bullet Destroy
        Destroy(gameObject);
    }

    // Observer Pattern
    void Subscribe()
    {
        //GameManager.instance.Subscribe(this);   //GameManager
    }
    void UnSubscribe()
    {
        GameManager.instance.UnSubscribe(this);
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
        UnSubscribe();
        Destroy(gameObject);
    }
}
