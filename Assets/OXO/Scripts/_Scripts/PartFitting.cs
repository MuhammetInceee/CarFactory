using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class PartFitting : MonoBehaviour
{
    private GameObject _target;
    public ParticleSystem puffEffect;
    public ProgressTrigger progressTrigger;

    private PlayerController PlayerController => PlayerController.Instance;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(progressTrigger.activeCar == null) return;
            CheckTarget();
            if(_target.GetComponent<MeshRenderer>().enabled) return;
            Touch(PlayerController.trolleyObjectList);
        }

        if (other.CompareTag("Worker"))
        {
            if(progressTrigger.activeCar == null) return;
            CheckTarget();
            if(_target.GetComponent<MeshRenderer>().enabled) return;
            WorkerController controller = other.GetComponentInParent<WorkerController>();
            Touch(controller.trolleyObjectList);
        }
    }
    
    private void Touch(List<GameObject> trolleyObjects)
    {
        if (trolleyObjects.Count != 0)
        {
            GameObject lastObj = trolleyObjects[^1];
            if (lastObj != null)
            {
                lastObj.transform.parent = null;
                Fitter(lastObj, gameObject.tag, trolleyObjects);
            }
        }
    }

    private void Fitter(GameObject lastObj, string gOTag, List<GameObject> trolleyObjects)
    {
        if (gameObject.CompareTag(gOTag))
        {
            lastObj.transform.DOLocalMove(_target.transform.position, 0.2f).OnComplete(() =>
            {
                trolleyObjects.Remove(lastObj);
                Destroy(lastObj);
                puffEffect.transform.position = _target.transform.position;
                puffEffect.Play();
                _target.GetComponent<CarPiece>().Bought();
            });
        }
    }
    
    private void CheckTarget()
    {
        if(progressTrigger.activeCar == null) return;
        foreach (Transform tr in progressTrigger.activeCar.transform)
        {
            if (tr.CompareTag($"{gameObject.tag}Car"))
            {
                _target = tr.gameObject;
            }
        }
    }
}
