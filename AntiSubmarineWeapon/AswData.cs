using System;
using UnityEngine;

namespace AntiSubmarineWeapon
{
    public class AswData
    {
        private string _type;
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (value == "Torpedo" || value == "DepthCharge" || value == "DepthChargeProjector" ||
                    value == "Sonora")
                {
                    _type = value;
                }
            }
        }

        private float _maxDetonateDepth;
        public float MaxDetonateDepth
        {
            get
            {
                return _maxDetonateDepth;
            }
            set
            {
                if (value >= 0)
                    _maxDetonateDepth = value;
            }
        }

        private float _maxDepth;
        public float MaxDepth
        {
            get { return _maxDepth; }
            set
            {
                if (value >= 0)
                    _maxDepth = value;
            }
        }

        private int _errorRange;
        public int ErrorRange
        {
            get { return _errorRange; }
            set { _errorRange = value >= 0 ? value : 5; }
        }

        public bool VolumeBuoyancy { get; set; }

        private float _volume;
        public float Volume
        {
            get { return _volume; }
            set
            {
                if (value >= 0 || value == -1)
                    _volume = value;
            }
        }

        private float _buoyancyForce;
        public float BuoyancyForce
        {
            get { return _buoyancyForce; }
            set
            {
                if (value >= 0 || value == -1)
                    _buoyancyForce = value;
            }
        }

        public Vector3 AirMaxTorque { get; set; }

        public Vector3 WaterMaxTorque { get; set; }

        public Vector3 AirCocf { get; set; }

        public Vector3 WaterCocf { get; set; }

        public bool AutoReload { get; set; }

        private float _reloadTime;
        public float ReloadTime
        {
            get { return _reloadTime; }
            set
            {
                _reloadTime = Math.Abs(value);
            }
        }

        private float _ejectForce;
        public float Ejectforce
        {
            get { return _ejectForce; }
            set
            {
                _ejectForce = Math.Abs(value);
            }
        }

        public bool ZeroThrustSink { get; set; }

        private float _cruiseDepth;
        public float CruiseDepth
        {
            get { return _cruiseDepth; }
            set
            {
                _cruiseDepth = -Math.Abs(value);
            }
        }

        private string _guideType;
        public string GuideType
        {
            get { return _guideType; }
            set
            {
                if (value == "ActiveL" || value == "PassiveL" || value == "Inertial")
                    _guideType = value;
                else
                    _guideType = "Inertial";
            }
        }

        public Vector3 MaxTorque { get; set; }

        public Vector3 MaxRotate { get; set; }

        public Vector3 PidAngleInfo { get; set; }

        public Vector3 PidTorqueInfo { get; set; }

        private float _accuracy;
        public float Accuracy
        {
            get { return _accuracy; }
            set
            {
                _accuracy = Math.Abs(value);
            }
        }

        private float _agor;
        public float Agor
        {
            get { return _agor; }
            set
            {
                _agor = Math.Abs(value);
            }
        }

        private float _ador;
        public float Ador
        {
            get { return _ador; }
            set
            {
                _ador = Math.Abs(value);
            }
        }

        public Vector3 VelocityFeature { get; set; }

        private string _sonoraType;
        public string SonoraType
        {
            get { return _sonoraType; }
            set
            {
                if (value == "CS" || value == "FS" || value == "TS")
                    _sonoraType = value;
            }
        }

        private float _passivePerformance;
        public float PassivePerformance
        {
            get { return _passivePerformance; }
            set
            {
                _passivePerformance = Math.Abs(value);
            }
        }

        private float _activePerformance;
        public float ActivePerformance
        {
            get { return _activePerformance; }
            set
            {
                _activePerformance = Math.Abs(value);
            }
        }

        private float _referenceNoise;
        public float ReferenceNoise
        {
            get { return _referenceNoise; }
            set
            {
                _referenceNoise = Math.Abs(value);
            }
        }

        private float _pMaximumRecognition;
        public float PMaximumRecognition
        {
            get { return _pMaximumRecognition; }
            set
            {
                _pMaximumRecognition = Math.Abs(value);
            }
        }

        private float _aMaximumRecognition;
        public float AMaximumRecognition
        {
            get { return _aMaximumRecognition; }
            set
            {
                _aMaximumRecognition = Math.Abs(value);
            }
        }

        private float _recognitionAccuracy;
        public float RecognitionAccuracy
        {
            get { return _recognitionAccuracy; }
            set
            {
                _recognitionAccuracy = Math.Abs(value);
            }
        }
    }
}
