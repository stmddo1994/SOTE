using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    
    BoxCollider2D hitBox;
    public int attackDamage = 3;
    public float knockback = 0f;


    private void Awake()
    {
        hitBox = GetComponent<BoxCollider2D>();
    }
    // Start is called before the first frame update
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Can be hit
        Damagable damageable = collision.GetComponent<Damagable>();
        if (damageable != null)
        {
            //transform.parent.localScale.x indicates direction of knockback based on sprite direction
            damageable.TakeDamage(attackDamage, transform.parent.localScale.x * knockback);
            Debug.Log(collision.name + "hit for" + attackDamage);
        }
    }
}
