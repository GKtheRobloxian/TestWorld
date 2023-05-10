using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kilosoult : MonoBehaviour
{

    public GameObject lightningParticles;
    public GameObject spark;
    public Transform playerCam;
    bool teleporting = true;
    public float teleportTimer;
    public float attackTimer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AttackCoroutine());
        StartCoroutine(TeleportCoroutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(attackTimer + Random.Range(-0.5f, 0.5f));

        var randomCoroutine = Random.Range(0, 3);
        if (randomCoroutine == 0)
        {
            StartCoroutine(XLightningCoroutine());
        }
        else if (randomCoroutine == 1)
        {
            StartCoroutine(SoulCoroutine());
        }
        else if (randomCoroutine == 2)
        {
            StartCoroutine(ExplodeCoroutine());
        }
        teleporting = false;
        StopCoroutine(AttackCoroutine());
    }

    IEnumerator TeleportCoroutine()
    {
        if (teleporting)
        {
            yield return new WaitForSeconds(teleportTimer);

            transform.position = new Vector3(Random.Range(-8f, 8f), Random.Range(1f, 8f), Random.Range(-8f, 8f));
            StartCoroutine(TeleportCoroutine());
        }
    }

    IEnumerator XLightningCoroutine()
    {
        yield return new WaitForSeconds(2f);

        Instantiate(lightningParticles, new Vector3(Random.Range(-8f, 8f), 0.5f, Random.Range(-8f, 8f)), Quaternion.identity);
        Instantiate(lightningParticles, new Vector3(playerCam.position.x, 0.5f, playerCam.position.z), Quaternion.identity);;
        Instantiate(lightningParticles, new Vector3(Random.Range(-8f, 8f), 0.5f, Random.Range(-8f, 8f)), Quaternion.identity);

        StartCoroutine(AttackCoroutine());
        teleporting = true;
        StartCoroutine(TeleportCoroutine());
        StopCoroutine(XLightningCoroutine());
    }

    IEnumerator SoulCoroutine()
    {
        yield return new WaitForSeconds(3f);

        Instantiate(spark, transform.position, Quaternion.identity);
        StartCoroutine(AttackCoroutine());
        teleporting = true;
        StartCoroutine(TeleportCoroutine());
        StopCoroutine(XLightningCoroutine());
    }
    
    IEnumerator ExplodeCoroutine()
    {
        yield return new WaitForSeconds(2f);

        GameObject[] sparks = GameObject.FindGameObjectsWithTag("Spark");
        for (int i = 0; i < sparks.Length; i++)
        {
            Destroy(sparks[i]);
        }
            StartCoroutine(AttackCoroutine());
            teleporting = true;
            StartCoroutine(TeleportCoroutine());
            StopCoroutine(XLightningCoroutine());
    }
}
