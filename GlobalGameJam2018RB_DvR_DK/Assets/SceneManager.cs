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
        if(_playingVideo)
            return;

        bool canStart = true;

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
                        if(state == false)
                        {
                            canStart = state;
                        }
                    }
                    else
                    {
                        canStart = false;
                    }
                }
                else
                {
                    canStart = false;
                }
                TextPlayerReadyState[i].text = sState;
            }
            else
            {
                canStart = false;
            }
        }

        if (canStart)
        {
            _playingVideo = true;
            StartCoroutine(SwitchScene());
        }
    }

    IEnumerator SwitchScene()
    {
        while (_playingVideo)
        {
            yield return null;
            _playingVideo = false;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(NextScene);
    }
}