using Dreamteck.Splines;
using System.Collections.Generic;
using DG.Tweening;
using MuhammetInce.DesignPattern.Singleton;
using UnityEngine;
using System.Collections;

public enum PlayerStates
{
    Move,
    Stop
}

public class PlayerController : LazySingleton<PlayerController>
{
    private static readonly int Speed = Animator.StringToHash("speed");


    public float weight;
    public IKController IKController;
    
    public PlayerStates playerStates;
    public float stopPos;

    public int activeSpline;
    public int activeHolder;
    
    public List<GameObject> trolleyHoldersList;
    public List<GameObject> trolleyObjectList;

    public SplineFollower splineFollower;
    public Animator playerAnimator;
    public GameObject camTr;
    public GameObject trolley;


    public float speed;
    public float maxSpeed;
    public float acceleration;
    
    public float AnimSpeed
    {
        get => playerAnimator.GetFloat(Speed);
        set => playerAnimator.SetFloat(Speed, value);
    }
    
    

    private void Start()
    {
        OpenIK();
        playerStates = PlayerStates.Move;
        splineFollower = gameObject.GetComponent<SplineFollower>();
        playerAnimator = gameObject.GetComponent<Animator>();
        activeSpline = PlayerPrefs.GetInt("ActiveSpline");
        splineFollower.spline = GameController.Instance.SplineComputerList[activeSpline];
        StartCoroutine(CheckLastSpline());
        activeHolder = PlayerPrefs.GetInt("ActiveHolder");
        GameController.Instance.TrolleyHolderHolder[activeHolder].SetActive(true);
    }

    private void Update()
    {
        camTr.transform.position = transform.position;
        
        Movement();
        CheckTrolleyRot();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Worker"))
        {
            SplineFollower follower = other.transform.parent.GetComponent<SplineFollower>();
            follower.followSpeed = 10;
        }

        if (other.CompareTag("Stopper"))
        {
            playerStates = PlayerStates.Stop;
            splineFollower.SetPercent(stopPos);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Worker"))
        {
            Transform parent = other.transform.parent;
            
            WorkerController controller = parent.GetComponent<WorkerController>();
            SplineFollower follower = parent.GetComponent<SplineFollower>();
            
            follower.followSpeed =  controller.followSpeed;
        }
    }

    private void Movement()
    {
        if (playerStates != PlayerStates.Move) return;
        if (Input.GetMouseButton(0))
        {
            speed = Mathf.Lerp(speed, speed + Time.deltaTime * acceleration,0.2f);
            AnimSpeed = speed;
        }
        else
        {
            speed = Mathf.Lerp(speed, speed - Time.deltaTime * acceleration*5, 0.2f);
            AnimSpeed = speed;
        }
        speed = Mathf.Clamp(speed, 0, maxSpeed);
        splineFollower.followSpeed = speed;
    }

    public void UpgradeSpline()
    {
        activeSpline++;
        PlayerPrefs.SetInt("ActiveSpline", activeSpline);
    }

    public void UpgradeHolder()
    {
        activeHolder++;
        PlayerPrefs.SetInt("ActiveHolder", activeHolder);
    }

    private void CheckTrolleyRot()
    {
        if (PlayerPrefs.GetInt("goLeft") == 1)
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
    public void PlayerStop()
    {
        playerStates = PlayerStates.Stop;
        splineFollower.followSpeed = 0;
        speed = 0;
        AnimSpeed = 0;
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (IKController.leftHandIKTransform != null)
        {
            playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
            playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, weight);
            playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, IKController.leftHandIKTransform.position);
            playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, IKController.leftHandIKTransform.rotation);
        }
        if (IKController.rightHandIKTransform != null)
        {
            playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
            playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, weight);
            playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, IKController.rightHandIKTransform.position);
            playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, IKController.rightHandIKTransform.rotation);
        }
    }

    public void OpenIK()
    {
        DOTween.To(() => weight, (newValue) => weight = newValue, 1f, .3f);
    }
    public void CloseIK()
    {
        DOTween.To(() => weight, (newValue) => weight = newValue, 0f, .8f);
    }

    private IEnumerator CheckLastSpline()
    {
        yield return new WaitForSeconds(0.5f);
        if(activeSpline == 5)
        {
            UIManager.Instance.EndCanvasEffect();
            UIManager.Instance.tapToPlay.SetActive(false);
        }
    }
}

[System.Serializable]
public struct IKController
{
    public Transform leftHandIKTransform;
    public Transform rightHandIKTransform;
}
