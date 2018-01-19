using UnityEngine;

public class ModuleRotor : PartModule
{
    [KSPField]
    private Transform[] RotateTransforms = null;
    private float rotateAngle;

    public string RotateTransformName = "RotateTransform";

    public bool IsForwardRotation = true;
    public float rotateRPM = 30.0f;
    public bool torpedo = false;


    public override void OnStart(PartModule.StartState state)
    {
        RotateTransforms = base.part.FindModelTransforms(RotateTransformName);
    }

    void FixedUpdate()
    {
        if (vessel.parts.Count == 1 && (!torpedo || vessel.Splashed)) //!torpedo || (torpedo && splashed)
        {
            rotateAngle = rotateRPM * 6 * TimeWarp.fixedDeltaTime; //DPS = RPM * 360 / 60 = RPM * 6

            if (!IsForwardRotation)
            {
                rotateAngle = rotateAngle * -1;
            }

            for (int i = 0; i < RotateTransforms.Length; i++)
            {
                RotateTransforms[i].Rotate(0, 0, rotateAngle);
            }
        }
    }
}
