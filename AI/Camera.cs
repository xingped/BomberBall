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
using Microsoft.Xna.Framework.Input;

namespace Platform
{
    public class Camera
    {
        public enum CameraType { ChaseCamera, FreeCamera };

        public CameraType cameraType;

        public Matrix viewMatrix;

        public Matrix projectionMatrix;

        public Vector3 headOffset;

        public Vector3 targetOffset;

        public Vector3 position;

        public Vector3 cameraUpVector;

        public float reference;

        public Vector3 turnAmount;

        protected float m_AngleY;
        protected float m_AngleX;
        public Camera()
        {
            viewMatrix = Matrix.Identity;
            projectionMatrix = Matrix.Identity;

            headOffset = new Vector3(0, 15, 17);
            targetOffset = new Vector3(0, 5, 0);

            cameraType = CameraType.ChaseCamera;

            position = headOffset;
            reference = 0;
            turnAmount = Vector3.Zero;
            cameraUpVector = Vector3.Up;
            m_AngleX = 0;
            m_AngleY = 0;
        }

        public void Update(float yRotation, Vector3 position, float aspectRatio)
        {
            Matrix yRotationMatrix = Matrix.CreateRotationY(yRotation);

            Vector3 transformedHeadOffset = Vector3.Transform(headOffset, yRotationMatrix);

            Vector3 transformedReference = Vector3.Transform(targetOffset, yRotationMatrix);

            Vector3 cameraPosition = position + transformedHeadOffset;
            Vector3 cameraTarget = position + transformedReference;

            this.position = position;
            reference = yRotation;

            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(PlatformParameters.FOV), aspectRatio, PlatformParameters.NEAR, PlatformParameters.FAR);
        }
    }
}
