using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankUIManager : MonoBehaviour
{
    RectTransform turretTransform;
    [SerializeField] Transform tankTurret;

    private void Awake()
    {
        turretTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, -tankTurret.eulerAngles.y);
        turretTransform.localRotation = targetRotation;
    }
}