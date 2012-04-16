using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Platform
{
    public class Rangefinder
    {
        protected Agent m_Agent;
        public Agent Agent
        {
            get { return m_Agent; }
            set { m_Agent = value; }
        }

        protected float m_Length;
        public float Length
        {
            get { return m_Length; }
            set { m_Length = value; }
        }

        protected float m_Angle;
        public float Angle
        {
            get { return m_Angle; }
            set { m_Angle = value; }
        }

        protected Ray m_Ray;

        /// <summary>
        /// Creates a Rangefinder of a given length to serve as an agent's sense for feelers
        /// </summary>
        /// <param name="startPosition">Where the rangefinder originates</param>
        /// <param name="length">The length of the feeler</param>
        /// <param name="angle">The angle offset from the heading of the agent</param>
        public Rangefinder(Agent agent, float length, float angle)
        {
            //assign the owning agent
            m_Agent = agent;

            //assign the length of the rangefinder
            m_Length = length;

            //get an angle representing the direction of the rangefinder
            m_Angle = angle;

            //create a rotation matrix for the angle applied to the heading
            Matrix orientationMatrix = Matrix.CreateRotationY(angle + m_Agent.Heading);

            //use the rotation matrix to transform a forward pointing Vector
            //be sure to normalize the direction vector
            Vector3 forward = Vector3.Forward;

            Vector3 direction = Vector3.Normalize(Vector3.Transform(forward, orientationMatrix));

            //create the ray from the given information
            m_Ray = new Ray(m_Agent.Position, direction);
        }

        public Rangefinder(Agent agent, float length, Ray ray)
        {
            //assign the owning Agent
            m_Agent = agent;

            //assign the length of the rangefinder
            m_Length = length;

            //create the new ray from the given ray
            m_Ray = ray;
        }

        public void Update()
        {
            //create a roation matrix which applies the angle to the agents heading
            Matrix orientationMatrix = Matrix.CreateRotationY(m_Angle + m_Agent.Heading);

            //create a forward vector to apply the transformation to
            Vector3 forward = Vector3.Forward;

            //apply the transformation and normalize
            Vector3 direction = Vector3.Normalize(Vector3.Transform(forward, orientationMatrix));

            //update the ray position
            m_Ray.Position = m_Agent.Position;

            //and diresction
            m_Ray.Direction = direction;

        }


        /// <summary>
        /// Return value between 0 and m_Length
        /// </summary>
        /// <param name="box">Bounding Box to intersect</param>
        /// <returns></returns>
        public float Intersects(BoundingBox box)
        {
            //check for ray, box intersection
            float? retVal = m_Ray.Intersects(box);

            if (retVal == null || retVal >= m_Length)
            {
                return m_Length;
            }

            return (float)retVal;
        }

        public float Intersects(BoundingSphere sphere)
        {
            //check intersection of the ray and the sphere
            float? retVal = m_Ray.Intersects(sphere);

            if (retVal == null || retVal >= m_Length)
            {
                return m_Length;
            }

            return (float)retVal;
        }

        public float Intersects(Plane plane)
        {
            float? retVal = m_Ray.Intersects(plane);

            if (retVal == null || retVal >= m_Length)
            {
                return m_Length;
            }

            return (float)retVal;
        }

        /// <summary>
        /// Return null, or values between 0 and 1
        /// </summary>
        /// <param name="box">Bounding Box to intersect</param>
        /// <returns></returns>
        public float? IntersectsNullible(BoundingBox box)
        {
            return m_Ray.Intersects(box);
        }

        /// <summary>
        /// Return null, or values between 0 and 1
        /// </summary>
        /// <param name="sphere">Bounding sphere to intersect</param>
        /// <returns></returns>
        public float? IntesectsNullible(BoundingSphere sphere)
        {
            return m_Ray.Intersects(sphere);
        }

        /// <summary>
        /// Return null, or values between 0 and 1
        /// </summary>
        /// <param name="plane">Plane to intersect</param>
        /// <returns></returns>
        public float? IntersectsNullible(Plane plane)
        {
            return m_Ray.Intersects(plane);
        }

        public float GetMinimumFeelerRange(List<GameEntity> obstacles)
        {
            //get the max length of the range finder
            float min = m_Length;

            //loop through each wall
            for (int i = 0; i < obstacles.Count; i++)
            {
                //update current length
                float currentLength = Intersects(obstacles[i].BoundingBox);

                //check to see if it's less than current minimum
                if (currentLength < min)
                {
                    min = currentLength;
                }
            }

            return min;
        }

        public override String ToString()
        {
            //string to return
            String retString = "";

            //Give the starting position of the rangefinder
            String position = "" + m_Agent.Position;

            //Give the Lenth of the rangefinder
            String length = "" + m_Length;

            //give the direction of the rangefinder
            String direction = "" + MathHelper.ToDegrees((float)Math.Round(MathHelper.WrapAngle(m_Agent.Heading + m_Angle), 2));

            retString += "Position: " + position + "\nLength: " + length + "\nDirection: " + direction + " degrees";
            return retString;
        }
    }
}
