using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kilosoult : MonoBehaviour
{
    public GameObject spark;
    public GameObject dashTrail;
    public GameObject buildUp;
    public GameObject lightningX;
    public Transform playerCam;
    bool teleporting = true;
    bool souled = false;
    public float teleportTimer;
    public float attackTimer;
    int soulTimer = 0;
    Light lighting;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AttackCoroutine());
        StartCoroutine(TeleportCoroutine());
        lighting = gameObject.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(attackTimer + Random.Range(-0.5f, 0.5f));
        if (soulTimer >= 2)
        {
            StartCoroutine(ExplodeCoroutine());
        }
        else
        {
            var randomCoroutine = Random.Range(0, 10);
            if (randomCoroutine <= 4)
            {
                StartCoroutine(XLightningCoroutine());
            }
            else if (randomCoroutine <= 6 && randomCoroutine > 4)
            {
                StartCoroutine(SoulCoroutine());
            }
            else if (randomCoroutine > 6)
            {
                StartCoroutine(DashCoroutine());
            }
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
        yield return new WaitForSeconds(1f);

        Instantiate(lightningX, new Vector3(Random.Range(-8f, 8f), 0.5f, Random.Range(-8f, 8f)), Quaternion.identity);

        if (souled == true)
        {
            soulTimer += 1;
        }

        StartCoroutine(AttackCoroutine());
        teleporting = true;
        StartCoroutine(TeleportCoroutine());
        StopCoroutine(XLightningCoroutine());
    }

    IEnumerator SoulCoroutine()
    {
        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < 3; i ++)
        {
            Instantiate(spark, transform.position + Vector3.right * Random.Range(-3.0f, 3.0f), Quaternion.identity);
        }
        StartCoroutine(AttackCoroutine());
        teleporting = true;
        souled = true;
        soulTimer += 1;
        StartCoroutine(TeleportCoroutine());
        StopCoroutine(SoulCoroutine());
    }
    
    IEnumerator ExplodeCoroutine()
    {
        yield return new WaitForSeconds(2f);

        GameObject[] sparks = GameObject.FindGameObjectsWithTag("Spark");
        for (int i = 0; i < sparks.Length; i++)
        {
            sparks[i].GetComponent<FollowPlayer>().SparkExplode();
        }
        StartCoroutine(AttackCoroutine());
        teleporting = true;
        souled = false;
        soulTimer = 0;
        StartCoroutine(TeleportCoroutine());
        StopCoroutine(ExplodeCoroutine());
    }

    IEnumerator DashCoroutine()
    {
        lighting.range = 12f;
        lighting.intensity = 4f;
        buildUp.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        dashTrail.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(teleportTimer / 3f);
            transform.position = new Vector3(Random.Range(-8f, 8f), Random.Range(1f, 8f), Random.Range(-8f, 8f));
        }
        if (souled == true)
        {
            soulTimer += 1;
        }

        StartCoroutine(AttackCoroutine());
        teleporting = true;
        lighting.range = 6f;
        lighting.intensity = 2f;
        dashTrail.SetActive(false);
        buildUp.SetActive(false);
        StartCoroutine(TeleportCoroutine());
        StopCoroutine(DashCoroutine());
    }
}
