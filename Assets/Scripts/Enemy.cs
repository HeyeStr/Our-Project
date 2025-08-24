using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPawn, IEntity
{
    Rigidbody2D rb;
    int health;
    bool isGrounded;
    CapsuleCollider2D bd;
    BoxCollider2D tg;
    [SerializeField] float jumpForce = 300f; // ��Ծ����
    [SerializeField] float moveSpeed = 5f; // �ƶ��ٶ�
    [SerializeField] int maxHealth = 100; // �������ֵ
    [SerializeField] GameObject[] abilities; // ���ܶ���
    [SerializeField] GameObject[] effectAppliedOnAbsorb; // ����ʱӦ�õ�Ч��

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        tg = GetComponent<BoxCollider2D>();
        bd = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth; // ��ʼ������ֵ
        tag = "Enemy"; // ���ñ�ǩΪ Enemy
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ��ײ
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            var player = collision.gameObject.GetComponent<IEntity>();
            if ((player != null) && GetComponent<IEntity>().IsDead())
            {
                Physics2D.IgnoreCollision(bd, collision.collider);
            }
        }
    }

    // ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<IEntity>();
            if (player != null)
            {
                Debug.Log("Enemy triggered by Player");
            }
        }
    }

    // �ڲ��߼�
    private void Die()
    {
        if (rb == null)
        {
            return;
        }
        Debug.Log($"{tag} has died.");
        rb.velocity = Vector2.zero; // ֹͣ��ɫ�ƶ�
        rb.constraints = RigidbodyConstraints2D.None;
    }

    // ʵ�ֽӿ�IPawn
    public void Move(float direction)
    {
        Vector2 movement = new Vector2(direction * moveSpeed, rb.velocity.y);
        rb.velocity = movement; // ���ø�����ٶ�
    }

    // ʵ�ֽӿ�IEntity
    public void Damaged(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
        Debug.Log($"{tag} took {damage} damage, current health: {health}");
        if (health <= 0)
        {
            // �����ɫ�����߼�
            Die();
        }
    }
    public bool IsDead()
    {
        return health <= 0;
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public GameObject[] GetAbilities()
    {
        return abilities;
    }
    public bool IsCreature()
    {
        return true;
    }
    public GameObject[] GetEffects()
    {
        return effectAppliedOnAbsorb;
    }
}
