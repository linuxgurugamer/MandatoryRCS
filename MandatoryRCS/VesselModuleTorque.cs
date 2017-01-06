using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace MandatoryRCS
{
    class VesselModuleTorque : VesselModule
    {
        [KSPField(isPersistant = true)]
        public Vector3 posRCSTorque;

        [KSPField(isPersistant = true)]
        public Vector3 negRCSTorque;

        [KSPField(isPersistant = true)]
        public Vector3 RWTorque;

        public bool updateModules = true;

        public List<ModuleRCS> RCSModules = new List<ModuleRCS>();
        public List<ModuleReactionWheel> RWModules = new List<ModuleReactionWheel>();

        public Vector3 posRWTorque;
        public Vector3 negRWTorque;

        private void FixedUpdate()
        {
            if (FlightGlobals.ready && Vessel.loaded)
            {
                if (updateModules)
                {
                    UpdateTorqueModules(Vessel);
                    getVesselRWTorque();
                    updateModules = false;
                }         

                // Check if the vessel RCS toggle is enabled
                if (RCSModules.Count > 0 && RCSModules.First().rcs_active)
                {
                    getVesselRCSTorque();



                }

            }


        }

        public void UpdateTorqueModules(Vessel v)
        {
            RCSModules.Clear();
            RWModules.Clear();

            foreach (Part p in v.Parts)
            {
                RCSModules.AddRange(p.Modules.GetModules<ModuleRCS>());
                RWModules.AddRange(p.Modules.GetModules<ModuleReactionWheel>());
            }
        }

        public void getVesselRCSTorque()
        {
            posRCSTorque = Vector3.zero;
            negRCSTorque = Vector3.zero;
            Vector3 posv;
            Vector3 negv;

            foreach (ModuleRCS rcs in RCSModules)
            {
                rcs.GetPotentialTorque(out posv, out negv); // The values are wrong...
                posRCSTorque += posv;
                negRCSTorque += negv;
            }
        }

        public void getVesselRWTorque()
        {
            RWTorque = Vector3.zero;

            posRWTorque = Vector3.zero;
            negRWTorque = Vector3.zero;
            Vector3 posv;
            Vector3 negv;



            foreach (ModuleReactionWheel rw in RWModules)
            {
                rw.GetPotentialTorque(out posv, out negv);
                posRWTorque += posv;
                negRWTorque += negv;

                //RWTorque.x += rw.PitchTorque;
                //RWTorque.y += rw.RollTorque;
                //RWTorque.z += rw.YawTorque;
            }
        }
    }
}
