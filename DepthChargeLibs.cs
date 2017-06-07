using System;
using UnityEngine;
using AntiSubmarineWeapon;

namespace AntiSubmarineWeapon
{
    public class BombSystem
    {
        private int r;
        ASWData Data = new ASWData();
        public void ChargeDataWrite(int errorRange, float maxDetonateDepth, float maxDepth)
        {
            Data.errorRange = errorRange;
            Data.maxDetonateDepth = maxDetonateDepth;
            Data.maxDepth = maxDepth;
        }
        //public void ChargeDataWrite(float autoDestroyDepth)
        //{
        //    Data.maxDetonateDepth = autoDestroyDepth;
        //}
        public void ran()
        {
            System.Random ran = new System.Random();
            r = ran.Next(-(int)Data.errorRange, (int)Data.errorRange);
        }
        public bool Bomb(float hight,float destoryDepth)
        {
            if (hight < 0)
            {
                if (Math.Abs(hight) > Math.Abs(destoryDepth) + r || Math.Abs(hight) > Math.Abs(Data.maxDetonateDepth))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
    public class Buoyancy
    {
        ASWData Data = new ASWData();
        public void BuoyancyDataWrite(bool volumebuoyancy, float volume, float buoyancyForce)
        {
            Data.volumeBuoyancy = volumebuoyancy;
            Data.volume = volume;
            Data.buoyancyForce = buoyancyForce;
        }
        public Vector3 Force(float hight)
        {
            if (hight < 0 && Math.Abs(hight)>Math.Abs(Data.maxDepth))
            {
                if (Data.volumeBuoyancy)
                    return new Vector3(0, Data.volume * 10, 0);
                else
                {
                    if (!(Data.buoyancyForce == -1))
                        return new Vector3(0, Data.buoyancyForce, 0);
                    else if (Data.volumeBuoyancy)
                        return new Vector3(0, Data.buoyancyForce, 0);
                    else
                        return Vector3.zero;
                }
            }
            else
                return Vector3.zero;
        }
    }
    public class RotationSystem
    {
        ASWData Data = new ASWData();
        public void RotationData(Vector3 airMaxTorque, Vector3 waterMaxTorque, Vector3 airCocf, Vector3 waterCocf)
        {
            Data.airMaxTorque = airMaxTorque;
            Data.waterMaxTorque = waterMaxTorque;
            Data.airCocf = airCocf;
            Data.waterCocf = waterCocf;
        }
        public Vector3 Torque(Vector3 TransformUp, Vector3 velocity, float hight)
        {
            if (hight >= 0)
            {
                return new Vector3(Vector3.Cross(TransformUp, velocity).x * Data.airMaxTorque.x, Vector3.Cross(TransformUp, velocity).y * Data.airMaxTorque.y + 0.1f, Vector3.Cross(TransformUp, velocity).z * Data.airMaxTorque.z);
            }
            else
                return new Vector3(Vector3.Cross(TransformUp, velocity).x * Data.waterMaxTorque.x, Vector3.Cross(TransformUp, velocity).y * Data.waterMaxTorque.y + 0.1f, Vector3.Cross(TransformUp, velocity).z * Data.waterMaxTorque.z);
        }
    }
}
