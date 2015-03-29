using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Code
{
    public enum TestType
    {
        Standard = 0,
        BloodCultures,
        Citrate,
        GelSeparator,
        Serum,
        RapidSerum,
        HeparinGelSeparator,
        Heparin,
        Edta,
        EdtaWithGel,
        Glucose
    }

    public class PatientGenerator
    {
        private const int WrongNameChance = 5;
        private const int WrongIdChance = 5;
        private const int RebeliousChance = 15;
        static readonly private List<string> LastNames = new List<string>
        {
            "DaCunha",
            "Laakes",
            "Moni",
            "Bush",
            "Obama",
            "Roosavelt",
            "Bin Laden",
            "Washington",
            "Heussein",
            "Blair",
            "Reagan",
            "McDonald"
        };

        static readonly private List<string> FirstNames = new List<string>
        {
            "Ryan",
            "Asher",
            "Rohit",
            "George",
            "Ted",
            "Barack",
            "Theodore",
            "Osama",
            "George",
            "Saddam",
            "Tony",
            "Ronald",
            "Teddy",
            "Ronald"
        };

        public Patient GeneratePatient()
        {
            Patient fab = new Patient();

            fab.FirstName = FirstNames[Random.Range(0, FirstNames.Count)];
            fab.LastName = LastNames[Random.Range(0, LastNames.Count)];

            fab.WristbandFirstName = fab.FirstName;
            fab.WristbandLastName = fab.LastName;
            if (Random.Range(0, 100) < WrongNameChance)
            {
                do
                {
                    fab.WristbandFirstName = FirstNames[Random.Range(0, FirstNames.Count)];
                    fab.WristbandLastName = LastNames[Random.Range(0, LastNames.Count)];
                } while (fab.WristbandFirstName == fab.FirstName && fab.WristbandLastName == fab.LastName);
            }

            fab.Id = Guid.NewGuid();

            fab.WristbandId = fab.Id;
            if (Random.Range(0, 100) < WrongIdChance)
            {
                do
                {
                    fab.WristbandId = Guid.NewGuid();
                } while (fab.WristbandId == fab.Id);
            }

            fab.Rebellious = false;
            if (Random.Range(0, 100) < RebeliousChance)
            {
                fab.Rebellious = true;
            }

            int numPossibleTests = Enum.GetNames(typeof(TestType)).Count();
            fab.DoctorsOrders = (TestType)Random.Range(0, numPossibleTests);

            return fab;
        }
    }
}
