using UnityEngine;
public class Parallax : MonoBehaviour {
    private float _start;
    public float _speed;
    void Start() {
        _start = transform.position.x;
    }
    void FixedUpdate() {
        float change = Camera.main.transform.position.x * _speed;
        transform.position = new Vector3(_start + change, transform.position.y, transform.position.z);
    }
}