using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenechange : MonoBehaviour
{
    //Type in name of scene to change to
    public string SceneName;

    // Update is called once per frame
    void Update()
    {
        //Changes scene when enter is pressed
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}
