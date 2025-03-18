using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SigmaTrigger : MonoBehaviour
{
    public bool Activar;
    public GameObject Sigma;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Sigma")) {
            Sigma = GameObject.Find("Sigma");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            Sigma.SetActive(Activar);
        }
    }


}
