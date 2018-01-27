using System.Collections.Generic;
using UnityEngine;

public class TerminalInteract : MonoBehaviour
{
    //Parameters
    public float maxDistance = 2.0f;
    public float loadTime = 3.0f;
    public Transform beginCentre;
    public GameObject packet;

    //Private members
    float loadStatus = 0.0f;
    bool isLoading = false;
    Terminal lastTerminal;

    PlayerInput pInput;

    void Start()
    {
        pInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (!pInput.IsActive())
            return;

        if (!isLoading && pInput.IsActing())
            isLoading = true;

        if (isLoading)
        {
            if (!pInput.IsActing())
            {
                isLoading = false;
                loadStatus = 0.0f;

                if (lastTerminal)
                {
                    lastTerminal.StopLoading(pInput.team);
                    lastTerminal = null;
                }
            }
            else
            {
                GameObject terminal = GetClosestTerminal(maxDistance);

                if (terminal)
                {
                    Terminal t = terminal.GetComponent<Terminal>();

                    if (lastTerminal != t)
                    {
                        if(lastTerminal)
                            lastTerminal.StopLoading(pInput.team);

                        lastTerminal = t;
                        lastTerminal.StartLoading(pInput.team);
                    }

                    loadStatus += Time.deltaTime;

                    if (loadStatus >= loadTime)
                    {
                        isLoading = false;
                        loadStatus = 0.0f;

                        lastTerminal.FinishLoading(pInput.team);
                        lastTerminal = null;

                        //Spawn packet
                        Package package = Instantiate(packet, beginCentre.position, Quaternion.identity).GetComponent<Package>();
                        package.hookedTo = beginCentre.GetComponentInChildren<Hook>().transform;
                    }
                }
                else if (lastTerminal)
                {
                    lastTerminal.StopLoading(pInput.team);
                    lastTerminal = null;
                }
            }
        }
    }

    GameObject GetClosestTerminal(float maxDistance)
    {
        KeyValuePair<GameObject, float> closest = new KeyValuePair<GameObject, float>(null, maxDistance);

        foreach (GameObject terminalObj in GameObject.FindGameObjectsWithTag("Terminal"))
        {
            float distance = Vector3.Distance(transform.position, terminalObj.transform.position);

            if (distance < closest.Value)
                closest = new KeyValuePair<GameObject, float>(terminalObj, distance);
        }

        return closest.Key;
    }
}
