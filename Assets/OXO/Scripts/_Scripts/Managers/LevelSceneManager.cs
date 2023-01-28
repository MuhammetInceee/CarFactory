using System;
using System.Collections.Generic;
using DG.Tweening;
using MuhammetInce.DesignPattern.Singleton;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class LevelSceneManager : LazySingleton<LevelSceneManager>
{
    public int currentLevel;
    public bool isLastLevel => currentLevel >= levelNameList.Count - 1;
    public List<string> levelNameList;
    
    private void Awake()
    {
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
        currentLevel = PlayerPrefs.GetInt("CurrentLevel",0);
        SetLevel();
    }

    private void OnEnable()
    {
        Actions.OnNextLevelButtonClicked += NextLevel;
        Actions.OnExitButtonClicked += ExitButton;
    }

    private void OnDisable()
    {
        Actions.OnNextLevelButtonClicked -= NextLevel;
        Actions.OnExitButtonClicked -= ExitButton;    
    }

    public void SetLevel()
    {
        if (isLastLevel)
        {
            SceneManager.LoadScene(levelNameList[levelNameList.Count-1]);
            OnLastLevelLoaded();
            return;
        }
        if (currentLevel > 0)
        {
            DOVirtual.DelayedCall(.2f, () => SceneManager.LoadScene(levelNameList[currentLevel]));
           // SceneManager.LoadScene("Level1");
            
            OnNewLevelLoaded();
        }
    }

    public void ReLoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ResetLevel()
    {
        PlayerPrefs.DeleteAll();
        ManagerMoney.Instance.SetPlayerPrefs();
    }
    public void IncreaseLevel()
    {
        currentLevel++;
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
    }
    [Button]
    public void NextLevel()
    {
        //On Last scene
        if (currentLevel >= levelNameList.Count - 1)  
        {
          ResetLevel();
          IncreaseLevel();
          SceneManager.LoadScene(levelNameList[levelNameList.Count-1]);
          OnLastLevelLoaded();
        }
        else
        {
             ResetLevel();
             IncreaseLevel();
             SetLevel();
        }
    }

    public void OnNewLevelLoaded()
    {
        
    }

    public void OnLastLevelLoaded()
    {
        CarColorChanger();

    }
    public void ExitButton()
    {
        ResetLevel();
        ReLoadScene();
        CarColorChanger();
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
    }

    private void CarColorChanger()
    {
        DOVirtual.DelayedCall(.3f, () =>
        {
            PainterTrigger.Instance.targetMaterial =
                GameController.Instance.AllCarMaterials[Random.Range(0, GameController.Instance.AllCarMaterials.Count)];
        });
    }
    
}
