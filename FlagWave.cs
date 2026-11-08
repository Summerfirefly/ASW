using UnityEngine;

namespace AntiSubmarineWeapon
{
    // Waving flag animation for NAS_Flag. Not in the upstream repository (c640ea3); reconstructed from the
    // AntiSubmarineWeapon.dll 0.6.5.2 shipped with NAS 0.7.0, made public and null-safe.
    public class FlagWave : PartModule
    {
        [KSPField]
        public float speed = 12f;

        [KSPField]
        public float scale = 0.09f;

        private Transform flag1;
        private Transform flag2;
        private Transform flagm1;
        private Transform flagm2;

        private MeshFilter flagMesh1;
        private MeshFilter flagMesh2;
        private MeshFilter flagmMesh1;
        private MeshFilter flagmMesh2;

        private Vector3[] flagVertex1;
        private Vector3[] flagVertex2;
        private Vector3[] flagmVertex1;
        private Vector3[] flagmVertex2;

        public void PrintChild(Transform father)
        {
            if (father.name == "flagTransform")
                flag1 = father;
            if (father.name == "flagTransform2")
                flag2 = father;
            if (father.name == "flagMiddle")
                flagm1 = father;
            if (father.name == "flagMiddle2")
                flagm2 = father;

            for (int i = 0; i < father.childCount; i++)
                PrintChild(father.GetChild(i));
        }

        private void Wave(Vector3[] flag, MeshFilter mesh)
        {
            if (flag == null || mesh == null) return;
            for (int i = 0; i < flag.Length; i++)
            {
                if (flag[i].x != 0.5f)
                {
                    flag[i].z += Mathf.Sin(Time.time * speed + flag[i].x + 5f * (flag[i].y + flag[i].x));
                    flag[i].z *= scale;
                }
            }
            mesh.mesh.vertices = flag;
        }

        private void WaveT(Vector3[] flag, MeshFilter mesh)
        {
            if (flag == null || mesh == null) return;
            for (int i = 0; i < flag.Length; i++)
            {
                if (flag[i].x != 0.5f)
                {
                    flag[i].z -= Mathf.Sin(Time.time * speed + flag[i].x + 5f * (-flag[i].y + flag[i].x));
                    flag[i].z *= scale;
                }
            }
            mesh.mesh.vertices = flag;
        }

        private static MeshFilter GetMesh(Transform t) => t != null ? t.GetComponent<MeshFilter>() : null;

        private static Vector3[] GetVertices(MeshFilter mf) => mf != null ? mf.mesh.vertices : null;

        // MeshFilter.mesh (in GetVertices and Wave) gives each flag its own copy of the shared mesh. Unity does
        // not free such copies when the part is destroyed, so keep them and destroy them in OnDestroy.
        private Mesh[] ownedMeshes;

        private static Mesh GetOwnedMesh(MeshFilter mf) => mf != null ? mf.mesh : null;

        private void OnDestroy()
        {
            if (ownedMeshes == null) return;
            for (int i = 0; i < ownedMeshes.Length; i++)
            {
                if (ownedMeshes[i] != null)
                    Destroy(ownedMeshes[i]);
            }
            ownedMeshes = null;
        }

        public override void OnStart(StartState state)
        {
            base.OnStart(state);
            if (!HighLogic.LoadedSceneIsFlight) return;

            PrintChild(transform);
            flagMesh1 = GetMesh(flag1);
            flagMesh2 = GetMesh(flag2);
            flagmMesh1 = GetMesh(flagm1);
            flagmMesh2 = GetMesh(flagm2);
            flagVertex1 = GetVertices(flagMesh1);
            flagVertex2 = GetVertices(flagMesh2);
            flagmVertex1 = GetVertices(flagmMesh1);
            flagmVertex2 = GetVertices(flagmMesh2);
            ownedMeshes = new[] { GetOwnedMesh(flagMesh1), GetOwnedMesh(flagMesh2), GetOwnedMesh(flagmMesh1), GetOwnedMesh(flagmMesh2) };
            if (flagMesh1 == null && flagMesh2 == null && flagmMesh1 == null && flagmMesh2 == null)
                Debug.LogWarning("[NAS-ASW] FlagWave: no flag meshes found on " + part.partInfo?.name);
        }

        public override void OnUpdate()
        {
            if (!HighLogic.LoadedSceneIsFlight) return;
            Wave(flagVertex1, flagMesh1);
            Wave(flagmVertex1, flagmMesh1);
            Wave(flagVertex2, flagMesh2);
            WaveT(flagmVertex2, flagmMesh2);
        }
    }
}
