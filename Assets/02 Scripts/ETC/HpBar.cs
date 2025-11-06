using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    private Transform target;
    private Vector3 offset = new Vector3(0, 1.5f, 0); 

    public void Init(Transform targetTransform)
    {
        target = targetTransform;
    }

    public void UpdateHp(float current, float max)
    {
        fillImage.fillAmount = Mathf.Clamp01(current / max);
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
