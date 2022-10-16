using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestMovement : MonoBehaviour
{

    float a;
    float b;
    float d;
    float c;
    float bounceMultiplier2;

    //declare variables
    float distToGround;
    private Rigidbody2D body;
    BoxCollider2D collider_player;
    Vector2 otherForcesInitialJump;
    Vector2 otherForces;
    float adaptiveForceX;
    float counterWallJump;
    float xDiff;
    float yDiff;
    Vector2 mouseJumpDirSlowTime;

    //TRIGERS
    bool triggerWallJumpLeft;
    bool triggerWallJumpRight;
    bool triggerDoubleJump;
    bool bTimeSlowTrigger;

    // SERIALIZED FIELDS
    [SerializeField] float wallJumpStrength;
    [SerializeField] float wallJumptrengthY;
    [SerializeField] float speed;
    [SerializeField] private LayerMask doNotCollide;
    [SerializeField] float jumpStrength;
    [SerializeField] float otherSpeed;
    [SerializeField] float otherForcesY;
    [SerializeField] float frictionFactor;
    [SerializeField] float frictionFactorOnMove;
    [SerializeField] float bounceMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        counterWallJump = 0;
        body = GetComponent<Rigidbody2D>();
        collider_player = GetComponent<BoxCollider2D>();
        distToGround = collider_player.bounds.extents.y;

        //Sets layers not to collide with
        doNotCollide = ~doNotCollide;
    }

    // Update is called once per frame
    void Update()
    {

        body.freezeRotation = true;

        CheckSlowTimeWallJump();
        WallJumpSlowTime();

        ResetTriggers();
        WalkDefualt();
        Jump();
        DoubleJump();
        
    }

    private void FixedUpdate()
    {
        //otherForces = new Vector2(Mathf.Clamp(otherForces.x * (1 - Time.deltaTime * 0.1f), 0, otherSpeed), otherForces.y);
    }


    //Walk left and right
    void WalkDefualt()
    {

        if (body.velocity.x <= speed + 5 && triggerWallJumpLeft == true) // POTENTIAL BUG
        {
            otherSpeed = 20;
        }

        if (body.velocity.x >= -speed - 5 && triggerWallJumpRight == true) // POTENTIAL BUG
        {
            otherSpeed = 20;
        }

        //NOR LEFT NOR RIGHT
        if (triggerWallJumpLeft == false && triggerWallJumpRight == false)
        {
            body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);
        }

        //LEFT
        if (triggerWallJumpLeft == true && triggerWallJumpRight == false)
        {
            counterWallJump += frictionFactor * Time.deltaTime;
            otherForces = new Vector2(Mathf.Clamp(otherForces.x - counterWallJump / otherForces.x, 0, otherSpeed), otherForces.y);

            //Initial bounce off the wall
            otherForcesInitialJump = new Vector2(Mathf.Clamp(Time.deltaTime * 10, 0, otherSpeed), Mathf.Clamp(Time.deltaTime * 10, 0, otherSpeed));
            if (otherForcesInitialJump.x == otherSpeed)
            {
                otherForcesInitialJump.x = 0;
            }
            // END


            if (Input.GetAxis("Horizontal") > 0)
            {
                if (otherForces.x != 0)
                {
                    body.velocity = new Vector2(speed / 2 + otherForces.x, body.velocity.y);
                }
                else
                {
                    body.velocity = new Vector2(speed, body.velocity.y);
                }
                counterWallJump += frictionFactorOnMove * Time.deltaTime;
            }

            if (Input.GetAxis("Horizontal") < 0)
            {
                counterWallJump += frictionFactorOnMove * Time.deltaTime;
                body.velocity = new Vector2(-speed + otherForces.x, body.velocity.y);
            }

            if (Input.GetAxis("Horizontal") == 0)
            {
                body.velocity = new Vector2(otherForces.x, body.velocity.y);
            }
        }

        //RIGHT

        if (triggerWallJumpRight == true && triggerWallJumpLeft == false)
        {
            counterWallJump += frictionFactor * Time.deltaTime;
            otherForces = new Vector2(Mathf.Clamp(otherForces.x + counterWallJump / Mathf.Abs(otherForces.x), -otherSpeed, 0), otherForces.y);

            //Initial bounce off the wall
            otherForcesInitialJump = new Vector2(Mathf.Clamp(Time.deltaTime * 10, 0, otherSpeed), Mathf.Clamp(Time.deltaTime * 10, 0, otherSpeed));
            if (otherForcesInitialJump.x == otherSpeed)
            {
                otherForcesInitialJump.x = 0;
            }
            // END


            if (Input.GetAxis("Horizontal") < 0)
            {
                if (otherForces.x != 0)
                {
                    body.velocity = new Vector2(-speed / 2 + otherForces.x, body.velocity.y);
                }
                else
                {
                    body.velocity = new Vector2(-speed, body.velocity.y);
                }
                counterWallJump += frictionFactorOnMove * Time.deltaTime;
            }

            if (Input.GetAxis("Horizontal") > 0)
            {
                counterWallJump += frictionFactorOnMove * Time.deltaTime;
                body.velocity = new Vector2(speed + otherForces.x, body.velocity.y);
            }

            if (Input.GetAxis("Horizontal") == 0)
            {
                body.velocity = new Vector2(otherForces.x, body.velocity.y);
            }
        }


    }

    //END

    //Jumping
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (IsGrounded(doNotCollide))
            {
                triggerDoubleJump = true;
                body.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
            }
            else if (IsGroundedLeft(doNotCollide) && bTimeSlowTrigger == false)
            {
                triggerWallJumpLeft = true;
                triggerWallJumpRight = false;
                otherForces = new Vector2(otherSpeed, 0);
                body.velocity = new Vector2(body.velocity.x, 0); // TEST
                body.AddForce(new Vector2(0, wallJumpStrength), ForceMode2D.Impulse);
                counterWallJump = 0;
            }

            else if (IsGroundedRight(doNotCollide) && bTimeSlowTrigger == false)
            {
                triggerWallJumpRight = true;
                triggerWallJumpLeft = false;
                body.velocity = new Vector2(body.velocity.x, 0); // TEST
                otherForces = new Vector2(-otherSpeed, 0);
                body.AddForce(new Vector2(-otherSpeed, wallJumpStrength), ForceMode2D.Impulse);
                counterWallJump = 0;

            }

        }
    }
    //END

    // DOUBLE JUMP
    void DoubleJump()
    {
        if (triggerDoubleJump == true && Input.GetKeyDown(KeyCode.W) && IsGrounded(doNotCollide) == false && IsGroundedLeft(doNotCollide) == false && IsGroundedRight(doNotCollide) == false)
        {
            body.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
            triggerDoubleJump = false;
        }
    }
    // END

    //Is grounded and touching walls checks methods

    bool IsGrounded(LayerMask mask)
    {
        Debug.DrawRay(transform.position, Vector2.down * distToGround, Color.red);
        return Physics2D.Raycast(transform.position, Vector2.down, distToGround + 0.3f, mask.value);
    }

    bool IsGroundedRight(LayerMask mask)
    {
        return Physics2D.Raycast(transform.position, Vector2.right, distToGround + 0.5f, mask.value);
    }
    bool IsGroundedLeft(LayerMask mask)
    {
        return Physics2D.Raycast(transform.position, Vector2.left, distToGround + 0.5f, mask.value);
    }

    //END

    //reset triggers if on ground
    void ResetTriggers()
    {
        if (IsGrounded(doNotCollide))
        {
            triggerWallJumpLeft = false;
            triggerWallJumpRight = false;
        }
    }
    //END

        //-------------------- SLOW TIME WALL JUMP

    void CheckSlowTimeWallJump()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            bTimeSlowTrigger = true;
        }
        else
        {
            bTimeSlowTrigger = false;
        }
    }

    void WallJumpSlowTime()
    {

        if (Input.GetKey(KeyCode.LeftShift))
        {
            bTimeSlowTrigger = true;
        }

        if (bTimeSlowTrigger == true)
        {
            if (IsGroundedLeft(doNotCollide))
            {
                Time.timeScale = 0.15f;

                if (Input.GetMouseButtonDown(1))
                {
                    CalculateSlowMoWallJumpLeft();
                }
            }

            else if (IsGroundedRight(doNotCollide))
            {
                Time.timeScale = 0.15f;

                if (Input.GetMouseButtonDown(1))
                {
                    CalculateSlowMoWallJumpRight();
                }
            }
        }
        else if (bTimeSlowTrigger == false)
        {
            Time.timeScale = 1f;
        }
    }

    Vector2 ArrowJumping()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);




        xDiff = mousePos.x - gameObject.transform.position.x;
        yDiff = mousePos.y - gameObject.transform.position.y;

        mouseJumpDirSlowTime = new Vector2(xDiff, yDiff).normalized;

        return mouseJumpDirSlowTime;
    }

    //END

    void CalculateSlowMoWallJumpLeft()
    {
        counterWallJump = 0;
        otherForces = new Vector2(0, 0);
        body.velocity = new Vector2(0, 0);


        a = ArrowJumping().x;
        b = ArrowJumping().y;

        bounceMultiplier2 = 55 / (Mathf.Sqrt(Mathf.Pow(a, 2) + Mathf.Pow(b, 2)));

        c = a * bounceMultiplier2;
        d = b * bounceMultiplier2;

        otherSpeed = c;
        otherForces = new Vector2(c, 0);
        body.AddForce(new Vector2(c, d + Physics2D.gravity.magnitude), ForceMode2D.Impulse);




        triggerWallJumpLeft = true;
        triggerWallJumpRight = false;
    }

    void CalculateSlowMoWallJumpRight()
    {
        counterWallJump = 0;
        otherForces = new Vector2(0, 0);
        body.velocity = new Vector2(0, 0);


        a = ArrowJumping().x;
        b = ArrowJumping().y;

        bounceMultiplier2 = 55 / (Mathf.Sqrt(Mathf.Pow(a, 2) + Mathf.Pow(b, 2)));

        c = a * bounceMultiplier2;
        d = b * bounceMultiplier2;

        otherSpeed = Mathf.Abs(c);
        otherForces = new Vector2(c, 0);
        body.AddForce(new Vector2(c, d + Physics2D.gravity.magnitude), ForceMode2D.Impulse);




        triggerWallJumpLeft = false;
        triggerWallJumpRight = true;
    }
}