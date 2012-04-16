using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Platform
{
    public class SteeringBehaviors
    {

        public enum DECELERATION { slow, normal, fast };

        protected DECELERATION m_Deceleration;
        public DECELERATION Deceleration
        {
            get { return m_Deceleration; }
            set { m_Deceleration = value; }
        }

        protected float m_PanicDistance;
        public float PanicDistance
        {
            get { return m_PanicDistance; }
            set { m_PanicDistance = value; }
        }

        protected Agent m_Agent;
        public Agent Agent
        {
            get { return m_Agent; }
            set { m_Agent = value; }
        }

        public SteeringBehaviors(Agent agent)
        {
            m_Agent = agent;
        }

        public Vector3 Seek(Vector3 targetPosition)
        {
            //get normalized vector conntecting agent to target position
            Vector3 desiredDirection = Vector3.Normalize(targetPosition - m_Agent.Position);

            //get the desired speed to travel along the desired direction
            Vector3 desiredDirectionMaxSpeed = desiredDirection * Agent.DEFAULT_SPEED;

            if (Vector3.DistanceSquared(m_Agent.Position, targetPosition) > 1)
            {
                return desiredDirectionMaxSpeed - m_Agent.Velocity;
            }

            return Vector3.Zero;
        }

        public Vector3 Flee(Vector3 targetPosition)
        {
            float distanceSquared = Vector3.DistanceSquared(m_Agent.Position, targetPosition);

            if (distanceSquared > m_PanicDistance)
            {
                return Vector3.Zero;
            }

            Vector3 desiredVelocity = Vector3.Normalize(m_Agent.Position - targetPosition) * Agent.MAX_SPEED;

            return desiredVelocity - m_Agent.Velocity;
        }

        public Vector3 Arrive(Vector3 targetPosition, DECELERATION deceleration)
        {
            Vector3 toTarget = targetPosition - m_Agent.Position;

            float distance = toTarget.Length();

            if (distance > 0)
            {
                const float decelTweaker = 0.3f;

                float speed = distance / ((float)deceleration * decelTweaker);

                speed = Math.Min(speed, Agent.MAX_SPEED);

                Vector3 desiredVelocity = toTarget * speed / distance;

                return desiredVelocity - m_Agent.Position;
            }

            return Vector3.Zero;
        }

        public Vector3 Pursuit(Agent targetAgent)
        {
            //get to target vector
            Vector3 toTarget = targetAgent.Position - m_Agent.Position;

            double lookAheadTime = toTarget.Length() / (m_Agent.MaxSpeed + targetAgent.Speed);

            return Seek(targetAgent.Position + targetAgent.Velocity * (float)lookAheadTime);
        }


    }
}
