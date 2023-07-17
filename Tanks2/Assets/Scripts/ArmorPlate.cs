using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPlate : MonoBehaviour
{
    public ParticleSystem hitParticle;
    

    Dictionary<Collider,float> armorPlates = new Dictionary<Collider, float>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetThickness(Collider collider)
    {
        if(armorPlates.ContainsKey(collider))
        {
            return armorPlates[collider];
        }
        return -1;
    }
    void OnCollisionStay(Collision collision) {
        if(hitParticle)
        foreach (ContactPoint contact in collision.contacts) {

            GameObject.Instantiate(hitParticle,contact.point,transform.rotation);
        }
    }
    public void SpawnHitParticles(Collision other)
    {
        if(hitParticle)
        foreach (ContactPoint contact in other.contacts) {
            ParticleSystem particle=GameObject.Instantiate(hitParticle,contact.point,Quaternion.identity);

           Vector3 direction=-other.relativeVelocity.normalized;
            particle.transform.LookAt(particle.transform.position+direction);
        }
    }
    public void AddPlate(Collider collider,float thickness_)
    {
        armorPlates.Add(collider,thickness_);
    }

    public bool HasPlate(Collider collider)
    {
        return armorPlates.ContainsKey(collider);
    }
}
