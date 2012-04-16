/**
 * Josh Cook, Paul Gatterdam, Ryan McPadden
 * Assignment 1 - Sensors
 * CAP 4053 - AI For Game Programming
 * 2/14/2011
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Platform
{
    public class PieSliceSensor
    {
        protected List<PieSlice> m_PieSlices;
        public List<PieSlice> PieSlices
        {
            get { return m_PieSlices; }
            set { m_PieSlices = value; }
        }

        protected AgentAdjacencyList m_AdjacencyList;
        public AgentAdjacencyList AdjacencyList
        {
            get { return m_AdjacencyList; }
            set { m_AdjacencyList = value; }
        }

        protected int m_NumSlices;
        public int NumSlices
        {
            get { return m_NumSlices; }
            set { ;}
        }

        protected float m_CurrentAngle;
        public float CurrentAngle
        {
            get { return m_CurrentAngle; }
            set { m_CurrentAngle = value; }
        }

        protected float m_PreviousAngle;
        public float PreviousAngle
        {
            get { return m_PreviousAngle; }
            set { m_PreviousAngle = value; }
        }

        /// <summary>
        /// Takes in a list of ranges(in degrees)(minAngle, maxAngle), each range specifying a pie slice
        /// </summary>
        /// <param name="ranges"></param>
        public PieSliceSensor(List<Tuple<float, float>> ranges, AgentAdjacencyList agentAdjacencyList)
        {
            m_AdjacencyList = agentAdjacencyList;

            //create pie slices
            m_PieSlices = new List<PieSlice>();
            m_NumSlices = ranges.Count;
            for (int i = 0; i < m_NumSlices; i++)
            {
                //create new slice
                PieSlice p = new PieSlice(ranges[i].Item1, ranges[i].Item2);

                //insert p
                m_PieSlices.Add(p);
            }

            m_CurrentAngle = ((Agent)m_AdjacencyList.Owner).Heading;
            m_PreviousAngle = m_CurrentAngle;
            
        }

        /// <summary>
        /// Creates a PieSliceSensor based on a list of pieslices
        /// </summary>
        /// <param name="pieSlices">The list of pieslices to be maintained by the sensor</param>
        public PieSliceSensor(List<PieSlice> pieSlices)
        {
            m_PieSlices = pieSlices;
            m_NumSlices = pieSlices.Count;
        }

        
        public void Update()
        {
            //update angles
            m_PreviousAngle = m_CurrentAngle;
            m_CurrentAngle = ((Agent)m_AdjacencyList.Owner).Heading;

            float angleChange = m_CurrentAngle - m_PreviousAngle;

            //reset each slice activation level
            this.Reset();

            //update the adjacency list
            m_AdjacencyList.Update();

            //loop through each edge in the adjacency list
            for (int i = 0; i < m_AdjacencyList.Edges.Count; i++)
            {
                //current edge
                AgentAdjacencyListEdge currentEdge = m_AdjacencyList.Edges[i];

                //GameEntity on edge
                GameEntity currentEntity = currentEdge.Entity;

                //distance of entity on edge
                float currentDistance = currentEdge.Distance;

                if (currentDistance <= 0)
                {
                    currentDistance = 0.001f;
                }

                //relative angle of entity
                float currentAngle = currentEdge.Angle;

                //loop through each slice in the pie slices list
                for (int j = 0; j < m_PieSlices.Count; j++)
                {
                    //grab the current slice
                    PieSlice currentSlice = m_PieSlices[j];

                    //currentSlice.Update(angleChange);

                    //grab the angles
                    float minAngle = currentSlice.MinAngle;
                    float maxAngle = currentSlice.MaxAngle;

                    //if the current angle is in the slice
                    //increment the activation level of the
                    //current slice.


                    //to check this we need to compare the min and max angle to handle
                    //wrap around checks

                    //first check if min ang is actually less than max angle
                    //in this situation, the check and activation is intuitive
                    if (minAngle <= maxAngle)
                    {
                        //check if the current angle falls in between
                        if (currentAngle >= minAngle && currentAngle < maxAngle)
                        {
                            //increment activation level
                            currentSlice.ActivationLevel += (1.0f / currentDistance);

                            //entity can only be in one slice
                            break;
                        }
                    }
                    else    //max angle < min angle
                    {
                        //check from min angle to 0
                        if ((currentAngle >= minAngle && currentAngle < MathHelper.TwoPi) ||
                            (currentAngle < maxAngle && currentAngle >= 0))//and check from 0 to maxAngle
                        {
                            //increment activation level
                            
                            currentSlice.ActivationLevel += (1.0f / currentDistance);

                            //entity can only be in one slice
                            break;
                        }
                    }
                }
            }
        }

        public void Reset()
        {
            //go through the list of slices
            for (int i = 0; i < m_PieSlices.Count; i++)
            {
                //reset pie slice
                m_PieSlices[i].Reset();
            }
        }
    }
}
