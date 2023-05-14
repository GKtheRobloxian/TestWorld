using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject objectTracking;
    float healthy;
    public Slider sliding;
    public Image fill;

    // Start is called before the first frame update
    void Start()
    {
        healthy = objectTracking.GetComponent<Damageable>().health;
        sliding.maxValue = healthy;
        sliding.value = sliding.maxValue;
        sliding.fillRect.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void UpdateValue(float damage)
    {
        sliding.fillRect.gameObject.SetActive(true);
        sliding.value = Mathf.Lerp(sliding.value, damage, 0.01f);
    }
}
