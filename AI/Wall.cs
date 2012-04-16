using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace Platform
{
    public class Wall : GameEntity
    {
        #region Fields/Properties

        public enum Type { Wall, Block };
        protected Type m_WallType;
        public Type WallType
        {
            get { return m_WallType; }
            set { m_WallType = value; }
        }

        public enum Status { Up, Down, None };
        protected Status m_WallStatus;
        public Status WallStatus
        {
            get { return m_WallStatus; }
            set { m_WallStatus = value; }
        }

        /// <summary>
        /// The name of the model.
        /// </summary>
        protected String m_ModelName;
        public String ModelName
        {
            get { return m_ModelName; }
            set { m_ModelName = value; }
        }

        /// <summary>
        /// Point representing the Min corner of the bounding box.
        /// </summary>
        protected Vector3 m_Min;
        public Vector3 Min
        {
            get { return m_Min; }
            set { m_Min = value; }
        }

        /// <summary>
        /// Point representing the Max corner of the bounding box.
        /// </summary>
        protected Vector3 m_Max;
        public Vector3 Max
        {
            get { return m_Max; }
            set { m_Max = value; }
        }

        /// <summary>
        /// Scale to apply to the model.
        /// </summary>
        protected int m_Scale;
        public int Scale
        {
            get { return m_Scale; }
            set { m_Scale = value; }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public Wall() :
            base()
        {
            m_ModelName = null;
            m_Min = Vector3.Zero;
            m_Max = Vector3.Zero;
            m_Scale = 3;
        }

        /// <summary>
        /// Loads a model's content to get ready for rendering
        /// </summary>
        /// <param name="content">ContentManager to manage the loading of content</param>
        /// <param name="modelName">T</param>
        public void LoadContent(ContentManager content, string modelName)
        {
            //load model's content
            m_Model = content.Load<Model>(modelName);

            //set model's name
            m_ModelName = modelName;

            //set model in down position
            m_Position = Vector3.Zero;

            //set initial the min of model
            m_Min = new Vector3(-1, 0, -1) * m_Scale;

            //set the initia; max of model
            m_Max = new Vector3(1, 0, 1) * m_Scale;

        }

        /// <summary>
        /// Set the position of the wall.
        /// Updates the bounding box.
        /// </summary>
        /// <param name="position">The new position to set the wall in</param>
        public void SetPosition(Vector3 position)
        {
            //set position
            m_Position = position;

            //apply change to bounding box positions
            m_Min = position + new Vector3(-m_Scale, m_Position.Y, -m_Scale);
            m_Max = position + new Vector3(m_Scale, m_Scale, m_Scale);

            //update bounding box
            m_BoundingBox = new BoundingBox(m_Min, m_Max);
        }

        /// <summary>
        /// Renders the model.
        /// </summary>
        /// <param name="viewMatrix">Matrix to transform from world space to camera space</param>
        /// <param name="projectionMatrix">Matrix to transform from camera space to projection space</param>
        public void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            //create an array to hold transformations
            Matrix[] transformations = new Matrix[m_Model.Bones.Count];
            
            //copy transformation to transformation matrix
            m_Model.CopyAbsoluteBoneTransformsTo(transformations);

            //translate model the m_Position
            Matrix translationMatrix = Matrix.CreateTranslation(m_Position);

            //the world matrix consists of only the translation to m_Position
            Matrix worldMatrix = translationMatrix;

            //go through each mesh in m_Model
            foreach (ModelMesh mesh in m_Model.Meshes)
            {
                //go through each basic effect in the mesh effects
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //World matrix consists of all transformation to the mesh
                    //which would be the world matrix(translation matrix) and
                    //the bone transformations
                    effect.World = worldMatrix * transformations[mesh.ParentBone.Index];
                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;

                    //enable default lighting
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
                //draw mesh after setting effects
                mesh.Draw();
            }
        }
    }
}
