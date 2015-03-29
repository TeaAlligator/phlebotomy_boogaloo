using System;
using Assets.Code;
using UnityEngine;
using System.Collections;

namespace Assets.Code
{
    public class Patient
    {
        public string FirstName;
        public string WristbandFirstName;
        public string LastName;
        public string WristbandLastName;
        public Guid Id;
        public Guid WristbandId;
        public TestType DoctorsOrders;
        public bool Rebellious;
    }
}
