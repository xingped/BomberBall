using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AI.ANN;

namespace Platform
{
    public class Map
    {
        protected ContentManager m_Content;
        public ContentManager Content
        {
            get { return m_Content; }
            set { ;}
        }

        protected String m_Filename;
        public String Filename
        {
            get { return m_Filename; }
            set { m_Filename = value; }
        }

        protected List<Vector3> m_Explosions;
        public List<Vector3> Explosions
        {
            get { return m_Explosions; }
            set { m_Explosions = value; }
        }

        protected List<GameEntity> m_Walls;
        public List<GameEntity> Walls
        {
            get { return m_Walls; }
            set { m_Walls = value; }
        }

        protected List<GameEntity> m_Blocks;
        public List<GameEntity> Blocks
        {
            get { return m_Blocks; }
            set { m_Blocks = value; }
        }

        protected List<GameEntity> m_AllAgents;
        public List<GameEntity> AllAgents
        {
            get { return m_AllAgents; }
            set { m_AllAgents = value; }
        }

        protected List<GameEntity> m_Agents;
        public List<GameEntity> Agents
        {
            get { return m_Agents; }
            set { m_Agents = value; }
        }

        protected List<GameEntity> m_Bombs;
        public List<GameEntity> Bombs
        {
            get { return m_Bombs; }
            set { m_Bombs = value; }
        }

        protected List<GameEntity> m_Nodes;
        public List<GameEntity> Nodes
        {
            get { return m_Nodes; }
            set { m_Nodes = value; }
        }

        protected GameEntity m_Player;
        public GameEntity Player
        {
            get { return m_Player; }
            set { m_Player = value; }
        }

        protected GameEntity m_SingleTargetAgent;
        public GameEntity SingleTargetAgent
        {
            get { return m_SingleTargetAgent; }
            set { m_SingleTargetAgent = value; }
        }

        protected Vector3 m_StartAgentLocation;
        public Vector3 StartAgentLocation
        {
            get { return m_StartAgentLocation; }
            set { m_StartAgentLocation = value; }
        }

        protected int m_MapWidth;
        public int MapWidth
        {
            get { return m_MapWidth; }
            set { m_MapWidth = value; }
        }

        protected int m_MapLength;
        public int MapLength
        {
            get { return m_MapLength; }
            set { m_MapLength = value; }
        }

        protected Camera m_WorldCamera;
        public Camera WorldCamera
        {
            get { return m_WorldCamera; }
            set { m_WorldCamera = value; }
        }

        protected int m_NextId;
        public int NextId
        {
            get { return m_NextId; }
            set { m_NextId = value; }
        }

        protected Matrix m_ViewMatrix;
        public Matrix ViewMatrix
        {
            get { return m_ViewMatrix; }
            set { m_ViewMatrix = value; }
        }

        protected Matrix m_ProjectionMatrix;
        public Matrix ProjectionMatrix
        {
            get { return m_ProjectionMatrix; }
            set { m_ProjectionMatrix = value; }
        }

        protected GameEntity m_Ground;
        public GameEntity Ground
        {
            get { return m_Ground; }
            set { m_Ground = value; }
        }

        protected List<Vector3> m_StartPositions;
        public List<Vector3> StartPositions
        {
            get { return m_StartPositions; }
            set { m_StartPositions = value; }
        }

        #region Training Information
        protected Boolean m_Training;
        public Boolean Training
        {
            get { return m_Training; }
            set { ;}
        }

        protected Params m_Parameters;
        public Params Parameters
        {
            get { return m_Parameters; }
            set { m_Parameters = value; }
        }

        protected int m_NumTicks;
        public int NumTicks
        {
            get { return m_NumTicks; }
            set { m_NumTicks = value; }
        }

        protected int m_Generation;
        public int Generation
        {
            get { return m_Generation; }
            set { m_Generation = value; }
        }

        protected List<Genome> m_GenomePopulation;
        public List<Genome> GenomePopulation
        {
            get { return m_GenomePopulation; }
            set { m_GenomePopulation = value; }
        }

        protected AgentGA m_GeneticAlgorithm;
        public AgentGA GeneticAlgorithm
        {
            get { return m_GeneticAlgorithm; }
            set { m_GeneticAlgorithm = value; }
        }

        protected int m_NumWeights;
        public int NumWeights
        {
            get { return m_NumWeights; }
            set { m_NumWeights = value; }
        }

        protected List<double> m_AverageFitness;
        public List<double> AverageFitness
        {
            get { return m_AverageFitness; }
            set { m_AverageFitness = value; }
        }

        protected List<double> m_BestFitness;
        public List<double> BestFitness
        {
            get { return m_BestFitness; }
            set { m_BestFitness = value; }
        }

        protected int m_PreviousFittest;
        public int PreviousFittest
        {
            get { return m_PreviousFittest; }
            set { m_PreviousFittest = value; }
        }

        protected int m_NumAgents;
        public int NumAgents
        {
            get { return m_NumAgents; }
            set { m_NumAgents = value; }
        }

        protected bool m_SenseBombs;
        public bool SenseBombs
        {
            get { return m_SenseBombs; }
            set { m_SenseBombs = value; }
        }

        protected bool m_SenseAgents;
        public bool SenseAgents
        {
            get { return m_SenseAgents; }
            set { m_SenseAgents = value; }
        }
        #endregion


        protected void InitializeLists()
        {
            m_Walls = new List<GameEntity>();
            m_Blocks = new List<GameEntity>();
            m_Agents = new List<GameEntity>();
            m_Bombs = new List<GameEntity>();
            m_StartPositions = new List<Vector3>();
            m_AllAgents = new List<GameEntity>();
            m_Nodes = new List<GameEntity>();

            m_Explosions = new List<Vector3>();
        }

        public Map(ContentManager content, String filename)
        {
            //set content
            m_Content = content;


            //init lists
            InitializeLists();

            //load player
            m_Player = new Agent(this, m_NextId++, false);
            ((Agent)m_Player).LoadContent(content, "agent1");
            m_Player.IsActive = true;
            ((Agent)m_Player).SetPosition(Vector3.Zero);

            m_StartAgentLocation = new Vector3(0, 10, 0);

            //target
            m_SingleTargetAgent = new Agent(this, m_NextId++, false);
            m_SingleTargetAgent.Model = Content.Load<Model>("agent_target1");

            //other players

            //init ground
            m_Ground = new GameEntity();
            m_Ground.Model = Content.Load<Model>("floor");

            //init cam
            m_WorldCamera = new Camera();

            //init parameters
            m_Parameters = new Params("Params.ini");

            //Load the map file
            LoadNewMap(filename);

            m_SenseBombs = true;
            m_SenseAgents = true;
        }

        public void LoadNewMap(String filename)
        {
            System.IO.StreamReader fin;
            fin = new System.IO.StreamReader(@filename);

            String currentString;
            currentString = fin.ReadLine();
            m_StartPositions.Clear();
            m_Agents.Clear();
            m_AllAgents.Clear();

            bool readingWalls = false;
            bool readingBlocks = false;
            bool readingNodes = false;
            bool readingTarget = false;
            bool readingSpawn = false;

            float x, y, z;

            List<Vector3> wallPositions = new List<Vector3>();
            List<Vector3> blockPositions = new List<Vector3>();
            List<Vector3> nodePositions = new List<Vector3>();
            Vector3 targetPosition = Vector3.Zero;
            Vector3 spawnPosition = Vector3.Zero;

            while (currentString != "[End]")
            {
                //check to see if we're reading walls
                if (currentString == "[Walls]")
                {
                    readingWalls = true;
                    readingBlocks = false;
                    readingNodes = false;
                    readingTarget = false;
                    readingSpawn = false;
                    //get next line
                    currentString = fin.ReadLine();
                }
                else if (currentString == "[Blocks]")
                {
                    readingWalls = false;
                    readingBlocks = true;
                    readingNodes = false;
                    readingTarget = false;
                    readingSpawn = false;
                    //get next line
                    currentString = fin.ReadLine();
                }
                else if (currentString == "[Nodes]")
                {
                    readingWalls = false;
                    readingBlocks = false;
                    readingNodes = true;
                    readingTarget = false;
                    readingSpawn = false;
                    //get next line
                    currentString = fin.ReadLine();
                }
                else if (currentString == "[Target]")
                {
                    readingWalls = false;
                    readingBlocks = false;
                    readingNodes = false;
                    readingTarget = true;
                    readingSpawn = false;
                    //get next line
                    currentString = fin.ReadLine();
                }
                else if (currentString == "[Spawn]")
                {
                    readingWalls = false;
                    readingBlocks = false;
                    readingNodes = false;
                    readingTarget = false;
                    readingSpawn = true;
                    //get next line
                    currentString = fin.ReadLine();
                }
                else if (currentString == "")
                {
                    currentString = fin.ReadLine();
                }
                else
                {

                    //current string should now be 3 float values separated by spaces
                    //create a array of characters the length of the current string
                    char[] c = new char[currentString.Length];
                    string xString = "";
                    string yString = "";
                    string zString = "";
                    int space1 = 0, space2 = 0;

                    //create a string reader and attach it to the current character string
                    System.IO.StringReader sr = new System.IO.StringReader(currentString);

                    //scan x
                    for (int i = 0; i < currentString.Length; i++)
                    {
                        if (currentString[i] == ' ')
                        {
                            space1 = i;
                            break;
                        }
                        xString += currentString[i];
                    }

                    //scan y
                    for (int i = space1 + 1; i < currentString.Length; i++)
                    {
                        if (currentString[i] == ' ')
                        {
                            space2 = i;
                            break;
                        }
                        yString += currentString[i];
                    }

                    //scan z
                    for (int i = space2 + 1; i < currentString.Length; i++)
                    {
                        zString += currentString[i];
                    }

                    x = float.Parse(xString);
                    y = float.Parse(yString);
                    z = float.Parse(zString);

                    Vector3 newPosition = new Vector3(x, y, z);
                    //check for what we're adding to
                    if (readingWalls)
                    {
                        wallPositions.Add(newPosition);
                    }
                    else if (readingBlocks)
                    {
                        blockPositions.Add(newPosition);
                    }
                    else if (readingNodes)
                    {
                        nodePositions.Add(newPosition);
                    }
                    else if (readingSpawn)
                    {
                        spawnPosition = newPosition;
                        m_StartPositions.Add(newPosition);
                    }
                    else if (readingTarget)
                    {
                        targetPosition = newPosition;
                    }

                    //read in next string
                    currentString = fin.ReadLine();
                }

            }

            //currentString == [End]
            //so close file
            fin.Close();

            //wallPositions contains all wall positions
            LoadNewMapWalls(wallPositions);

            //blockPositions contains all block positions
            LoadNewMapBlocks(blockPositions);

            //nodePositions contains all node positions
            LoadNewMapNodes(nodePositions);

            //spawnPosition contains new m_StartAgentLocation
            if (m_StartPositions.Count == 1)
            {
                //create agents for genetic algorithm
                m_NumAgents = Params.POP_SIZE;
                m_Agents.Clear();
                for (int i = 0; i < m_NumAgents; i++)
                {
                    GameEntity newAgent = new Agent(this, m_NextId++, false);
                    ((Agent)newAgent).LoadContent(m_Content, "agent1");
                    m_Agents.Add(newAgent);
                }
                //load spawn positions
                LoadNewMapSpawn(spawnPosition);
                LoadNewMapTarget(targetPosition);
            }
            else
            {
                //create 3 more agents
                for (int i = 0; i < 3; i++)
                {
                    GameEntity newAgent = new Agent(this, m_NextId++, false);
                    ((Agent)newAgent).LoadContent(m_Content, "agent1");

                    //add new agent
                    m_Agents.Add(newAgent);
                }
                LoadNewMapStartPositions();
            }
            //targetPosition contains new m_StartTargetLocation
            
            //set adjacency lists
            SetAdjacencyListEnemies();
            SetAdjacencyListNodes();

            //reset tick count
            m_NumTicks = 0;

        }

        public List<GameEntity> explosion(Bomb b)
        {

            List<GameEntity> affectedByExplosion = new List<GameEntity>();

            float x = b.Position.X;
            float y = b.Position.Y;
            float z = b.Position.Z;
            float halfR = (float)(b.Radius*.5);
            float explosionLength = (float)(b.ExplosionLength);
            float tempLength = 0;

            //add an explosion at bomb center, explosion cube ☼
            m_Explosions.Add(b.Position);

            BoundingBox up = BoundingBox.CreateFromPoints(new Vector3[8]
            {
                new Vector3(x+halfR,y,z),
                new Vector3(x+halfR,y,z),
                new Vector3(x-halfR,y,z),
                new Vector3(x-halfR,y,z),
                new Vector3(x+halfR,y,z+explosionLength),
                new Vector3(x+halfR,y,z+explosionLength),
                new Vector3(x-halfR,y,z+explosionLength),
                new Vector3(x-halfR,y,z+explosionLength),
            });

            //this block and others like it do the following
            //1. determine the farthest the explosion can go
            //2. add that distance in referrence to the bombs to a list of positions to have an explosion model on 
            //3. calls a function that returns a list of objects affected by explosion to bigger list of those affected
            tempLength = ClosestCollidingWallOrBlock(b, up);
            m_Explosions.Add(b.Position + new Vector3(0,0,-tempLength));
            affectedByExplosion.AddRange(ExplosionCollision(b, up, tempLength));

            BoundingBox down = BoundingBox.CreateFromPoints(new Vector3[8]
            {
                new Vector3(x+halfR,y,z),
                new Vector3(x+halfR,y,z),
                new Vector3(x-halfR,y,z),
                new Vector3(x-halfR,y,z),
                new Vector3(x+halfR,y,z-explosionLength),
                new Vector3(x+halfR,y,z-explosionLength),
                new Vector3(x-halfR,y,z-explosionLength),
                new Vector3(x-halfR,y,z-explosionLength),
            });

            tempLength = ClosestCollidingWallOrBlock(b, down);
            m_Explosions.Add(b.Position + new Vector3(0,0,tempLength));
            affectedByExplosion.AddRange(ExplosionCollision(b, down, tempLength));

            BoundingBox left = BoundingBox.CreateFromPoints(new Vector3[8]
            {
                new Vector3(x,y,z+halfR),
                new Vector3(x,y,z+halfR),
                new Vector3(x,y,z-halfR),
                new Vector3(x,y,z-halfR),
                new Vector3(x-explosionLength,y,z+halfR),
                new Vector3(x-explosionLength,y,z+halfR),
                new Vector3(x-explosionLength,y,z-halfR),
                new Vector3(x-explosionLength,y,z-halfR),
            });


            tempLength = ClosestCollidingWallOrBlock(b, left);
            m_Explosions.Add(b.Position + new Vector3(-tempLength,0,0));
            affectedByExplosion.AddRange(ExplosionCollision(b, left, tempLength));

            BoundingBox right = BoundingBox.CreateFromPoints(new Vector3[8]
            {
                new Vector3(x,y,z+halfR),
                new Vector3(x,y,z+halfR),
                new Vector3(x,y,z-halfR),
                new Vector3(x,y,z-halfR),
                new Vector3(x+explosionLength,y,z+halfR),
                new Vector3(x+explosionLength,y,z+halfR),
                new Vector3(x+explosionLength,y,z-halfR),
                new Vector3(x+explosionLength,y,z-halfR),
            });

            tempLength = ClosestCollidingWallOrBlock(b, right);
            m_Explosions.Add(b.Position + new Vector3(tempLength,0,0));
            affectedByExplosion.AddRange(ExplosionCollision(b, right, tempLength));


            return affectedByExplosion;

            //BoundingBox.CreateFromPoints();
        }


        /*Checks for objects within explosion range
         *(dictated by the closest colliding wall to the bombs original explosion range)
         */
        private List<GameEntity> ExplosionCollision(Bomb b, BoundingBox bB, float explosionLength)
        {
            List<GameEntity> affectedByExplosion = new List<GameEntity>(); 
            
            float temp;

            //does bomb hit player?
            if (bB.Intersects(m_Player.BoundingSphere))
            {
                affectedByExplosion.Add(m_Player);
            }


            #region Blocks in explosion range
            for (int i = 0; i < m_Blocks.Count; i++)
            {
                if (bB.Intersects(m_Blocks[i].BoundingBox))
                {
                    temp = Vector3.Distance(b.Position, m_Blocks[i].Position);
                    if (temp <= explosionLength)
                    {
                        affectedByExplosion.Add(m_Blocks[i]);
                    }
                }
            }
            #endregion

            #region Agents in explosion range
            for (int i = 0; i < m_Agents.Count; i++)
            {
                if (bB.Intersects(m_Agents[i].BoundingSphere))
                {
                    temp = Vector3.Distance(b.Position, m_Agents[i].Position);
                    if (temp <= explosionLength)
                    {
                        affectedByExplosion.Add(m_Agents[i]);
                    }
                }
            }
            #endregion

            #region Bombs in explosion range
            for (int i = 0; i < m_Bombs.Count; i++)
            {
                if (bB.Intersects(m_Bombs[i].BoundingSphere))
                {
                    temp = Vector3.Distance(b.Position, m_Bombs[i].Position);
                    if (temp <= explosionLength)
                    {
                        affectedByExplosion.Add(m_Bombs[i]);
                    }
                }
            }
            #endregion

            return affectedByExplosion;

        }

        //closest colliding wall to a bomb within a bounding box
        private float ClosestCollidingWallOrBlock(Bomb b, BoundingBox bB)
        {
            float max = (float)b.ExplosionLength;
            float temp;


            for (int i = 0; i < m_Walls.Count; i++)
            {
                if (bB.Intersects(m_Walls[i].BoundingBox))
                {

                    temp = Vector3.Distance(b.Position, m_Walls[i].Position);
                    if (temp < max)
                    {
                        max = temp;
                    }
                }
            }

            for (int i = 0; i < m_Blocks.Count; i++)
            {
                if (bB.Intersects(m_Blocks[i].BoundingBox))
                {

                    temp = Vector3.Distance(b.Position, m_Blocks[i].Position);
                    if (temp < max)
                    {
                        max = temp;
                    }
                }
            }

            return max;

        }

        public void LoadNewMapWalls(List<Vector3> wallPositions)
        {
            //erase old walls
            m_Walls.Clear();

            foreach (Vector3 position in wallPositions)
            {
                //create new wall
                Wall newWall = new Wall();

                //load content
                newWall.LoadContent(Content, "Models//wall2");

                //wall status
                newWall.WallStatus = Wall.Status.Down;

                //set position
                newWall.SetPosition(position);

                //add wall
                m_Walls.Add(newWall);
            }
        }

        public void LoadNewMapBlocks(List<Vector3> blockPositions)
        {
            //erase old blocks
            m_Blocks.Clear();

            foreach (Vector3 position in blockPositions)
            {
                //create new block
                Block newBlock = new Block();

                //load content
                newBlock.LoadContent(Content, "Models//block2");

                //block status
                newBlock.BlockStatus = Block.Status.Down;

                //set position
                newBlock.SetPosition(position);

                //add block
                m_Blocks.Add(newBlock);
            }
        }

        public void LoadNewMapNodes(List<Vector3> nodePositions)
        {
            //erase old blocks
            m_Nodes.Clear();

            foreach (Vector3 position in nodePositions)
            {
                //create new node
                Node newNode = new Node(m_NextId++);

                //load content
                newNode.LoadContent(Content, "Models//navNode");

                //set position
                newNode.SetPosition(position);

                //add node
                m_Nodes.Add(newNode);
            }

            // Connect all the nodes to each other
            foreach (Node node in m_Nodes)
            {
                // Connect possible left node
                foreach (Node connNode in m_Nodes)
                {
                    if (connNode.Position.X == node.Position.X + 6
                        && connNode.Position.Z == node.Position.Z)
                    {
                        node.adjNodes.Add(connNode);
                    }
                    else if (connNode.Position.X == node.Position.X
                        && connNode.Position.Z == node.Position.Z + 6)
                    {
                        node.adjNodes.Add(connNode);
                    }
                    else if (connNode.Position.X == node.Position.X - 6
                        && connNode.Position.Z == node.Position.Z)
                    {
                        node.adjNodes.Add(connNode);
                    }
                    else if (connNode.Position.X == node.Position.X
                        && connNode.Position.Z == node.Position.Z - 6)
                    {
                        node.adjNodes.Add(connNode);
                    }
                }
            }
        }

        public void LoadNewMapTarget(Vector3 target)
        {
            //set tart location
            ((Agent)m_SingleTargetAgent).SetPosition(target);
        }

        public void LoadNewMapSpawn(Vector3 spawnPosition)
        {
            //set new spawn position
            m_StartAgentLocation = spawnPosition;

            ((Agent)m_Player).SetPosition(m_StartAgentLocation);
            //set each agents position
            for (int i = 0; i < m_Agents.Count; i++)
            {
                ((Agent)m_Agents[i]).SetPosition(m_StartAgentLocation);

                //reset fitness
                ((Agent)m_Agents[i]).ResetFitness();
            }
        }

        public void LoadNewMapStartPositions()
        {
            ((Agent)m_Player).SetPosition(m_StartPositions[0]);
            ((Agent)m_Player).StartingLocation = m_StartPositions[0];
            for (int i = 0; i < m_Agents.Count; i++)
            {
                ((Agent)m_Agents[i]).SetPosition(m_StartPositions[i + 1]);
                ((Agent)m_Agents[i]).StartingLocation = m_StartPositions[i + 1];
            }
        }

        public void SetAdjacencyListEnemies()
        {
            m_AllAgents.Clear();
            m_AllAgents.Add(m_Player);
            for(int i = 0; i < m_Agents.Count; i++)
            {
                m_AllAgents.Add(m_Agents[i]);
            }

            for (int i = 0; i < m_AllAgents.Count; i++)
            {
                ((Agent)m_AllAgents[i]).SetAdjacencyListEnemies(m_AllAgents);
            }
        }

        public void SetAdjacencyListNodes()
        {
            m_AllAgents.Clear();
            m_AllAgents.Add(m_Player);
            for (int i = 0; i < m_Agents.Count; i++)
            {
                m_AllAgents.Add(m_Agents[i]);
            }

            for (int i = 0; i < m_AllAgents.Count; i++)
            {
                ((Agent)m_AllAgents[i]).SetAdjacencyListNodes(m_Nodes);
            }
        }

        public void EmptyAdjacencyListEnemies()
        {
            m_AllAgents.Clear();
            ((Agent)m_Player).SetAdjacencyListEnemies(m_AllAgents);

            for (int i = 0; i < m_Agents.Count; i++)
            {
                ((Agent)m_Agents[i]).SetAdjacencyListEnemies(m_AllAgents);
            }
        }

        public void ToggleSenseBombs()
        {
            m_SenseBombs = !m_SenseBombs;
            if (m_SenseBombs)
            {
                ((Agent)m_Player).SetAdjacencyListBombs(m_Bombs);
                for (int i = 0; i < m_Agents.Count; i++)
                {
                    ((Agent)m_Agents[i]).SetAdjacencyListBombs(m_Bombs);
                }
            }
            else
            {
                ((Agent)m_Player).SetAdjacencyListBombs(new List<GameEntity>());
                for (int i = 0; i < m_Agents.Count; i++)
                {
                    ((Agent)m_Agents[i]).SetAdjacencyListBombs(new List<GameEntity>());
                }
            }
        }

        public void ToggleSenseAgents()
        {
            m_SenseAgents = !m_SenseAgents;
            if (m_SenseAgents)
            {
                SetAdjacencyListEnemies();
            }
            else
            {
                EmptyAdjacencyListEnemies();
            }
        }
        #region Draw Terrain
        public void DrawTerrain(Model model)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = Matrix.CreateScale(1.0f);

                    //use the matrices provided by ther game camera
                    effect.View = m_ViewMatrix;
                    effect.Projection = m_ProjectionMatrix;
                }
                mesh.Draw();
            }
        }
        #endregion
    }
}
