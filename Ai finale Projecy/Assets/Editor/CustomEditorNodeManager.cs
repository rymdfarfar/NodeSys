using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NodeManager))]
public class CustomEditorNodeManager : Editor {

  

    bool connected;


    public override void OnInspectorGUI()
    {
        NodeManager myTarget = (NodeManager)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Build Node Partition"))
        {
            Debug.ClearDeveloperConsole();
            myTarget.CreatePartitionOfNodes();

            if (myTarget.partitionCreated)
            {
                foreach (NodeManager.NodeSystem ns in myTarget.nodeSystems)
                {
                    foreach (Node n in ns.nodes)
                    {
                        Undo.RegisterCreatedObjectUndo(n.gameObject, "Created Node");
                    }
                }
                Undo.RecordObject(myTarget, "built node partition");
                myTarget.partitionCreated = false;
                myTarget.nodesSpawned = 0;
         
            }


        }
        if (GUILayout.Button("Reset Node Partition"))
        {
           
            Debug.ClearDeveloperConsole();
            foreach (Transform t in myTarget.transform)
            {
                Undo.DestroyObjectImmediate(t.gameObject);
            }
            Undo.RecordObject(myTarget, "built node partition");
            myTarget.nodeSystems.Clear();
           
        }

        if (GUILayout.Button("Build Node Information"))
        {
            Debug.ClearDeveloperConsole();
            foreach (string tag in myTarget.tags)
            {
                myTarget.level.AddRange(GameObject.FindGameObjectsWithTag(tag));
            }

            Debug.Log("kina");
            Active();
        


        }

       

    }
   

  
    #region NodeInMapSpace
    public void Closed()
    {
        NodeManager myTarget = (NodeManager)target;
        bool overlapping;

        foreach (NodeManager.NodeSystem ns in myTarget.nodeSystems)
        {
            foreach (Node n in ns.nodes)
            {
                
                foreach (GameObject go in myTarget.level)
                {
                    Collider col = go.GetComponent<Collider>();
                    overlapping = n.cube.Intersects(col.bounds);
                    if (overlapping)
                    {

                        n.type = NodeManager.NodeTypes.Invalid;
                    }
                    //Undo.RecordObject(n, "changed");
                }
            }
          
        }
      

        //if (this != null)
        //{
        //    Undo.RecordObject(this, "built node partition");
        //}

    }



    public void ConnectNodes()
    {
        NodeManager myTarget = (NodeManager)target;
        foreach (NodeManager.NodeSystem ns in myTarget.nodeSystems)
        {


            foreach (Node n in ns.nodes)
            {
                NodeManager.NodeSystem nodeSys = myTarget.nodeSystems[n.myNodeSysId];
                if (n.type != NodeManager.NodeTypes.Invalid)
                {

                    //Right
                    int number = n.x + 1;
                    int index = number + n.z * myTarget.xNumber + n.y * (myTarget.xNumber * myTarget.zNumber);
                    if (index >= 0 && index < nodeSys.nodes.Count  && number<= myTarget.xNumber -1)
                    {
                        if (nodeSys.nodes[index].type != NodeManager.NodeTypes.Invalid)
                            n.connectingNodes.Add(nodeSys.nodes[index]);
                    }



                    //Left
                    number = n.x - 1;
                    index = number + n.z * myTarget.xNumber + n.y * (myTarget.xNumber * myTarget.zNumber);
                    if (index >= 0 && index < nodeSys.nodes.Count && number >= 0)
                    {
                        if (nodeSys.nodes[index].type != NodeManager.NodeTypes.Invalid)
                            n.connectingNodes.Add(nodeSys.nodes[index]);
                    }


                    //Up
                    number = n.y + 1;
                    index = n.x + n.z * myTarget.xNumber + number * (myTarget.xNumber * myTarget.zNumber);
                    if (index >= 0 && index < nodeSys.nodes.Count && number<= myTarget.yNumber -1 )
                    {
                        if (nodeSys.nodes[index].type != NodeManager.NodeTypes.Invalid)
                            n.connectingNodes.Add(nodeSys.nodes[index]);
                    }



                    //Below
                    number = n.y - 1;
                    index = n.x + n.z * myTarget.xNumber + number * (myTarget.xNumber * myTarget.zNumber);
                    if (index >= 0 && index < nodeSys.nodes.Count && number >= 0)
                    {
                        if (nodeSys.nodes[index].type != NodeManager.NodeTypes.Invalid)
                            n.connectingNodes.Add(nodeSys.nodes[index]);
                    }




                    //forward Node
                    number = n.z + 1;
                    index = n.x + number * myTarget.xNumber + n.y  * (myTarget.xNumber * myTarget.zNumber);
                    if (index >= 0 && index < nodeSys.nodes.Count && number <= myTarget.zNumber -1)
                    {
                        if (nodeSys.nodes[index].type != NodeManager.NodeTypes.Invalid)
                            n.connectingNodes.Add(nodeSys.nodes[index]);
                    }



                    //Backward Node
                    number = n.z - 1;
                    index = n.x + number * myTarget.xNumber + n.y  * (myTarget.xNumber * myTarget.zNumber);
                    if (index >= 0 && index < nodeSys.nodes.Count && number >=0)
                    {
                        n.connectingNodes.Add(nodeSys.nodes[index]);
                    }



                }


            }

            //if (this != null)
            //    Undo.RecordObject(this, "built node partition");
        }
    }


    #endregion
    public void Active()
    {

        NodeManager myTarget = (NodeManager)target;
        Undo.RecordObject(this, "level");
        myTarget.level.Clear();
        foreach (string tag in myTarget.tags)
        {
            myTarget.level.AddRange( GameObject.FindGameObjectsWithTag(tag));
        }
        foreach (NodeManager.NodeSystem ns in myTarget.nodeSystems)
        {
            ns.doorNodes.Clear();
            foreach (Node n in ns.nodes)
            {

                n.myNodeSysId = ns.id;
                n.myNodeManager = myTarget;
                n.Activate();
               
              
               
            }
        }
               
        
     
     
            //Closed();
            //ConnectNodes();
        
       
        Undo.RecordObject(this, "nodeRaycasting");

    }
}
