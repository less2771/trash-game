using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour
{

    public GameObject[] walls; //0 up, 1 down, 2 right, 3 left
    public GameObject[] doors;

    public bool[] testStatus;

    // Start is called before the first frame update
    void Start()
    {
        UpdateRoom(testStatus);
    }

    // Update is called once per frame
    void UpdateRoom(bool[] doorstatus)
    {
        for (int i = 0; i < doorstatus.Length; i++)
        {
            doors[i].SetActive(doorstatus[i]);
            walls[i].SetActive(!doorstatus[i]);
        }


    }
}
