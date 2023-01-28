using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirrPara : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            StartCoroutine(ManagerMoney.Instance.SpawnMoney(10, other.gameObject));
            ManagerMoney.Instance.ChangeMoney(ManagerMoney.Instance.SellCarMoney);
        }
    }
}
