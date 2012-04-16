using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Platform
{
    public class PieSlice
    {
        protected float m_MinAngle;
        public float MinAngle
        {
            get { return m_MinAngle; }
            set { m_MinAngle = value; }
        }

        protected float m_MaxAngle;
        public float MaxAngle
        {
            get { return m_MaxAngle; }
            set { m_MaxAngle = value; }
        }

        protected float m_ActivationLevel;
        public float ActivationLevel
        {
            get { return m_ActivationLevel; }
            set { m_ActivationLevel = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="minAngle">Angle representing the min, radian</param>
        /// <param name="maxAngle">Angle representing the max, radian</param>
        public PieSlice(float minAngle, float maxAngle)
        {
            //first wrap the angles
            minAngle = MathHelper.WrapAngle(minAngle);
            maxAngle = MathHelper.WrapAngle(maxAngle);

            //values are now between 180 amd -180
            //get between 0 and 360
            if (minAngle < 0)
                minAngle += MathHelper.TwoPi;
            if (maxAngle < 0)
                maxAngle += MathHelper.TwoPi;

            m_MinAngle = minAngle;
            m_MaxAngle = maxAngle;
            m_ActivationLevel = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        public void Update(float angle)
        {
            //update min angle
            m_MinAngle += angle;
            m_MinAngle = MathHelper.WrapAngle(m_MinAngle);
            if (m_MinAngle < 0)
                m_MinAngle += MathHelper.TwoPi;

            //update max angle
            m_MaxAngle += angle;
            m_MaxAngle = MathHelper.WrapAngle(m_MaxAngle);
            if (m_MaxAngle < 0)
                m_MaxAngle += MathHelper.TwoPi;

        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            m_ActivationLevel = 0;
        }
    }
}
