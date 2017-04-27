using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToMouse : MonoBehaviour {
    public NodeManager nm;
    Vector3 movePos;
    public float speed;
    public int xp;
    public int yp;
    public int zp;
    public int index;
    public Vector3 pos;
    public int nodeSys;

    // Use this for initialization
    void Start () {
     pos = transform.position;
        foreach (Node n in nm.nodeSystems[0].nodes)
        {
            if (n.size.Contains(pos))
            {
                xp = n.x;
                yp = n.y;
                zp = n.z;
                pos = new Vector3(xp, yp, zp);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
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
        pos = MyPos(nm.nodeSystems[0].widht, nm.nodeSystems[0].widht, nm.nodeSystems[0].widht);
    }

    public Vector3 MyPos(float width, float height, float depth)
    {
        float difX = pos.x - transform.position.x;

        float difY = pos.y - transform.position.y;
        float difZ = pos.z - transform.position.z;
        if (difX > width)
        {
            ++xp;
        }
        else if (difX < 0)
        {
            --xp;
        }

        if (difY > height)
        {
            ++yp;
        }
        else if (difY < 0)
        {
            --yp;
        }

        if (difZ > depth)
        {
            ++zp;
        }
        else if (difZ < 0)
        {
            --zp;
        }

        Vector3 posInSys = new Vector3(xp, yp, zp);
        return posInSys;
           
    }
}
