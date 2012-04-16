using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Platform
{
    public class GameEntity
    {
        #region Fields/Properties
        //keep id for entity
        protected int m_ID;
        public int ID
        {
            get { return m_ID; }
            set { ;}
        }

        //keep track of entity's position
        protected Vector3 m_Position;
        public Vector3 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        //keep bounding spheres for entity
        protected BoundingSphere m_BoundingSphere;
        public BoundingSphere BoundingSphere
        {
            get { return m_BoundingSphere; }
            set { m_BoundingSphere = value; }
        }

        //keep bounding box for entity
        protected BoundingBox m_BoundingBox;
        public BoundingBox BoundingBox
        {
            get { return m_BoundingBox; }
            set { m_BoundingBox = value; }
        }

        //keep Model for entity
        protected Model m_Model;
        public Model Model
        {
            get { return m_Model; }
            set { m_Model = value; }
        }

        protected bool m_IsActive;
        public bool IsActive
        {
            get { return m_IsActive; }
            set { m_IsActive = value; }
        }
        #endregion

        //ctor
        public GameEntity()
        {
            //null the model
            m_Model = null;

            //zero the position vector
            m_Position = Vector3.Zero;

            //instantiate new bounding boxes and spheres
            m_BoundingBox = new BoundingBox();

            m_BoundingSphere = new BoundingSphere();

            m_ID = -1;
        }

        public GameEntity(int id)
        {
            m_ID = id;
        }

        protected BoundingSphere CalculateBoundingSphere()
        {
            //create a merged sphere to represent the bounding sphere
            BoundingSphere mergedSphere = new BoundingSphere();
            
            //create an array of bounding sphere to represent the bounding sphere for each mesh
            BoundingSphere[] boundingSpheres;

            //get mesh count 
            int meshCount = m_Model.Meshes.Count;

            //set the size of boundingSpheres
            boundingSpheres = new BoundingSphere[meshCount];

            //loop through each mesh and set the bounding sphere
            for (int i = 0; i < meshCount; i++)
            {
                boundingSpheres[i] = m_Model.Meshes[i].BoundingSphere;
            }

            //set the the first part of the merged sphere to be the first element of the bounding sphere
            mergedSphere = boundingSpheres[0];

            //loop through the remaining spheres and add to merge
            for (int i = 1; i < meshCount; i++)
            {
                mergedSphere = BoundingSphere.CreateMerged(mergedSphere, boundingSpheres[i]);
            }
            
            mergedSphere.Center.Y = 0;
            return mergedSphere;
        }

    }
}
