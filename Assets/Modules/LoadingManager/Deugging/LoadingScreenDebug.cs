using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenDebug : MonoBehaviour
{
    //LoadManager.Instance.TESTING();
    //LevelManager.Instance.
    //LevelManager.Instance.LoadScene(0);


    public void LoadLevel()
    {
        LoadingManager.Instance.LoadScene("LoadTest");
    }
}
