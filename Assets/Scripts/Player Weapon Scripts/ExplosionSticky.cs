using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSticky : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DoTimer(1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("AAA");
        if (col.gameObject.tag == "Enemy")
        {
            Destroy(col.gameObject);
        }
    }


    IEnumerator DoTimer(float countTime = 1f)
    {
        int count = 0;

        while (true)
        {
            yield return new WaitForSeconds(countTime);
            count++;
            Debug.Log(count);

            if (count == 1)
            {
                Destroy(gameObject);
                yield break;
            }
        }
    }
}
