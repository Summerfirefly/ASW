using System;
using UnityEngine;

namespace Sonar
{
    public class SonarSystem
    {
        System.Random Ran = new System.Random();
        private Vector2 Position;
        private Vector2 Screen;
        static float[] r = new float[3];
        private Vector2[] originalRound = new Vector2[360];
        static Vector2[,] detectionCirclevertex = new Vector2[3, 360];
        public void Rposition(Vector2 px,Vector2 screen)
        {
            r[0] = 0.125f;
            r[1] = 0.375f;
            r[2] = 0.625f;
            this.Screen = screen;
            Position = px;
        }
        public void Round()
        {
            for (int i = 0; i < 360; i++)
            {
                originalRound[i] = new Vector2((float)Math.Cos((2*Math.PI/360)*i),(float)Math.Sin((2*Math.PI/360)*i));
            }
        }
        public Vector2[,] DrawOuterCircle()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int k = 0; k < 360; k++)
                {
                    detectionCirclevertex[i, k] = ((200 * r[i] + ran(i)) / (200 * r[i])) * 200f * r[i] * (originalRound[k]) + Position;
                }
            }
            return detectionCirclevertex;
        }
        private short ran(int Type)
        {
            if (Type == 0)
                return (short)Ran.Next(-3, 3);
            if (Type == 1)
                return (short)Ran.Next(-4, 4);
            if (Type == 2)
                return (short)Ran.Next(-5, 5);
            else
                return 0;
        }
    }
}
