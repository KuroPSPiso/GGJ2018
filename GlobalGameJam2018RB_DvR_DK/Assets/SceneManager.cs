using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public int NextScene;

    public ControllersManager ControllersManagerInstance;

    public Text[] TextPlayerReadyState;

    private bool _playingVideo = false;

    // Update is called once per frame
    void Update () {
        for (int i = 0; i < 4; i++)
        {
            string sState = "Not Ready";
            if (ControllersManagerInstance.PlayerReadyState.ContainsKey(i))
            {
                bool state = false;
                if (ControllersManagerInstance.PlayerReadyState.TryGetValue(i, out state))
                {
                    if (state)
                    {
                        sState = "Ready";
                    }
                }
                TextPlayerReadyState[i].text = sState;
            }
        }

        _playingVideo = true;
        StartCoroutine(SwitchScene());
    }

    IEnumerator SwitchScene()
    {
        while (_playingVideo)
        {
            yield return null;
            _playingVideo = false;
        }

        UnityEngine.SceneManagement.SceneManager.SetActiveScene(
            UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(NextScene));
    }
}