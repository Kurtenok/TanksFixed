using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float damage;
    float penetration;
    GameObject origin;
    ArmorPlate armorHit;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyBullet",5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetDamage(float damage_)
    {
        damage=damage_;
    }
    public void SetPenetration(float penetration_)
    {
        penetration=penetration_;
    }
    public void SetOrigin(GameObject origin_)
    {
        origin=origin_;
    }
    void DestroyBullet()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision other)
    {
    Vector3 objectToOther = other.contacts[0].normal;
    Vector3 objectForward = transform.forward;

    float angle = Vector3.Angle(objectToOther, -objectForward);
    
    if(angle>90)
    {
        angle-=90;
    }

    if(other.gameObject.TryGetComponent<ArmorPlate>(out armorHit))
    {
        armorHit.SpawnHitParticles(other);
        float thickness=armorHit.GetThickness(other.collider);
        if(thickness>=0)
        {
            float way=thickness/Mathf.Cos(angle*Mathf.Deg2Rad);
            //Debug.Log("Bullet way"+way);
            //Debug.Log("Buulet" +angle+" "+way);
        // Debug.Log("Cos"+Mathf.Cos(angle*Mathf.Deg2Rad));
            if(penetration>=way)
            {
                Debug.Log("Penetrated");
                TurretController controller;
                if(origin.gameObject.TryGetComponent<TurretController>(out controller))
                {
                    controller.BulletPenetrated();
                }
            }
            else
            {
                TurretController controller;
                if(origin.gameObject.TryGetComponent<TurretController>(out controller))
                {
                    controller.BulletNotPenetrated();
                }
                Debug.Log("Not Penetrated");
            }
        }
    }


    Invoke("DestroyBullet",0.01f);
    }
    private void OnTriggerEnter(Collider other) 
    
    {
        
    
    }
    bool TryGetArmorComponent(Collision other)
    {
        if(other.gameObject.transform.root.TryGetComponent<ArmorPlate>(out armorHit) || other.gameObject.TryGetComponent<ArmorPlate>(out armorHit))
        {
            return true;
        }
        else
        {
            Transform parent=other.gameObject.transform.parent;
            while(parent)
            {
                if(parent.gameObject.TryGetComponent<ArmorPlate>(out armorHit))
                {
                    return true;
                }
                parent=parent.parent;
            }
            return false;
        }
    }
}

