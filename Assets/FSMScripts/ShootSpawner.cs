using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSpawner : MonoBehaviour {

    public GameObject obj;
    public float interval = 5.0f;

    private float timer = 0;


    private void Update()
    {
        timer += Time.deltaTime;

        if(timer >= interval)
        {
            timer = 0;
            GameObject nobj = Instantiate(obj);
            nobj.transform.position = transform.position;
            Vector3 posmod = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            nobj.transform.position += posmod;
        }
    }

}
