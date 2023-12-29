using AMBRD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMBRD.BL
{
    public class GenerateBookingId
    {
        public string GenerateHospitalId()
        {
            using (abdul_amurdEntities11 ent = new abdul_amurdEntities11())
            {
                string data = ent.Hospitals.OrderByDescending(a => a.Id).Select(a => a.HospitalId).FirstOrDefault();

                if (data != null)
                {
                    string PartitionValue = data.Substring(2); // Get the numeric part of the existing ID
                    int IncrementedVal = Convert.ToInt32(PartitionValue) + 1;

                    if (IncrementedVal < 10)
                    {
                        return "H000" + IncrementedVal;
                    }
                    else if (IncrementedVal < 100)
                    {
                        return "H00" + IncrementedVal;
                    }
                    else if (IncrementedVal < 1000)
                    {
                        return "H0" + IncrementedVal;
                    }
                    else if (IncrementedVal < 10000)
                    {
                        return "H" + IncrementedVal;
                    }
                    else
                    {
                        throw new Exception("Hospital ID overflow");
                    }
                }
                else
                {
                    return "H0001";
                }
            }
        }
        public string GeneratePatientId()
        {
            using (abdul_amurdEntities11 ent = new abdul_amurdEntities11())
            {
                string data = ent.Patients.OrderByDescending(a => a.Id).Select(a => a.PatientRegNo).FirstOrDefault();

                if (data != null)
                {
                    string PartitionValue = data.Substring(2); // Get the numeric part of the existing ID
                    int IncrementedVal = Convert.ToInt32(PartitionValue) + 1;

                    if (IncrementedVal < 10)
                    {
                        return "P000" + IncrementedVal;
                    }
                    else if (IncrementedVal < 100)
                    {
                        return "P00" + IncrementedVal;
                    }
                    else if (IncrementedVal < 1000)
                    {
                        return "P0" + IncrementedVal;
                    }
                    else if (IncrementedVal < 10000)
                    {
                        return "P" + IncrementedVal;
                    }
                    else
                    {
                        throw new Exception("Patient ID overflow");
                    }
                }
                else
                {
                    return "P0001";
                }
            }
        }
    }
}