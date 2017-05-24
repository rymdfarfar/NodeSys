using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToMouse : MonoBehaviour {
   
    Vector3 movePos;
    public float speed;
    public Vector3 pos;
    public int nodeSys;
    public Node cn;

    // Use this for initialization
    void Start () {
     pos = transform.position;
       
    }
	
	// Update is called once per frame
	void Update () {
        if (cn == null)
        {
            foreach (Node n in NodeManager.singelton.nodeSystems[nodeSys].nodes)
            {
                if (n.cube.Contains(transform.position))
                {
                    cn = n;
                    GameManager.instance.playesCurrentNode = cn;
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

        foreach (Node n in cn.connectingNodes)
        {
            if (n.cube.Contains(transform.position))
            {
                cn = n;
                GameManager.instance.playesCurrentNode = cn;
                return;
            }
        }
    }

    //public Vector3 MyPos(float width, float height, float depth)
    //{
    //    float difX = pos.x - transform.position.x;

    //    float difY = pos.y - transform.position.y;
    //    float difZ = pos.z - transform.position.z;
    //    if (difX > width)
    //    {
    //        ++xp;
    //    }
    //    else if (difX < 0)
    //    {
    //        --xp;
    //    }

    //    if (difY > height)
    //    {
    //        ++yp;
    //    }
    //    else if (difY < 0)
    //    {
    //        --yp;
    //    }

    //    if (difZ > depth)
    //    {
    //        ++zp;
    //    }
    //    else if (difZ < 0)
    //    {
    //        --zp;
    //    }

    //    Vector3 posInSys = new Vector3(xp, yp, zp);
    //    return posInSys;
           
    //}
}
