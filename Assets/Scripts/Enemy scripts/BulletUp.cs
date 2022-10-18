using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletUp : MonoBehaviour
{

    Rigidbody2D body;
    [SerializeField] float speed;
    bool triggerCollision;

    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();

        body.velocity = new Vector2(0, speed);
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
