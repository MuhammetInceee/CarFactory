using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DG.Tweening;
using UnityEngine;
using Random = System.Random;

public class GetPart : MonoBehaviour
{
    private PartsManufacturer PartsManufacturer => GetComponentInParent<PartsManufacturer>();
    private PlayerController PlayerController => PlayerController.Instance;
    

    public GameObject worker;
    

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < PartsManufacturer.stackedObjList.Count ; i++)
            {
                if(PlayerController.trolleyObjectList.Count < 4)
                {
                    GameObject currentObject = PartsManufacturer.stackedObjList[i];
                    if (currentObject == null) yield break;
                
                    PartsManufacturer.stackedObjList.Remove(currentObject);
                    PlayerController.trolleyObjectList.Add(currentObject);
                    StartCoroutine(StackWheel(currentObject));
                    yield return new WaitForSeconds(.3f);
                }
            }
        }

        if (other.CompareTag("Worker"))
        {
            if(other.gameObject != worker) yield break;
            WorkerController controller = other.GetComponentInParent<WorkerController>();
            
            for (int i = 0; i < PartsManufacturer.stackedObjList.Count ; i++)
            {
                if(controller.trolleyObjectList.Count < 4)
                {
                    GameObject currentObject = PartsManufacturer.stackedObjList[i];
                    if (currentObject == null) yield break;
                
                    PartsManufacturer.stackedObjList.Remove(currentObject);
                    controller.trolleyObjectList.Add(currentObject);
                    StartCoroutine(StackWheelWorker(currentObject, controller));
                    yield return new WaitForSeconds(.3f);
                }
            }
        }
    }

    private IEnumerator StackWheel(GameObject wheel)
    {
        GameObject parent = PlayerController.trolleyHoldersList
            .Where(m => m.activeInHierarchy).FirstOrDefault(x => x.transform.childCount == 0);
        if (wheel == null) yield break;
        if (parent != null) wheel.transform.parent = parent.transform;
        wheel.transform.DOKill();
        wheel.transform.localScale = Vector3.one;
        wheel.transform.localRotation = Quaternion.Euler(Vector3.zero);

        wheel.transform.DOLocalJump(Vector3.zero, 1f, 1, 0.4f).OnComplete(() =>
        {
        });
        yield break;
    }
    
    private IEnumerator StackWheelWorker(GameObject wheel, WorkerController controller)
    {
        GameObject parent = controller.trolleyHolderList
            .Where(m => m.activeInHierarchy).FirstOrDefault(x => x.transform.childCount == 0);
        if (wheel == null) yield break;
        if (parent != null) wheel.transform.parent = parent.transform;
        wheel.transform.DOKill();
        wheel.transform.localScale = Vector3.one;
        wheel.transform.localRotation = Quaternion.Euler(Vector3.zero);

            wheel.transform.DOLocalJump(Vector3.zero, 1f, 1, 0.4f).OnComplete(() =>
        {
        });
        yield break;
    }
}
