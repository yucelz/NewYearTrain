using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagBehaviour : MonoBehaviour
{
    [SerializeField] int followDist;
    [SerializeField] Vector3 positionOffset;
    private List<Vector3> charPreviousPositions;
    private GameObject character;

    // Start is called before the first frame update
    void Start()
    {
        charPreviousPositions = new List<Vector3>();
        character = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        charPreviousPositions.Add(character.transform.position + positionOffset);
        if (charPreviousPositions.Count >= followDist) {
            gameObject.transform.position = charPreviousPositions[0] + positionOffset;
            charPreviousPositions.RemoveAt(0);
        }
    }
}
