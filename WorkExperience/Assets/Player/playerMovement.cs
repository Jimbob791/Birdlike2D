using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public GameObject groundTestObj;
    public LayerMask platformLayers;

    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private ParticleSystem pfx;

    public float playerSpeed = 1f;
    public float jumpPower = 1f;
    public float dashDistance = 3f;
    public float counterJumpPower = 1f;

    public bool airControl;
    public bool canDash = false;
    public bool canDJump = false;

    public float downForce = 1f;
    public float jumpGraceTime = 1f;

    bool isGrounded = false;
    bool isJumping = false;
    bool isDashing = false;
    bool hasDJumped = false;

    bool jumpPressed = false;
    bool dashPressed = false;

    Vector2 vel;
    Vector2 jumpForce;

    float inputY;
    float inputX;
    float lastInputX = 1;
    float falling = 0;
    float moveForce = 0;

    float? lastGroundedTime;
    float? lastJumpPressedTime;
    float? lastTimeDashed;

    void Start()
    {
        if(GameObject.Find("LoadManager").GetComponent<saveController>() != null)
        {
            saveController con = GameObject.Find("LoadManager").GetComponent<saveController>();
            canDash = con.hasDash;
            canDJump = con.hasDJump;
        }
    }

    void Update()
    {
        if(GameObject.Find("LoadManager").GetComponent<saveController>() != null)
        {
            saveController con = GameObject.Find("LoadManager").GetComponent<saveController>();
            canDash = con.hasDash;
            canDJump = con.hasDJump;
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpPressed = true;
            lastJumpPressedTime = Time.time;

            Jump(false);
        }
        else if(Input.GetKeyUp(KeyCode.UpArrow))
        {
            jumpPressed = false;
            if(!isGrounded && isJumping && rb.velocity.y > 0)
            {   
                Vector2 counterJumpForce = Vector2.down * counterJumpPower;
                rb.AddForce(counterJumpForce);
            }
        }

        if(Input.GetKeyDown(KeyCode.C) && canDash)
        {
            StartCoroutine(Dash(lastInputX));
        }
        
        if(Input.GetKey(KeyCode.RightArrow))
        {
            inputX = 1;
            lastInputX = 1;
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            inputX = -1;
            lastInputX = -1;
        } else 
        {
            inputX = 0;
        }

        gameObject.transform.localScale = new Vector3(lastInputX * 0.9f, 0.9f, 0.9f);
        anim.SetFloat("yVel", falling);
        if(inputX != 0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }


    void FixedUpdate()
    {
        if(isGrounded)
        {
            rb.gravityScale = 2.1f;
            lastGroundedTime = Time.time;
        }
        else if(rb.velocity.y < 0 && isJumping)
        {
            rb.gravityScale += downForce;
            falling = -1;
        }

        if(!isDashing)
        {
            moveForce = Mathf.MoveTowards(moveForce, inputX * playerSpeed, 0.8f);
            rb.velocity = new Vector2(moveForce, rb.velocity.y);
        }
        
        if(Physics2D.BoxCast(groundTestObj.transform.position, new Vector2(0.4f, 0.01f), 0f, new Vector2(0, 0), platformLayers))
        {
            if(!isGrounded)
            {
                isGrounded = true;
                falling = 0;
                hasDJumped = false;
                isJumping = false;
                rb.gravityScale = 2.1f;
                if(Time.time - lastJumpPressedTime <= jumpGraceTime && Input.GetKey(KeyCode.UpArrow))
                {
                    Jump(true);
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.tag == "Platform")
        {
            isGrounded = false;
        }
    }

    void Jump(bool forceJump)
    {
        if(forceJump)
        {
            jumpForce = Vector2.up * jumpPower;
            vel = rb.velocity;
            vel.y = jumpForce.y;
            rb.velocity = vel;
            isGrounded = false;
            isJumping = true;
            falling = 1;
        } 
        else 
        {
            if(canDJump && isJumping && !hasDJumped)
            {
                jumpForce = Vector2.up * jumpPower;
                vel = rb.velocity;
                vel.y = jumpForce.y;
                rb.velocity = vel;
                isGrounded = false;
                isJumping = true;
                hasDJumped = true;
                falling = 1;

                return;
            } 

            if(!canDJump && isJumping)
            {
                return;
            }
            
            if(!canDJump && Time.time - lastGroundedTime > jumpGraceTime / 2)
            {
                return;
            }
            else if(canDJump && Time.time - lastGroundedTime > jumpGraceTime / 2 && hasDJumped)
            {
                return;
            }

            jumpForce = Vector2.up * jumpPower;
            vel = rb.velocity;
            vel.y = jumpForce.y;
            rb.velocity = vel;
            isGrounded = false;
            isJumping = true;
            falling = 1;

            return;
        }
    }

    IEnumerator Dash(float direction)
    {
        isDashing = true;
        pfx.Play();
        anim.SetBool("isDashing", true);
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(new Vector2(dashDistance * direction, 0f), ForceMode2D.Impulse);
        rb.gravityScale = 0f;
        yield return new WaitForSeconds(0.2f);
        rb.gravityScale = 2.1f;
        isDashing = false;
        anim.SetBool("isDashing", false);
    }
}
