using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletImageCharacteritics : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI textMeshProPrefab;
    [SerializeField] Vector3 textOffset;
    TextMeshProUGUI textMeshPro;
    TurretController.BulletType charactreristics;
    private void Awake() {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowBulletCharacteristics() {
        if(textMeshProPrefab)
        {
        Transform canvas=this.gameObject.transform.parent;
        textMeshPro=Instantiate(textMeshProPrefab,canvas.position,canvas.transform.rotation);
        textMeshPro.transform.SetParent(canvas);
        textMeshPro.transform.localPosition=this.transform.localPosition+textOffset;
        textMeshPro.transform.localScale=new Vector3(0.5f,0.5f,0.5f);
        
        textMeshPro.text="Name: "+charactreristics.name+'\n'+
        "Damage: "+charactreristics.damage+'\n'+
        "Penetration: "+charactreristics.penetration+'\n'+
        "Speed: "+charactreristics.speed;
        }
    }
    public void SetСharacteristiс(TurretController.BulletType charactreristics_)
    {
        charactreristics=charactreristics_;
    }
    public void HideBulletCharacteristics() {
        if(textMeshPro)
        {
            Destroy(textMeshPro.gameObject);
        }
    }
}
