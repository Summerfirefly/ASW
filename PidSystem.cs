using UnityEngine;

namespace AntiSubmarineWeapon
{
    public class PidData
    {
        public float Lasterror { get; set; }

        public float Integral { get; set; }

        public float Derivative { get; set; }

        public float Error { get; set; }

        public float MaxOut { get; set; }

        public float K { get; set; }

        public float KIntegral { get; set; }

        public float KDerivative { get; set; }
    }
    
    public class PidSystem
    {
        private readonly PidData _data = new PidData();
        public float PidControl(float target, float present, float deltaTime)
        {
            _data.Error = target - present;
            _data.Integral = _data.Integral + _data.Error * deltaTime;
            _data.Derivative = (_data.Error - _data.Lasterror) / deltaTime;
            var outPut = _data.K * _data.Error + _data.KIntegral * _data.Integral + _data.KDerivative * _data.Derivative;
            _data.Lasterror = _data.Error;
            return Mathf.Clamp(outPut, -_data.MaxOut, _data.MaxOut);
        }
        public void PidDataWrite(float maxOut, float k, float kIntergral, float kDerivative)
        {
            _data.MaxOut = maxOut;
            _data.K = k;
            _data.KIntegral = kIntergral;
            _data.KDerivative = kDerivative;
        }
    }
}

