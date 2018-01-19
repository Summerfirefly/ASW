using System;
using UnityEngine;

namespace AntiSubmarineWeapon
{
    public class SonarSystem
    {
        private readonly System.Random _ran = new System.Random();
        private Vector2 _position;
        private Vector2 _screen;
        private static readonly float[] R = new float[3];
        private readonly Vector2[] _originalRound = new Vector2[360];
        private static readonly Vector2[,] DetectionCirclevertex = new Vector2[3, 360];
        
        public void Rposition(Vector2 px,Vector2 screen)
        {
            R[0] = 0.125f;
            R[1] = 0.375f;
            R[2] = 0.625f;
            _screen = screen;
            _position = px;
        }
        public void Round()
        {
            for (var i = 0; i < 360; i++)
            {
                _originalRound[i] = new Vector2((float)Math.Cos((2*Math.PI/360)*i),(float)Math.Sin((2*Math.PI/360)*i));
            }
        }
        public Vector2[,] DrawOuterCircle()
        {
            for (var i = 0; i < 3; i++)
            {
                for (var k = 0; k < 360; k++)
                {
                    DetectionCirclevertex[i, k] = ((200 * R[i] + Ran(i)) / (200 * R[i])) * 200f * R[i] * (_originalRound[k]) + _position;
                }
            }
            return DetectionCirclevertex;
        }
        private short Ran(int type)
        {
            switch (type)
            {
                case 0:
                    return (short)_ran.Next(-3, 3);
                case 1:
                    return (short)_ran.Next(-4, 4);
                case 2:
                    return (short)_ran.Next(-5, 5);
                default:
                    return 0;
            }
        }
    }
}
