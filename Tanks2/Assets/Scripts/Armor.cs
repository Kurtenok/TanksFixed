using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    [SerializeField] ParticleSystem hitParticle;
    // Start is called before the first frame update
    [System.Serializable]
    struct ArmorPlateStruct
    {
        public Collider plateCol;
        public float thickness;

        
    }
   [SerializeField] List<ArmorPlateStruct> armorPlates;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Awake() {
         foreach(ArmorPlateStruct armorPlate in armorPlates)
        {
            ArmorPlate plate;
            if(armorPlate.plateCol)
            {
                if(!armorPlate.plateCol.gameObject.TryGetComponent<ArmorPlate>(out plate))
                {
                    plate=armorPlate.plateCol.gameObject.AddComponent<ArmorPlate>();
                    if(hitParticle)
                    plate.hitParticle=hitParticle;
                    else 
                    Debug.Log("Please set hitParticle for object "+ this.gameObject.name);
                }
                if(!plate.HasPlate(armorPlate.plateCol))
                {
                    plate.AddPlate(armorPlate.plateCol,armorPlate.thickness);
                }
                else
                {
                    Debug.Log(armorPlate.plateCol.gameObject.name+" has already collider "+armorPlate.plateCol.name);
                }
            }
            else
            {
                Debug.Log("Please set collider in sript for "+this.gameObject.name+" object");
            }
           
        }
    }
}
