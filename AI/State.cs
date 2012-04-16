using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Platform
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class State
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        public abstract void Execute(Agent agent);
    }

    /// <summary>
    /// 
    /// </summary>
    public class State_Navigate : State
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        public override void Execute(Agent agent)
        {
            agent.MaxSpeed = Agent.MAX_SPEED;
            agent.TurnSpeed = Agent.MAX_TURN_SPEED;

            // It is assured that by the time this is executed, all the
            // nodes will exist in the world.
            if (agent.EscapeNode == null)
            {
                float tempNodeDist = float.MaxValue;
                foreach (AgentAdjacencyListEdge edge in agent.AdjacencyListNodes.Edges)
                {
                    if (edge.Distance < tempNodeDist)
                    {
                        tempNodeDist = edge.Distance;
                        agent.EscapeNode = (Node)edge.Entity;
                    }
                }
            }

            if (agent.Invuln == true && DateTime.Now > agent.InvulnTimer)
            {
                agent.Invuln = false;
            }

            if (agent.NearBomb())
            {
                agent.NavPath = null;
                agent.ChangeState(new State_Escape());
            }
            else if (agent.NearEnemy())
            {
                agent.ChangeState(new State_Attack());
            }
            else
            {
                //Console.WriteLine("Executing navigate for agent " + agent.ID);
                //return;
                // Give the agent time to move to prevent constant course correction
                if (agent.NavDelay > DateTime.Now && agent.NavPath != null && agent.NextNode != null)
                {
                    //Console.WriteLine("Navigation Delay");
                    agent.NextNode = agent.MoveAlongPath(agent.NavPath, agent.NextNode);
                    return;
                }
                else if (agent.NavDelay < DateTime.Now)
                {
                    //Console.WriteLine("Adding NavDelay");
                    agent.NavDelay = DateTime.Now.AddSeconds(5);
                }

                // Check to see if the next node is under a block and drop bomb if appropriate
                foreach (Block block in agent.World.Blocks)
                {
                    if (agent.NextNode != null && agent.NextNode.Position == block.Position)
                    {
                        agent.DropBomb();
                        return;
                    }
                }

                // First find the closest agent
                //Console.Write("Finding closest agent... ");
                float tempAgentDist = float.MaxValue;
                Agent tempTargetAgent = null;
                foreach (AgentAdjacencyListEdge edge in agent.AdjacencyListEnemies.Edges)
                {
                    if (edge.Distance < tempAgentDist)
                    {
                        tempAgentDist = edge.Distance;
                        tempTargetAgent = (Agent)edge.Entity;
                    }
                }
                //Console.WriteLine("Agent " + tempTargetAgent.ID + " at " + tempTargetAgent.Position);

                // If that agent is different from the current target agent,
                // recalculate navigation, otherwise continue moving.
                if (!tempTargetAgent.Equals(agent.TargetAgent))
                {
                    // Find the node closest to the target enemy, use as end
                    //Console.Write("Finding end node... ");
                    float tempEndNodeDist = float.MaxValue;
                    foreach (AgentAdjacencyListEdge edge in tempTargetAgent.AdjacencyListNodes.Edges)
                    {
                        //Console.WriteLine("Looking at edge.\n");
                        if (edge.Distance < tempEndNodeDist)
                        {
                            tempEndNodeDist = edge.Distance;
                            agent.EndNode = (Node)edge.Entity;
                        }
                    }
                    //if (agent.EndNode == null)
                      //  return;
                    //Console.WriteLine("Node " + agent.EndNode.ID + " at " + agent.EndNode.Position);

                    // Find the node closest to the current agent, use as start
                    //Console.Write("Finding start node... ");
                    float tempStartNodeDist = float.MaxValue;
                    foreach (AgentAdjacencyListEdge edge in agent.AdjacencyListNodes.Edges)
                    {
                        if (edge.Distance < tempStartNodeDist)
                        {
                            tempStartNodeDist = edge.Distance;
                            agent.StartNode = (Node)edge.Entity;
                        }
                    }
                    //if (agent.StartNode == null)
                        //return;
                    //Console.WriteLine("Node " + agent.StartNode.ID + " at " + agent.StartNode.Position);

                    agent.NavPath = agent.FindPath(agent.StartNode, agent.EndNode);
                    agent.NextNode = agent.StartNode;
                }
                else
                {
                    agent.NextNode = agent.MoveAlongPath(agent.NavPath, agent.NextNode);
                }
            }

            return;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class State_Escape : State
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        public override void Execute(Agent agent)
        {
            //agent.MaxSpeed = Agent.PANIC_SPEED;
            agent.TurnSpeed = Agent.PANIC_SPEED;
            if (!agent.NearBomb())
            {
                agent.NavPath = null;
                agent.ChangeState(new State_Navigate());
            }
            else
            {
                //Console.WriteLine("Executing navigate for agent " + agent.ID);
                //return;
                // Give the agent time to move to prevent constant course correction
                if (agent.NavDelay > DateTime.Now && agent.NavPath != null && agent.NextNode != null)
                {
                    //Console.WriteLine("Navigation Delay");
                    agent.NextNode = agent.MoveAlongPath(agent.NavPath, agent.NextNode);
                    return;
                }
                else if (agent.NavDelay < DateTime.Now)
                {
                    //Console.WriteLine("Adding NavDelay");
                    agent.NavDelay = DateTime.Now.AddSeconds(5);
                }

                //find the smallest distance to the escape position
                AgentAdjacencyListEdge escapeEdge = new AgentAdjacencyListEdge();
                GameEntity escapeNode = escapeEdge.Entity;
                escapeEdge.Distance = float.MaxValue;

                //find node closest to agent
                AgentAdjacencyListEdge closestEdge = new AgentAdjacencyListEdge();
                GameEntity closestNode = closestEdge.Entity;
                closestEdge.Distance = float.MaxValue;

                //loop through all the edges in the adjacency list of nodes
                //check all the distances and retain the smallest distance
                //and record that node as the node to traverse to
                for (int i = 0; i < agent.AdjacencyListNodes.Edges.Count; i++)
                {
                    //get distance from node to escape position
                    float distance = Vector3.Distance(agent.StartingLocation, agent.AdjacencyListNodes.Edges[i].Entity.Position);

                    //compare the distances
                    if (distance < escapeEdge.Distance)
                    {
                        //set new escape edge
                        escapeEdge = agent.AdjacencyListNodes.Edges[i];

                        //set the new escape node
                        escapeNode = ((Node)escapeEdge.Entity);
                    }

                    //comparison for closest edge
                    distance = Vector3.Distance(agent.Position, agent.AdjacencyListNodes.Edges[i].Entity.Position);

                    //compare the distances
                    if (distance < closestEdge.Distance)
                    {
                        //set the closest edge
                        closestEdge = agent.AdjacencyListNodes.Edges[i];

                        //set the closest node
                        closestNode = ((Node)closestEdge.Entity);
                    }
                }

                agent.NavPath = agent.FindPath(((Node)closestNode), ((Node)escapeNode));
                agent.NextNode = (Node)closestNode;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class State_Attack : State
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        public override void Execute(Agent agent)
        {
            if (agent.NearBomb())
            {
                agent.NavPath = null;
                agent.ChangeState(new State_Escape());
            }
            else if (!agent.NearEnemy())
            {
                agent.NavPath = null;
                agent.ChangeState(new State_Navigate());
            }
            else
            {
                agent.DropBomb();
            }
        }
    }
}
