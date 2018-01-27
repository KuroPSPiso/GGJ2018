using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookDirection : MonoBehaviour
{
    private enum Direction
    {
        Forward,
        Left,
        Right
    }

    private Vector3 _defaultEuler = new Vector3();
    private Direction _direction = Direction.Forward;

    void Start()
    {
        this._defaultEuler = this.gameObject.transform.rotation.eulerAngles;
    }

    void Update()
    {
        switch (_direction)
        {
            case Direction.Forward:
                this.gameObject.transform.rotation = Quaternion.Lerp(this.gameObject.transform.rotation, Quaternion.Euler(_defaultEuler), Time.deltaTime * 9);
                break;
            case Direction.Left:
                this.gameObject.transform.rotation = Quaternion.Lerp(this.gameObject.transform.rotation, Quaternion.Euler(_defaultEuler + new Vector3(0, -30, 0)), Time.deltaTime * 9);
                break;
            case Direction.Right:
                this.gameObject.transform.rotation = Quaternion.Lerp(this.gameObject.transform.rotation, Quaternion.Euler(_defaultEuler + new Vector3(0, 30, 0)), Time.deltaTime * 9);
                break;
        }
    }

    public void SetTurnLeft()
    {
        this._direction = Direction.Left;
    }

    public void SetNeutral()
    {
        this._direction = Direction.Forward;
    }

    public void SetTurnRight()
    {
        this._direction = Direction.Right;
    }
}
