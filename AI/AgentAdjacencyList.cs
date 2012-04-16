using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Platform
{
    public class AgentAdjacencyList
    {
        protected List<GameEntity> m_Entities;
        public List<GameEntity> Entities
        {
            get { return m_Entities; }
            set { m_Entities = value; }
        }

        protected List<AgentAdjacencyListEdge> m_Edges;
        public List<AgentAdjacencyListEdge> Edges
        {
            get { return m_Edges; }
            set { m_Edges = value; }
        }

        protected float m_Radius;
        public float Radius
        {
            get { return m_Radius; }
            set { m_Radius = value; }
        }

        protected Agent m_Owner;
        public Agent Owner
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetObjects"></param>
        /// <param name="radius"></param>
        /// <param name="owner"></param>
        public AgentAdjacencyList(Agent owner, List<GameEntity> targetObjects, float radius)
        {
            //grab the list to track
            m_Entities = targetObjects;

            //get the radius of the list
            m_Radius = radius;

            //get the owner of the list
            m_Owner = owner;

            //init the InRange list
            m_Edges = new List<AgentAdjacencyListEdge>();
        }

        public void Update()
        {
            //Console.WriteLine("Updating Adjacency List of Entity " + m_Owner.ID + ".\nSearching through " + m_Entities.Count + " entities.\n Using a radius of " + m_Radius);
            //work in squared distances
            float radiusSquared = m_Radius * m_Radius;

            //clear the list
            m_Edges.Clear();

            //loop through all the entities
            for (int i = 0; i < m_Entities.Count; i++)
            {
                //Console.Write("Entity " + m_Entities[i].ID + ": ");
                if (m_Entities[i].ID != m_Owner.ID)
                {
                    //get vector to target
                    Vector3 toTarget = m_Entities[i].Position - m_Owner.Position;

                    //get length(squared) of toTarget Vector
                    float lengthSquared = toTarget.LengthSquared();

                    //Console.WriteLine("Squared Distance = " + lengthSquared + ".");
                    //see if it's within the radius
                    if (lengthSquared <= radiusSquared)
                    {
                        //get the actual distance
                        float distance = (float)Math.Sqrt(lengthSquared);

                        //get the direction of the toTarget vector
                        float angle = (float)Math.Atan2(-toTarget.Z, toTarget.X);

                        //[-pi, pi)
                        angle = MathHelper.WrapAngle(angle);

                        //target angle
                        float targetAngle = -MathHelper.WrapAngle(m_Owner.Heading - angle + MathHelper.ToRadians(90));
                        targetAngle = MathHelper.WrapAngle(targetAngle);
                        if (targetAngle < 0)
                            targetAngle += MathHelper.ToRadians(360.0f);

                        //create the new object to insert into the list
                        AgentAdjacencyListEdge edge = new AgentAdjacencyListEdge(m_Entities[i], distance, targetAngle);

                        //insert it into the list

                        m_Edges.Add(edge);
                        //Console.WriteLine("Entity " + edge.Entity.ID + " added to list");
                    }
                }
            }
            //Console.WriteLine("Update Complete");
        }

        /// <summary>
        /// 
        /// </summary>
        public void PrintList()
        {
            for (int i = 0; i < m_Edges.Count; i++)
            {
            }
        }
    }
}
