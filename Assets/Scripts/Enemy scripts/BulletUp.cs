using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletUp : MonoBehaviour
{

    Rigidbody2D body;
    [SerializeField] float speed;
    bool triggerCollision;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        body = gameObject.GetComponent<Rigidbody2D>();

        if (player.transform.position.x > gameObject.transform.position.x)
        {
            body.velocity = new Vector2(speed, 0);
        }
        else
        {
            body.velocity = new Vector2(-speed, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(DoTimer(0.1f));
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (triggerCollision == true)
        {   
        Destroy(gameObject);
        }
    }

    IEnumerator DoTimer(float countTime = 0.1f)
    {
        int count = 0;

        while (true)
        {
            yield return new WaitForSeconds(countTime);
            count++;

            if (count > 0.1f)
            {
                triggerCollision = true;
                yield break;
            }
        }
    }

}
