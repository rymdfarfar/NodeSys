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

        if (GUILayout.Button("Connect Node Doors"))
        {
            Debug.ClearDeveloperConsole();
            foreach (NodeManager.NodeSystem ns in myTarget.nodeSystems)
            {
                foreach (Node nd in ns.doorNodes)
                {
                    //nd.connectingNodes.Clear();
                    //nd.ConnectNodes();
                    nd.ConnectDoors();
                }
            }



        }



    }

    public void Active()
    {

        NodeManager myTarget = (NodeManager)target;
        Undo.RecordObject(this, "level");
        myTarget.level.Clear();
        foreach (string tag in myTarget.tags)
        {
            myTarget.level.AddRange(GameObject.FindGameObjectsWithTag(tag));
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

        Undo.RecordObject(this, "nodeRaycasting");

    }
}
