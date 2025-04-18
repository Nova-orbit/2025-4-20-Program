using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BridgeManager : MonoBehaviour
{
    public GameObject PreBridge;
    private Rigidbody2D SelfRigidbody = null;
    private Joint2D[] joints = null;
    private GameObject JointobjA, JointobjB;

    //// Start is called before the first frame update
    private void Start()
    {
        SelfRigidbody = GetComponent<Rigidbody2D>();
        joints = GetComponents<Joint2D>(); 
        JointobjA = joints[0].connectedBody.gameObject;
        JointobjB = joints[1].connectedBody.gameObject;
    }
    // Update is called once per frame
    private void Update()
    {
        if (Global.StartButtonOn)
        {
            SelfRigidbody.gravityScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SelfRigidbody.gravityScale = 1;
        }
        if (Global.AdjustMode)
        {
            Vector3 Direction = (JointobjA.transform.position - JointobjB.transform.position);
            float Distance = Direction.magnitude;
            Direction.Normalize();//长度与方向

            Vector3 BridgePosition = JointobjB.transform.position + Distance * Direction / 2;//计算位置
            Direction.z = 0;
            Quaternion BridgeRotation = Quaternion.LookRotation(Vector3.forward, Direction) * Quaternion.Euler(0, 0, 90);//计算角度/调整前方向为x轴
            gameObject.transform.position = BridgePosition;
            gameObject.transform.rotation = BridgeRotation;
            gameObject.transform.localScale = new Vector3(Distance, 0.25f, 1);//拉伸长度
        }
    }

    private void OnJointBreak2D(Joint2D joint)
    {
        GameObject point = joint.connectedBody.gameObject;
        if (point.tag == "ConnectPoint")
        {
            Destroy(joint.connectedBody.gameObject);
        }
    }
}
