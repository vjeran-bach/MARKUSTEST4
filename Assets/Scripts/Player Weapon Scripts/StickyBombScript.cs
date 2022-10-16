using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBombScript : MonoBehaviour
{
    [SerializeField] float iStickyRange;
    [SerializeField] public float moveSpeed;

    GameObject currentObj;

    Rigidbody2D body;

    Vector2 vectorToEnemy;

    public GameObject explosion;

    public GameObject explosionNew;


    // Start is called before the first frame update
    void Awake()
    {
        currentObj = FindClosestEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        body = gameObject.GetComponent<Rigidbody2D>();

        vectorToEnemy = currentObj.transform.position - gameObject.transform.position;
        vectorToEnemy = vectorToEnemy.normalized;

        body.velocity = vectorToEnemy * moveSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        explosionNew = Instantiate(explosion, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), transform.rotation);
        Destroy(gameObject);
    }


    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}
