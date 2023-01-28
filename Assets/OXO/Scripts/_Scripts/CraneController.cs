using System;
using System.Collections;
using Dreamteck.Splines;
using MuhammetInce.HelperUtils;
using UnityEngine;

public class CraneController : MonoBehaviour
{
    private MeshRenderer _bodyRender;
    private bool IsInGame => PlayerPrefs.GetInt(_bodyRender.gameObject.GetInstanceID().ToString()) != 0;
    public SplineFollower Follower => GetComponent<SplineFollower>();
    public bool IsStop => Follower.followSpeed < 1;

    [Header("About Player Prefs: "), Space]
    public float lastPos;
    private float _lastSpeed;
    
    [Header("About Crane Order: "), Space]
    public int id;
    public int RemainingPiece;
    public ProgressTrigger progressTrigger;
    [SerializeField] private CraneController nextCar;
    
    [Header("About Painter: "), Space]
    public Material targetMaterial;

    private void Awake()
    {
        Follower.spline = GameObject.FindWithTag("Rail").GetComponent<SplineComputer>();
    }

    private void Start()
    {
        _bodyRender = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();

        RemainingPiece = PlayerPrefs.GetInt($"{gameObject.GetInstanceID().ToString()}piece", RemainingPiece);

        id = PlayerPrefs.GetInt($"{gameObject.GetInstanceID().ToString()}id" ,0);
        
        if (IsInGame)
        {
            PassGate();
            Follower.SetPercent(PlayerPrefs.GetFloat($"{gameObject.GetInstanceID().ToString()}pos", 0));
            Follower.followSpeed = PlayerPrefs.GetFloat($"{gameObject.GetInstanceID().ToString()}speed", GameController.Instance.craneSpeed);
        }
    }

    private void OnDisable()
    {
        Close();
    }

    private void OnApplicationQuit()
    {
        Close();
    }

    private void Close()
    {
        lastPos = (float)Follower.GetPercent();
        PlayerPrefs.SetFloat($"{gameObject.GetInstanceID().ToString()}pos", lastPos);
        _lastSpeed = Follower.followSpeed;
        PlayerPrefs.SetFloat($"{gameObject.GetInstanceID().ToString()}speed", _lastSpeed);
        SetRemain();
    }

    public void CheckCar()
    {
        if (GameController.Instance.AreaProgress[id])
        {
            HelperUtils.StopIt(Follower);
        }
        
        else
        {
            HelperUtils.SpeedUp(Follower);
            GameController.Instance.AreaProgress[id] = true;
        }
    }

    public void EndProgress()
    {
        if (nextCar != null)
        {
            if(nextCar.IsStop)
                nextCar.CheckCar();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (nextCar != null && other.gameObject == nextCar.gameObject)
        {
            HelperUtils.StopIt(nextCar.Follower);
        }

        if (other.CompareTag("GateTrigger"))
        {
            PassGate();
        }

        if (other.CompareTag("ProgressTrigger"))
        {
            progressTrigger = other.gameObject.GetComponent<ProgressTrigger>();
            progressTrigger.activeCar = gameObject.transform.GetChild(0).gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (nextCar != null && other.gameObject == nextCar.gameObject)
        {
            HelperUtils.SpeedUp(nextCar.Follower);
        }
        
        if (other.CompareTag("ProgressTrigger"))
        {
            progressTrigger = other.gameObject.GetComponent<ProgressTrigger>();
            progressTrigger.activeCar = null;
        }
    }

    private void PassGate()
    {
        _bodyRender.enabled = true;
        PlayerPrefs.SetInt(_bodyRender.gameObject.GetInstanceID().ToString(), 1);
    }

    public void IdUpgrade()
    {
        id++;
        PlayerPrefs.SetInt($"{gameObject.GetInstanceID().ToString()}id", id);
    }

    public void RemainDeacreaser()
    {
        RemainingPiece--;
    }

    public void SetRemain()
    {
        PlayerPrefs.SetInt($"{gameObject.GetInstanceID().ToString()}piece", RemainingPiece);
    }
}
