using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAroundObject : MonoBehaviour
{
    [SerializeField]
    private float _rotationSensitivity = 3.0f;

    private float _rotationY;
    private float _rotationX;

    [SerializeField]
    public Transform _target;

    [SerializeField]
    private float _distanceFromTarget = 3.0f;

    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;

    [SerializeField]
    private float _smoothTime = 0.2f;

    [SerializeField]
    private Vector2 _rotationXMinMax = new Vector2(-40, 40);

    void Update()
    {
        if(_target == null)
        {
            return;
        }
        float rotationX = 0; ;
        float rotationY = 0;
        if (Input.GetKey(KeyCode.W))
        {
            rotationY = _rotationSensitivity * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            rotationY = -_rotationSensitivity * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            rotationX = _rotationSensitivity * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotationX = -_rotationSensitivity * Time.deltaTime;
        }

        _rotationY += rotationX;
        _rotationX += rotationY;

        // Apply clamping for x rotation 
        _rotationX = Mathf.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);

        Vector3 nextRotation = new Vector3(_rotationX, _rotationY);

        // Apply damping between rotation changes
        _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
        transform.localEulerAngles = _currentRotation;

        // Substract forward vector of the GameObject to point its forward vector to the target
        transform.position = _target.position - transform.forward * _distanceFromTarget;
    }
}