using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject box_Prefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SpawnBox()
    {
        GameObject box_Obj = Instantiate(box_Prefab);
        Vector3 temp = transform.position;
        //To adjust z to keep at 0f
        temp.z = 0f;
        box_Obj.transform.position = temp;
    }
}
