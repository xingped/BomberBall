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
using System.Collections;


namespace Platform
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Node : GameEntity
    {
        public List<Node> adjNodes;
        public int id;

        public double g_dist;
        public double h_dist;
        public double f_dist;

        public Node(int id)
            : base(id)
        {
            // TODO: Construct any child components here
            adjNodes = new List<Node>();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize()
        {
            // TODO: Add your initialization code here

        }


        public void LoadContent(ContentManager content, string modelName)
        {
            m_Model = content.Load<Model>(modelName);
            m_Position = Vector3.Zero;
        }

        public void SetPosition(Vector3 new_position)
        {
            m_Position = new_position;
            float scale = 1.0f;
            float xDif = scale;
            float yDif = scale;
            float zDif = scale;
            m_BoundingSphere = new BoundingSphere(m_Position, scale);
        }

        public void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            if (m_IsActive || !m_IsActive)
            {
                Matrix[] transforms = new Matrix[m_Model.Bones.Count];
                m_Model.CopyAbsoluteBoneTransformsTo(transforms);
                Matrix translateMatrix = Matrix.CreateTranslation(m_Position);
                Matrix worldMatrix = translateMatrix;

                foreach (ModelMesh mesh in m_Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = worldMatrix * transforms[mesh.ParentBone.Index];
                        effect.View = viewMatrix;
                        effect.Projection = projectionMatrix;

                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;

                    }
                    mesh.Draw();
                }
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

        }
    }
}
