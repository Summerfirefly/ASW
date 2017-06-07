using System;
using BahaTurret;
using UnityEngine;
using AntiSubmarineWeapon;

namespace AntiSubmarineWeapon
{
    class ModuleDepthChargeProjector : PartModule
    {
        public enum ProjectorType
        {
            KGun,
            YGun
        }

        [KSPField]
        public ProjectorType projectorType = ProjectorType.KGun;

        [KSPField]
        public bool autoReload = false;

        [KSPField]
        public float reloadTime = 0;

        [KSPField]
        public float ejectForce = 0;
    }
}
