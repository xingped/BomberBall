/**
 * Joshua Cook, Paul Gatterdam, Ryan McPadden
 * Assignment 1 - Sensors
 * CAP 4053 - AI For Game Programming
 * 2/14/2011
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using AI.ANN;

namespace Platform
{
    public class Agent : GameEntity
    {
        #region Fields/Properties
        public static float MAX_SPEED = 0.25f;
        public static float DEFAULT_SPEED = 0.25f;
        public static float MAX_TURN_SPEED = 0.075f;
        public static float DEFAULT_TURN_SPEED = 0.075f;
        public static float PANIC_SPEED = 1.5f;
        public static float RAY_LENGTH = 50.0f;

        protected System.IO.StreamWriter m_GeneticFile;
        public System.IO.StreamWriter GeneticFile
        {
            get { return m_GeneticFile; }
            set { m_GeneticFile = value; }
        }


        protected State m_CurrentState;
        public State CurrentState
        {
            get { return m_CurrentState; }
            set { m_CurrentState = value; }
        }
        protected String m_Name;
        public String Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        protected bool m_OutputGenes;
        public bool OutputGenes
        {
            get { return m_OutputGenes; }
            set { m_OutputGenes = value; }
        }

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

        protected DateTime m_NavDelay;
        public DateTime NavDelay
        {
            get { return m_NavDelay; }
            set { m_NavDelay = value; }
        }

        protected List<Node> m_NavPath;
        public List<Node> NavPath
        {
            get { return m_NavPath; }
            set { m_NavPath = value; }
        }

        protected Node m_StartNode;
        public Node StartNode
        {
            get { return m_StartNode; }
            set { m_StartNode = value; }
        }

        protected Node m_NextNode;
        public Node NextNode
        {
            get { return m_NextNode; }
            set { m_NextNode = value; }
        }

        protected Node m_EndNode;
        public Node EndNode
        {
            get { return m_EndNode; }
            set { m_EndNode = value; }
        }

        protected Node m_EscapeNode;
        public Node EscapeNode
        {
            get { return m_EscapeNode; }
            set { m_EscapeNode = value; }
        }
        
        protected bool m_Invuln;
        public bool Invuln
        {
            get { return m_Invuln; }
            set { m_Invuln = value; }
        }

        protected DateTime m_InvulnTimer;
        public DateTime InvulnTimer
        {
            get { return m_InvulnTimer; }
            set { m_InvulnTimer = value; }
        }

        protected PieSliceSensor m_PieSliceSensor;
        public PieSliceSensor PieSliceSensor
        {
            get { return m_PieSliceSensor; }
            set { m_PieSliceSensor = value; }
        }

        protected List<Tuple<Agent, float, float>> m_AgentAdjacencyList;
        public List<Tuple<Agent, float, float>> AgentAdjacencyList
        {
            get { return m_AgentAdjacencyList; }
            set { m_AgentAdjacencyList = value; }
        }

        protected bool m_OutputAdjacentAgents;
        public bool OutputAdjacentAgents
        {
            get { return m_OutputAdjacentAgents; }
            set { m_OutputAdjacentAgents = value; }
        }

        protected bool m_OutputInfo;
        public bool OutputInfo
        {
            get { return m_OutputInfo; }
            set { m_OutputInfo = value; }
        }

        protected bool m_OutputRayInfo;
        public bool OutputRayInfo
        {
            get { return m_OutputRayInfo; }
            set { m_OutputRayInfo = value; }
        }


        protected int m_Id;
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        protected SteeringBehaviors m_SteeringBehaviors;
        public SteeringBehaviors SteeringBehaviors
        {
            get { return m_SteeringBehaviors; }
            set { m_SteeringBehaviors = value; }
        }

        protected Map m_World;
        public Map World
        {
            get { return m_World; }
            set { ;}
        }

        protected NeuralNetwork m_Brain;
        public NeuralNetwork Brain
        {
            get { return m_Brain; }
            set { m_Brain = value; }
        }

        protected Vector3 m_TargetPosition;
        public Vector3 TargetPosition
        {
            get { return m_TargetPosition; }
            set { m_TargetPosition = value; }
        }

        protected Agent m_TargetAgent;
        public Agent TargetAgent
        {
            get { return m_TargetAgent; }
            set { m_TargetAgent = value; }
        }

        protected double m_Fitness;
        public double Fitness
        {
            get { return m_Fitness; }
            set { m_Fitness = value; }
        }

        protected List<double> m_MemoryOutputs;
        public List<double> MemoryOutputs
        {
            get { return m_MemoryOutputs; }
            set { m_MemoryOutputs = value; }
        }

        protected Vector3 m_StartingLocation;
        public Vector3 StartingLocation
        {
            get { return m_StartingLocation; }
            set { m_StartingLocation = value; }
        }

        protected float m_TurnSpeed;
        public float TurnSpeed
        {
            get { return m_TurnSpeed; }
            set { m_TurnSpeed = value; }
        }

        protected float m_MovementSpeed;
        public float MovementSpeed
        {
            get { return m_MovementSpeed; }
            set { m_MovementSpeed = value; }
        }

        #region Modified Sensory

        protected List<Rangefinder> m_RangefinderWalls;
        public List<Rangefinder> RangefinderWalls
        {
            get { return m_RangefinderWalls; }
            set { m_RangefinderWalls = value; }
        }

        protected List<Rangefinder> m_RangefinderBlocks;
        public List<Rangefinder> RangefinderBlocks
        {
            get { return m_RangefinderBlocks; }
            set { m_RangefinderBlocks = value; }
        }

        protected PieSliceSensor m_PieSliceSensorEnemies;
        public PieSliceSensor PieSliceSensorEnemies
        {
            get { return m_PieSliceSensorEnemies; }
            set { m_PieSliceSensorEnemies = value; }
        }

        protected PieSliceSensor m_PieSliceSensorBombs;
        public PieSliceSensor PieSliceSensorBombs
        {
            get { return m_PieSliceSensorBombs; }
            set { m_PieSliceSensorBombs = value; }
        }

        protected AgentAdjacencyList m_AdjacencyListEnemies;
        public AgentAdjacencyList AdjacencyListEnemies
        {
            get { return m_AdjacencyListEnemies; }
            set { m_AdjacencyListEnemies = value; }
        }

        protected AgentAdjacencyList m_AdjacencyListBombs;
        public AgentAdjacencyList AdjacencyListBombs
        {
            get { return m_AdjacencyListBombs; }
            set { m_AdjacencyListBombs = value; }
        }

        protected AgentAdjacencyList m_AdjacencyListNodes;
        public AgentAdjacencyList AdjacencyListNodes
        {
            get { return m_AdjacencyListNodes; }
            set { m_AdjacencyListNodes = value; }
        }
        #endregion
        #region Output Information Variables

        protected List<float> m_InformationRangefinderWallLengths;
        public List<float> InformationRangefinderWallLengths
        {
            get { return m_InformationRangefinderWallLengths; }
            set { m_InformationRangefinderWallLengths = value; }
        }

        protected List<float> m_InformationRangefinderBlockLengths;
        public List<float> InformationRangefinderBlockLengths
        {
            get { return m_InformationRangefinderBlockLengths; }
            set { m_InformationRangefinderBlockLengths = value; }
        }
        #endregion
        #endregion

        public Agent(Map world, int id, bool outputGenes)
            : base()
        {
            //heading is initially 0
            m_Heading = 0.0f;

            //side is 90 degrees CW from heading
            m_Side = m_Heading - (float)Math.PI / 2.0f;

            //velocity, acceleration, speed are initially zero
            m_Velocity = Vector3.Zero;
            m_Acceleration = Vector3.Zero;
            m_Speed = 0.0f;
            m_TurnAmount = 0.0f;

            //init max speed
            m_MaxSpeed = MAX_SPEED;

            m_AgentAdjacencyList = new List<Tuple<Agent, float, float>>();

            //agent is initially inactive
            m_IsActive = false;

            //sensory radius for adjacent agents and pie slice
            m_SensoryRadius = 400;

            //do no initially output info
            m_OutputAdjacentAgents = false;
            m_OutputInfo = false;
            m_OutputRayInfo = false;

            //init agent's id
            this.m_ID = id;

            m_SteeringBehaviors = new SteeringBehaviors(this);

            m_World = world;

            m_Brain = new NeuralNetwork();
            m_OutputGenes = outputGenes;
            if (m_OutputGenes == true)
            {
                m_GeneticFile = new System.IO.StreamWriter("Agent" + Id + ".gene");
            }

            m_MemoryOutputs = new List<double>();
            m_MemoryOutputs.Add(0);
            m_MemoryOutputs.Add(0);
            m_MemoryOutputs.Add(0);
            m_MemoryOutputs.Add(0);
            m_Fitness = 0;

            //init rangefinder list
            InitializeRangefinderWalls();
            InitializeRangefinderBlocks();

            //init pie slices
            InitializePieSliceSensors();

            //init node adj list
            InitializeAdjacencyListNodes();

            //init state is navigate
            m_CurrentState = new State_Navigate();

            m_NavDelay = DateTime.Now;

            //begin invulnerability timer
            m_Invuln = true;

            m_InvulnTimer = DateTime.Now.AddSeconds(12);

            // Initialize the Escape node to null to set it later
            m_EscapeNode = null;

            m_StartingLocation = Vector3.Up;

            m_MovementSpeed = MAX_SPEED;
            m_TurnSpeed = MAX_TURN_SPEED;
        }

        public void InitializeRangefinderWalls()
        {
            m_RangefinderWalls = new List<Rangefinder>();
            m_InformationRangefinderWallLengths = new List<float>();

            //add rangefinders
            for (int i = 2; i >= -2; i--)
            {
                m_RangefinderWalls.Add(new Rangefinder(this, 50, m_Heading - (i * MathHelper.PiOver4)));
            }
        }

        public void InitializeRangefinderBlocks()
        {
            m_RangefinderBlocks = new List<Rangefinder>();
            m_InformationRangefinderBlockLengths = new List<float>();

            //add rangefinders
            for (int i = 2; i >= -2; i--)
            {
                m_RangefinderBlocks.Add(new Rangefinder(this, 50, m_Heading - (i * MathHelper.PiOver4)));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitializePieSliceSensors()
        {
            InitializePieSliceSensorBombs();
            InitializePieSliceSensorEnemies();
        }

        public void InitializePieSliceSensorEnemies()
        {
            //create ranges to add
            List<Tuple<float, float>> ranges = new List<Tuple<float, float>>();
            ranges.Add(new Tuple<float, float>(MathHelper.ToRadians(315), MathHelper.ToRadians(45)));
            ranges.Add(new Tuple<float, float>(MathHelper.ToRadians(45), MathHelper.ToRadians(135)));
            ranges.Add(new Tuple<float, float>(MathHelper.ToRadians(135), MathHelper.ToRadians(225)));
            ranges.Add(new Tuple<float, float>(MathHelper.ToRadians(225), MathHelper.ToRadians(315)));

            //create adjacency list that is fed into the pie slice sensor
            InitializeAdjacencyListEnemies();

            //create the pie slice sensor
            m_PieSliceSensorEnemies = new PieSliceSensor(ranges, m_AdjacencyListEnemies);

        }

        public void InitializePieSliceSensorBombs()
        {
            //create ranges to add
            List<Tuple<float, float>> ranges = new List<Tuple<float, float>>();
            for (int i = 0; i < 8; i++)
            {
                ranges.Add(new Tuple<float, float>(MathHelper.ToRadians(i * 45), MathHelper.ToRadians(((i + 1) % 8) * 45)));
            }

            //create adjacency list to be fed into pie slices sensor
            InitializeAdjacencyListBombs();

            //create the pie slice sensor
            m_PieSliceSensorBombs = new PieSliceSensor(ranges, m_AdjacencyListBombs);
        }

        public void InitializeAdjacencyListEnemies()
        {
            List<GameEntity> enemies = new List<GameEntity>();
            for (int i = 0; i < m_World.Agents.Count; i++)
            {
                if(m_World.Agents[i].ID != m_ID)
                {
                    //add to enemy list
                    enemies.Add(m_World.Agents[i]);
                }
            }

            if (m_World.Player != null && m_World.Player.ID != m_ID)
            {
                enemies.Add(m_World.Player);
            }

            m_AdjacencyListEnemies = new AgentAdjacencyList(this, enemies, 400);
        }

        public void InitializeAdjacencyListBombs()
        {
            m_AdjacencyListBombs = new AgentAdjacencyList(this, m_World.Bombs, 50);
        }

        public void InitializeAdjacencyListNodes()
        {
            m_AdjacencyListNodes = new AgentAdjacencyList(this, m_World.Nodes, 50);
        }

        public void SetAdjacencyListEnemies(List<GameEntity> enemies)
        {
            m_AdjacencyListEnemies.Entities = enemies;
        }

        public void SetAdjacencyListBombs(List<GameEntity> bombs)
        {
            m_AdjacencyListBombs.Entities = bombs;
        }

        public void SetAdjacencyListNodes(List<GameEntity> nodes)
        {
            m_AdjacencyListNodes.Entities = nodes;
        }


        /// <summary>
        /// 
        /// </summary>
        public void DropBomb()
        {
            m_World.Bombs.Add(new Bomb(this, m_World.NextId++));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="modelName"></param>
        public void LoadContent(ContentManager content, string modelName)
        {
            m_Model = content.Load<Model>(modelName);
            m_Position = Vector3.Down;
            m_BoundingSphere = CalculateBoundingSphere();

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

        private void turn(float turnAmount, float turnSpeed)
        {
            float modifiedTurnAmount = turnAmount * turnSpeed;

            MathHelper.Clamp(modifiedTurnAmount, -MAX_TURN_SPEED, MAX_TURN_SPEED);
            m_Heading += modifiedTurnAmount;

            if (MathHelper.ToDegrees(m_Heading) > 360)
                m_Heading -= MathHelper.ToRadians(360);
            else if (MathHelper.ToDegrees(m_Heading) < 0)
                m_Heading += MathHelper.ToRadians(360);

            m_Side += modifiedTurnAmount;

            if (MathHelper.ToDegrees(m_Side) > 360)
                m_Side -= MathHelper.ToRadians(360);
            else if (MathHelper.ToDegrees(m_Side) < 0)
                m_Side += MathHelper.ToRadians(360);
        }

        public void Update(KeyboardState keyboardState, KeyboardState lastKeyboardState, List<GameEntity> walls, List<GameEntity> blocks)
        {
            if (m_IsActive)
            {
                Vector3 predictedPosition = m_Position;
                float turnAmount = 0;

                //set turn amount
                if (!keyboardState.IsKeyDown(Keys.LeftShift) && !keyboardState.IsKeyDown(Keys.LeftControl))
                {
                    if (keyboardState.IsKeyDown(Keys.A))
                    {
                        turnAmount = 1.0f;
                    }
                    else if (keyboardState.IsKeyDown(Keys.D))
                    {
                        turnAmount = -1.0f;
                    }

                    turn(turnAmount, MAX_TURN_SPEED);
                    Matrix orientationMatrix = Matrix.CreateRotationY(m_Heading);

                    Vector3 movement = Vector3.Zero;
                    if (keyboardState.IsKeyDown(Keys.W))
                    {
                        movement.Z = -1;
                    }
                    else if (keyboardState.IsKeyDown(Keys.S))
                    {
                        movement.Z = 1;
                    }

                    if (keyboardState.IsKeyDown(Keys.Q))
                    {
                        movement.X = -1;
                    }
                    else if (keyboardState.IsKeyDown(Keys.E))
                    {
                        movement.X = 1;
                    }

                    Vector3 speed = Vector3.Transform(movement, orientationMatrix) * MAX_SPEED;
                    predictedPosition = m_Position + speed;

                    BoundingSphere predictedBoundingSphere = m_BoundingSphere;
                    predictedBoundingSphere.Center.X = predictedPosition.X;
                    predictedBoundingSphere.Center.Z = predictedPosition.Z;


                    //position is set to future position <=> future position is not within the bounds of the wall
                    if (!WallCollision(predictedBoundingSphere, m_World.Walls) && !BlockCollision(predictedBoundingSphere, m_World.Blocks))
                    {
                        m_Position = predictedPosition;
                        m_BoundingSphere = predictedBoundingSphere;
                    }

                    

                    //update range finders for blocks
                    UpdateRangefinderBlocks();

                    //update range finders for walls
                    UpdateRangefinderWalls();

                    //update pss for bombs
                    m_PieSliceSensorBombs.Update();

                    //update pss for enemies
                    m_PieSliceSensorEnemies.Update();

                    m_AdjacencyListNodes.Update();

                    //drop bomb 
                    if (!lastKeyboardState.IsKeyDown(Keys.Space) && keyboardState.IsKeyDown(Keys.Space))
                    {
                        DropBomb();
                    }
                }
            }

        }

        
        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            if (!m_IsActive)
            {
                //keep heading between 0 and 360
                m_Heading = MathHelper.WrapAngle(m_Heading);
                if (m_Heading < 0)
                    m_Heading += MathHelper.TwoPi;
                //update adjacency lists
                m_AdjacencyListBombs.Update();
                m_AdjacencyListEnemies.Update();
                m_AdjacencyListNodes.Update();

                //execute the current state
                m_CurrentState.Execute(this);
            }
        }

        #region Genetic Updates
        public bool GeneticUpdate()
        {
            if (!IsActive && m_TargetAgent != null)
            {
                //inputs for the brain
                //inputs = rangefinders, andgle between header and target position
                List<double> inputs = new List<double>();

                //range finders
                for (int i = 0; i < rayIntersections.Length; i++)
                {
                    double inputVal = (double)(rayIntersections[i] / RAY_LENGTH);
                    inputs.Add(inputVal);
                }

                //get angle between header and target position
                double angle = 0;
                double distance = 0;

                //get target Position
                m_TargetPosition = m_TargetAgent.Position;
                Vector3 targetPosition = m_TargetPosition;

                //get vector connecting to target position
                Vector3 toTarget = targetPosition - m_Position;

                distance = toTarget.LengthSquared();
                inputs.Add(1.0 / (1.0 + distance));

                angle = (float)Math.Atan2(-toTarget.Z, toTarget.X);
                double relativeAngle = -(double)MathHelper.WrapAngle(m_Heading - (float)angle + MathHelper.ToRadians(90));
                //relativeAngle = (double)MathHelper.ToDegrees((float)relativeAngle);
                //relativeAngle = -MathHelper.WrapAngle((float)relativeAngle - m_Heading + MathHelper.ToRadians(90));
                if (relativeAngle < 0)
                    relativeAngle += MathHelper.ToRadians(360.0f);
                //add the angle to inputs
                relativeAngle /= MathHelper.ToRadians(360.0f);
                inputs.Add(relativeAngle);

                #region Memory
                //inputs memory
                /*for (int i = 0; i < m_MemoryOutputs.Count; i++)
                {
                    inputs.Add(m_MemoryOutputs[i]);
                }*/
                #endregion

                #region Genetic Output: Ticks, Input, Weights
                if (m_OutputGenes)
                {
                    m_GeneticFile.WriteLine("Tick: " + m_World.NumTicks);
                    for (int i = 0; i < inputs.Count; i++)
                    {
                        m_GeneticFile.WriteLine("Input " + i + ": " + inputs[i]);
                    }


                    List<double> weights = m_Brain.GetWeights();
                    if (m_World.NumTicks == 0)
                    {
                        m_GeneticFile.WriteLine();
                        m_GeneticFile.WriteLine("Weights");
                        for (int i = 0; i < weights.Count; i++)
                        {
                            m_GeneticFile.WriteLine("Weight" + i + ": " + weights[i]);
                        }
                    }
                }
                #endregion

                //update the brain
                List<double> output = m_Brain.Update(inputs);

                //check for errors in calculating the outputs
                if (output.Count != Params.NUM_OUTPUTS)
                {
                    Console.WriteLine("Error in Brain");
                    return false;
                }

                #region Genetic Output: Outputs
                if (m_OutputGenes)
                {
                    m_GeneticFile.WriteLine();
                    m_GeneticFile.WriteLine("Outputs");
                    for (int i = 0; i < output.Count; i++)
                    {
                        m_GeneticFile.WriteLine("Output " + i + ": " + output[i]);
                    }
                    m_GeneticFile.WriteLine();
                    m_GeneticFile.WriteLine();
                }
                #endregion

                //get outputs
                double turnAmount = output[0] - output[1];
                turnAmount *= DEFAULT_TURN_SPEED;
                turnAmount = MathHelper.Clamp((float)turnAmount, -DEFAULT_TURN_SPEED, DEFAULT_TURN_SPEED);

                m_Heading += (float)turnAmount;

                if (MathHelper.ToDegrees(m_Heading) > 360)
                    m_Heading -= MathHelper.ToRadians(360);
                else if (MathHelper.ToDegrees(m_Heading) < 0)
                    m_Heading += MathHelper.ToRadians(360);

                m_Side += (float)turnAmount;

                if (MathHelper.ToDegrees(m_Side) > 360)
                    m_Side -= MathHelper.ToRadians(360);
                else if (MathHelper.ToDegrees(m_Side) < 0)
                    m_Side += MathHelper.ToRadians(360);


                Matrix orientationMatrix = Matrix.CreateRotationY(m_Heading);

                Vector3 movement = Vector3.Zero;
                movement.Z = (float)(output[2] - output[3]);

                m_Velocity = Vector3.Transform(movement, orientationMatrix) * DEFAULT_SPEED;

                Vector3 futurePosition = m_Position + m_Velocity;

                BoundingSphere futureSphere = m_BoundingSphere;
                futureSphere.Center.X = futurePosition.X;
                futureSphere.Center.Z = futurePosition.Z;


                if (!WallCollision(futureSphere, m_World.Walls) && !BlockCollision(futureSphere, m_World.Blocks))
                {
                    m_Position = futurePosition;
                    m_BoundingSphere = futureSphere;
                    m_Fitness += 1.0 / (1.0 + Vector3.Distance(m_Position, m_TargetAgent.Position));    //increment for not crashing
                    if (movement.Z <= 0)
                    {
                        m_Fitness++;
                    }
                }


                /*for (int i = 0; i < m_MemoryOutputs.Count; i++)
                {
                    m_MemoryOutputs[i] = output[i];
                }*/
                //Update();
                return true;

            }
            return false;
        }

        public void GeneticUpdate2()
        {
            //create inputs
            List<double> input = new List<double>();

            //add in range finders for walls
            for (int i = 0; i < m_RangefinderWalls.Count; i++)
            {
                //add walls to the input list
                input.Add(m_RangefinderWalls[i].GetMinimumFeelerRange(m_World.Walls));
            }

            //add blocks
            for (int i = 0; i < m_RangefinderBlocks.Count; i++)
            {
                //add blocks to the input list
                input.Add(m_RangefinderBlocks[i].GetMinimumFeelerRange(m_World.Blocks));
            }

            //add enemy pie slices
            for (int i = 0; i < m_PieSliceSensorEnemies.PieSlices.Count; i++)
            {
                //add activation level
                input.Add(m_PieSliceSensorEnemies.PieSlices[i].ActivationLevel);
            }

            //add bomb pie slices
            for (int i = 0; i < m_PieSliceSensorBombs.PieSlices.Count; i++)
            {
                //add activation level
                input.Add(m_PieSliceSensorBombs.PieSlices[i].ActivationLevel);
            }

            //get output
            List<double> output = m_Brain.Update(input);
            if (output.Count != Params.NUM_OUTPUTS)
            {
                Console.WriteLine("Error in Brain");
                return;
            }

            //handle output
            #region Turning
            //turning
            double turnAmount = output[1];
            
            //default turn speed
            turnAmount *= DEFAULT_TURN_SPEED;
            
            //clamp turn amount
            turnAmount = MathHelper.Clamp((float)turnAmount, -DEFAULT_TURN_SPEED, DEFAULT_TURN_SPEED);
            
            //adjust heading from turn amount
            m_Heading += (float)turnAmount;
            m_Side += (float)turnAmount;
            
            //keep heading between 0 and 360
            m_Heading = MathHelper.WrapAngle(m_Heading);
            if (m_Heading < 0)
            {
                m_Heading += MathHelper.TwoPi;
            }

            m_Side = MathHelper.WrapAngle(m_Side);
            if (m_Side < 0)
            {
                m_Side += MathHelper.TwoPi;
            }
            #endregion

            #region Movement
            Matrix orientationMatrix = Matrix.CreateRotationY(m_Heading);

            Vector3 movement = Vector3.Zero;
            movement.Z = (float)(output[1]);

            m_Velocity = Vector3.Transform(movement, orientationMatrix) * DEFAULT_SPEED;

            Vector3 futurePosition = m_Position + m_Velocity;

            BoundingSphere futureSphere = m_BoundingSphere;
            futureSphere.Center.X = futurePosition.X;
            futureSphere.Center.Z = futurePosition.Z;

            if (!WallCollision(futureSphere, m_World.Walls) && !WallCollision(futureSphere, m_World.Blocks))
            {
                m_Position = futurePosition;
                m_BoundingSphere = futureSphere;    //increment for not crashing
                m_Fitness -= movement.Z;    //want - z movement
            }
            #endregion
        }
        #endregion

        public void UpdateRangefinderBlocks()
        {
            //clear rangefinder information
            m_InformationRangefinderBlockLengths.Clear();

            //update rangefinders for walls
            for (int i = 0; i < m_RangefinderBlocks.Count; i++)
            {
                //update rangefinders
                m_RangefinderBlocks[i].Update();

                //update rangefinder information
                m_InformationRangefinderBlockLengths.Add(m_RangefinderBlocks[i].GetMinimumFeelerRange(m_World.Blocks));

            }
        }

        public void UpdateRangefinderWalls()
        {
            //clear rangefinder information
            m_InformationRangefinderWallLengths.Clear();

            //update rangefinders for walls
            for (int i = 0; i < m_RangefinderWalls.Count; i++)
            {
                //update rangefinders
                m_RangefinderWalls[i].Update();

                //update rangefinder information
                m_InformationRangefinderWallLengths.Add(m_RangefinderWalls[i].GetMinimumFeelerRange(m_World.Walls));

            }
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
            for (int i = 0; i < blocks.Count; i++)
            {
                if (bs.Intersects(blocks[i].BoundingBox))
                {
                    return true;
                }
            }
            return false;
        }

        private bool AgentCollision(BoundingSphere bs, List<Tuple<Agent, float, float>> adjacentAgents)
        {
            foreach (Tuple<Agent, float, float> item in adjacentAgents)
            {
                return (bs.Intersects(item.Item1.BoundingSphere));
            }
            return false;
        }

        private bool TargetAgentCollision(BoundingSphere bs)
        {
            return bs.Intersects(m_TargetAgent.BoundingSphere);
        }

        private bool BoundaryCollision(Vector3 futurePosition)
        {
            if (futurePosition.X <= 130 && futurePosition.X >= -130 && futurePosition.Z <= 130 && futurePosition.Z >= -130)
                return false;
            return true;
        }

        public void UpdateSensor(List<Agent> agents, float radius)
        {
            int numAgents = agents.Count;
            float radiusSquared = radius * radius;

            //empty adjacency list for recalculation
            m_AgentAdjacencyList.Clear();

            if (m_IsActive || !m_IsActive)
            {
                for (int i = 0; i < numAgents; i++)
                {
                    //set current agent
                    Agent targetAgent = agents[i];
                    targetAgent.m_Position.Y = 0;

                    //make sure current agent is not the same as agent updating its sensory info
                    if (this.m_Id != targetAgent.m_Id)
                    {
                        //get position of current agent
                        //lets call it target position
                        Vector3 targetPosition = targetAgent.m_Position;

                        //get vector connecting this agent to target agent
                        Vector3 targetVector = targetPosition - m_Position;

                        //find squared distance
                        float distanceSquared = targetVector.X * targetVector.X + targetVector.Z * targetVector.Z;


                        //if squared distance <= squared radius, add to list
                        if (distanceSquared <= radiusSquared)
                        {
                            //determine target vector info
                            //distance
                            float distance = (float)Math.Sqrt(distanceSquared);

                            //vector angle
                            float angle = (float)Math.Atan2(-targetVector.Z, targetVector.X);

                            if (MathHelper.ToDegrees(angle) > 360)
                                angle -= MathHelper.ToRadians(360);
                            else if (MathHelper.ToDegrees(angle) < 0)
                                angle += MathHelper.ToRadians(360);

                            //target angle
                            float targetAngle = -MathHelper.WrapAngle(m_Heading - angle + MathHelper.ToRadians(90));
                            if (targetAngle < 0)
                                targetAngle += MathHelper.ToRadians(360.0f);

                            Tuple<Agent, float, float> adjacentAgent = new Tuple<Agent, float, float>(targetAgent, distance, targetAngle);
                            m_AgentAdjacencyList.Add(adjacentAgent);
                        }
                    }
                }
            }

        }

        public void SetPosition(Vector3 position)
        {
            m_Position = position;
            m_BoundingSphere.Center.X = position.X;
            m_BoundingSphere.Center.Z = position.Z;

            m_Heading = 0;
            m_Side = MathHelper.ToRadians(270);
        }

        public String outputAdjacentInfo()
        {
            //outputAdjacentAgents = false;
            String headingString = "" + Math.Round(MathHelper.ToDegrees(m_Heading), 2);
            String sideString = "" + Math.Round(MathHelper.ToDegrees(m_Side), 2);
            String retVal = "Heading : " + headingString + " degrees\nSide : " + sideString + " degrees\nPosition: " + m_Position + "\n[\n";

            foreach (Tuple<Agent, float, float> tuple in m_AgentAdjacencyList)
            {
                //int tupleIndex = agentAdjacencyList.IndexOf(agent);
                //Tuple<float, float> currentTuple = adjacentAgentRelativePosition.ElementAt(tupleIndex);
                Agent agent = tuple.Item1;
                float distance = tuple.Item2;
                float relativeAngle = tuple.Item3;
                retVal += "Agent ID: " + agent.m_Id + " (" + Math.Round(distance, 2) + "," + Math.Round(MathHelper.ToDegrees(relativeAngle), 2) +
                    ")\n";
            }
            return retVal + ']';
        }

        public void SetUpRays()
        {
            int numRays = 5;
            rays = new Ray[numRays];
            rayIntersections = new float?[numRays];
            UpdateRays();
        }

        public void UpdateRays()
        {
            rays = new Ray[5];

            for (int i = 0; i < rays.Length; i++)
                rays[i].Position = this.m_Position;

            for (int i = 0, angle = 90; i < rays.Length; i++, angle -= 45)
            {
                Vector3 farpoint = new Vector3(0f, 0f, -RAY_LENGTH);
                Matrix rotMatrix = Matrix.CreateRotationY(m_Heading + MathHelper.ToRadians(angle));
                farpoint = Vector3.Transform(farpoint, rotMatrix);
                farpoint += m_Position;
                rays[i] = new Ray(m_Position, (farpoint - m_Position));
            }
        }

        public void CheckRayCollisions(List<Wall> walls, List<Block> blocks)
        {
            rayIntersections = new float?[5];

            for (int i = 0; i < this.rays.Length; i++)
            {
                this.rayIntersections[i] = RAY_LENGTH;
                
                for (int j = 0; j < walls.Count; j++)
                {
                    float? intersection = this.rays[i].Intersects(walls[j].BoundingBox);

                    if (intersection != null && (intersection * RAY_LENGTH) < this.rayIntersections[i])
                        this.rayIntersections[i] = intersection * RAY_LENGTH;
                }

                for (int j = 0; j < blocks.Count; j++)
                {
                    float? intersection = this.rays[i].Intersects(blocks[j].BoundingBox);

                    if (intersection != null && (intersection * RAY_LENGTH) < this.rayIntersections[i])
                        this.rayIntersections[i] = intersection * RAY_LENGTH;
                }
            }
        }

        public void PutWeights(List<double> w)
        {
            m_Brain.PutWeights(w);
        }

        public void ResetFitness()
        {
            m_Fitness = 0;
        }


        public Node MoveAlongPath(List<Node> path, Node nextNode)
        {
            /*
             * Moves the agent along the generated path
             */
            Vector3 headingVector = Vector3.Forward;
            Matrix rotMatrix = Matrix.CreateRotationY(Heading);
            headingVector = Vector3.Transform(headingVector, rotMatrix);

            //get line connecting the current position to the next node
            Vector3 nextNodeVector = nextNode.Position - Position;

            //find the angle necessary to face the node
            double relativeAngle = Math.Atan2(headingVector.Z, headingVector.X) - Math.Atan2(nextNodeVector.Z, nextNodeVector.X);

            if (Vector3.Distance(this.Position, nextNode.Position) < 1)
            {
                nextNode.IsActive = false;
                if (!path.ElementAt(path.Count - 1).Equals(nextNode))
                {
                    nextNode = path.ElementAt(path.IndexOf(nextNode) + 1);
                    nextNode.IsActive = true;
                }
                return (nextNode);
            }
            else
            {
                if (relativeAngle < -0.1 || relativeAngle > 0.1)
                {
                    turn((float)MathHelper.WrapAngle((float)relativeAngle), m_TurnSpeed);
                }
                //else if (relativeAngle > 0.05)
                //{
                //    turn(-1, Game1.MAX_TURN_SPEED);
                //}
                else if (relativeAngle > -0.1 && relativeAngle < 0.1)
                {
                    Matrix orientationMatrix = Matrix.CreateRotationY(Heading);

                    Vector3 predictedPosition = Position;
                    Vector3 movement = new Vector3(0, 0, -1);

                    Vector3 speed = Vector3.Normalize(Vector3.Transform(movement, orientationMatrix)) * m_MaxSpeed;
                    predictedPosition = Position + speed;

                    BoundingSphere predictedSphere = m_BoundingSphere;
                    predictedSphere.Center.X = predictedPosition.X;
                    predictedSphere.Center.Z = predictedPosition.Z;

                    if (!WallCollision(predictedSphere, m_World.Walls) && !BlockCollision(predictedSphere, m_World.Blocks))
                    {
                        m_Position = predictedPosition;
                        m_BoundingSphere = predictedSphere;
                        //Console.WriteLine("Updated BoundingSpheres Agent " + m_ID + "\n" + m_Position + "\n" + m_BoundingSphere.Center);
                    }
                    
                }

                return (nextNode);
            }
        }

        public List<Node> FindPath(Node startNode, Node endNode)
        {
            /*
             * Determine a navigation path from startNode to endNode using A*.
             */
            //System.IO.TextWriter tw = new System.IO.StreamWriter("output.txt");

            startNode.g_dist = 0;
            startNode.h_dist = Vector3.Distance(startNode.Position, endNode.Position);
            startNode.f_dist = startNode.g_dist + startNode.h_dist;

            Dictionary<Node, Node> lastNode = new Dictionary<Node, Node>();

            List<Node> blockedSet = new List<Node>();
            List<Node> potentialSet = new List<Node>();

            potentialSet.Add(startNode);

            while (potentialSet.Count > 0)
            {
                Node nodeA = null;

                double f_min = Double.MaxValue;

                /*
                 * Choose the shortest path so far to continue from.
                 */
                //tw.WriteLine("Cycling through...");
                foreach (Node tempNode in potentialSet)
                {
                    //tw.WriteLine("Node " + tempNode.id + ", f_dist = " + tempNode.f_dist);
                    if (tempNode.f_dist < f_min)
                    {
                        f_min = tempNode.f_dist;
                        nodeA = tempNode;
                    }
                }

                //tw.WriteLine();

                // End when the next node chosen is the endNode
                if (nodeA.Equals(endNode))
                    return (createNavPath(lastNode, endNode));

                /*
                 * Since A is chosen to move on from,
                 * Block it so it is not chosen again.
                 */
                potentialSet.Remove(nodeA);
                blockedSet.Add(nodeA);

                //tw.WriteLine("Checking nodes adjacent to Node " + nodeA.id);
                //tw.WriteLine("====================================");

                foreach (Node nodeB in nodeA.adjNodes)
                {
                    /*
                     * Add the next nearest node onto the path
                     */
                    if (!blockedSet.Contains(nodeB))
                    {
                        bool addNode;

                        //tw.WriteLine("Node " + nodeB.id + ": " +
                        //    "{pos - (" + nodeB.Position.X + ", " + nodeB.Position.Y + ", " + nodeB.Position.Z + ")}, " +
                        //    "{g = " + nodeB.g_dist + ", h = " + nodeB.h_dist + ", f = " + nodeB.f_dist + "}");

                        double g_temp = nodeA.g_dist + Vector3.Distance(nodeA.Position, nodeB.Position);

                        if (!potentialSet.Contains(nodeB))
                        {
                            // Add the node to the potential nodes
                            // if it is not already there.
                            potentialSet.Add(nodeB);
                            addNode = true;
                        }
                        else if (g_temp < nodeB.g_dist)
                            addNode = true;
                        else
                            addNode = false;

                        if (addNode)
                        {
                            // Add the node if either it was not in the
                            // potential node list yet or it was the next
                            // shortest node to traverse towards the end.
                            lastNode[nodeB] = nodeA;

                            nodeB.g_dist = g_temp;
                            nodeB.h_dist = Vector3.Distance(nodeB.Position, endNode.Position);
                            nodeB.f_dist = nodeB.g_dist + nodeB.h_dist;
                        }
                    }
                }

                //tw.WriteLine();
                //tw.WriteLine();
            }
            //tw.Close();
            return (null);
        }

        public List<Node> createNavPath(Dictionary<Node, Node> lastNode, Node crnt_node)
        {
            /*
             * Construct the navigation graph for the agent to follow.
             */
            List<Node> result = new List<Node>();
            if (lastNode.ContainsKey(crnt_node))
            {
                result = createNavPath(lastNode, lastNode[crnt_node]);
                result.Add(crnt_node);
                return (result);
            }
            else
            {
                result.Add(crnt_node);
                return (result);
            }
        }

        /// <summary>
        /// Transition to appropriate state.
        /// </summary>
        /// <param name="newState">State to transition to.</param>
        public void ChangeState(State newState)
        {
            m_CurrentState = newState;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool NearBomb()
        {
            //loop through every bomb in the list
            for (int i = 0; i < m_AdjacencyListBombs.Edges.Count; i++)
            {
                //if the distance is less than half the bomb radius
                if (m_AdjacencyListBombs.Edges[i].Distance < Bomb.RADIUS / 2.0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool NearEnemy()
        {
            //loop through all the enemies
            for (int i = 0; i < m_AdjacencyListEnemies.Edges.Count; i++ )
            {
                //if the distance is less than the bomb's range then it's in range
                if(m_AdjacencyListEnemies.Edges[i].Distance < Bomb.RADIUS / 3.0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
