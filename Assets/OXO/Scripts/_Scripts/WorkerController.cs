using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;
using Dreamteck.Splines.Primitives;
using MuhammetInce.HelperUtils;
using UnityEngine;

public class WorkerController : MonoBehaviour
{
    private static readonly int Stop = Animator.StringToHash("stop");

    public List<GameObject> trolleyHolderList;
    public List<GameObject> trolleyObjectList;

    public GameObject trolley;
    public bool isLeftWorker;

    public Animator animator;
    public SplineFollower follower;

    public int followSpeed;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        follower = GetComponent<SplineFollower>();
        trolley = transform.GetChild(0).gameObject;
        
        follower.followSpeed = followSpeed;

        TrolleyRotator();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            follower.followSpeed = 0;
            animator.SetBool(Stop, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ExitWorker());
        }
    }

    private IEnumerator ExitWorker()
    {
        yield return new WaitForSeconds(1f);
        follower.followSpeed = followSpeed;
        animator.SetBool(Stop, false);
    }

    private void TrolleyRotator()
    {
        if (isLeftWorker)
        {
            trolley.transform.localPosition = new Vector3(-0.01590833f, 0.8f, 0.7286074f);
            trolley.transform.localEulerAngles = new Vector3(-25, -8.589f, 0);
        }
        else
        {
            trolley.transform.localPosition = new Vector3(0.034f, 0.8f, 0.735f);
            trolley.transform.localEulerAngles = new Vector3(-25, 15.565f, 0);
        }
    }
    
}
