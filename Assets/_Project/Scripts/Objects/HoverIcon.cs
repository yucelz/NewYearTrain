using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverIcon : MonoBehaviour
{
    [SerializeField] private float _hoverSpeed = 1.0f;
    [SerializeField] private float _hoverStrength = 0.5f;
    private float _initialYPosition;

    private void Start()
    {
        _initialYPosition = transform.position.y;
    }

    private void Update()
    {
        //calculate the new postion with a hovering effect
        float hoverOffset = Mathf.Sin(Time.time * _hoverSpeed) * _hoverStrength;
        Vector3 newPosition = new Vector3(transform.position.x, _initialYPosition) + Vector3.up * hoverOffset;

        transform.position = newPosition;
    }
}
