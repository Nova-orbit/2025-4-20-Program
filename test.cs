using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class test : MonoBehaviour
{
    public GameObject PrefabConnectPoint;
    public GameObject PrefabBridge, PrefabSupport, PrefabRope;
    public GameObject StartPoint = null, EndPoint = null, Middlepoint = null;
    public GameObject Bridge = null;
    private GameObject BigStart = null,BigTouch = null,BigBridge=null;
    private HingeJoint2D HingeA, HingeB;
    private SpringJoint2D SpringA, SpringB;
    private  bool StartPointset = false, EndPointset = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!Global.StartButtonOn)
        {
            if (Global.BuildingMode) BuildM();
            if (Global.DestoryMode)DestoryM();
            if (Global.AdjustMode)Adjust();
            Big();
        }

        void Adjust()
        {
            if (Mousetouchbool(LayerMask.GetMask("MouseOnly")))
            {
                if (TouchConnectPoint())
                {
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        Mousetouch(LayerMask.GetMask("MouseOnly")).transform.position = mouseWorldPos();
                    }
                }
            }
        }
    }

    private void DestoryM()
    {
        if (Mousetouchbool())
        {
            if (TouchConnectPoint() || TouchBridge())
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Destroy(Mousetouch());
                }
            }
        }
    }

    private void BuildM()
    {
        if (Global.NumToBuild != 0)
        {
            PlaceConnectPiont();
            PlaceBuilding(BuildingtoPlace());

            void PlaceConnectPiont()//放置连接点
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (!IsPointerOverUI())
                    {
                        if (StartPointset) SetEndPoint();
                        else SetStartPoint();
                    }
                    else
                    {
                        StartPointset = false; StartPoint = null;
                        EndPointset = false; EndPoint = null;
                    }
                }
                void SetStartPoint()
                {
                    if (Mousetouchbool(LayerMask.GetMask("MouseOnly")))
                    {
                        if (TouchConnectPoint())
                        {
                            StartPoint = Mousetouch(LayerMask.GetMask("MouseOnly"));
                            StartPointset = true;
                        }
                    }
                    else
                    {
                        StartPoint = Instantiate(PrefabConnectPoint, mouseWorldPos(), transform.rotation);
                        StartPointset = true;
                    }

                }
                void SetEndPoint()
                {
                    if (Mousetouchbool(LayerMask.GetMask("MouseOnly")))
                    {
                        if (TouchConnectPoint())
                        {
                            EndPoint = Mousetouch(LayerMask.GetMask("MouseOnly"));
                            EndPointset = true;
                        }
                    }
                    else
                    {
                        EndPoint = Instantiate(PrefabConnectPoint, mouseWorldPos(), transform.rotation);
                        EndPointset = true;
                    }

                }
            }
            void PlaceBuilding(GameObject building)//放置桥梁
            {
                if (EndPointset)
                {
                    CalculateBridge(building);
                    if (building.tag == "Bridge")
                    {
                        HingeSet(HingeA, StartPoint, -0.5f);
                        HingeSet(HingeB, EndPoint, 0.5f);
                    }
                    else if (building.tag == "Rope")
                    {
                        SpringSet(SpringA, StartPoint, -0.5f);
                        SpringSet(SpringB, EndPoint, 0.5f);
                    }
                    BackSet();
                }



                void CalculateBridge(GameObject building)
                {
                    Vector3 Direction = (EndPoint.transform.position - StartPoint.transform.position);
                    float Distance = Direction.magnitude;
                    Direction.Normalize();//长度与方向

                    Vector3 BridgePosition = StartPoint.transform.position + Distance * Direction / 2;//计算位置
                    Direction.z = 0;
                    Quaternion BridgeRotation = Quaternion.LookRotation(Vector3.forward, Direction) * Quaternion.Euler(0, 0, 90);//计算角度/调整前方向为x轴
                    Bridge = Instantiate(building, BridgePosition, BridgeRotation);//生成桥梁
                    Bridge.transform.localScale = new Vector3(Distance, 0.25f, 1);//拉伸长度
                }

                void BackSet()
                {
                    if (Global.ContinuousMode)
                    {
                        StartPointset = true; StartPoint = EndPoint;
                    }
                    else { StartPointset = false; StartPoint = null; }
                    EndPointset = false; EndPoint = null;
                }

                void HingeSet(HingeJoint2D hg, GameObject point, float side)
                {
                    hg = Bridge.AddComponent<HingeJoint2D>();//添加连接组件
                    hg.connectedBody = point.GetComponent<Rigidbody2D>();
                    hg.anchor = new Vector2(side, 0);
                    hg.breakForce = 100f; hg.breakTorque = 100f;
                }

                void SpringSet(SpringJoint2D sp, GameObject point, float side)
                {
                    sp = Bridge.AddComponent<SpringJoint2D>();//添加连接组件
                    sp.connectedBody = point.GetComponent<Rigidbody2D>();
                    sp.anchor = new Vector2(side, 0);
                    sp.enableCollision = true;
                    sp.breakForce = 100000f;


                }
            }
        }
    }
    private void Big()
    {
        if (Global.BuildingMode)
        {
            StartBig();
            TouchBig();
        }
        if (Global.DestoryMode)
        {
            BridgeBig();
            TouchBig();
        }
        if (Global.AdjustMode)
        {
            TouchBig();
        }

        void BridgeBig()
        {
            if (BigBridge != Mousetouch())
            {
                if (BigBridge != null) BigBridge.transform.localScale = new Vector3(BigBridge.transform.localScale.x, 0.25f, 0.25f);
                if (TouchBridge()) BigBridge = Mousetouch(); else BigBridge = null;
                if (BigBridge != null) BigBridge.transform.localScale = new Vector3(BigBridge.transform.localScale.x, 0.35f, 0.35f);
            }
        }
        void TouchBig()
        {
            if (BigTouch != Mousetouch(LayerMask.GetMask("MouseOnly")))
            {
                if (BigTouch != null && BigTouch != BigStart) BigTouch.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                if (TouchConnectPoint()) BigTouch = Mousetouch(LayerMask.GetMask("MouseOnly")); else BigTouch = null;
                if (BigTouch != null && BigTouch != BigStart) BigTouch.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
            }
        }
        void StartBig()
        {
            if (BigStart != StartPoint)
            {
                if (BigStart != null) BigStart.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                if (TouchConnectPoint()) BigStart = StartPoint; else BigStart = null;
                if (BigStart != null) BigStart.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
            }
        }
        
    }

    public Vector3 mouseWorldPos()//返回鼠标坐标
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldPos0 = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos0.z = 0;
        return mouseWorldPos0;
    }
    public GameObject Mousetouch(LayerMask layer)//返回鼠标所触碰的物体
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(mouseWorldPos(), Vector2.zero, Mathf.Infinity, layer);
        if (hit.Length > 0)
        {
            if (hit[0].collider != null) //*涉及hit.collider 应立刻判断是非为null，不能直接调用其他部分
                return hit[0].collider.gameObject;
        }
        return null;
    }
    public GameObject Mousetouch()//返回鼠标所触碰的物体
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(mouseWorldPos(), Vector2.zero, Mathf.Infinity);
        if (hit.Length > 0)
        {
            if (hit[0].collider != null) //*涉及hit.collider 应立刻判断是非为null，不能直接调用其他部分
                return hit[0].collider.gameObject;
        }
        return null;
    }
    public bool Mousetouchbool(LayerMask lay)
    {
        if (Mousetouch(lay) == null) return false;
        return true;
    }
    public bool Mousetouchbool()
    {
        if (Mousetouch() == null) return false;
        return true;
    }
    private bool TouchConnectPoint()
    {
        if (Mousetouch(LayerMask.GetMask("MouseOnly")) != null) return (Mousetouch(LayerMask.GetMask("MouseOnly")).tag == "ConnectPoint") || (Mousetouch(LayerMask.GetMask("MouseOnly")).tag == "StaticConnectPoint");
        else return false;
    }
    private bool TouchBridge()
    {
        if (Mousetouch()!=null)  return (Mousetouch().tag == "Bridge") || (Mousetouch().tag == "Rope");
        else return false;
    }
    
    
    

    private GameObject BuildingtoPlace()
    {
        if (Global.NumToBuild==1)
            return PrefabBridge;
        else if (Global.NumToBuild == 2)
            return PrefabSupport;
        else if(Global.NumToBuild ==3)
            return PrefabRope;
        else return null;
    }
    bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();

    }
}