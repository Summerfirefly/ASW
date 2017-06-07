using System;
using UnityEngine;
using UnityEngine.Rendering;
using Sonar;

namespace AntiSubmarineWeapon
{
    public class KerwisRedRound : MonoBehaviour
    {
        public float nexttime = 0;
        public static SonarSystem sonarSystem;
        public static Material lineMaterial;
        public static Vector2[,] vertexArray;
        static void CreateLineMaterial()
        {
            if (!lineMaterial)
            {
                lineMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
                 "SubShader { Pass { " +
                 "    Blend SrcAlpha OneMinusSrcAlpha " +
                 "    ZWrite Off Cull Off Fog { Mode Off } " +
                 "    BindChannels {" +
                 "      Bind \"vertex\", vertex Bind \"color\", color }" +
                 "} } }");
                lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
                // Turn on alpha blending
                lineMaterial.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
                lineMaterial.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                // Turn backface culling off
                lineMaterial.SetInt("_Cull", (int)CullMode.Off);
                // Turn off depth writes
                lineMaterial.SetInt("_ZWrite", 0);
            }
        }
        public static void start()
        {
            sonarSystem = new SonarSystem();
            sonarSystem.Rposition(new Vector2(Screen.width / 2, Screen.height / 2), new Vector2(Screen.width, Screen.height));
            sonarSystem.Round();
            CreateLineMaterial();
        }
        //void Update()
        //{
        //    if (Time.time > nexttime)
        //    {
        //        vertexArray = sonarSystem.DrawOuterCircle();
        //        nexttime = (float)(nexttime + 0.1);
        //    }
        //}
        void OnPostRender()
        {
            //Draw();
        }
        internal static void Draw()
        {
            if (!lineMaterial || vertexArray == null || sonarSystem == null)
                start();
            GL.PushMatrix();
            GL.LoadPixelMatrix();
            lineMaterial.SetPass(0);
            GL.Color(new Color(1f,0.4f,0f));
            GL.Begin(GL.LINES);
            var length = vertexArray.GetLength(0);
            var length2 = vertexArray.GetLength(1);
            for (int index = 0; index < length; index++)
            {
                for (int index2 = 0; index2 < length2 - 1; index2++)
                {
                    GL.Vertex3(vertexArray[index, index2].x, vertexArray[index, index2].y, 0);
                    GL.Vertex3(vertexArray[index, index2 + 1].x, vertexArray[index, index2 + 1].y, 0);
                }
                GL.Vertex3(vertexArray[index, 0].x, vertexArray[index, 0].y, 0);
                GL.Vertex3(vertexArray[index, length2 - 1].x, vertexArray[index, length2 - 1].y, 0);
            }
            GL.End();
            GL.PopMatrix();
        }
    }
}

