using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class PartsManufacturer : MonoBehaviour
{
    [Header("About Part Holders: "),Space]
    public List<GameObject> holderList;
    public Transform holderParent;

    [Header("About Part: "), Space] 
    public List<GameObject> stackedObjList;
    public Transform partCreateTr;
    public GameObject partPrefab;
    public float partCreateThreshold;

    private WaitForSeconds Wait => new WaitForSeconds(partCreateThreshold);

    private void Start()
    {
        for (int i = 0; i < holderParent.childCount; i++)
        {
            holderList.Add(holderParent.GetChild(i).gameObject);
        }
        
        StartCoroutine(PartCreator());
    }



    private IEnumerator PartCreator()
    {
        yield return Wait;

        GameObject targetHolder = holderList.FirstOrDefault(m => m.transform.childCount == 0);
        if (targetHolder != null)
        {
            GameObject createdObj = Instantiate(partPrefab, partCreateTr.position, partPrefab.transform.rotation, targetHolder.transform);
            createdObj.transform.DOLocalMove(partPrefab.transform.position, 1f);
            stackedObjList.Add(createdObj);
        }
        
        StartCoroutine(PartCreator());
    }
}
