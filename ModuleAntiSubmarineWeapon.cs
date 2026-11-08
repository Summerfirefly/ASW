using System;
using System.Collections.Generic;
using KSP.Localization;
using UnityEngine;
using BDArmory.Weapons;
using BDArmory.Weapons.Missiles;

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

        // NAS part configs spell this key "volume"; accept both spellings.
        [KSPField(isPersistant = false)]
        public float volume = -1;

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

        private const float MinDetonationDepth = 25f; // matches the UI_FloatRange minimum below
        private const float MinCruiseDepth = 1f;

        private AswData _data;
        private BombSystem _bomb;
        private Buoyancy _buoyancy;
        private RotationSystem _rota;
        private PidSystem _pida ;
        private PidSystem _pidt;

        private UI_Control _depthField;
        private MissileLauncher _missileLauncher;
        private Rigidbody _partRigidbodyValue;
        private bool _waitingForArmingLogged;
        private bool _cfgIsTimed;
        private bool _missCancelLogged;
        private List<BDWarheadBase> _warheads;

        private Rigidbody PartRigidbody
        {
            get
            {
                if (!_partRigidbodyValue)
                    _partRigidbodyValue = part.rb != null ? part.rb : part.GetComponent<Rigidbody>();
                return _partRigidbodyValue;
            }
        }

        // Altitude of this part (not the vessel root), so it is also correct before the weapon separates.
        private float Height => (float)vessel.mainBody.GetAltitude(part.transform.position);

        private Vector3 UpAxis => FlightGlobals.getUpAxis(vessel.mainBody, part.transform.position);

        private float VolumeValue => volum != 0 ? volum : volume;

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
                    _buoyancy.BuoyancyDataWrite(volumebuoyancy,VolumeValue,buoyancyForce);

                    _rota = new RotationSystem();
                    _rota.RotationData(airMaxTorque, waterMaxTorque, airCocf, waterCocf);

                    _missileLauncher = part.FindModuleImplementing<MissileLauncher>();
                    if(_missileLauncher == null)
                        Debug.LogError("[NAS-ASW] Failed to find the MissileLauncher on " + part.partInfo?.name);
                    else
                        _cfgIsTimed = _missileLauncher.isTimed;

                    _depthField = Fields["autoDestroyDepth"].uiControlFlight;

                    Fields["cruiseDepth"].guiActive = false;
                    Fields["cruiseDepth"].guiActiveEditor = false;
                    Fields["autoDestroyDepth"].guiActive = true;
                    Fields["autoDestroyDepth"].guiActiveEditor = true;
                    Fields["autoDestroyDepth"].guiName = Localizer.Format("#autoLOC_NAS_Editor_detonateDepth");

                    ((UI_FloatRange)Fields["autoDestroyDepth"].uiControlEditor).maxValue = maxDepth;
                    ((UI_FloatRange)Fields["autoDestroyDepth"].uiControlFlight).maxValue = maxDepth;

                    // Keep the per-part depth from the PAW/craft file; only clamp it into the valid range.
                    autoDestroyDepth = Mathf.Clamp(autoDestroyDepth, Mathf.Min(MinDetonationDepth, maxDepth), maxDepth);
                    break;
                case "Torpedo":
                    _pida = new PidSystem();
                    _pida.PidDataWrite(maxRotate.z, pidAngleInfo.x, pidAngleInfo.y, pidAngleInfo.z);
                    _pidt = new PidSystem();
                    _pidt.PidDataWrite(maxTorque.x, pidTorqueInfo.x, pidTorqueInfo.y, pidTorqueInfo.z);

                    _buoyancy = new Buoyancy();
                    _buoyancy.BuoyancyDataWrite(volumebuoyancy, VolumeValue, buoyancyForce);

                    _rota = new RotationSystem();
                    _rota.RotationData(airMaxTorque, waterMaxTorque, airCocf, waterCocf);

                    _missileLauncher = part.FindModuleImplementing<MissileLauncher>();
                    if (_missileLauncher == null)
                        Debug.LogError("[NAS-ASW] Failed to find the MissileLauncher on " + part.partInfo?.name);
                    else
                        _cfgIsTimed = _missileLauncher.isTimed;

                    Fields["autoDestroyDepth"].guiActive = false;
                    Fields["autoDestroyDepth"].guiActiveEditor = false;
                    Fields["cruiseDepth"].guiActive = true;
                    Fields["cruiseDepth"].guiActiveEditor = true;
                    Fields["cruiseDepth"].guiName = Localizer.Format("#autoLOC_NAS_Editor_cruiseDepth");

                    ((UI_FloatRange)Fields["cruiseDepth"].uiControlEditor).maxValue = maxDepth - 2;
                    ((UI_FloatRange)Fields["cruiseDepth"].uiControlFlight).maxValue = maxDepth - 2;

                    cruiseDepth = Mathf.Clamp(cruiseDepth, MinCruiseDepth, Mathf.Max(MinCruiseDepth, maxDepth - 2));
                    break;
                default:
                    Debug.LogWarning("[NAS-ASW]Neither depth charge nor torpedo! (" + part.partInfo?.name + ")");
                    break;
            }
            // NAS 0.7.0 overwrote both depths here with the toolbar window's values, which discarded the
            // per-part setting from the editor. The toolbar window still sets every unfired weapon when moved.
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (_data == null || !HighLogic.LoadedSceneIsFlight || vessel == null || _missileLauncher == null) return;
            if (!_missileLauncher.HasFired || _missileLauncher.HasExploded) return;

            CancelMissSelfDestruct();

            if (_data.Type != "DepthCharge") return;

            if (_depthField != null && _depthField.controlEnabled)
                _depthField.controlEnabled = false;

            var height = Height;
            if (!_bomb.Bomb(height, autoDestroyDepth)) return;

            // BDArmory 1.13 keeps a missile warhead disarmed until it is 1.25x its blast radius away from the
            // launching vessel, and Detonate() on a disarmed warhead deletes the charge without an explosion.
            // Keep sinking until armed; BDArmory's altitude fuze (altitudeFuzeMode in the part cfg) is the backstop.
            if (!WarheadsArmed())
            {
                if (!_waitingForArmingLogged)
                {
                    Debug.Log("[NAS-ASW] DepthCharge reached " + height + "m but is not armed yet (too close to the launching vessel); waiting.");
                    _waitingForArmingLogged = true;
                }
                return;
            }

            Debug.Log("DepthCharge Bombed At " + height + "m on " + vessel.mainBody.name);
            _missileLauncher.Detonate();
        }

        // BDArmory 1.13 runs its missile "miss" check on unguided weapons too: once the weapon points away from, or
        // passes, the dummy target it places 5 km ahead of the launch point, it is flagged as missed and self-destructs
        // 1.5 s later (MissileBase.isTimed/detonationTime). For a depth charge that can happen right after it enters the
        // water. NAS weapons have their own fuzes (depth fuze, contact, BDArmory altitude fuze), so cancel that timer
        // unless the part config itself asked for a timed fuze.
        private void CancelMissSelfDestruct()
        {
            if (_cfgIsTimed || !_missileLauncher.HasMissed || !_missileLauncher.isTimed) return;
            _missileLauncher.isTimed = false;
            if (!_missCancelLogged)
            {
                Debug.Log("[NAS-ASW] Cancelled BDArmory miss self-destruct for " + _data.Type + " " + part.partInfo?.name);
                _missCancelLogged = true;
            }
        }

        private bool WarheadsArmed()
        {
            // Called every frame while a charge waits to be armed; FindModulesImplementing allocates a new list
            // per call and a part's modules do not change, so look them up once.
            if (_warheads == null)
                _warheads = part.FindModulesImplementing<BDWarheadBase>();
            var warheads = _warheads;
            for (int i = 0; i < warheads.Count; i++)
            {
                if (warheads[i] != null && !warheads[i].Armed)
                    return false;
            }
            return true;
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (_data == null || !HighLogic.LoadedSceneIsFlight || vessel == null || PartRigidbody == null) return;
            // Only steer/float weapons that BDArmory has actually launched.
            if (_missileLauncher == null || !_missileLauncher.HasFired || _missileLauncher.HasExploded) return;

            var height = Height;
            switch (_data.Type)
            {
                case "DepthCharge":
                    if (height < 0)
                        PartRigidbody.AddForce(UpAxis * _buoyancy.Force(height).y);
                    PartRigidbody.AddTorque(_rota.Torque(vessel.vesselTransform.forward, vessel.srf_velocity, height));
                    break;
                case "Torpedo":
                    // zeroThrustSink (part cfg, default true; unused before): once the motor has burnt out, stop depth
                    // keeping and buoyancy so the spent torpedo sinks instead of hovering at its cruise depth.
                    if (zeroThrustSink && _missileLauncher.MissileState == MissileBase.MissileStates.PostThrust) break;
                    if (height < 0 && height > -Math.Abs(maxDepth))
                    {
                        var up = UpAxis;
                        var angle = _pida.PidControl(-cruiseDepth, height, TimeWarp.fixedDeltaTime);
                        var torque = _pidt.PidControl(angle, 90 - Vector3.Angle(part.transform.forward, up), TimeWarp.fixedDeltaTime);
                        var horizonPitchAxis = Vector3.Cross(up, part.transform.forward).normalized;
                        PartRigidbody.AddTorque(_rota.Torque(vessel.vesselTransform.forward, vessel.srf_velocity, height));
                        PartRigidbody.AddTorque(-horizonPitchAxis * torque);
                        PartRigidbody.AddForce(up * _buoyancy.Force(height).y);
                    }
                    break;
            }
        }
    }
}
