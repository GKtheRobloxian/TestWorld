using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    float left;
    float forward;
    float fireRateInitial = 0.49f;
    float fireRate;
    bool grounded = true;
    public GameObject projHit;
    public GameObject projTrail;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        fireRate = fireRateInitial;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        BasicMovement();
        Shooting();
    }

    void BasicMovement()
    {
        left = Input.GetAxis("Horizontal");
        forward = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * 15f * forward * Time.deltaTime);
        transform.Translate(Vector3.right * 15f * left * Time.deltaTime);
        transform.rotation = Quaternion.Euler(new Vector3(0, GameObject.Find("PlayerCam").transform.localEulerAngles.y, 0));
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddRelativeForce(Vector3.up*10f, ForceMode.Impulse);
            grounded = false;
        }
    }

    void AdvancedMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rb.velocity = new Vector3 (0, rb.velocity.y, 0);
            rb.AddRelativeForce(Vector3.forward * 20f, ForceMode.Impulse);
        }
    }

    void Shooting()
    {
        fireRate -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && fireRate <= 0)
        {
            Vector3 startRaycast = transform.position;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = 1<<LayerMask.GetMask("TrailTrigger");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                fireRate = fireRateInitial;
                Instantiate(projHit, hit.point, Quaternion.identity);
                projTrail.GetComponent<ProjectileTrail>().SetPosition(hit.point, startRaycast);
                Damageable damaging = hit.collider.gameObject.GetComponent<Damageable>();
                if (damaging == null)
                {
                    return;
                }
                hit.collider.gameObject.GetComponent<Damageable>().Damaged(5);
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
