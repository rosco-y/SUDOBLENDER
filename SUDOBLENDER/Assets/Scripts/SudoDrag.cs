using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static eTurnDirection;
using System;
public enum eTurnDirection
{
    LEFT,
    RIGHT,
    UP,
    DOWN
}

public class SudoDrag : MonoBehaviour
{

    Vector3 _screenPoint;
    Vector3 _offSet;
    Vector3 _originalPosition;
    Quaternion _originalRotation;
    //bool _processingMouseCommand = false;
    [SerializeField]

    float _xMovement;
    float _yMovement;
    public float _rotateSpeed = 1f;
    Quaternion _toRotation = Quaternion.identity;

    private void Awake()
    {
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, _toRotation, _rotateSpeed * Time.deltaTime);
        //_processingMouseCommand = !isApproximate(transform.rotation, _toRotation, 0.0000004f);
    }

    bool isApproximate(Quaternion q1, Quaternion q2, float precision)
    {
        return Mathf.Abs(Quaternion.Dot(q1, q2)) >= 1 - precision;
    }

    void OnMouseDown()
    {
        //_processingMouseCommand = true;
        _screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        _offSet = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z));
    }

    private void OnMouseUp()
    {
        _originalPosition = Vector3.zero;
        // _processingMouseCommand = false;
        resetCube();
    }

    private void resetCube()
    {
        //transform.position = Vector3.zero;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            turn(RIGHT);  // intuitive to turn the cube right when the left-arrow is pressed
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            turn(LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            turn(DOWN);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            turn(UP);
        }
    }
    void OnMouseDrag()
    {
        //if (_processingMouseCommand)
        //    return;
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + _offSet;
        if (_originalPosition == Vector3.zero)
        {
            _originalRotation = transform.rotation;
            _originalPosition = curPosition;
        }
        else
        {
            _xMovement = curPosition.x - _originalPosition.x;
            _yMovement = curPosition.y - _originalPosition.y;
            if (Mathf.Abs(_xMovement) < Mathf.Abs(_yMovement))
            {
                // moving either left or right.
                if (_xMovement < 0)
                    turn(LEFT);
                else if (_xMovement > 0)
                    turn(RIGHT);
            }
            else
            {
                // dragging either up or down.
                if (_yMovement > 0)
                    turn(UP);
                else if (_yMovement < 0)
                    turn(DOWN);
            }
        }
    }



    void turn(eTurnDirection direction)
    {
        //_processingMouseCommand = true;
        _originalRotation = transform.rotation;
        print(direction.ToString());
        float curX = _originalRotation.eulerAngles.x;
        float curY = _originalRotation.eulerAngles.y;
        float curZ = _originalRotation.eulerAngles.z;
        bool UpDownOnZ = false;
        switch (direction)
        {
            case LEFT:
                _toRotation = Quaternion.Euler(0f, curY + 90f, 0f); // Y
                UpDownOnZ = !UpDownOnZ;
                break;
            case RIGHT:
                _toRotation = Quaternion.Euler(0f, curY - 90f, 0f); // -Y
                UpDownOnZ = !UpDownOnZ;
                break;
            case UP:
                if (!UpDownOnZ)
                    _toRotation = Quaternion.Euler(curX - 90f, 0f, 0f); // -X
                else
                    _toRotation = Quaternion.Euler(0f, 0f, curZ - 90f); // -Z
                break;
            case DOWN:
                if (!UpDownOnZ)
                    _toRotation = Quaternion.Euler(curX + 90f, 0f, 0f); // X
                else
                    _toRotation = Quaternion.Euler(0f, 0f, curZ + 90f); // Z
                break;
        }
        float x = correctError(_toRotation.eulerAngles.x);
        float y = correctError(_toRotation.eulerAngles.y);
        float z = correctError(_toRotation.eulerAngles.z);
        _toRotation.eulerAngles = new Vector3(x, y, z);
        print(_toRotation);

    }

    float correctError(float x)
    {

        bool negative = x < 0;
        float minDiff;
        float angle = 0f;
        minDiff = Math.Abs(x - 0); // x can be < 0;

        if (Math.Abs(x - 90) < minDiff)
        {
            minDiff = Math.Abs(x - 90);
            angle = 90f;
        }
        if (Math.Abs(x - 180) < minDiff)
        {
            angle = 180f;
            minDiff = Math.Abs(x - 180);
        }
        if (Math.Abs(x - 270) < minDiff)
        {
            minDiff = Math.Abs(x - 270);
            angle = 270f;
        }
        if (Math.Abs(x - 360) < minDiff)
        {
            angle = 360f;  // reset angle to 0?
        }

        if (negative)
            angle *= -1;

        return angle;
    }

}


