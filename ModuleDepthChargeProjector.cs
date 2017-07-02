namespace AntiSubmarineWeapon
{
    public class ModuleDepthChargeProjector : PartModule
    {
        public enum ProjectorType
        {
            KGun,
            YGun
        }

        [KSPField]
        public ProjectorType projectorType = ProjectorType.KGun;

        [KSPField]
        public bool autoReload;

        [KSPField]
        public float reloadTime;

        [KSPField]
        public float ejectForce;
    }
}
