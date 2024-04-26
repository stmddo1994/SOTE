using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private Rigidbody2D enemy;
    private BoxCollider2D coll;
    private Animator enemyAnimation;

    private ContactFilter2D CF;
    Damagable damagable;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        enemyAnimation = GetComponent<Animator>();
        damagable = GetComponent<Damagable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (damagable.isHit)
        {
            enemy.velocity = new Vector2(damagable.knockbackTake, enemy.velocity.y);
        }
        enemyAnimation.SetBool("isGrounded", IsGrounded());
    }
    private bool IsGrounded()
    {
        //return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, ground);
        return enemy.IsTouching(CF);
    }
}
