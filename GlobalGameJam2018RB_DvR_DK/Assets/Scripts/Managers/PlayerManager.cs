using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Range(1, 4)]
    public int PlayerCount = 4;

    public ControllersManager ControllersManager;
    public RopeManager ropeManager;
    public GameObject PlayerGameObject;
    public GameObject[] SkinGameObjects;

    private List<GameObject> _playerGameObjects = new List<GameObject>(4);

    // Use this for initialization
    void Start()
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
                PlayerMovement playerInput = newPlayer.GetComponent<PlayerMovement>();
                playerInput.lookDirection = lookDirectionOfSkin;
                PlayerInput pInput = newPlayer.GetComponent<PlayerInput>();
                pInput.controllerId = i;
                pInput.controllersManager = ControllersManager;
                pInput.ropeManager = ropeManager;
                _playerGameObjects.Add(newPlayer);

                if (i % 2 == 0)
                {
                    newPlayer.GetComponent<RopeLauncher>().enabled = false;
                }
                else
                {
                    newPlayer.GetComponent<HookSpawner>().enabled = false;
                    newPlayer.GetComponent<PackageTransfer>().enabled = false;
                }
            }
        }
    }
}
