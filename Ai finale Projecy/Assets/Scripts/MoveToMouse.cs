using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToMouse : MonoBehaviour {
    public NodeManager aiFlying;
    public NodeManager aiCharging;
    Vector3 movePos;
    public float speed;
    public Vector3 pos;
    public int nodeSysFlying;
    public int nodeSysCharging;
    public Node cnFlying;
    public Node cnCharging;
    

    // Use this for initialization
    void Start () {
     pos = transform.position;
       
    }
	
	// Update is called once per frame
	void Update () {
        if (cnFlying == null)
        {
            foreach (Node n in aiFlying.nodeSystems[nodeSysFlying].nodes)
            {
                if (n.cube.Contains(transform.position))
                {
                    cnFlying = n;
                    n.myNodeSysId = nodeSysFlying;
                    AiManager.instance.playesCurrentNodeFlying = cnFlying;
                    return;
                }


            }
        }

        if (cnCharging == null)
        {
            foreach (Node n in aiCharging.nodeSystems[nodeSysFlying].nodes)
            {
                if (n.cube.Contains(transform.position))
                {
                    cnCharging = n;
                    n.myNodeSysId = nodeSysCharging;
                    AiManager.instance.playesCurrentNodeCharging = cnCharging;
                    return;
                }


            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                movePos = hit.point;
               
            }
          

        }
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, movePos, step);

        foreach (Node n in cnFlying.connectingNodes)
        {
            if (n.cube.Contains(transform.position))
            {
                cnFlying = n;
                n.myNodeSysId = nodeSysFlying;
                AiManager.instance.playesCurrentNodeFlying = cnFlying;
                return;
            }
        }

        foreach (Node n in cnCharging.connectingNodes)
        {
            if (n.cube.Contains(transform.position))
            {
                cnCharging = n;
                n.myNodeSysId = nodeSysCharging;
                AiManager.instance.playesCurrentNodeCharging = cnCharging;
                return;
            }
        }
    }

   
}
