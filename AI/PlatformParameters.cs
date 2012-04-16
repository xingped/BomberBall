using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AI.ANN;
namespace Platform
{
    public static class PlatformParameters
    {
        public static Params m_Params;
        public const float NEAR = 1.0f;
        public const float FAR = 1000.0f;
        public const float FOV = 45.0f;

        public const float MAX_SPEED = 0.75f;
        public const float MAX_TURN_SPEED = 0.025f;
        public const float DEFAULT_TURN_SPEED = 0.025f;
    }
}
