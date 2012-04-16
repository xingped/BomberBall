using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Platform
{
    public class AgentAdjacencyListEdge
    {
        protected GameEntity m_Entity;
        public GameEntity Entity
        {
            get { return m_Entity; }
            set { m_Entity = value; }
        }

        protected float m_Distance;
        public float Distance
        {
            get { return m_Distance; }
            set { m_Distance = value; }
        }

        protected float m_Angle;
        public float Angle
        {
            get { return m_Angle; }
            set { m_Angle = value; }
        }

        /// <summary>
        /// Creates a new edge with null and 0 values
        /// </summary>
        public AgentAdjacencyListEdge()
        {
            //init fields
            m_Entity = null;

            m_Distance = 0;

            m_Angle = 0;
        }

        /// <summary>
        /// Creates a new edge for use with an AgentAgencyList
        /// </summary>
        /// <param name="entity">The entity that the adjacency list is tracking.</param>
        /// <param name="distance">The distance this entity is from the owner entity of the list.</param>
        /// <param name="angle">The angle, in relation to the owner's heading that this entity is.</param>
        public AgentAdjacencyListEdge(GameEntity entity, float distance, float angle)
        {
            m_Entity = entity;
            m_Distance = distance;
            m_Angle = angle;
            m_Angle = MathHelper.WrapAngle(m_Angle);
            if (m_Angle < 0)
                m_Angle += MathHelper.TwoPi;
        }
    }
}
