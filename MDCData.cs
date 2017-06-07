using System;
using UnityEngine;

namespace AntiSubmarineWeapon
{
    public class ASWData
    {
        private string Type;
        public string type
        {
            get { return Type; }
            set
            {
                if (value == "Torpedo" ||value ==  "DepthCharge" ||  value == "DepthChargeProjector" || value == "Sonora")
                {
                    Type = value;
                }
            }
        }

        private float MaxDetonateDepth;
        public float maxDetonateDepth
        {
            get { return MaxDetonateDepth; }
            set
            {
                if (value >= 0)
                    MaxDetonateDepth = value;
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private float MaxDepth;
        public float maxDepth
        {
            get { return MaxDepth; }
            set
            {
                if (value >= 0)
                    MaxDepth = value;
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private int ErrorRange;
        public int errorRange
        {
            get { return ErrorRange; }
            set
            {
                if (value >= 0)
                    ErrorRange = (int)value;
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
                else
                    ErrorRange = 5;
            }
        }

        private bool VolumeBuoyancy;
        public bool volumeBuoyancy
        {
            get { return VolumeBuoyancy; }
            set { VolumeBuoyancy = value; }
        }

        private float Volume;
        public float volume
        {
            get { return Volume; }
            set
            {
                if (value >= 0 || value == -1)
                    Volume = value;
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private float BuoyancyForce;
        public float buoyancyForce
        {
            get { return BuoyancyForce; }
            set
            {
                if (value >= 0 || value == -1)
                    BuoyancyForce = value;
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private Vector3 AirMaxTorque;
        public Vector3 airMaxTorque
        {
            get { return AirMaxTorque; }
            set
            {
                AirMaxTorque = value;
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private Vector3 WaterMaxTorque;
        public Vector3 waterMaxTorque
        {
            get { return WaterMaxTorque; }
            set
            {
                WaterMaxTorque = value;
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private Vector3 AirCocf;
        public Vector3 airCocf
        {
            get { return AirCocf; }
            set
            {
                AirCocf = value;
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private Vector3 WaterCocf;
        public Vector3 waterCocf
        {
            get { return WaterCocf; }
            set
            {
                WaterCocf = value;
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        //private string ProjectorType;
        //public string projectorType
        //{
        //    get { return ProjectorType; }
        //    set
        //    {
        //        if (value == "KGun" || value == "YGun")
        //        {
        //            ProjectorType = value;
        //        }
        //        /*else
        //        {
        //            ErrorDeal ErrorObj = new ErrorDeal();
        //            ErrorObj.errordeal("AutoDestroyDepth is not true or none");
        //        }
        //        */
        //    }
        //}

        private bool AutoReload;
        public bool autoReload
        {
            get { return AutoReload; }
            set
            {
                AutoReload = value;
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private float ReloadTime;
        public float reloadTime
        {
            get { return ReloadTime; }
            set
            {
                ReloadTime = Math.Abs(value);
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private float EjectForce;
        public float ejectforce
        {
            get { return EjectForce; }
            set
            {
                EjectForce = Math.Abs(value);
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private bool ZeroThrustSink;
        public bool zeroThrustSink
        {
            get { return ZeroThrustSink; }
            set
            {
                ZeroThrustSink = value;
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private float CruiseDepth;
        public float cruiseDepth
        {
            get { return CruiseDepth; }
            set
            {
                CruiseDepth = -Math.Abs(value);
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private string GuideType;
        public string guideType
        {
            get { return GuideType; }
            set
            {
                if (value == "ActiveL" || value == "PassiveL" || value == "Inertial")
                    GuideType = value;
                else
                    GuideType = "Inertial";
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private Vector3 MaxTorque;
        public Vector3 maxTorque
        {
            get { return MaxTorque; }
            set
            {
                MaxTorque = value;
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private Vector3 MaxRotate;
        public Vector3 maxRotate
        {
            get { return MaxRotate; }
            set
            {
                MaxRotate = value;
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private Vector3 PidAngleInfo;
        public Vector3 pidAngleInfo
        {
            get { return PidAngleInfo; }
            set
            {
                PidAngleInfo = value;
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private Vector3 PidTorqueInfo;
        public Vector3 pidTorqueInfo
        {
            get { return PidTorqueInfo; }
            set
            {
                PidTorqueInfo = value;
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private float Accuracy;
        public float accuracy
        {
            get { return Accuracy; }
            set
            {
                Accuracy = Math.Abs(value);
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private float Agor;
        public float agor
        {
            get { return Agor; }
            set
            {
                Agor = Math.Abs(value);
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private float Ador;
        public float ador
        {
            get { return Ador; }
            set
            {
                Ador = Math.Abs(value);
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private Vector3 VelocityFeature;
        public Vector3 velocityFeature
        {
            get { return VelocityFeature; }
            set
            {
                VelocityFeature = value;
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private string SonoraType;
        public string sonoraType
        {
            get { return SonoraType; }
            set
            {
                if (value == "CS" || value == "FS" || value == "TS")
                    SonoraType = value;
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private float PassivePerformance;
        public float passivePerformance
        {
            get { return PassivePerformance; }
            set
            {
                PassivePerformance = Math.Abs(value);
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private float ActivePerformance;
        public float activePerformance
        {
            get { return ActivePerformance; }
            set
            {
                ActivePerformance = Math.Abs(value);
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private float ReferenceNoise;
        public float referenceNoise
        {
            get { return ReferenceNoise; }
            set
            {
                ReferenceNoise = Math.Abs(value);
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private float PMaximumRecognition;
        public float pMaximumRecognition
        {
            get { return PMaximumRecognition; }
            set
            {
                PMaximumRecognition = Math.Abs(value);
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private float AMaximumRecognition;
        public float aMaximumRecognition
        {
            get { return AMaximumRecognition; }
            set
            {
                AMaximumRecognition = Math.Abs(value);
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }

        private float RecognitionAccuracy;
        public float recognitionAccuracy
        {
            get { return RecognitionAccuracy; }
            set
            {
                RecognitionAccuracy = Math.Abs(value);
                /*else
                {
                    ErrorDeal ErrorObj = new ErrorDeal();
                    ErrorObj.errordeal("AutoDestroyDepth is not true or none");
                }
                */
            }
        }
    }
}
