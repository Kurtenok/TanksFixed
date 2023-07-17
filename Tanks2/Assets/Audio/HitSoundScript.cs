using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSoundScript : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource source;
    private void Awake() {
        source=this.gameObject.GetComponent<AudioSource>();
    }
    void Start()
    {
        //source.Play();
 
    }

    // Update is called once per frame
    void Update()
    {
        if(!source.isPlaying)
        {
            DestroySorce();
        }
    }
    void DestroySorce()
    {
       Destroy(this.gameObject);
    }
}
