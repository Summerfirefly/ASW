using UnityEngine;

public class ModuleRotor : PartModule
{
    private Transform[] RotateTransforms = null;
    private float rotateAngle;

    // These were plain fields in NAS 0.7.0, so the part cfg values (RPM, direction, torpedo) were ignored.
    [KSPField]
    public string RotateTransformName = "RotateTransform";

    [KSPField]
    public bool IsForwardRotation = true;
    [KSPField]
    public float rotateRPM = 30.0f;
    [KSPField]
    public bool torpedo = false;


    public override void OnStart(PartModule.StartState state)
    {
        RotateTransforms = base.part.FindModelTransforms(RotateTransformName);
    }

    void FixedUpdate()
    {
        if (!HighLogic.LoadedSceneIsFlight || vessel == null || RotateTransforms == null) return;

        if (vessel.parts.Count == 1 && (!torpedo || vessel.Splashed)) //!torpedo || (torpedo && splashed)
        {
            rotateAngle = rotateRPM * 6 * TimeWarp.fixedDeltaTime; //DPS = RPM * 360 / 60 = RPM * 6

            if (!IsForwardRotation)
            {
                rotateAngle = rotateAngle * -1;
            }

            for (int i = 0; i < RotateTransforms.Length; i++)
            {
                if (RotateTransforms[i] != null)
                    RotateTransforms[i].Rotate(0, 0, rotateAngle);
            }
        }
    }
}
