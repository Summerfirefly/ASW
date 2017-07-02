using System;
using UnityEngine;

namespace AntiSubmarineWeapon
{
    public class BombSystem
    {
        private int _r;
        private readonly AswData _data = new AswData();
        
        public void ChargeDataWrite(int errorRange, float maxDetonateDepth, float maxDepth)
        {
            _data.ErrorRange = errorRange;
            _data.MaxDetonateDepth = maxDetonateDepth;
            _data.MaxDepth = maxDepth;
        }
        
        //public void ChargeDataWrite(float autoDestroyDepth)
        //{
        //    _data.MaxDetonateDepth = autoDestroyDepth;
        //}
        
        public void Ran()
        {
            var ran = new System.Random();
            _r = ran.Next(-_data.ErrorRange, _data.ErrorRange);
        }
        
        public bool Bomb(float hight,float destoryDepth)
        {
            if (hight < 0)
            {
                return Math.Abs(hight) > Math.Abs(destoryDepth) + _r ||
                       Math.Abs(hight) > Math.Abs(_data.MaxDetonateDepth);
            }
            
            return false;
        }
    }
    
    public class Buoyancy
    {
        private readonly AswData _data = new AswData();
        
        public void BuoyancyDataWrite(bool volumebuoyancy, float volume, float buoyancyForce)
        {
            _data.VolumeBuoyancy = volumebuoyancy;
            _data.Volume = volume;
            _data.BuoyancyForce = buoyancyForce;
        }
        
        public Vector3 Force(float hight)
        {
            if (hight < 0 && Math.Abs(hight)>Math.Abs(_data.MaxDepth))
            {
                if (_data.VolumeBuoyancy)
                    return new Vector3(0, _data.Volume * 10, 0);
                
                if (Math.Abs(_data.BuoyancyForce+1) >= 1e-7)
                    return new Vector3(0, _data.BuoyancyForce, 0);

                if (_data.VolumeBuoyancy)
                    return new Vector3(0, _data.BuoyancyForce, 0);

                return Vector3.zero;
            }

            return Vector3.zero;
        }
    }
    public class RotationSystem
    {
        private readonly AswData _data = new AswData();
        
        public void RotationData(Vector3 airMaxTorque, Vector3 waterMaxTorque, Vector3 airCocf, Vector3 waterCocf)
        {
            _data.AirMaxTorque = airMaxTorque;
            _data.WaterMaxTorque = waterMaxTorque;
            _data.AirCocf = airCocf;
            _data.WaterCocf = waterCocf;
        }
        
        public Vector3 Torque(Vector3 transformUp, Vector3 velocity, float hight)
        {
            if (hight >= 0)
            {
                return new Vector3(Vector3.Cross(transformUp, velocity).x * _data.AirMaxTorque.x,
                    Vector3.Cross(transformUp, velocity).y * _data.AirMaxTorque.y + 0.1f,
                    Vector3.Cross(transformUp, velocity).z * _data.AirMaxTorque.z);
            }

            return new Vector3(Vector3.Cross(transformUp, velocity).x * _data.WaterMaxTorque.x,
                Vector3.Cross(transformUp, velocity).y * _data.WaterMaxTorque.y + 0.1f,
                Vector3.Cross(transformUp, velocity).z * _data.WaterMaxTorque.z);
        }
    }
}
