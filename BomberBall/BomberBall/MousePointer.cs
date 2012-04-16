using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
namespace Bann
{
    public class MousePointer : DrawableGameComponent
    {
        protected MouseState m_MousePointerState;
        public MouseState MousePointerState
        {
            get { return m_MousePointerState; }
            set { m_MousePointerState = value; }
        }

        protected Vector2 m_Position;
        public Vector2 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        protected Vector2 m_Center;
        public Vector2 Center
        {
            get { return m_Center; }
            set { m_Center = value; }
        }
        
        protected Texture2D m_Pointer;
        public Texture2D Pointer
        {
            get { return m_Pointer; }
            set { m_Pointer = value; }
        }

        protected SpriteBatch m_SpriteBatch;
        public SpriteBatch SpriteBatch
        {
            get { return m_SpriteBatch; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public MousePointer(Game game) :
            base(game)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            //load the texture associated with the pointer
            m_Pointer = Game.Content.Load<Texture2D>("Images\\pointer_crosshair");
            
            //textures draw from the top left position
            //we want the center of the crosshair to be the mouse position
            //so we need to calculate the centper position
            m_Center.X = m_Pointer.Width / 2;
            m_Center.Y = m_Pointer.Height / 2;

            m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);

            base.LoadContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            m_MousePointerState = Mouse.GetState();
            
            //the position of the mouse
            m_Position.X = m_MousePointerState.X;
            m_Position.Y = m_MousePointerState.Y;

            

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            m_SpriteBatch.Begin();
            m_SpriteBatch.Draw(m_Pointer, m_Position, null, Color.White, 0.0f, m_Center, 1.0f, SpriteEffects.None, 0.0f);
            m_SpriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
