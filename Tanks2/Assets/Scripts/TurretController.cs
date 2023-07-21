using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TurretController : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject sphere;
    SphereCollider sphereCollider;
    [SerializeField] GameObject cannon;
    [SerializeField] float reloadTime;
    [SerializeField] GameObject bulletSpawner;
    [SerializeField] float rotSpeed;
    [SerializeField] float minScatter=1;
    [SerializeField] float lowScatter=0.001f;
    [SerializeField] float maxScatter=2;
    [SerializeField] float UpScatterDegree;
    [SerializeField] float maxUpRotAgle=90;
    [SerializeField] float maxDownRotAgle=-90;
    [SerializeField] Image circle;
    [SerializeField] RectTransform canvas;
    [SerializeField] Image willPenetateScope;
    [SerializeField] Image NotWillPenetrateScope;
    [SerializeField] Image UnknownPenetratescope;
    [System.Serializable]
    public struct BulletType
    {
        public string name;
        public Image image;
        public GameObject bullet;
        public float penetration;
        public float damage;
        public float speed;
             public string GetName()
    {return name;}
      public Image GetImage()
    {return image;}
    public float GetPenetration()
    {return penetration;}
    public float GetDamage()
    {return damage;}
    }   
    [SerializeField]  BulletType[] bulletTypes;
    PlayerSounds sounds;
    bool isReloading=false;
    RaycastHit hit;
    Vector3 pastFrameCoord;
    float circleStartScale;
    Coroutine coroutine=null;
    Image scope;
    [SerializeField] Color standartBulletColor;
    [SerializeField] TextMeshProUGUI  gearNumerator;
    [SerializeField] Vector2 gearNumberOffset;
   
    Color chosenBulletColor=Color.green;
    int chosenBulletIndex=-1;
    Dictionary<int,Image> bulletImageKeys=new Dictionary<int,Image>();
    Coroutine reload;
    bool turretRotationLocked=false;
    RaycastHit hit2;
    ArmorPlate armorHit;
    void Start()
    {
        StartCoroutine( SpawnBulletsImage());
    }
    private void FixedUpdate() {
        Ray ray;
        ray=Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Cursor.lockState==CursorLockMode.None || Input.GetMouseButton(1))
        {
            turretRotationLocked=true;
        }
        else
        {
            if(turretRotationLocked)
            turretRotationLocked=false;
        }
        if(Physics.Raycast(ray,out hit,500) )
        {
            if(!turretRotationLocked)
            {
                
                Vector3 target=hit.point;
                Vector3 direct= Vector3.RotateTowards(transform.forward,target-transform.position,rotSpeed,0f);


                Vector3 canonDirect= Vector3.RotateTowards(cannon.transform.forward,target-cannon.transform.position,rotSpeed,0f);

                Vector3 sphereDirect= Vector3.RotateTowards(transform.forward,target-transform.position,5f,0f);

                sphere.transform.rotation=Quaternion.LookRotation(sphereDirect);
                
                Vector3 temp=direct;
                temp.y=0;
                transform.rotation=Quaternion.LookRotation(temp);
                if(maxUpRotAgle>270 && maxDownRotAgle<90)
                {
                    if(((sphere.transform.localEulerAngles.x<maxDownRotAgle)||(sphere.transform.localEulerAngles.x>maxUpRotAgle)) && !turretRotationLocked)
                    {
                        cannon.transform.rotation=Quaternion.LookRotation(canonDirect);    
                    }              
                }
                else
                {
                    Debug.Log("Please difine max Up and Down Rot angles(they should be more than 270 and less than 90)");
                }
            }
                    Vector2 screenPos;
 
                    LayerMask layerMask = ~LayerMask.GetMask("Bullet");
                    if(Physics.Raycast(cannon.transform.position,cannon.transform.forward,out hit2,500,layerMask))
                    {
                        if(hit2.point!=pastFrameCoord && !turretRotationLocked)
                        {
                            ChangeScatter();
                            if(coroutine == null)
                            {coroutine=StartCoroutine(LowScatter());}
                        }
                        pastFrameCoord=hit2.point;
                        Vector3 screenPoint = Camera.main.WorldToScreenPoint(hit2.point);
                        screenPoint.z = 0;
                        
                        
                        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPoint, Camera.main, out screenPos))
                        {
                            circle.transform.localPosition=screenPos;
                                 
                            if(hit2.collider.gameObject.TryGetComponent<ArmorPlate>(out armorHit))
                            {
                                //Vector3 objectToOther = hit2.collider.gameObject.transform.forward;
                                Vector3 objectForward = cannon.transform.forward;    
                                //Debug.DrawLine(hit2.point,hit2.normal,Color.red,5f);
                                Vector3 objectToOther = hit2.normal;
                                
                                float angle = Vector3.Angle(objectToOther, -objectForward);

                                if(angle>90)
                                {
                                    angle-=90;
                                }
                                float thickness=armorHit.GetThickness(hit2.collider);
                                if(thickness>=0)
                                {
                                    float way=thickness/Mathf.Cos(angle*Mathf.Deg2Rad);
                                    //Debug.Log(angle+" "+ way);
                                    if(scope!=null)
                                    {
                                        Destroy(scope.gameObject);
                                        scope=null;
                                    }

                                    if(chosenBulletIndex-1>-1)
                                    {
                                    if(bulletTypes[chosenBulletIndex-1].penetration>=way)
                                    {
                                    if(bulletTypes[chosenBulletIndex-1].penetration>way+bulletTypes[chosenBulletIndex-1].penetration/10)
                                        {
                                            scope=Instantiate(willPenetateScope,canvas.transform.position,canvas.transform.rotation);
                                            
                                            
                                        }
                                        else 
                                        {
                                            scope=Instantiate(UnknownPenetratescope,canvas.transform.position,canvas.transform.rotation);
                                        }
                                        
                                    }
                                    else
                                    {
                                        scope=Instantiate(NotWillPenetrateScope,canvas.transform.position,canvas.transform.rotation);
                                    }
                                    scope.transform.SetParent(canvas.gameObject.transform);
                                    scope.transform.localScale=new Vector3(0.1f,0.1f,0.1f);
                                    scope.transform.localPosition=screenPos;
                                    }
                                }
                            }
                            else
                            {
                                if(scope!=null)
                                {
                                    Destroy(scope.gameObject);
                                    scope=null;
                                }
                            }
                            
                        }

                    }
                 
                    

        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0)&& !isReloading && chosenBulletIndex>=0)
        {
            RaycastHit hit2;

            Vector3 pos=(circle.transform.position+(Random.insideUnitSphere*circle.transform.localScale.x*0.1f))-Camera.main.transform.position;
            //Debug.Log("POS "+ pos);
            if(Physics.Raycast(Camera.main.transform.position,pos,out hit2,500))
            {
                //Debug.Log("HIT "+ hit2.point);
                bulletSpawner.transform.LookAt(hit2.point);
            }
            GameObject bul= Instantiate(bulletTypes[chosenBulletIndex-1].bullet,bulletSpawner.transform.position,bulletSpawner.transform.rotation);
            CheckBulletComponents(ref bul);
           

            bul.SendMessage("SetDamage",bulletTypes[chosenBulletIndex-1].damage);
            bul.SendMessage("SetPenetration",bulletTypes[chosenBulletIndex-1].penetration);
            bul.SendMessage("SetOrigin",this.gameObject);
            Rigidbody rig;
            rig = bul.GetComponent<Rigidbody>();
            
            rig.AddRelativeForce(Vector3.forward*bulletTypes[chosenBulletIndex-1].speed);
            reload=StartCoroutine(reloading());

            if(sounds)
            sounds.PlayMainCaliberSound();
        }




        int temp;
        if(int.TryParse(Input.inputString,out temp))
        {
            if(bulletImageKeys.ContainsKey(temp))
            {
                if(chosenBulletIndex!=temp)
                {
                int pastBulletIndex=chosenBulletIndex;
                
                if(chosenBulletIndex>0)
                bulletImageKeys[chosenBulletIndex].color=standartBulletColor;
                chosenBulletIndex=temp;
                bulletImageKeys[temp].color=chosenBulletColor;
                if(isReloading)
                {
                bulletImageKeys[pastBulletIndex].fillAmount=1;
                StopCoroutine(reload);
                reload=StartCoroutine(reloading());
                }
                }
            }
        }
        
    }
    private void Awake() {
       sphere=GameObject.CreatePrimitive(PrimitiveType.Sphere);
       sphere.transform.position=transform.position;
       sphere.transform.localScale=new Vector3(1f,1f,1f);
       sphere.transform.parent=this.gameObject.transform;
       sphereCollider=sphere.GetComponent<SphereCollider>();
       sphereCollider.isTrigger=true;
       MeshRenderer sphereRenderer = sphere.GetComponent<MeshRenderer>();
       sphereRenderer.enabled = false;
       circleStartScale=circle.transform.localScale.x;


       standartBulletColor=bulletTypes[0].GetImage().color;

       sounds=Camera.main.GetComponent<PlayerSounds>();
       
    }
    IEnumerator reloading()
    {
        isReloading=true;
        float elapsedTime = 0f;
        float currentValue = 0f;
        while (elapsedTime < reloadTime)
        {
            currentValue = Mathf.Lerp(0, 1.01f, elapsedTime / reloadTime);

            bulletImageKeys[chosenBulletIndex].fillAmount=currentValue;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isReloading=false;
    }
     Vector3 GetRandomPointInSphere(float scatter_)
    {
        Vector3 randomDirection = Random.insideUnitSphere;
        Vector3 randomPoint = randomDirection.normalized * scatter_;
        return randomPoint;     
    }
   
    void ChangeScatter()
    {
        float degree=(pastFrameCoord-hit2.point).magnitude;
        degree*=UpScatterDegree;
        if(circle.transform.localScale.x+degree<maxScatter*circleStartScale)
        {
        circle.transform.localScale+=new Vector3(degree,degree,degree);
        }
        else
        {
            circle.transform.localScale=new Vector3(maxScatter*circleStartScale,maxScatter*circleStartScale,maxScatter*circleStartScale);
        }

    }
    IEnumerator LowScatter()
    {
        while(circle.transform.localScale.x!=minScatter*circleStartScale)
        {
            if(circle.transform.localScale.x>minScatter*circleStartScale)
            {
                if((circle.transform.localScale.x-minScatter*circleStartScale)>lowScatter)
                {
                    circle.transform.localScale-=new Vector3(lowScatter,lowScatter,lowScatter);
                }
                else
                {
                    circle.transform.localScale=new Vector3(minScatter*circleStartScale,minScatter*circleStartScale,minScatter*circleStartScale);
                }        
            }
            else
            {
                if((minScatter*circleStartScale-circle.transform.localScale.x)>lowScatter)
                {
                    circle.transform.localScale+=new Vector3(lowScatter,lowScatter,lowScatter);
                }
                else
                {
                    circle.transform.localScale=new Vector3(minScatter*circleStartScale,minScatter*circleStartScale,minScatter*circleStartScale);
                }  
            }
            yield return new WaitForEndOfFrame();
        }
        coroutine=null;
    }
    IEnumerator SpawnBulletsImage()
    {
        yield return new WaitForEndOfFrame();
        int i=bulletTypes.Length;
        int j=0;
       foreach(BulletType bullet in bulletTypes)
       {    
            Image spawnedImage;
            Image definedImage=bulletTypes[j].GetImage();
            Vector2 pos=canvas.transform.position+new Vector3((Screen.width/2)-((definedImage.rectTransform.sizeDelta.x*definedImage.transform.localScale.x+40)*i),30,0);
            Vector2 screenPos;
            if(RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, pos, Camera.main, out screenPos))
            {
          
           spawnedImage=GameObject.Instantiate(definedImage,pos,canvas.transform.rotation);
           spawnedImage.color=standartBulletColor;
           spawnedImage.transform.SetParent(canvas.gameObject.transform);
           spawnedImage.transform.localPosition=screenPos;
           spawnedImage.transform.localScale=new Vector3(0.4f,0.4f,0.4f);
            SetGearNumber(j+1,screenPos);
           bulletImageKeys.Add(j+1,spawnedImage);

            BulletImageCharacteritics bulletCharacteristic;
           if(spawnedImage.TryGetComponent<BulletImageCharacteritics>(out bulletCharacteristic))
           {
                bulletCharacteristic.SetСharacteristiс(bullet);
           }
           else
           {
            Debug.Log("Please set Script BulletImageCharacteritics on bullet image "+definedImage.name);
           }
           --i;   
           j++;
            }
       }
    }

    public void BulletPenetrated()
    {
        sounds.PlayPenetratedSound();
    }
    public void BulletNotPenetrated()
    {
        sounds.PlayNotPenetratedSound();
    }

    bool TryGetArmorComponent()
    {
        if(hit2.collider.gameObject.transform.root.TryGetComponent<ArmorPlate>(out armorHit) || hit2.collider.gameObject.TryGetComponent<ArmorPlate>(out armorHit))
        {
            return true;
        }
        else
        {
            Transform parent=hit2.collider.gameObject.transform.parent;
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
    void SetGearNumber(int number,Vector2 imagePos)
    {
        if(gearNumerator)
        {
            TextMeshProUGUI text;
            //Vector2 pos=new Vector2(imagePos.x,imagePos.y-offsetY);
            Vector2 pos=imagePos-gearNumberOffset;
            Vector2 screenPos;
            if(RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, pos, Camera.main, out screenPos))
            {
                text=GameObject.Instantiate(gearNumerator,pos,canvas.transform.rotation);
                text.transform.SetParent(canvas.gameObject.transform);
                text.text=number.ToString();
                text.transform.localPosition=screenPos;
                text.transform.localScale=new Vector3(1,1,1);
            }
        }
        else
        Debug.Log("Please set text for gear");
    }
    void CheckBulletComponents(ref GameObject bul)
    {
        
        if(!(bul.TryGetComponent<Bullet>(out Bullet bullet)))
        bul.AddComponent<Bullet>();
        if(!bul.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
        {
        bul.AddComponent<Rigidbody>();
        Rigidbody rigit=bul.GetComponent<Rigidbody>();
        rigit.collisionDetectionMode=CollisionDetectionMode.ContinuousDynamic;
        }
        if(!bul.TryGetComponent<Collider>(out Collider collider))
        {
        bul.AddComponent<MeshCollider>();
        MeshCollider meshCollider=bul.GetComponent<MeshCollider>();
        meshCollider.convex=true;
        }
        if(bul.layer!=3)
        {
            bul.layer=3;
        }
    }
}
