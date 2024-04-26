using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damagable : MonoBehaviour
{
    public int maxHealth = 20;
    public int health;
    [SerializeField]
    private bool isAlive = true;
    [SerializeField]
    private bool isInvincible = false;
    public bool isHit = false;
    Animator userAnimation;
    private float timeSinceHit;
    public float invincibilityTime = 0.25f;
    public float knockbackTake = 0f;
    // Start is called before the first frame update
    private void Awake()
    {
        health = maxHealth;
        userAnimation = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isInvincible)
        {
            if(timeSinceHit > invincibilityTime)
            {
                //Remove invincibility
                isInvincible = false;
                timeSinceHit = 0;
                isHit = false;
            }
            timeSinceHit += Time.deltaTime;
        }
        userAnimation.SetBool("isAlive", isAlive);
        userAnimation.SetBool("isHit", isHit);
    }

    //damage from other
    public void TakeDamage(int damage, float knockback)
    {
        if (isAlive && !isInvincible)
        {
            health -= damage;
            isInvincible = true;

            //Notify components about knockback
            isHit = true;
            knockbackTake = knockback;
            if (health <= 0)
            {
                isAlive = false;
            }
        }
    }

}
