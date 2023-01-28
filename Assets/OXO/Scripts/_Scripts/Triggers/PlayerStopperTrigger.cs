using System.Collections;
using Cinemachine;
using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;
using MuhammetInce.HelperUtils;

public class PlayerStopperTrigger : MonoBehaviour
{
    private bool _isPlayerOnInside;
    public float _delayHelper;
    private float _delayer = 30;
    private bool oneShot;
    
    [Header("About Stop Transform: "),Space]
    [SerializeField] private float stopPos;
    [SerializeField] private Vector3 stopRot;

    [Header("About Level End: "), Space] 
    [SerializeField] private float delay;
    [SerializeField] private ProgressTrigger beforeTrigger;


    [Header("About Need Objects: "), Space]
    [SerializeField] private GameObject progress;
    [SerializeField] private GameObject painterStopper;
    [SerializeField] private CinemachineVirtualCamera gameCamera;
    [SerializeField] private CinemachineVirtualCamera paintCamera;

    private SplineFollower PlayerSpline => PlayerController.Instance.splineFollower;
    private ProgressTrigger ProgressTrigger => progress.GetComponentInParent<ProgressTrigger>();

    private void Start()
    {
        _delayHelper = delay;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerOnInside = true;
            PlayerController.Instance.PlayerStop();
            PlayerSpline.SetPercent(stopPos);
            progress.SetActive(false);
            PlayerSpline.enabled = false;
            gameCamera.gameObject.SetActive(false);
            paintCamera.gameObject.SetActive(true);
            PlayerController.Instance.gameObject.transform.DORotate(stopRot, 0.5f).OnComplete(() =>
            {
                painterStopper.SetActive(false);
            });
        }
    }

    private void Update()
    {
        EnesAkifkaplan();
        MuhammetInce();
    }

    private void EnesAkifkaplan()
    {
        if (beforeTrigger.activeCar != null || !_isPlayerOnInside) return;
        _delayHelper -= Time.deltaTime;

        if (_delayHelper <= 0)
        {
            if (!oneShot)
            {
                StartCoroutine(JustStupidFunc());
            }
        }
    }

    private void MuhammetInce()
    {
        if(!_isPlayerOnInside) return;
        
        _delayer -= Time.deltaTime;
        if (_delayer <= 0)
        {
            if (!oneShot)
            {
                StartCoroutine(JustStupidFunc());
            }
        }
    }
    
    private IEnumerator JustStupidFunc()
    {
        yield return null;
        UIManager.Instance.EndCanvasEffect();
        oneShot = true;
    }
}
