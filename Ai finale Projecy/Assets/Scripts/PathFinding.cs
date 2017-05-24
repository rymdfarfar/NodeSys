using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public bool checkStartGoal;
    public bool foundGoal;


    public List<Node> openList;
   
    public List<Node> pathToGoal;
  


    public Node start;
    public Node end;
    public Node current;
    public int nodeSystem;
    bool aiMoveToPlayer;
    public bool move;
    bool first;
    bool firstMove;
    int indexPos;
    public float speed;
    float step;
    // Use this for initialization
    void Start()
    {
        //firstMove = true;
        
        first = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!move)
        {



            //Node destination = MoveToRandomPoint();
            PathFind(GameManager.instance.playesCurrentNode);
            firstMove = true;
          
           
           
        }


        if (foundGoal && move)
        {
            if (firstMove)
            {
                indexPos = pathToGoal.Count - 1;
                firstMove = false;
            }


            //Debug.Log(indexPos);

            //Debug.Log(boundingBox);
            bool nextNode = pathToGoal[indexPos].cube.Contains(this.transform.position);
            step = speed * Time.deltaTime;
            Vector3 moveTo = Vector3.MoveTowards(transform.position, pathToGoal[indexPos].cube.center, step);
       
            transform.position = moveTo;
            if (nextNode)
            {
                current = pathToGoal[indexPos];
                --indexPos;
              

                step = 0;

            }


            if (indexPos <0)
            {
                move = false;
                checkStartGoal = false;

            }

            if (pathToGoal.Count > 0 && move)
            {
                foreach (Node n in pathToGoal)
                {
                    int index = pathToGoal.IndexOf(n) + 1;
                    if (index < pathToGoal.Count)
                    {

                        Debug.DrawLine(n.cube.center, pathToGoal[index].cube.center, Color.cyan, 5);
                    }

                }
            }

        }
    }
    

    public int PosToIndex(Vector3 pos, NodeManager.NodeSystem ns)
    {
        int index = (int)(pos.x + pos.z * ns.widht + pos.y * (ns.widht * ns.depth));
        return index;
    }

    public Node MoveToRandomPoint()
    {
        int rnd = Random.Range(0, NodeManager.singelton.nodeSystems[nodeSystem].nodes.Count - 1);
        while (NodeManager.singelton.nodeSystems[nodeSystem].nodes[rnd].type == NodeManager.NodeTypes.Invalid)
        {
            rnd = Random.Range(0, NodeManager.singelton.nodeSystems[nodeSystem].nodes.Count - 1);
        }
        Debug.Log(rnd);
        Node temp = NodeManager.singelton.nodeSystems[nodeSystem].nodes[rnd];
        return temp;
    }

    public void PathFind(Node endPos)
    {
        if (!checkStartGoal)
        {

            openList.Clear();
            pathToGoal.Clear();
        }


        if (!first)
        {
            start = current;
        }
        foreach (Node n in NodeManager.singelton.nodeSystems[nodeSystem].nodes)
        {
            n.closed = false;
            if (first)
            {
                if (n.cube.Contains(this.transform.position))
                    start = n;
            }

        }
        if (end.myNodeSysId != start.myNodeSysId)
        {
            foreach (Node n in NodeManager.singelton.nodeSystems[end.myNodeSysId].nodes)
            {
                n.closed = false;
            }
        }
       



        first = false;
        end = endPos;
        checkStartGoal = true;

        SetStartAndGoal(start, end);
        move = true;
        if (checkStartGoal)
        {

            ContinuePath();
        }





    }

    public void SetStartAndGoal(Node start, Node Goal)
    {
        start.G = 0;
        start.H = start.ManHattanDistance(Goal);
        start.F = start.G + start.H;
        start.parent = null;
        openList.Add(start);
    }
    public Node GetNextNode()
    {
        float bestF = 99999.0f;
        int nodeIndex = -1;
        Node nextNode = null;
        Node temp = null;
        for (int index = 0; index < openList.Count; ++index)
        {
            temp = openList[index];
            if (temp.F < bestF)
            {
                bestF = temp.F;
                nodeIndex = index;
            }
        }
        if (nodeIndex >= 0)
        {
            nextNode = openList[nodeIndex];
            nextNode.closed = true;
            openList.Remove(nextNode);
        }

        return nextNode;
    }

    public void ContinuePath()
    {
        while (openList.Count > 0)
        {

            Node currentNode = GetNextNode();
            if (currentNode.id == end.id && currentNode.myNodeSysId == end.myNodeSysId)
            {
                end.parent = currentNode.parent;
                foundGoal = true;
                break;
            }
            else
            {
                foreach (Node n in currentNode.connectingNodes)
                {

                    PathOpened(n, 1, currentNode);
                }
            }

        }

        if (foundGoal)
        {

            Node getPath;
            for (getPath = end; getPath != null; getPath = getPath.parent)
            {
                pathToGoal.Add(getPath);
               
            }
        }

    }
    public void PathOpened(Node currentNode, float newCost, Node parent)
    {
        if (currentNode != null)
        {

            int id = currentNode.id;

            foreach (NodeManager.NodeSystem ns in NodeManager.singelton.nodeSystems)
            {
                if (currentNode.myNodeSysId == ns.id)
                {
                    if (ns.nodes[id].closed || ns.nodes[id].type == NodeManager.NodeTypes.Invalid)
                        return;
                }
                
            }

          


            currentNode.G = parent.G + newCost;
            currentNode.H = parent.ManHattanDistance(end);
            currentNode.F = currentNode.G + currentNode.H;
            currentNode.parent = parent;
            Node temp = null;
            for (int index = 0; index < openList.Count; ++index)
            {
                temp = openList[index];
                if (id == temp.id)
                {
                    float newF = currentNode.G + temp.H;
                    if (temp.F > newF)
                    {
                        temp.G = currentNode.G;
                        currentNode.F = currentNode.G + currentNode.H;
                        temp.parent = currentNode;
                    }

                    return;
                }

            }
            openList.Add(currentNode);
        }
      
    }
}
