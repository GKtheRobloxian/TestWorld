using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    float left;
    float forward;
    public float fireRateInitial = 0.49f;
    public float fireRate;
    bool grounded = true;
    public GameObject[] projHit;
    public GameObject[] projTrails;
    public float[] reloadTimes;
    float weaponScrollWheel = 1;
    public float maxDashStamina;
    float dashStamina;
    bool dashing = false;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        fireRateInitial = reloadTimes[Mathf.FloorToInt(weaponScrollWheel)];
        dashStamina = maxDashStamina;
        fireRate = fireRateInitial;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dashing)
        {
            dashStamina += Time.deltaTime * (1f/3f);
        }
        if (dashStamina >= maxDashStamina)
        {
            dashStamina = maxDashStamina;
        }
        else if (dashStamina <= 0)
        {
            dashStamina = 0;
        }
        GameObject.Find("Dash Stamina").GetComponent<DashBar>().SetValue(dashStamina);
        BasicMovement();
        Shooting();
        AdvancedMovement();
    }

    void BasicMovement()
    {
        left = Input.GetAxis("Horizontal");
        forward = Input.GetAxis("Vertical");
        if (!dashing)
        {
            transform.Translate(Vector3.forward * 15f * forward * Time.deltaTime);
            transform.Translate(Vector3.right * 15f * left * Time.deltaTime);
        }
        transform.rotation = Quaternion.Euler(new Vector3(0, GameObject.Find("PlayerCam").transform.localEulerAngles.y, 0));
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddRelativeForce(Vector3.up*10f, ForceMode.Impulse);
            grounded = false;
        }
    }

    void AdvancedMovement()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (dashStamina > 0)
            {
                dashStamina -= Time.deltaTime;
                dashing = true;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                transform.Translate(Vector3.forward * 30f * Time.deltaTime);
            }
        }
        else
        {
            dashing = false;
        }
    }

    void UpdateReload(int weaponChosen)
    {
        fireRateInitial = reloadTimes[weaponChosen];
        fireRate = fireRateInitial;
    }

    void Shooting()
    {
        fireRate -= Time.deltaTime;
        weaponScrollWheel += Input.mouseScrollDelta.y;
        int actualScroll = Mathf.FloorToInt(Mathf.Abs(weaponScrollWheel) % projTrails.Length);
        if (Input.mouseScrollDelta.y != 0)
        {
            UpdateReload(actualScroll);
        }
        GameObject.Find("Reload Timer").GetComponent<ReloadTimer>().SetValue(fireRateInitial, fireRate);
        if (Input.GetMouseButtonDown(0) && fireRate <= 0)
        {
            Vector3 startRaycast = transform.position;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = 1<<LayerMask.GetMask("TrailTrigger");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                fireRate = fireRateInitial;
                Instantiate(projHit[actualScroll], hit.point, Quaternion.identity);
                projTrails[actualScroll].GetComponent<ProjectileTrail>().SetPosition(hit.point, startRaycast);
                Damageable damaging = hit.collider.gameObject.GetComponent<Damageable>();
                if (damaging == null)
                {
                    return;
                }
                DamageField damageScript = projTrails[actualScroll].GetComponent<DamageField>();
                if (damageScript == null)
                {
                    return;
                }
                else
                {
                    float damageAmount = damageScript.damage;
                    damaging.Damaged(damageAmount);
                    if (damageScript.eleCannon)
                    {
                        damageAmount = 0;
                        GetComponent<Damageable>().damageTaken = 0;
                    }
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == GameObject.Find("Ground"))
        {
            grounded = true;
        }
    }
}
