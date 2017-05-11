using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;


#endif
[ExecuteInEditMode]
public class Node : MonoBehaviour
{

    public NodeManager myNodeManager;
    //[HideInInspector]
    public int myNodeSysId;
   
    public NodeManager.NodeTypes type;
    public List<Node> connectingNodes;
    public int id;
    public int x;
    public int y;
    public int z;
    public bool closed;
    public int index;
    [HideInInspector]
    public float G;
    [HideInInspector]
    public float H;
    [HideInInspector]
    public float F;
    [HideInInspector]
    public Vector3 pos;
    public Node parent;
    [HideInInspector]
    public Bounds cube;

    

    public float t;

    public bool raycasting = false;
    [HideInInspector]
    public bool delete;
 

    [HideInInspector]
    public bool up;
    [HideInInspector]
    public bool down;
    [HideInInspector]
    public bool left;
    [HideInInspector]
    public bool right;
    [HideInInspector]
    public bool forward;
    [HideInInspector]
    public bool back;

    public float raytimer;


    // Use this for initialization
    void Start()
    {

       


    }

    // Update is called once per frame

    public void Update()
    {


        //if(size.Contains(GameManager.instance.player.transform.position))
        //{
        //    GameManager.instance.index = id;
        //}
    }


    private void OnDrawGizmosSelected()
    {

        if (type == NodeManager.NodeTypes.Invalid)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireCube(cube.center, cube.size);

        }
        else
        {
            if (type == NodeManager.NodeTypes.Door)
            {
                Gizmos.color = Color.cyan;
            }
            else
                Gizmos.color = Color.green;

            Gizmos.DrawWireCube(cube.center, cube.size);
            foreach (Node n in connectingNodes)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(cube.center, n.cube.center);
            }
        }

    }

    public float ManHattanDistance(Node nodeEnd)
    {
        float x = Mathf.Abs(pos.x - nodeEnd.pos.x);
        float y = Mathf.Abs(pos.y - nodeEnd.pos.y);
        float z = Mathf.Abs(pos.z - nodeEnd.pos.z);

        return x + y + z;
    }
    #region editor
#if UNITY_EDITOR
    public void EditorUpdate()
    {

        if (!EditorApplication.isUpdating)
        {

            Debug.Log("update");
        }
        Debug.Log("raycasting");
        Vector3 upVec = transform.TransformDirection(Vector3.up);
        Vector3 downVec = transform.TransformDirection(Vector3.down);
        Vector3 leftVec = transform.TransformDirection(Vector3.left);
        Vector3 rightVec = transform.TransformDirection(Vector3.right);
        Vector3 backVec = transform.TransformDirection(Vector3.back);
        Vector3 forwardVec = transform.TransformDirection(Vector3.forward);
        Vector3 maxBWorldPos = myNodeManager.nodeSystems[myNodeSysId].area.center - (myNodeManager.nodeSystems[myNodeSysId].area.max / 2);
        if (raycasting)
        {
         

            t += Time.deltaTime;




            if (Physics.Raycast((cube.center), upVec, 100000))
            {
                up = true;

            }

            if (Physics.Raycast(cube.center, downVec, 100000))
            {
                down = true;

            }
            if (type != NodeManager.NodeTypes.Invalid)
            {
                if (x == 0)
                {
                    if (!Physics.Raycast(cube.center, leftVec, 5))
                    {
                        type = NodeManager.NodeTypes.Door;


                    }
                }
                else if (x == myNodeManager.nodeSystems[myNodeSysId].widht - 1)
                {
                    if (!Physics.Raycast(cube.center, rightVec, 5))
                    {
                        type = NodeManager.NodeTypes.Door;


                    }
                }
                else
                {

                    if (z == 0)
                    {
                        if (!Physics.Raycast(cube.center, backVec, 5))
                        {
                            type = NodeManager.NodeTypes.Door;


                        }
                    }
                    else if (z == myNodeManager.nodeSystems[myNodeSysId].depth - 1)
                    {
                        if (!Physics.Raycast(cube.center, forwardVec, 5))
                        {
                            type = NodeManager.NodeTypes.Door;


                        }
                    }
                }
            }


                if (t > raytimer)
            {
                Debug.Log("china");
                delete = true;
                raycasting = false;
            }

         


            if (delete)
            {
                if (up != true || down != true)
                {
                    Undo.RecordObject(this, "changed");
                    type = NodeManager.NodeTypes.Invalid;

                }
               
                delete = false;

            }
            if (!raycasting)
            {
                //Undo.RecordObject(this, "changed");
                Debug.Log("china 2");
                Closed();

                EditorApplication.update -= EditorUpdate;
          

            }
        }

    }


    public void ConnectNodes()
    {
        if (type != NodeManager.NodeTypes.Invalid)
        {
            Undo.RecordObject(this, "changed");
            //Right
            int number = x + 1;
            int index = number + z * myNodeManager.nodeSystems[myNodeSysId].widht + y * (myNodeManager.nodeSystems[myNodeSysId].widht * myNodeManager.nodeSystems[myNodeSysId].depth);
            if (index >= 0 && index < myNodeManager.nodeSystems[myNodeSysId].nodes.Count && number <= myNodeManager.nodeSystems[myNodeSysId].widht - 1)
            {
                if (myNodeManager.nodeSystems[myNodeSysId].nodes[index].type != NodeManager.NodeTypes.Invalid)
                    connectingNodes.Add(myNodeManager.nodeSystems[myNodeSysId].nodes[index]);
            }



            //Left
            number = x - 1;
            index = number + z * myNodeManager.nodeSystems[myNodeSysId].widht + y * (myNodeManager.nodeSystems[myNodeSysId].widht * myNodeManager.nodeSystems[myNodeSysId].depth);
            if (index >= 0 && index < myNodeManager.nodeSystems[myNodeSysId].nodes.Count && number >= 0)
            {
                if (myNodeManager.nodeSystems[myNodeSysId].nodes[index].type != NodeManager.NodeTypes.Invalid)
                    connectingNodes.Add(myNodeManager.nodeSystems[myNodeSysId].nodes[index]);
            }


            //Up
            number = y + 1;
            index = x + z * myNodeManager.nodeSystems[myNodeSysId].widht + number * (myNodeManager.nodeSystems[myNodeSysId].widht * myNodeManager.nodeSystems[myNodeSysId].depth);
            if (index >= 0 && index < myNodeManager.nodeSystems[myNodeSysId].nodes.Count && number <= myNodeManager.nodeSystems[myNodeSysId].height - 1)
            {
                if (myNodeManager.nodeSystems[myNodeSysId].nodes[index].type != NodeManager.NodeTypes.Invalid)
                    connectingNodes.Add(myNodeManager.nodeSystems[myNodeSysId].nodes[index]);
            }



            //Below
            number = y - 1;
            index = x + z * myNodeManager.nodeSystems[myNodeSysId].widht + number * (myNodeManager.nodeSystems[myNodeSysId].widht * myNodeManager.nodeSystems[myNodeSysId].depth);
            if (index >= 0 && index < myNodeManager.nodeSystems[myNodeSysId].nodes.Count && number >= 0)
            {
                if (myNodeManager.nodeSystems[myNodeSysId].nodes[index].type != NodeManager.NodeTypes.Invalid)
                    connectingNodes.Add(myNodeManager.nodeSystems[myNodeSysId].nodes[index]);
            }




            //forward Node
            number = z + 1;
            index = x + number * myNodeManager.nodeSystems[myNodeSysId].widht + y * (myNodeManager.nodeSystems[myNodeSysId].widht * myNodeManager.nodeSystems[myNodeSysId].depth);
            if (index >= 0 && index < myNodeManager.nodeSystems[myNodeSysId].nodes.Count && number <= myNodeManager.nodeSystems[myNodeSysId].depth - 1)
            {
                if (myNodeManager.nodeSystems[myNodeSysId].nodes[index].type != NodeManager.NodeTypes.Invalid)
                    connectingNodes.Add(myNodeManager.nodeSystems[myNodeSysId].nodes[index]);
            }



            //Backward Node
            number = z - 1;
            index = x + number * myNodeManager.nodeSystems[myNodeSysId].widht + y * (myNodeManager.nodeSystems[myNodeSysId].widht * myNodeManager.nodeSystems[myNodeSysId].depth);
            if (index >= 0 && index < myNodeManager.nodeSystems[myNodeSysId].nodes.Count && number >= 0)
            {
                connectingNodes.Add(myNodeManager.nodeSystems[myNodeSysId].nodes[index]);
            }


         
        }
       


    }

    public void Closed()
    {
       
        bool overlapping = false;


        

            foreach (GameObject go in myNodeManager.level)
            {
                Collider col = go.GetComponent<Collider>();
                overlapping = cube.Intersects(col.bounds);
          
                if (overlapping)
                {
                Undo.RecordObject(this, "changed");
                type = NodeManager.NodeTypes.Invalid;
               
                    break;  
                
                }
               
            
              
            }
          
      
        if (!overlapping &&type!= NodeManager.NodeTypes.Invalid)
        {

            if (type == NodeManager.NodeTypes.Door)
            {
                myNodeManager.nodeSystems[myNodeSysId].doorNodes.Add(this);
            }
            Debug.Log("connecy");
            ConnectNodes();
        }
        
         







    }

    public void Activate()
    {
        Undo.RecordObject(this, "changed");
        closed = false;
        type = NodeManager.NodeTypes.Standard;
        raycasting = true;
        raytimer = 0.1f;
      
        EditorApplication.update += EditorUpdate;
        Debug.Log("activate");
      
       

    }
#endif
    #endregion
}
