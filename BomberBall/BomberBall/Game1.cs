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
using Platform;
using AI.ANN;
namespace Bann
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Members/Properties
        public enum GameState { CoverMenu, ModeSelect, CampaignSelection, QuickBattleSelection, 
                                NewCampaignSetup, CampaignManagement, ControlSchemeSelection, TeamMapSelection,
                                PlayGameCoach, PlayGamePerson, PlayGameStar, PlayGameTraining};

        public enum PlayGameState { None, Running, Paused};

        public enum GamePath { None, Campaign, QuickBattle, Training};

        protected GameOptions m_GameParameters;
        public GameOptions GameParameters
        {
            get { return m_GameParameters; }
            set { ;}
        }
        
        protected GameState m_CurrentGameState;
        public GameState CurrentGameState
        {
            get { return m_CurrentGameState; }
            set { m_CurrentGameState = value; }
        }

        protected PlayGameState m_CurrentPlayGameState;
        public PlayGameState CurrentPlayGameState
        {
            get { return m_CurrentPlayGameState; }
            set { m_CurrentPlayGameState = value; }
        }

        protected GamePath m_CurrentGamePath;
        public GamePath CurrentGamePath
        {
            get { return m_CurrentGamePath; }
            set { m_CurrentGamePath = value; }
        }
        protected GraphicsDeviceManager m_Graphics;
        public GraphicsDeviceManager Graphics
        {
            get { return m_Graphics; }
            set { m_Graphics = value; }
        }

        protected SpriteBatch m_SpriteBatch;
        public SpriteBatch SpriteBatch
        {
            get { return m_SpriteBatch; }
            set { m_SpriteBatch = value; }
        }

        protected KeyboardState m_CurrentKeyboardState;
        public KeyboardState CurrentKeyboardState
        {
            get { return m_CurrentKeyboardState; }
            set { m_CurrentKeyboardState = value; }
        }

        protected KeyboardState m_PreviousKeyboardState;
        public KeyboardState PreviousKeyboardState
        {
            get { return m_PreviousKeyboardState; }
            set { m_PreviousKeyboardState = value; }
        }

        protected MousePointer m_Cursor;
        public MousePointer Cursor
        {
            get { return m_Cursor; }
            set { m_Cursor = value; }
        }

        protected int m_ScreenWidth;
        public int ScreenWidth
        {
            get { return m_ScreenWidth; }
            set { m_ScreenWidth = value; }
        }

        protected int m_ScreenHeight;
        public int ScreenHeight
        {
            get { return m_ScreenHeight; }
            set { m_ScreenHeight = value; }
        }

        protected SpriteFont m_TitleFont;
        public SpriteFont TitleFont
        {
            get { return m_TitleFont; }
            set { ;}
        }

        protected Vector2 m_TitleFontPosition;
        public Vector2 TitleFontPosition
        {
            get { return m_TitleFontPosition; }
            set { m_TitleFontPosition = value; }
        }

        protected SpriteFont m_MenuFont;
        public SpriteFont MenuFont
        {
            get { return m_MenuFont; }
            set { ;}
        }

        protected Vector2 m_MenuFontPosition;
        public Vector2 MenuFontPosition
        {
            get { return m_MenuFontPosition; }
            set { m_MenuFontPosition = value; }
        }

        protected Map m_World;
        public Map World
        {
            get { return m_World; }
            set { m_World = value; }
        }
        #endregion

        #region Cover Menu
        protected Texture2D m_CoverMenuBackground;

        public Texture2D CoverMenuBackground
        {
            get { return m_CoverMenuBackground; }
            set { m_CoverMenuBackground = value; }
        }

        #endregion

        #region Mode Select

        protected Texture2D m_ModeSelectBackground;
        public Texture2D ModeSelectBackground
        {
            get { return m_ModeSelectBackground; }
            set { m_ModeSelectBackground = value; }
        }

        protected int m_ModeSelectNumber;

        protected string m_ModeSelectString;

        #endregion

        #region Campaign Selection
        #endregion

        #region Team/Map Selection

        protected String m_CurrentSelectedCharacterString;

        protected int m_CurrentSelectedCharacterNumber;

        protected String m_CurrentSelectedMapString;

        protected int m_CurrentSelectedMapNumber;

        #endregion

        #region 1st Person

        protected bool m_GameOverFlag;
        protected bool m_WinFlag;

        #endregion

        #region Training Information
        protected String m_InformationFeelers;
        protected String m_InformationPieSlices;
        protected String m_InformationAgent;
        protected String m_InformationGeneration;
        protected String m_InformationTick;
        protected String m_InformationTraining;
        #endregion
        public Game1()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            m_Graphics.PreferredBackBufferWidth = 1280;
            m_Graphics.PreferredBackBufferHeight = 800;
            m_Graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //init game options
            m_GameParameters = new GameOptions();

            //game initially starts out in cover menu
            m_CurrentGameState = GameState.CoverMenu;

            //init game path
            m_CurrentGamePath = GamePath.None;

            //init mouse pointer
            m_Cursor = new MousePointer(this);
            Components.Add(m_Cursor);

            

            m_ModeSelectString = "";
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            


            //load font info
            m_TitleFont = Content.Load<SpriteFont>("Fonts\\title_font");
            m_TitleFontPosition = Vector2.Zero;

            m_MenuFont = Content.Load<SpriteFont>("Fonts\\menu_font");
            m_MenuFontPosition = Vector2.Zero;

            #region Cover Menu
            InitializeCoverMenu();
            #endregion
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //get keyboard states to pass to the update function
            //set previous kb state to curren
            m_PreviousKeyboardState = m_CurrentKeyboardState;

            //update current kb state
            m_CurrentKeyboardState = Keyboard.GetState();

            if (m_CurrentKeyboardState.IsKeyDown(Keys.LeftAlt) && m_CurrentKeyboardState.IsKeyDown(Keys.F4))
            {
                this.Exit();
            }

            //update according to current GameState
            #region GameStateCheck
            switch (m_CurrentGameState)
            {
                case GameState.CampaignManagement:
                    {
                        UpdateCampaignManagement();
                        break;
                    }
                case GameState.CampaignSelection:
                    {
                        UpdateCampaignSelection();
                        break;
                    }
                case GameState.ControlSchemeSelection:
                    {
                        UpdateControlSchemeSelection();
                        break;
                    }
                case GameState.CoverMenu:
                    {
                        UpdateCoverMenu();
                        break;
                    }
                case GameState.ModeSelect:
                    {
                        UpdateModeSelect();
                        break;
                    }
                case GameState.NewCampaignSetup:
                    {
                        UpdateNewCampaignSetup();
                        break;
                    }
                case GameState.PlayGameCoach:
                    {
                        UpdatePlayGameNetworkCoach();
                        break;
                    }
                case GameState.PlayGamePerson:
                    {
                        UpdatePlayGameFirstPerson(gameTime);
                        break;
                    }
                case GameState.PlayGameStar:
                    {
                        UpdatePlayGameAStar();
                        break;
                    }
                case GameState.PlayGameTraining:
                    {
                        UpdatePlayGameTraining(gameTime);
                        break;
                    }
                case GameState.QuickBattleSelection:
                    {
                        UpdateQuickBattleSelection();
                        break;
                    }
                case GameState.TeamMapSelection:
                    {
                        UpdateTeamMapSelection();
                        break;
                    }
            }
            #endregion
            base.Update(gameTime);

            

        }
        
        

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            m_ScreenWidth = GraphicsDevice.Viewport.Width;
            m_ScreenHeight = GraphicsDevice.Viewport.Height;

            //set stencil state
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            //set blend state
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            m_SpriteBatch.Begin();

            //update according to current GameState
            #region GameStateCheck
            switch (m_CurrentGameState)
            {
                case GameState.CampaignManagement:
                    {
                        DrawCampaignManagement();
                        break;
                    }
                case GameState.CampaignSelection:
                    {
                        DrawCampaignSelection();
                        break;
                    }
                case GameState.ControlSchemeSelection:
                    {
                        DrawControlSchemeSelection();
                        break;
                    }
                case GameState.CoverMenu:
                    {
                        DrawCoverMenu();
                        break;
                    }
                case GameState.ModeSelect:
                    {
                        DrawModeSelect();
                        break;
                    }
                case GameState.NewCampaignSetup:
                    {
                        DrawNewCampaignSetup();
                        break;
                    }
                case GameState.PlayGameCoach:
                    {
                        DrawPlayGameNetworkCoach();
                        break;
                    }
                case GameState.PlayGamePerson:
                    {
                        m_SpriteBatch.End();
                        DrawPlayGameFirstPerson();
                        break;
                    }
                case GameState.PlayGameStar:
                    {
                        DrawPlayGameAStar();
                        break;
                    }
                case GameState.PlayGameTraining:
                    {
                        m_SpriteBatch.End();
                        DrawPlayGameTraining();
                        break;
                    }
                case GameState.QuickBattleSelection:
                    {
                        DrawQuickBattleSelection();
                        break;
                    }
                case GameState.TeamMapSelection:
                    {
                        DrawTeamMapSelection();
                        break;
                    }
            }
            #endregion
            m_SpriteBatch.End();
            base.Draw(gameTime);
        }

        protected bool KeyDownTwoInput(Keys k)
        {
            return (!m_PreviousKeyboardState.IsKeyDown(k) && m_CurrentKeyboardState.IsKeyDown(k));
        }

        #region Cover Menu
        #region Initialize
        protected void InitializeCoverMenu()
        {
            m_CoverMenuBackground = Content.Load<Texture2D>("Images\\CoverMenu");
        }
        #endregion
        #region Update
        /// <summary>
        /// 
        /// </summary>
        protected void UpdateCoverMenu()
        {
            UpdateKeyboardCoverMenu();
        }

        protected void UpdateKeyboardCoverMenu()
        {
            if (!m_PreviousKeyboardState.IsKeyDown(Keys.Enter) && m_CurrentKeyboardState.IsKeyDown(Keys.Enter))
            {
                m_CurrentGameState = GameState.ModeSelect;
                InitializeModeSelect();
            }
        }
        #endregion
        #region Draw
        /// <summary>
        /// 
        /// </summary>
        protected void DrawCoverMenu()
        {
            m_SpriteBatch.Draw(m_CoverMenuBackground, new Rectangle(0, 0, m_ScreenWidth, m_ScreenHeight), Color.White);
        }
        #endregion
        #endregion

        #region Mode Select
        #region Initialize
        protected void InitializeModeSelect()
        {
            //Load background image
            m_ModeSelectBackground = Content.Load<Texture2D>("Images\\ModeSelect");
            
            //init selection number
            m_ModeSelectNumber = 0;
        }
        #endregion
        #region Update
        /// <summary>
        /// 
        /// </summary>
        protected void UpdateModeSelect()
        {
            //update keyboard
            UpdateKeyboardModeSelect();

            //update mode select string
            m_ModeSelectString = GameOptions.MODE_SELECT_OPTIONS[m_ModeSelectNumber];

        }

        protected void UpdateKeyboardModeSelect()
        {
            //too keep mode select number in bounds
            int numModeSelectOptions = GameOptions.MODE_SELECT_OPTIONS.Count;

            //check for up keyboard button
            if (!m_PreviousKeyboardState.IsKeyDown(Keys.Up) && m_CurrentKeyboardState.IsKeyDown(Keys.Up))
            {
                //decrement mode select number
                m_ModeSelectNumber--;

                //wrap number
                if (m_ModeSelectNumber < 0)
                {
                    m_ModeSelectNumber = numModeSelectOptions-1;
                }
            }
            else if(!m_PreviousKeyboardState.IsKeyDown(Keys.Down) && m_CurrentKeyboardState.IsKeyDown(Keys.Down))//check for donw key
            {
                m_ModeSelectNumber = (m_ModeSelectNumber + 1) % numModeSelectOptions;//increment number, and wrap
            }

            if (!m_PreviousKeyboardState.IsKeyDown(Keys.Enter) && m_CurrentKeyboardState.IsKeyDown(Keys.Enter))//check for confirmation key(Enter key)
            {
                switch (GameOptions.MODE_SELECT_OPTIONS[m_ModeSelectNumber])//action is dependent on mode select number
                {
                    case "Free For All":    //go to free for all screen
                        {
                            m_CurrentGameState = GameState.TeamMapSelection;
                            //init team map variables
                            InitializeTeamMapSelection();
                            break;
                        }
                    case "Options": //go to options screen
                        {
                            break;
                        }
                    case "Training"://go to training screen
                        {
                            m_CurrentGameState = GameState.PlayGameTraining;
                            //init PlayGameTraining variables
                            InitializePlayGameTraining();
                            break;
                        }
                    case "Exit": //exits the game
                        {
                            this.Exit();
                            break;
                        }
                }
            }
        }
        #endregion
        #region Draw
        /// <summary>
        /// 
        /// </summary>
        protected void DrawModeSelect()
        {
            //m_SpriteBatch.Draw(m_ModeSelectBackground, new Rectangle(0, 0, m_ScreenWidth, m_ScreenHeight), Color.White);
            GraphicsDevice.Clear(Color.Black);
            
            //Draw "Mode Select" at the top of the screen
            String title = "Mode Select";
            
            Vector2 origin = m_TitleFont.MeasureString(title) / 2;

            //set the string in the top center of the screen
            m_TitleFontPosition = new Vector2(m_ScreenWidth/2, origin.Y);

            //draw the title
            m_SpriteBatch.DrawString(m_TitleFont, title, m_TitleFontPosition,
                Color.Red, 0, origin, 2.0f, SpriteEffects.None, 0.5f);

            //get the center
            origin = m_MenuFont.MeasureString(m_ModeSelectString) / 2;

            //set the string in the center of the screen
            m_MenuFontPosition = new Vector2(m_ScreenWidth / 2, m_ScreenHeight / 2);

            //draw the string
            m_SpriteBatch.DrawString(m_MenuFont, m_ModeSelectString, m_MenuFontPosition,
                Color.Red, 0, origin, 2.0f, SpriteEffects.None, 0.5f);
        }
        #endregion
        #endregion

        #region Campaign Selection
        #region Update
        /// <summary>
        /// 
        /// </summary>
        protected void UpdateCampaignSelection()
        {
        }
        #endregion
        #region Draw
        /// <summary>
        /// 
        /// </summary>
        protected void DrawCampaignSelection()
        {
        }
        #endregion
        #endregion

        #region Quick Battle Selection
        #region Update
        /// <summary>
        /// 
        /// </summary>
        protected void UpdateQuickBattleSelection()
        {
        }
        #endregion
        #region Draw
        /// <summary>
        /// 
        /// </summary>
        protected void DrawQuickBattleSelection()
        {
        }
        #endregion
        #endregion

        #region New Campaign Setup
        #region Update
        /// <summary>
        /// 
        /// </summary>
        protected void UpdateNewCampaignSetup()
        {
        }
        #endregion
        #region Draw
        /// <summary>
        /// 
        /// </summary>
        protected void DrawNewCampaignSetup()
        {
        }
        #endregion
        #endregion

        #region Campaign Management
        #region Update
        /// <summary>
        /// 
        /// </summary>
        protected void UpdateCampaignManagement()
        {
        }
        #endregion
        #region Draw
        /// <summary>
        /// 
        /// </summary>
        protected void DrawCampaignManagement()
        {
        }
        #endregion
        #endregion

        #region Control Scheme Selection
        #region Update
        /// <summary>
        /// 
        /// </summary>
        protected void UpdateControlSchemeSelection()
        {
        }
        #endregion
        #region Draw
        /// <summary>
        /// 
        /// </summary>
        protected void DrawControlSchemeSelection()
        {
        }
        #endregion
        #endregion

        #region Team/Map Selection
        #region Initialize
        protected void InitializeTeamMapSelection()
        {
            //init string for display of characters name
            m_CurrentSelectedCharacterString = "";

            //init number of selected character
            m_CurrentSelectedCharacterNumber = 0;

            //init string for display of map name
            m_CurrentSelectedMapString = "";

            //init number for current selected map
            m_CurrentSelectedMapNumber = 0;
        }
        #endregion
        #region Update
        /// <summary>
        /// 
        /// </summary>
        protected void UpdateTeamMapSelection()
        {
            UpdateKeyboardTeamMapSelection();

            //update current selected character string
            m_CurrentSelectedCharacterString = GameOptions.AGENT_INFO_LIST[m_CurrentSelectedCharacterNumber].Name;
        }

        protected void UpdateKeyboardTeamMapSelection()
        {
            int numCharacters = GameOptions.AGENT_INFO_LIST.Count;
            int numMaps = GameOptions.MAP_FILES.Count;

            //changes character
            if (!m_PreviousKeyboardState.IsKeyDown(Keys.Up) && m_CurrentKeyboardState.IsKeyDown(Keys.Up))
            {
                m_CurrentSelectedCharacterNumber--; //decrement character number
                if (m_CurrentSelectedCharacterNumber < 0)
                {
                    //perform wrap
                    m_CurrentSelectedCharacterNumber = numCharacters - 1;
                }   
            }
            else if(!m_PreviousKeyboardState.IsKeyDown(Keys.Down) && m_CurrentKeyboardState.IsKeyDown(Keys.Down))
            {
                m_CurrentSelectedCharacterNumber = (m_CurrentSelectedCharacterNumber + 1) % numCharacters;  //increment selected character num and wrap
            }

            if (!m_PreviousKeyboardState.IsKeyDown(Keys.Enter) && m_CurrentKeyboardState.IsKeyDown(Keys.Enter))
            {
                m_CurrentGameState = GameState.PlayGamePerson;
                InitializePlayGamePerson();
            }
            if (!m_PreviousKeyboardState.IsKeyDown(Keys.Escape) && m_CurrentKeyboardState.IsKeyDown(Keys.Escape))
            {
                //go to mode select
                m_CurrentGameState = GameState.ModeSelect;

                //init mode select variables
                InitializeModeSelect();
            }
        }
        #endregion
        #region Draw
        /// <summary>
        /// 
        /// </summary>
        protected void DrawTeamMapSelection()
        {
            //draw background
            GraphicsDevice.Clear(Color.Black);

            //set the title for the selection
            String title = "Choose Your Character";

            //get the center
            Vector2 origin = m_TitleFont.MeasureString(title) / 2;

            //set the position
            m_TitleFontPosition = new Vector2(m_ScreenWidth / 2, origin.Y);

            //draw title
            m_SpriteBatch.DrawString(m_TitleFont, title, m_TitleFontPosition, Color.Red,
                                    0.0f, origin, 1.5f, SpriteEffects.None, 0.5f);

            //update the center
            origin = m_MenuFont.MeasureString(m_CurrentSelectedCharacterString) / 2;

            //set the position
            m_MenuFontPosition = new Vector2(m_ScreenWidth / 2, m_ScreenHeight / 2);

            //draw the character's name
            m_SpriteBatch.DrawString(m_MenuFont, m_CurrentSelectedCharacterString, m_MenuFontPosition, Color.Red,
                                    0.0f, origin, 2.0f, SpriteEffects.None, 0.5f);
        }
        #endregion
        #endregion

        #region Play Game: Network Coach
        #region Update
        /// <summary>
        /// 
        /// </summary>
        protected void UpdatePlayGameNetworkCoach()
        {
        }
        #endregion
        #region Draw
        /// <summary>
        /// 
        /// </summary>
        protected void DrawPlayGameNetworkCoach()
        {
        }
        #endregion
        #endregion

        #region Play Game: 1st Person
        #region Initialize
        /// <summary>
        /// 
        /// </summary>
        protected void InitializePlayGamePerson()
        {
            //create the new map
            LevelGenerator LG = new LevelGenerator();
            LG.genLevel();
            m_World = new Map(Content, "Maps\\MapRand.ini");

            m_CurrentPlayGameState = PlayGameState.Running;

            //init game over flag
            m_GameOverFlag = false;
            m_WinFlag = false;

        }
        #endregion
        #region Update
        /// <summary>
        /// 
        /// </summary>
        protected void UpdatePlayGameFirstPerson(GameTime gameTime)
        {
            UpdateKeyboardPlayGameFirstPerson();
            if (m_CurrentPlayGameState == PlayGameState.Running)
            {
                //update agent input
                ((Agent)m_World.Player).Update(m_CurrentKeyboardState, m_PreviousKeyboardState, m_World.Walls, m_World.Blocks);

                //update camera
                m_World.WorldCamera.Update(((Agent)m_World.Player).Heading, m_World.Player.Position, GraphicsDevice.Viewport.AspectRatio);
                m_World.ViewMatrix = m_World.WorldCamera.viewMatrix;
                m_World.ProjectionMatrix = m_World.WorldCamera.projectionMatrix;

                //game time is 2 minutes just like presentation allots
                if (gameTime.TotalGameTime.Minutes > 2)
                {
                    m_GameOverFlag = true;
                    m_WinFlag = false;
                    m_CurrentPlayGameState = PlayGameState.Paused;
                }

                //only win condidtion, all agents are dead
                if (m_World.Agents.Count == 0)
                {
                    m_GameOverFlag = true;
                    m_WinFlag = true;
                    m_CurrentPlayGameState = PlayGameState.Paused;
                }



                foreach(Agent agent in m_World.Agents)
                {
                    agent.Update();
                }
                //bombs stuff
                for (int i = 0; i < m_World.Bombs.Count; i++)
                {
                    ((Bomb)m_World.Bombs[i]).update(gameTime);

                    if (((Bomb)m_World.Bombs[i]).LifeTime <= 0)
                    {
                        // bombRemovalList.Add(i);
                        List<GameEntity> affectedByExplosion = m_World.explosion(((Bomb)m_World.Bombs[i]));
                        m_World.Bombs.RemoveAt(i);

                        

                        foreach(GameEntity gameEntity in affectedByExplosion)
                        {

                            //if player gets hit by bomb game ends
                            if (m_World.Player.Equals(gameEntity))
                            {
                                m_GameOverFlag = true;
                                m_WinFlag = false;
                                m_CurrentPlayGameState = PlayGameState.Paused;
                            }

                            if (m_World.Bombs.Contains(gameEntity))
                            {

                                int bombIndex = m_World.Bombs.IndexOf((Bomb)gameEntity);
                                if (bombIndex != -1)
                                    ((Bomb)m_World.Bombs[bombIndex]).LifeTime = 0;

                            }

                            if (m_World.Blocks.Contains(gameEntity))
                            {
                                m_World.Blocks.Remove((Block)gameEntity);
                            }

                            if (m_World.Agents.Contains(gameEntity) && ((Agent)gameEntity).Invuln == false)
                            {
                                m_World.Agents.Remove((Agent)gameEntity);
                            }
                        }

                    }
                }
            }
        }

        protected void UpdateKeyboardPlayGameFirstPerson()
        {
            if (m_CurrentPlayGameState == PlayGameState.Running)
            {
                if (!m_PreviousKeyboardState.IsKeyDown(Keys.Escape) && m_CurrentKeyboardState.IsKeyDown(Keys.Escape))
                {
                    m_CurrentPlayGameState = PlayGameState.Paused;
                }
            }
            else if (m_CurrentPlayGameState == PlayGameState.Paused)
            {
                if (!m_PreviousKeyboardState.IsKeyDown(Keys.Enter) && m_CurrentKeyboardState.IsKeyDown(Keys.Enter))
                {
                    if(!m_GameOverFlag)
                        m_CurrentPlayGameState = PlayGameState.Running;
                    else
                        m_CurrentGameState = GameState.TeamMapSelection;
                }
                if (!m_PreviousKeyboardState.IsKeyDown(Keys.Escape) && m_CurrentKeyboardState.IsKeyDown(Keys.Escape))
                {
                    m_CurrentGameState = GameState.TeamMapSelection;
                }
            }
        }
        #endregion
        #region Draw
        /// <summary>
        /// 
        /// </summary>
        /// 

        //draws "explosion" block on target position
        private void explosionExtreme(Vector3 block)
        {
            Model m_Model = Content.Load<Model>("block");
            //create an array to hold transformations
            Matrix[] transformations = new Matrix[m_Model.Bones.Count];

            //copy transformation to transformation matrix
            m_Model.CopyAbsoluteBoneTransformsTo(transformations);

            //translate model the m_Position
            Matrix translationMatrix = Matrix.CreateTranslation(block);

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
                    effect.View = m_World.ViewMatrix;
                    effect.Projection = m_World.ProjectionMatrix;

                    //enable default lighting
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
                //draw mesh after setting effects
                mesh.Draw();

            }
        }

        protected void DrawPlayGameFirstPerson()
        {
            //clear the screen
            GraphicsDevice.Clear(Color.SkyBlue);

            //draw the terrain
            m_World.DrawTerrain(m_World.Ground.Model);

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            m_SpriteBatch.Begin();


            ((Agent)m_World.Player).Draw(m_World.ViewMatrix, m_World.ProjectionMatrix);
            //((Agent)m_World.SingleTargetAgent).Draw(m_World.ViewMatrix, m_World.ProjectionMatrix);


            while(m_World.Explosions.Count > 0)
            {
                explosionExtreme(m_World.Explosions.First());
                m_World.Explosions.RemoveAt(0);
            }


            foreach (Bomb bomb in m_World.Bombs)
            {
                bomb.Draw(m_World.ViewMatrix, m_World.ProjectionMatrix);
            }

            foreach (Agent agent in m_World.Agents)
            {
                agent.Draw(m_World.ViewMatrix, m_World.ProjectionMatrix);
            }

            foreach (Wall wall in m_World.Walls)
            {
                RasterizerState rs = new RasterizerState();
                rs.FillMode = FillMode.WireFrame;
                GraphicsDevice.RasterizerState = rs;
                wall.Draw(m_World.ViewMatrix, m_World.ProjectionMatrix);
                rs = new RasterizerState();
                rs.FillMode = FillMode.Solid;
                GraphicsDevice.RasterizerState = rs;
            }

            foreach (Block block in m_World.Blocks)
            {
                RasterizerState rs = new RasterizerState();
                rs.FillMode = FillMode.Solid;
                GraphicsDevice.RasterizerState = rs;
                block.Draw(m_World.ViewMatrix, m_World.ProjectionMatrix);
                rs = new RasterizerState();
                rs.FillMode = FillMode.Solid;
                GraphicsDevice.RasterizerState = rs;
            }

            /*foreach (Node node in m_World.Nodes)
            {
                node.Draw(m_World.ViewMatrix, m_World.ProjectionMatrix);
            }*/

            if (m_GameOverFlag)
            {
                if (!m_WinFlag)
                {
                    m_SpriteBatch.DrawString(m_TitleFont, "-**Game Over**-", new Vector2(m_ScreenWidth / 2, m_ScreenHeight / 2), Color.Red);
                    m_SpriteBatch.DrawString(m_TitleFont, "-**Game Over**-", new Vector2(m_ScreenWidth / 2 + 2, m_ScreenHeight / 2 + 2), Color.Black);
                }
                else
                {
                    m_SpriteBatch.DrawString(m_TitleFont, "~**You've Won!**~", new Vector2(m_ScreenWidth / 2, m_ScreenHeight/ 2), Color.Pink);
                    m_SpriteBatch.DrawString(m_TitleFont, "~**You've Won!**~", new Vector2(m_ScreenWidth / 2 + 2, m_ScreenHeight / 2 + 2), Color.Black);
                }

                m_SpriteBatch.DrawString(m_MenuFont, "press enter or esc to continue", new Vector2(m_ScreenWidth/2 + 100, m_ScreenHeight/2 + 100), Color.White);
                m_SpriteBatch.DrawString(m_MenuFont, "press enter or esc to continue", new Vector2(m_ScreenWidth / 2 + 102, m_ScreenHeight / 2 + 102), Color.Black);
                
            }
        }
        #endregion
        #endregion

        #region Play Game: A Star
        #region Update
        /// <summary>
        /// 
        /// </summary>
        protected void UpdatePlayGameAStar()
        {
        }
        #endregion
        #region Draw
        /// <summary>
        /// 
        /// </summary>
        protected void DrawPlayGameAStar()
        {
        }
        #endregion
        #endregion

        #region Play Game: Training
        #region Initialization
        /// <summary>
        /// 
        /// </summary>
        protected void InitializePlayGameTraining()
        {
            //instantiate a new map
            m_World = new Map(Content, "Maps//Map1.ini");

            //set the state to running
            m_CurrentPlayGameState = PlayGameState.Running;

            m_World.AverageFitness = new List<double>();
            m_World.BestFitness = new List<double>();
            int numWeights = ((Agent)m_World.Agents[0]).Brain.GetNumberWeights();
            List<int> splitPoints = ((Agent)m_World.Agents[m_World.NumAgents - 1]).Brain.CalculateSplitPoints();
            m_World.GeneticAlgorithm = new AgentGA(Params.POP_SIZE, Params.MUTATION_RATE, Params.CROSSOVER_RATE, numWeights, splitPoints);
            m_World.GenomePopulation = m_World.GeneticAlgorithm.Population;
            for (int i = 0; i < m_World.Agents.Count; i++)
            {
                ((Agent)m_World.Agents[i]).Brain.PutWeights(m_World.GenomePopulation[i].Weights);
            }

            ResetTrainingStrings();
        }

        protected void ResetTrainingStrings()
        {
            m_InformationAgent = "Agent: ";
            m_InformationFeelers = "";
            m_InformationPieSlices =    "Sense Agents: " + m_World.SenseAgents + 
                                        "\nSense Bombs: " + m_World.SenseBombs +
                                        "\n";
            m_InformationGeneration = "Generation: ";
            m_InformationTick = "Tick: ";
            m_InformationTraining = "Training Information\n";
        }
        #endregion
        #region Update
        /// <summary>
        /// 
        /// </summary>
        protected void UpdatePlayGameTraining(GameTime gameTime)
        {
            UpdateKeyboardPlayGameTraining();
            if (m_CurrentPlayGameState == PlayGameState.Running)
            {
                //update agent input
                ((Agent)m_World.Player).Update(m_CurrentKeyboardState, m_PreviousKeyboardState, m_World.Walls, m_World.Blocks);

                //update camera
                m_World.WorldCamera.Update(((Agent)m_World.Player).Heading, m_World.Player.Position, GraphicsDevice.Viewport.AspectRatio);
                m_World.ViewMatrix = m_World.WorldCamera.viewMatrix;
                m_World.ProjectionMatrix = m_World.WorldCamera.projectionMatrix;

                //handle genetic algorithm and updates
                if (m_World.NumTicks < Params.NUM_TICKS)
                {

                    //loop through and update each agent
                    for (int i = 0; i < m_World.Agents.Count; i++ )
                    {
                        ((Agent)m_World.Agents[i]).GeneticUpdate2();
                    }
                    //bombs stuff
                    for (int i = 0; i < m_World.Bombs.Count; i++)
                    {
                        ((Bomb)m_World.Bombs[i]).update(gameTime);

                        if (((Bomb)m_World.Bombs[i]).LifeTime <= 0)
                        {
                            // bombRemovalList.Add(i);
                            List<GameEntity> affectedByExplosion = m_World.explosion(((Bomb)m_World.Bombs[i]));
                            m_World.Bombs.RemoveAt(i);



                            foreach (GameEntity gameEntity in affectedByExplosion)
                            {
                                if (m_World.Bombs.Contains(gameEntity))
                                {

                                    int bombIndex = m_World.Bombs.IndexOf((Bomb)gameEntity);
                                    if (bombIndex != -1)
                                        ((Bomb)m_World.Bombs[bombIndex]).LifeTime = 0;

                                }

                                if (m_World.Blocks.Contains(gameEntity))
                                {
                                    m_World.Blocks.Remove((Block)gameEntity);
                                }
                            }

                        }
                    }

                    //update information
                    ResetTrainingStrings();

                    //update agent information
                    m_InformationAgent += "Position: " + m_World.Player.Position +
                                            " Heading: " + MathHelper.ToDegrees(((Agent)m_World.Player).Heading);

                    //update feeler information
                    for (int i = 0; i < ((Agent)m_World.Player).InformationRangefinderWallLengths.Count; i++)
                    {
                        if (i == 0)
                        {
                            m_InformationFeelers += "Wall Feelers\n";
                        }
                        m_InformationFeelers += "" + i + ": " + Math.Round(((Agent)m_World.Player).InformationRangefinderWallLengths[i], 2);
                        if (i < ((Agent)m_World.Player).InformationRangefinderWallLengths.Count - 1)
                        {
                            m_InformationFeelers += ", ";
                        }
                        else
                        {
                            m_InformationFeelers += "\n";
                        }
                    }
                    for (int i = 0; i < ((Agent)m_World.Player).InformationRangefinderBlockLengths.Count; i++)
                    {
                        if (i == 0)
                        {
                            m_InformationFeelers += "Block Feelers\n";
                        }
                        m_InformationFeelers += "" + i + ": " + Math.Round(((Agent)m_World.Player).InformationRangefinderBlockLengths[i], 2);
                        if (i < ((Agent)m_World.Player).InformationRangefinderBlockLengths.Count - 1)
                        {
                            m_InformationFeelers += ", ";
                        }
                    }

                    //update pie slice information
                    //enemies
                    //adjacency info
                    /*for (int i = 0; i < ((Agent)m_World.Player).PieSliceSensorEnemies.AdjacencyList.Edges.Count; i++)
                    {
                        if (i == 0)
                        {
                            m_InformationPieSlices += "Enemy Adjacency List.\n[";
                        }
                        m_InformationPieSlices += "" + ((Agent)m_World.Player).PieSliceSensorEnemies.AdjacencyList.Edges[i].Entity.ID;
                        m_InformationPieSlices += " at " + Math.Round(MathHelper.ToDegrees(((Agent)m_World.Player).PieSliceSensorEnemies.AdjacencyList.Edges[i].Angle), 2);
                        if (i < ((Agent)m_World.Player).PieSliceSensorEnemies.AdjacencyList.Edges.Count - 1)
                        {
                            m_InformationPieSlices += ",\n";
                        }
                        else
                        {
                            m_InformationPieSlices += "]\n";
                        }
                    }*/
                    for (int i = 0; i < ((Agent)m_World.Player).PieSliceSensorEnemies.NumSlices; i++)
                    {
                        if (i == 0)
                        {
                            m_InformationPieSlices += "Enemy Slices\n";
                        }
                        m_InformationPieSlices += "" + i + ": " + Math.Round(((Agent)m_World.Player).PieSliceSensorEnemies.PieSlices[i].ActivationLevel, 2);
                        if (i < ((Agent)m_World.Player).PieSliceSensorEnemies.NumSlices - 1)
                        {
                            m_InformationPieSlices += ", ";
                        }
                        else
                        {
                            m_InformationPieSlices += "\n";
                        }
                    }
                    //bombs
                    //adjacency list info
                    /*for (int i = 0; i < ((Agent)m_World.Player).PieSliceSensorBombs.AdjacencyList.Edges.Count; i++)
                    {
                        if (i == 0)
                        {
                            m_InformationPieSlices += "Bomb Adjacency List.\n[";
                        }
                        m_InformationPieSlices += "" + ((Agent)m_World.Player).PieSliceSensorBombs.AdjacencyList.Edges[i].Entity.ID;
                        m_InformationPieSlices += " at " + Math.Round(MathHelper.ToDegrees(((Agent)m_World.Player).PieSliceSensorBombs.AdjacencyList.Edges[i].Angle), 2);
                        if (i < ((Agent)m_World.Player).PieSliceSensorBombs.AdjacencyList.Edges.Count - 1)
                        {
                            m_InformationPieSlices += ",\n";
                        }
                        else
                        {
                            m_InformationPieSlices += "]\n";
                        }
                    }*/
                    for (int i = 0; i < ((Agent)m_World.Player).PieSliceSensorBombs.NumSlices; i++)
                    {
                        if (i == 0)
                        {
                            m_InformationPieSlices += "Bomb Slices\n";
                        }
                        m_InformationPieSlices += "" + i + ": " + Math.Round(((Agent)m_World.Player).PieSliceSensorBombs.PieSlices[i].ActivationLevel, 2);
                        if (i < ((Agent)m_World.Player).PieSliceSensorBombs.NumSlices - 1)
                        {
                            m_InformationPieSlices += ", ";
                        }
                    }
                    m_InformationTick += m_World.NumTicks;
                    m_InformationGeneration += m_World.GeneticAlgorithm.Generation;
                    m_InformationTraining += m_InformationAgent + "\n" +
                                            m_InformationFeelers + "\n" +
                                            m_InformationPieSlices + "\n" +
                                            m_InformationTick + "\n" +
                                            m_InformationGeneration + "\n";

                    m_World.NumTicks++;
                }
                else
                {
                    //reset tick counts
                    m_World.NumTicks = 0;

                    //create new population
                    m_World.GeneticAlgorithm.CalculateFitnessStats();
                    m_World.AverageFitness.Add(m_World.GeneticAlgorithm.AverageFitness);
                    m_World.BestFitness.Add(m_World.GeneticAlgorithm.BestFitness);
                    m_World.PreviousFittest = m_World.GeneticAlgorithm.FittestGenome;
                    m_World.GenomePopulation = m_World.GeneticAlgorithm.Epoch(m_World.GenomePopulation);
                    m_World.GeneticAlgorithm.Reset();

                    for (int i = 0; i < m_World.Agents.Count; i++)
                    {
                        //update brain by replacing weights
                        ((Agent)m_World.Agents[i]).Brain.PutWeights(m_World.GenomePopulation[i].Weights);

                        ((Agent)m_World.Agents[i]).ResetFitness();

                        ((Agent)m_World.Agents[i]).SetPosition(m_World.StartAgentLocation);
                        ((Agent)m_World.Agents[i]).Heading = 0;
                        ((Agent)m_World.Agents[i]).Side = MathHelper.ToRadians(270);
                        //agents[i].Position = RandomizeAgentPosition();
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected void UpdateKeyboardPlayGameTraining()
        {
            if (!m_CurrentKeyboardState.IsKeyDown(Keys.LeftControl))
            {
                if (m_CurrentPlayGameState == PlayGameState.Running)
                {
                    if (!m_PreviousKeyboardState.IsKeyDown(Keys.Escape) && m_CurrentKeyboardState.IsKeyDown(Keys.Escape))
                    {
                        m_CurrentPlayGameState = PlayGameState.Paused;
                    }
                    if (KeyDownTwoInput(Keys.F1))
                    {
                        m_World.LoadNewMap("Maps//Map1.ini");
                    }
                    else if (KeyDownTwoInput(Keys.F2))
                    {
                        m_World.LoadNewMap("Maps//Map2.ini");
                    }
                    else if (KeyDownTwoInput(Keys.F3))
                    {
                        m_World.LoadNewMap("Maps//Map3.ini");
                    }
                    else if (KeyDownTwoInput(Keys.F5))
                    {
                        m_World.LoadNewMap("Maps//Map5.ini");
                    }
                    else if (KeyDownTwoInput(Keys.F6))
                    {
                        m_World.LoadNewMap("Maps//Map6.ini");
                    }
                    else if (KeyDownTwoInput(Keys.F7))
                    {
                        m_World.LoadNewMap("Maps//Map7.ini");
                    }
                    else if (KeyDownTwoInput(Keys.F12))
                    {
                        LevelGenerator LG = new LevelGenerator();
                        LG.genLevel();
                        m_World.LoadNewMap("Maps//MapRand.ini");
                    }

                }
                else if (m_CurrentPlayGameState == PlayGameState.Paused)
                {
                    if (!m_PreviousKeyboardState.IsKeyDown(Keys.Enter) && m_CurrentKeyboardState.IsKeyDown(Keys.Enter))
                    {
                        m_CurrentPlayGameState = PlayGameState.Running;
                    }
                    if (!m_PreviousKeyboardState.IsKeyDown(Keys.Escape) && m_CurrentKeyboardState.IsKeyDown(Keys.Escape))
                    {
                        m_CurrentGameState = GameState.ModeSelect;
                        m_CurrentPlayGameState = PlayGameState.None;
                    }
                }
            }
            else
            {
                if (KeyDownTwoInput(Keys.F1))
                {
                    m_World.ToggleSenseBombs();
                }
                else if(KeyDownTwoInput(Keys.F2))
                {
                    m_World.ToggleSenseAgents();
                }
            }
        }
        #endregion
        #region Draw
        /// <summary>
        /// 
        /// </summary>
        protected void DrawPlayGameTraining()
        {
            //clear the screen
            GraphicsDevice.Clear(Color.SkyBlue);

            //draw the terrain
            m_World.DrawTerrain(m_World.Ground.Model);

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            m_SpriteBatch.Begin();


            ((Agent)m_World.Player).Draw(m_World.ViewMatrix, m_World.ProjectionMatrix);
            ((Agent)m_World.SingleTargetAgent).Draw(m_World.ViewMatrix, m_World.ProjectionMatrix);


            foreach (Bomb bomb in m_World.Bombs)
            {
                bomb.Draw(m_World.ViewMatrix, m_World.ProjectionMatrix);
            }

            foreach (Agent agent in m_World.Agents)
            {
                agent.Draw(m_World.ViewMatrix, m_World.ProjectionMatrix);
            }

            foreach (Wall wall in m_World.Walls)
            {
                RasterizerState rs = new RasterizerState();
                rs.FillMode = FillMode.WireFrame;
                GraphicsDevice.RasterizerState = rs;
                wall.Draw(m_World.ViewMatrix, m_World.ProjectionMatrix);
                rs = new RasterizerState();
                rs.FillMode = FillMode.Solid;
                GraphicsDevice.RasterizerState = rs;
            }

            foreach (Block block in m_World.Blocks)
            {
                RasterizerState rs = new RasterizerState();
                rs.FillMode = FillMode.Solid;
                GraphicsDevice.RasterizerState = rs;
                block.Draw(m_World.ViewMatrix, m_World.ProjectionMatrix);
                rs = new RasterizerState();
                rs.FillMode = FillMode.Solid;
                GraphicsDevice.RasterizerState = rs;
            }

            //draw stats to the screen
            Vector2 informationVector = m_MenuFont.MeasureString(m_InformationTraining);
            m_SpriteBatch.DrawString(m_MenuFont, m_InformationTraining, new Vector2(10, 0), Color.Red);
        }
        #endregion
        #endregion
    }
}
