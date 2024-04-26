using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 8f;
    private float xAxis = 0f;
    private Rigidbody2D player;

    private BoxCollider2D coll;
    [SerializeField] LayerMask ground;
    public float maxJumpTime = 0.35f;
    public float jumpTimeCounter;
    private bool isJumping;
    private ContactFilter2D CF;

    private Animator playerAnimation;

    public float lungeDelay = 0.1f;
    public float lungeDelayCounter;
    public float lungeDuration = 0.2f;
    public float lungeDurationCounter;
    public float lungeSpeed = 2.0f;

    public bool isAttacking = false;
    public float attackingTime = 0.2f;
    public float attackingCounter;

    Damagable damagable;
 
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        playerAnimation = GetComponent<Animator>();
        damagable = GetComponent<Damagable>();
        attackingCounter = -(attackingTime);
    }

    // Update is called once per frame
    void Update()
    {
        //Move()
        if (CanMove)
        {
            xAxis = Input.GetAxis("Horizontal");
            if (xAxis > 0f)
            {
                player.velocity = new Vector2(xAxis * speed, player.velocity.y);
                //flip sprite
                if (IsGrounded())
                {
                    transform.localScale = new Vector2(1f, 1f);
                }
            }
            else if (xAxis < 0f)
            {
                player.velocity = new Vector2(xAxis * speed, player.velocity.y);
                //flip sprite
                if (IsGrounded())
                {
                    transform.localScale = new Vector2(-1f, 1f);
                }
            }
            else
            {
                player.velocity = new Vector2(0, player.velocity.y);
            }
        }
        else
        {
            player.velocity = new Vector2(0, player.velocity.y);
        }
        //Jump()
        if (Input.GetButtonDown("Jump") && IsGrounded() && CanMove)
        {
            isJumping = true;
            jumpTimeCounter = maxJumpTime;
            player.velocity = new Vector2(player.velocity.x, jumpForce);
        }
        if (Input.GetButton("Jump") && isJumping)
        {
            if(jumpTimeCounter > 0)
            {
                player.velocity = new Vector2(player.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
        //Attack()
        if (Input.GetButtonDown("Fire1"))
        {
            playerAnimation.SetTrigger("attack");
            //next bit of code gives indication for rapid combo move
            if (attackingCounter + attackingTime < attackingTime * 2)
            {
                attackingCounter += attackingTime;
            }
            else
            {
                attackingCounter = attackingTime * 2;
            }
        }
        if (attackingCounter > -(attackingTime))
        {
            attackingCounter -= Time.deltaTime;
        }

        if (attackingCounter > 0)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }

        if (LungeAttack) //player moves forward with first attack
        {
            if(lungeDelayCounter > 0)
            {
                lungeDelayCounter -= Time.deltaTime;
            }
            else
            {
                if(lungeDurationCounter > 0)
                {
                    player.velocity = new Vector2(transform.localScale.x * lungeSpeed, 0);
                    lungeDurationCounter -= Time.deltaTime;
                }
                else
                {
                    player.velocity = new Vector2(0, 0);
                }
            }
        }
        else
        {
            lungeDelayCounter = lungeDelay;
            lungeDurationCounter = lungeDuration;
        }
        //knockback
        if (damagable.isHit)
        {
            player.velocity = new Vector2(damagable.knockbackTake, player.velocity.y);
        }

        //Animations
        playerAnimation.SetFloat("Speed", Mathf.Abs(player.velocity.x));
        playerAnimation.SetBool("isGrounded", IsGrounded());
        playerAnimation.SetFloat("yVelocity", player.velocity.y);
        playerAnimation.SetBool("isAttacking", isAttacking);
       
        
    }

    private bool IsGrounded()
    {
        //return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, ground);
        return player.IsTouching(CF);
    }

    public bool CanMove //ensure player can't move in certain animations
    {
        get
        {
            return playerAnimation.GetBool("canMove");
        }
    }

    public bool LungeAttack //player moves forward with first attack
    {
        get
        {
            return playerAnimation.GetBool("lungeAttack");
        }
    }
}
