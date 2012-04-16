using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Platform
{
    public class Bomb : GameEntity
    {

        public static double RADIUS = 25;
        /*
         *    Bomb inherits the following from game entity
         *    public Vector3 position;

              public Model model;

              public BoundingSphere boundingSphere;

              public BoundingBox boundingBox;

              public bool isActive;
        */

        #region moving object standards
        /*
         * Includes common things to have the bomb exist 
         * 
         */



        

        public Ray[] rays;
        public float?[] rayIntersections;

        protected float m_Heading;
        public float Heading
        {
            get { return m_Heading; }
            set { m_Heading = value; }
        }

        protected float m_Side;
        public float Side
        {
            get { return m_Side; }
            set { m_Side = value; }
        }

        protected Vector3 m_Velocity;
        public Vector3 Velocity
        {
            get { return m_Velocity; }
            set { m_Velocity = value; }
        }

        protected Vector3 m_Acceleration;
        public Vector3 Acceleration
        {
            get { return m_Acceleration; }
            set { m_Acceleration = value; }
        }

        protected float m_TurnAmount;
        public float TurnAmount
        {
            get { return m_TurnAmount; }
            set { m_TurnAmount = value; }
        }

        protected float m_Speed;
        public float Speed
        {
            get { return m_Speed; }
            set { m_Speed = value; }
        }

        protected float m_Mass;
        public float Mass
        {
            get { return m_Mass; }
            set { m_Mass = value; }
        }

        protected float m_MaxSpeed;
        public float MaxSpeed
        {
            get { return m_MaxSpeed; }
            set { m_MaxSpeed = value; }
        }

        protected float m_MaxTurnRate;
        public float MaxTurnRate
        {
            get { return m_MaxTurnRate; }
            set { m_MaxTurnRate = value; }
        }

        protected float m_SensoryRadius;
        public float SensoryRadius
        {
            get { return m_SensoryRadius; }
            set { m_SensoryRadius = value; }
        }


        protected PieSliceSensor m_PieSliceSensor;
        public PieSliceSensor PieSliceSensor
        {
            get { return m_PieSliceSensor; }
            set { m_PieSliceSensor = value; }
        }
        #endregion


        #region Bomb Variables
        //bomb specific variables

        protected int m_AgentIdTag;
        public int AgentIdTag
        {
            get {return m_AgentIdTag;}
            set {m_AgentIdTag = value;}
        }

        protected int m_LifeTime;
        public int LifeTime
        {
            get { return m_LifeTime; }
            set { m_LifeTime = value; }
        }

        protected Agent m_AgentParent;
        public Agent AgentParent
        {
            get { return m_AgentParent; }
            set { m_AgentParent = value; }
        }

        protected double m_Radius;
        public double Radius
        {
            get { return m_Radius; }
            set { m_Radius = value; }
        }

        protected double m_ExplosionLength;
        public double ExplosionLength
        {
            get { return m_ExplosionLength; }
            set { m_ExplosionLength = value; }
        }

        /*

        protected double m_UpExplosionLength;
        public double UpExplosionLength
        {
            get { return m_UpExplosionLength; }
            set { m_UpExplosionLength = value; }
        }

        protected double m_DownExplosionLength;
        public double DownExplosionLength
        {
            get { return m_DownExplosionLength; }
            set { m_DownExplosionLength = value; }
        }

        protected double m_ExplosionLength;
        public double ExplosionLength
        {
            get { return m_ExplosionLength; }
            set { m_ExplosionLength = value; }
        }

        protected double m_ExplosionLength;
        public double ExplosionLength
        {
            get { return m_ExplosionLength; }
            set { m_ExplosionLength = value; }
        }
        */
        #endregion

        protected Map m_World;
        public Map World
        {
            get { return m_World; }
            set { m_World= value; }
        }



        public Bomb(Agent agentParent, int id)
        {
            m_ID = id;
            this.m_AgentParent = agentParent;

            //to trace bombs parent
            m_AgentIdTag = agentParent.Id;

            m_World = agentParent.World;

            //inherits parents position
            this.m_Position = agentParent.Position;

            //heading is initially 0
            Heading = 0.0f;

            //load model 
            loadContent(agentParent.World.Content, "bomb");
            m_Position = agentParent.Position;

            //velocity, acceleration, speed are initially zero
            Velocity = Vector3.Zero;
            Acceleration = Vector3.Zero;
            Speed = 0.0f;

            //init max speed
            //?MaxSpeed = Bann.MAX_SPEED;


            //lifetime initialy set to 3000 ( 3 seconds )
            LifeTime = 1500;


            //sensory radius will be range of bomb
            /*
             *          |
             *          |
             *       ---o---
             *          |
             *          |
             * 
            */
            SensoryRadius = (float)Bomb.RADIUS;


            //logic to figure out what gets hit in explosion to be done
            // using all objects in a range and check if they 
            //collide with 2 rectangles looks to be the best way





        }


        public void update(GameTime time)
        {
            //decrement bombs lifespan
            LifeTime -= time.ElapsedGameTime.Milliseconds;



            Vector3 predictedPosition = m_Position;
            

            predictedPosition = m_Position + m_Velocity;

            BoundingSphere predictedBoundingSphere = m_BoundingSphere;
            predictedBoundingSphere.Center.X = predictedPosition.X;
            predictedBoundingSphere.Center.Z = predictedPosition.Z;


            //position is set to future position <=> future position is not within the bounds of the wall
            if (!WallCollision(predictedBoundingSphere, m_World.Walls)
                && !BlockCollision(predictedBoundingSphere, m_World.Blocks)
                && !AgentCollision(predictedBoundingSphere, m_World.Agents)
                && !BombCollision(predictedBoundingSphere, m_World.Bombs))
            {
                m_Position = predictedPosition;
                m_BoundingSphere = predictedBoundingSphere;
            }
            else
            {
                m_Velocity = Vector3.Zero;
            }

            //implement slowdown
            //Position += Velocity;

        }

        private bool WallCollision(BoundingSphere bs, List<GameEntity> walls)
        {
            for (int i = 0; i < walls.Count; i++)
            {
                if (bs.Intersects(walls[i].BoundingBox))
                {
                    return true;
                }
            }

            return false;
        }

        private bool BlockCollision(BoundingSphere bs, List<GameEntity> blocks)
        {
            foreach (Block block in blocks)
            {
                if (bs.Intersects(block.BoundingBox))
                {
                    return true;
                }
            }

            return false;
        }

        private bool AgentCollision(BoundingSphere bs, List<GameEntity> agents)
        {
            for (int i = 0; i < agents.Count; i++)
            {
                if (bs.Intersects(agents[i].BoundingSphere))
                {
                    return true;
                }
            }
            return false;
        }

        private bool BombCollision(BoundingSphere bs, List<GameEntity> bombs)
        {
            for (int i = 0; i < bombs.Count; i++)
            {
                if (bs.Intersects(bombs[i].BoundingSphere))
                {
                    return true;
                }
            }
            return false;
        }


        public void setPosition(float x, float y, float z)
        {
            Position = new Vector3(x, y, z);
        }

        //note: might want to load model before all of this, has noticeable hangup
        public void loadContent(ContentManager content, string modelName)
        {
            Model = content.Load<Model>(modelName);
            m_BoundingSphere = CalculateBoundingSphere();

            Radius = m_BoundingSphere.Radius;
            ExplosionLength = Radius * 10;

        }


        public void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            Matrix[] transforms = new Matrix[m_Model.Bones.Count];
            m_Model.CopyAbsoluteBoneTransformsTo(transforms);
            Matrix translateMatrix = Matrix.CreateTranslation(m_Position);
            Matrix worldMatrix = Matrix.Identity;

            Matrix rotationXMatrix = Matrix.Identity;// Matrix.CreateRotationX(heading.X);
            Matrix rotationYMatrix = Matrix.CreateRotationY(m_Heading);
            Matrix rotationZMatrix = Matrix.Identity;// Matrix.CreateRotationZ(heading.Z);

            worldMatrix = rotationZMatrix * rotationYMatrix * rotationXMatrix * translateMatrix;
            foreach (ModelMesh mesh in m_Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World =
                        worldMatrix * transforms[mesh.ParentBone.Index];
                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;

                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
                mesh.Draw();
            }

        }


    }
}
