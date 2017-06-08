using System;
using UnityEngine;

namespace AntiSubmarineWeapon
{
    public class PIDData
    {
        private float Lasterror;
        public float lasterror
        {
            get { return Lasterror; }
            set { Lasterror = value; }
        }

        private float Integral;
        public float integral
        {
            get { return Integral; }
            set { Integral = value; }
        }

        private float Derivative;
        public float derivative
        {
            get { return Derivative; }
            set { Derivative = value; }
        }

        private float Error;
        public float error
        {
            get { return Error; }
            set { Error = value; }
        }

        private float MaxOut;
        public float maxOut
        {
            get { return MaxOut; }
            set { MaxOut = value; }
        }

        private float K;
        public float k
        {
            get { return K; }
            set { K = value; }
        }

        private float Kintegral;
        public float kIntegral
        {
            get { return Kintegral; }
            set { Kintegral = value; }
        }

        private float KDerivative;
        public float kDerivative
        {
            get { return KDerivative; }
            set { KDerivative = value; }
        }
    }
    public class PIDSystem
    {
        PIDData Data = new PIDData();
        public float PIDControl(float target, float present, float deltaTime)
        {
            Data.error = target - present;
            Data.integral = Data.integral + Data.error * deltaTime;
            Data.derivative = (Data.error - Data.lasterror) / deltaTime;
            float outPut = Data.k * Data.error + Data.kIntegral * Data.integral + Data.kDerivative * Data.derivative;
            Data.lasterror = Data.error;
            return Mathf.Clamp(outPut, -Data.maxOut, Data.maxOut);
        }
        public void PIDDataWrite(float maxOut, float k, float kIntergral, float kDerivative)
        {
            Data.maxOut = maxOut;
            Data.k = k;
            Data.kIntegral = kIntergral;
            Data.kDerivative = kDerivative;
        }
    }
}

