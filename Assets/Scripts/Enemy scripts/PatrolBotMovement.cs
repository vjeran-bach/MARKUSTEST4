using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBotMovement : MonoBehaviour
{

    public float dir; // -1 - LEFT, 1 - Right

    Rigidbody2D body;
    BoxCollider2D collider_player;
    float distToGround;

    [SerializeField] private LayerMask doNotCollide;

    [SerializeField] GameObject BulletUp;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        collider_player = GetComponent<BoxCollider2D>();
        distToGround = collider_player.bounds.extents.y;

        doNotCollide = ~doNotCollide;

        dir = 1;

        StartCoroutine(DoTimer(1, Random.Range(1, 4)));
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGroundedRight(doNotCollide) == true || IsGroundedRightDown(doNotCollide) == false)
        {
            dir = -1;
        }

        if (IsGroundedLeft(doNotCollide) == true || IsGroundedLeftDown(doNotCollide) == false)
        {
            dir = 1;
        }

        body.velocity = new Vector2(dir * 10, body.velocity.y);
    }

    bool IsGroundedRight(LayerMask mask)
    {
        return Physics2D.Raycast(new Vector2(transform.position.x + distToGround + 1, transform.position.y), Vector2.right, 1, mask.value);
    }
    bool IsGroundedLeft(LayerMask mask)
    {
        return Physics2D.Raycast(new Vector2(transform.position.x - distToGround - 1, transform.position.y), Vector2.left, 1, mask.value);
    }

    bool IsGroundedRightDown(LayerMask mask)
    {
        return Physics2D.Raycast(new Vector2(transform.position.x + distToGround + 1, transform.position.y), new Vector2(1, -1), 2, mask.value);
    }
    bool IsGroundedLeftDown(LayerMask mask)
    {
        return Physics2D.Raycast(new Vector2(transform.position.x - distToGround - 1, transform.position.y), new Vector2(-1, -1), 2, mask.value);
    }

    IEnumerator DoTimer(float countTime = 1f, float waitTime = 1f)
    {
        int count = 0;

        while (true)
        {
            yield return new WaitForSeconds(countTime);
            count++;

            if (count > waitTime)
            {
                Instantiate(BulletUp, new Vector2(transform.position.x, transform.position.y + 1), BulletUp.transform.rotation);
                StartCoroutine(DoTimer(1, 1));
                yield break;
            }
        }
    }
}
