using System;
using KSP.Localization;
using UnityEngine;
using BDArmory.Parts;

namespace AntiSubmarineWeapon
{
    public class ModuleAntiSubmarineWeapon : PartModule
    {
        [KSPField(isPersistant = false)]
        public string type = "";

        [KSPField(isPersistant = false)]
        public float maxDepth;

        [KSPField(isPersistant = false)]
        public int errorRange;

        [KSPField(isPersistant = false)]
        public bool volumebuoyancy;

        [KSPField(isPersistant = false)]
        public float volum;

        [KSPField(isPersistant = false)]
        public float buoyancyForce;

        [KSPField(isPersistant = false)]
        public Vector3 airMaxTorque = Vector3.zero;

        [KSPField(isPersistant = false)]
        public Vector3 waterMaxTorque = Vector3.zero;

        [KSPField(isPersistant = false)]
        public Vector3 airCocf = Vector3.zero;

        [KSPField(isPersistant = false)]
        public Vector3 waterCocf = Vector3.zero;

        [KSPField(isPersistant = false)]
        public bool autoReload;

        [KSPField(isPersistant = false)]
        public float reloadTime;

        [KSPField(isPersistant = false)]
        public float ejectForce;

        [KSPField(isPersistant = false)]
        public bool zeroThrustSink = true;

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
        public float accuracy;

        [KSPField(isPersistant = false)]
        public float lineLength;

        [KSPField(isPersistant = false)]
        public string sonoraType = "";

        [KSPField(isPersistant = false)]
        public Vector2 sonoraPerformance = Vector2.zero;

        [KSPField(isPersistant = false)]
        public Vector2 referenceNoise = Vector2.zero;

        [KSPField(isPersistant = false)]
        public Vector2 sonoraMaxRecognition = Vector2.zero;

        [KSPField(isPersistant = false)]
        public float recognitionAccuracy;

        private AswData _data;
        private BombSystem _bomb;
        private Buoyancy _buoyancy;
        private RotationSystem _rota;
        private PidSystem _pida ;
        private PidSystem _pidt;

        private UI_Control _depthField;
        private MissileLauncher _missileLauncher;
        private Rigidbody _partRigidbodyValue;
        
        private Rigidbody PartRigidbody
        {
            get
            {
                if (!_partRigidbodyValue)
                    _partRigidbodyValue = part.GetComponent<Rigidbody>();
                return _partRigidbodyValue;
            }
        }
        
        private float Height => (float)vessel.mainBody.GetAltitude(vessel.GetWorldPos3D());

        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false, guiName = "Destroy Depth"),
         UI_FloatRange(controlEnabled = true, maxValue = 500, minValue = 25, scene = UI_Scene.All, stepIncrement = 1f)]
        public float autoDestroyDepth = 1.0f;

        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false, guiName = "Cruise Depth"),
         UI_FloatRange(controlEnabled = true, maxValue = 200, minValue = 1, scene = UI_Scene.All, stepIncrement = 1f)]
        public float cruiseDepth = 1.0f;

        public override void OnStart(StartState state)
        {
            base.OnStart(state);

            _data = new AswData {Type = type};

            switch (_data.Type)
            {
                case "DepthCharge":
                    _bomb = new BombSystem();
                    _bomb.ChargeDataWrite(errorRange,maxDepth,maxDepth);
                    _bomb.Ran();

                    _buoyancy = new Buoyancy();
                    _buoyancy.BuoyancyDataWrite(volumebuoyancy,volum,buoyancyForce);

                    _rota = new RotationSystem();
                    _rota.RotationData(airMaxTorque, waterMaxTorque, airCocf, waterCocf);

                    _missileLauncher = GetComponent<MissileLauncher>();
                    if(_missileLauncher == null)
                        Debug.LogError("Failed when find the MissileLauncher!");
                
                    _depthField = Fields["autoDestroyDepth"].uiControlFlight;

                    Fields["cruiseDepth"].guiActive = false;
                    Fields["cruiseDepth"].guiActiveEditor = false;
                    Fields["autoDestroyDepth"].guiActive = true;
                    Fields["autoDestroyDepth"].guiActiveEditor = true;
                    Fields["autoDestroyDepth"].guiName = Localizer.Format("#autoLOC_NAS_Editor_detonateDepth");

                    ((UI_FloatRange)Fields["autoDestroyDepth"].uiControlEditor).maxValue = maxDepth;
                    ((UI_FloatRange)Fields["autoDestroyDepth"].uiControlFlight).maxValue = maxDepth;
                    break;
                case "Torpedo":
                    _pida = new PidSystem();
                    _pida.PidDataWrite(maxRotate.z, pidAngleInfo.x, pidAngleInfo.y, pidAngleInfo.z);
                    _pidt = new PidSystem();
                    _pidt.PidDataWrite(maxTorque.x, pidTorqueInfo.x, pidTorqueInfo.y, pidTorqueInfo.z);

                    _buoyancy = new Buoyancy();
                    _buoyancy.BuoyancyDataWrite(volumebuoyancy, volum, buoyancyForce);

                    _rota = new RotationSystem();
                    _rota.RotationData(airMaxTorque, waterMaxTorque, airCocf, waterCocf);

                    _missileLauncher = GetComponent<MissileLauncher>();
                    if (_missileLauncher == null)
                        Debug.LogError("Failed when find the MissileLauncher!");

                    Fields["autoDestroyDepth"].guiActive = false;
                    Fields["autoDestroyDepth"].guiActiveEditor = false;
                    Fields["cruiseDepth"].guiActive = true;
                    Fields["cruiseDepth"].guiActiveEditor = true;
                    Fields["cruiseDepth"].guiName = Localizer.Format("#autoLOC_NAS_Editor_cruiseDepth");

                    ((UI_FloatRange)Fields["cruiseDepth"].uiControlEditor).maxValue = maxDepth - 2;
                    ((UI_FloatRange)Fields["cruiseDepth"].uiControlFlight).maxValue = maxDepth - 2;
                    break;
                default:
                    Debug.LogWarning("[NAS-ASW]Neither depth charge nor torpedo!");
                    break;
            }

            autoDestroyDepth = ModUiManager.setAllDepthCharges;
            cruiseDepth = ModUiManager.setAllTorpedos;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            if (_data.Type != "DepthCharge") return;
            if (!HighLogic.LoadedSceneIsFlight) return;
            if (!_missileLauncher.HasFired) return;
            
            if (_depthField.controlEnabled)
                _depthField.controlEnabled = false;
            
            if (!_bomb.Bomb(Height, autoDestroyDepth)) return;
            
            Debug.Log("DepthCharge Bombed At " + Height + "m on " + vessel.mainBody.name);
            _missileLauncher.Detonate();
            part.temperature = part.maxTemp * 2;
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            switch (_data.Type)
            {
                case "DepthCharge":
                    if (HighLogic.LoadedSceneIsFlight)
                    {
                        if (_missileLauncher.HasFired)
                        {
                            if (Height<0)
                                PartRigidbody.AddForce(FlightGlobals.getUpAxis() * _buoyancy.Force(Height).y);
                            PartRigidbody.AddTorque(_rota.Torque(vessel.vesselTransform.forward, vessel.srf_velocity, Height));
                        }
                    }
                    break;
                case "Torpedo":
                    if (vessel.mainBody.GetAltitude(vessel.GetWorldPos3D()) < 0 &&
                        vessel.mainBody.GetAltitude(vessel.GetWorldPos3D()) > -Math.Abs(maxDepth))
                    {
                        var angle = _pida.PidControl(-cruiseDepth, (float) vessel.mainBody.GetAltitude(vessel.GetWorldPos3D()), TimeWarp.fixedDeltaTime);
                        var torque = _pidt.PidControl(angle, 90 - Vector3.Angle(part.transform.forward, FlightGlobals.getUpAxis(part.transform.position)), TimeWarp.fixedDeltaTime);
                        var horizonPitchAxis = Vector3.Cross(FlightGlobals.getUpAxis(transform.position), transform.forward).normalized;
                        PartRigidbody.AddTorque(_rota.Torque(vessel.vesselTransform.forward, vessel.srf_velocity, Height));
                        PartRigidbody.AddTorque(-horizonPitchAxis * torque);
                        PartRigidbody.AddForce(FlightGlobals.getUpAxis() * _buoyancy.Force(Height).y);
                    }
                    break;
                default:
                    Debug.LogWarning("[NAS-ASW]Neither depth charge nor torpedo!");
                    break;
            }
        }
    }
}
