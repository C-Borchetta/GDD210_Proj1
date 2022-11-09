using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenechange2 : MonoBehaviour
{
    //Type in name of scene to change to
    public string SceneName;
    
public void SceneSwitch()
    {
        SceneManager.LoadScene(SceneName);
    }
}
