﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Range(1, 4)]
    public int PlayerCount = 4;

    public ControllersManager ControllersManager;
    public GameObject PlayerGameObject;
    public GameObject[] SkinGameObjects;

    private List<GameObject> _playerGameObjects = new List<GameObject>(4);

	// Use this for initialization
	void Start ()
	{
	    if (SkinGameObjects != null)
	    {
	        SpawnPlayers();

	        int count = 0;

            for (int i = 0; i < (count = _playerGameObjects.Count); i++)
            {
                for (int j = 0; j < count; j++)
	            {
	                if (i != j)
	                {
	                    Physics2D.IgnoreCollision(_playerGameObjects[i].GetComponent<BoxCollider2D>(), _playerGameObjects[j].GetComponent<BoxCollider2D>());
                    }
	            }
	        }
	    }
    }

    void SpawnPlayers()
    {
        for (int i = 0; i < PlayerCount; i++)
        {
            if (SkinGameObjects.Length >= i + 1)
            {
                GameObject newPlayer = GameObject.Instantiate(PlayerGameObject);
                GameObject newSkin = GameObject.Instantiate(SkinGameObjects[i], newPlayer.transform);
                LookDirection lookDirectionOfSkin = newSkin.GetComponent<LookDirection>();
                PlayerInput playerInput = newPlayer.GetComponent<PlayerInput>();
                playerInput.LookDirection = lookDirectionOfSkin;
                playerInput.ControllerId = i;
                playerInput.ControllerManagerEntity = ControllersManager;
                _playerGameObjects.Add(newPlayer);
            }
        }
    }
}