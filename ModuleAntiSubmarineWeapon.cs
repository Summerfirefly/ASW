using System;
using KSP.Localization;
using UnityEngine;
using BahaTurret;
using AntiSubmarineWeapon;

namespace AntiSubmarineWeapon
{
    class ModuleAntiSubmarineWeapon : PartModule
    {
        [KSPField(isPersistant = false)]
        public string type = "";

        //[KSPField]
        //public float maxDetonateDepth = 0f;

        [KSPField(isPersistant = false)]
        public float maxDepth = 0f;

        [KSPField(isPersistant = false)]
        public int errorRange = 0;

        [KSPField(isPersistant = false)]
        public bool volumebuoyancy = false;

        [KSPField(isPersistant = false)]
        public float volum = 0f;

        [KSPField(isPersistant = false)]
        public float buoyancyForce = 0f;

        [KSPField(isPersistant = false)]
        public Vector3 airMaxTorque = Vector3.zero;

        [KSPField(isPersistant = false)]
        public Vector3 waterMaxTorque = Vector3.zero;

        [KSPField(isPersistant = false)]
        public Vector3 airCocf = Vector3.zero;

        [KSPField(isPersistant = false)]
        public Vector3 waterCocf = Vector3.zero;

        [KSPField(isPersistant = false)]
        public bool autoReload = false;

        [KSPField(isPersistant = false)]
        public float reloadTime = 0f;

        [KSPField(isPersistant = false)]
        public float ejectForce = 0f;

        [KSPField(isPersistant = false)]
        public bool zeroThrustSink = true;

        //[KSPField]
        //public float curiseDepth = 0f;

        [KSPField(isPersistant = false)]
        public string guideType = "Inertial";

        [KSPField(isPersistant = false)]
        public Vector3 maxRotate = Vector3.zero;

        [KSPField(isPersistant = false)]
        public Vector3 maxTorque = Vector3.zero;

        [KSPField(isPersistant = false)]
        public Vector3 pidAngleInfo = Vector3.zero;

        [KSPField(isPersistant = false)]
        public Vector3 pidTorqueInfo = Vector3.zero;

        [KSPField(isPersistant = false)]
        public float accuracy = 0f;

        [KSPField(isPersistant = false)]
        public float lineLength = 0f;

        //[KSPField]
        //public float agor = 0f;

        //[KSPField]
        //public float aror = 0f;

        [KSPField(isPersistant = false)]
        public string sonoraType = "";

        [KSPField(isPersistant = false)]
        public Vector2 sonoraPerformance = Vector2.zero;

        [KSPField(isPersistant = false)]
        public Vector2 referenceNoise = Vector2.zero;

        [KSPField(isPersistant = false)]
        public Vector2 sonoraMaxRecognition = Vector2.zero;

        [KSPField(isPersistant = false)]
        public float recognitionAccuracy = 0f;

        private ASWData Data;
        private BombSystem Bomb;
        private Buoyancy buoyancy;
        private RotationSystem Rota;
        private PIDSystem PIDA ;
        private PIDSystem PIDT;


        //private bool isDepthCharge = false;
        private UI_Control depthField,depth;
        private MissileLauncher missileLauncher;
        private Rigidbody partRigidbodyValue;
        private Rigidbody partRigidbody
        {
            get
            {
                if (!partRigidbodyValue)
                    partRigidbodyValue = part.GetComponent<Rigidbody>();
                return partRigidbodyValue;
            }
        }
        float height
        {
            get
            {
                return (float)vessel.mainBody.GetAltitude(vessel.GetWorldPos3D());
            }
        }

        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false, guiName = "Destroy Depth"), UI_FloatRange(controlEnabled = true, maxValue = 500, minValue = 25, scene = UI_Scene.All, stepIncrement = 1f)]
        public float autoDestroyDepth = 1.0f;

        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false, guiName = "Cruise Depth"), UI_FloatRange(controlEnabled = true, maxValue = 200, minValue = 1, scene = UI_Scene.All, stepIncrement = 1f)]
        public float cruiseDepth = 1.0f;

        //private bool IsDepthCharge()
        //{
        //    if(Data.type == "DepthCharge")
        //        return true;
        //    else
        //        return false;

        //}
        public override void OnStart(StartState state)
        {
            base.OnStart(state);

            Data = new ASWData();
            Data.type = type;

            if (Data.type == "DepthCharge")
            {
                Bomb = new BombSystem();
                Bomb.ChargeDataWrite(errorRange,maxDepth,maxDepth);
                Bomb.ran();

                buoyancy = new Buoyancy();
                buoyancy.BuoyancyDataWrite(volumebuoyancy,volum,buoyancyForce);

                Rota = new RotationSystem();
                Rota.RotationData(airMaxTorque, waterMaxTorque, airCocf, waterCocf);

                missileLauncher = GetComponent<MissileLauncher>();
                if(missileLauncher == null)
                    Debug.LogError("Failed when find the MissileLauncher!");
                
                depthField = Fields["autoDestroyDepth"].uiControlFlight;

                Fields["cruiseDepth"].guiActive = false;
                Fields["cruiseDepth"].guiActiveEditor = false;
                Fields["autoDestroyDepth"].guiActive = true;
                Fields["autoDestroyDepth"].guiActiveEditor = true;
                Fields["autoDestroyDepth"].guiName = Localizer.Format("#autoLOC_NAS_Editor_detonateDepth");

                ((UI_FloatRange)Fields["autoDestroyDepth"].uiControlEditor).maxValue = this.maxDepth;
                ((UI_FloatRange)Fields["autoDestroyDepth"].uiControlFlight).maxValue = this.maxDepth;
            }
            else if (Data.type == "Torpedo")
            {
                PIDA = new PIDSystem();
                PIDA.PIDDataWrite(maxRotate.z, pidAngleInfo.x, pidAngleInfo.y, pidAngleInfo.z);
                PIDT = new PIDSystem();
                PIDT.PIDDataWrite(maxTorque.x, pidTorqueInfo.x, pidTorqueInfo.y, pidTorqueInfo.z);

                buoyancy = new Buoyancy();
                buoyancy.BuoyancyDataWrite(volumebuoyancy, volum, buoyancyForce);

                Rota = new RotationSystem();
                Rota.RotationData(airMaxTorque, waterMaxTorque, airCocf, waterCocf);

                missileLauncher = GetComponent<MissileLauncher>();
                if (missileLauncher == null)
                    Debug.LogError("Failed when find the MissileLauncher!");

                Fields["autoDestroyDepth"].guiActive = false;
                Fields["autoDestroyDepth"].guiActiveEditor = false;
                Fields["cruiseDepth"].guiActive = true;
                Fields["cruiseDepth"].guiActiveEditor = true;
                Fields["cruiseDepth"].guiName = Localizer.Format("#autoLOC_NAS_Editor_cruiseDepth");

                ((UI_FloatRange)Fields["cruiseDepth"].uiControlEditor).maxValue = this.maxDepth - 2;
                ((UI_FloatRange)Fields["cruiseDepth"].uiControlFlight).maxValue = this.maxDepth - 2;
            }

            this.autoDestroyDepth = ModUIManager.setAllDepthCharges;
            this.cruiseDepth = ModUIManager.setAllTorpedos;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Data.type == "DepthCharge")
            {
                if (HighLogic.LoadedSceneIsFlight)
                {
                    if (missileLauncher.HasFired)
                    {
                        if (depthField.controlEnabled)
                            depthField.controlEnabled = false;
                        if (Bomb.Bomb(height, autoDestroyDepth))
                        {
                            Debug.Log("DepthCharge Bombed At " + height + "m on " + vessel.mainBody.name);
                            missileLauncher.Detonate();
                            part.temperature = part.maxTemp * 2;
                        }
                    }
                }
                //else
                //{
                //    Bomb.ChargeDataWrite(autoDestroyDepth);
                //}
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (Data.type == "DepthCharge")
            {
                if (HighLogic.LoadedSceneIsFlight)
                {
                    if (missileLauncher.HasFired)
                    {
                        if (height<0)
                            partRigidbody.AddForce(FlightGlobals.getUpAxis() * buoyancy.Force(height).y);
                        partRigidbody.AddTorque(Rota.Torque(vessel.vesselTransform.forward, vessel.srf_velocity, height));
                    }
                }
            }
            else if (Data.type == "Torpedo")
            {
                if ((vessel.mainBody.GetAltitude(vessel.GetWorldPos3D())) < 0 && (vessel.mainBody.GetAltitude(vessel.GetWorldPos3D()))>-Math.Abs( maxDepth))
                {
                    float angle = PIDA.PIDControl(-cruiseDepth, (float)(vessel.mainBody.GetAltitude(vessel.GetWorldPos3D())), TimeWarp.fixedDeltaTime);
                    //partRigidbody.angularVelocity = new Vector3(0, PID2.PIDControl(0.785f, k, kI, kD, angle, (float)(((Vector3.Angle(transform.right, vessel.transform.position)) - 45) * (Math.PI / 180)), TimeWarp.fixedDeltaTime), 0);
                    float Torque = PIDT.PIDControl(angle, (90 - Vector3.Angle(this.part.transform.forward, FlightGlobals.getUpAxis(this.part.transform.position))), TimeWarp.fixedDeltaTime);
                    Vector3 horizonPitchAxis = Vector3.Cross(FlightGlobals.getUpAxis(transform.position), transform.forward).normalized;
                    partRigidbody.AddTorque(Rota.Torque(vessel.vesselTransform.forward, vessel.srf_velocity, height));
                    partRigidbody.AddTorque(-horizonPitchAxis * Torque);
                    //partRigidbody.AddTorque(Rota.Torque(vessel.vesselTransform.forward, vessel.srf_velocity, height));
                    partRigidbody.AddForce(FlightGlobals.getUpAxis() * buoyancy.Force(height).y);
                }
            }
        }
    }
}
