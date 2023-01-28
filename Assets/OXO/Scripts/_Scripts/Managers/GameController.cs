using System;
using System.Collections.Generic;
using Dreamteck.Splines;
using MuhammetInce.DesignPattern.Singleton;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : LazySingleton<GameController>
{
    public List<SplineComputer> SplineComputerList;
    public List<GameObject> TrolleyHolderHolder;
    public List<GameObject> AllCranes;
    public List<Material> AllGreyMaterials;
    public List<Material> AllCarMaterials;

    public List<bool> AreaProgress;
   
   public Color color;

   public float craneSpeed;
   public bool isTwoDoorCar;
    private void Start()
    {
        foreach (Material mat in AllGreyMaterials)
        {
            mat.color = color;
        }
    }
    
    private void OnEnable()
    {
        for (int i = 0; i < AreaProgress.Count; i++)
        {
            AreaProgress[i] = PlayerPrefs.GetInt($"AreProgressSaver{i}") == 1 ? true : false;

        }
      
    }

    private void OnDisable()
    {
        ProgressSaver();
    }

    private void OnApplicationQuit()
    {
        ProgressSaver();
    }

    private void ProgressSaver()
    {
        for (int i = 0; i < AreaProgress.Count; i++)
        {
            bool currentBool = AreaProgress[i];
             PlayerPrefs.SetInt($"AreProgressSaver{i}",currentBool ? 1 : 0);

        }
    }

    public void DestroyObj(object obj)
    {
        Destroy(obj as GameObject);
    }
}
