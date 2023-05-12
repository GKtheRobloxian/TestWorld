using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject explodeParticle;
    Transform playerPos;
    Vector3 randomPos;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Player").transform;
        randomPos = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerPos.position + randomPos);

        transform.Translate(Vector3.forward * 5f * Time.deltaTime);
    }

    public void SparkExplode()
    {
        Instantiate(explodeParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
