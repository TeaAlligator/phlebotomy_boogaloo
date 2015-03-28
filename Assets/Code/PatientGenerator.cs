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
    public enum test
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
        static private int _wrongNameChance = 5;
        static private int _wrongIdChance = 5;
        static private int _rebeliousChance = 15;
        static private List<string> _lastNames = new List<string>
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

        static private List<string> _firstNames = new List<string>
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

            fab.FirstName = _firstNames[Random.Range(0, _firstNames.Count)];
            fab.LastName = _lastNames[Random.Range(0, _lastNames.Count)];

            fab.WristbandFirstName = fab.FirstName;
            fab.WristbandLastName = fab.LastName;
            if (Random.Range(0, 100) < _wrongNameChance)
            {
                do
                {
                    fab.WristbandFirstName = _firstNames[Random.Range(0, _firstNames.Count)];
                    fab.WristbandLastName = _lastNames[Random.Range(0, _lastNames.Count)];
                } while (fab.WristbandFirstName == fab.FirstName && fab.WristbandLastName == fab.LastName);
            }

            fab.Id = Guid.NewGuid();

            fab.WristbandId = fab.Id;
            if (Random.Range(0, 100) < _wrongIdChance)
            {
                do
                {
                    fab.WristbandId = Guid.NewGuid();
                } while (fab.WristbandId == fab.Id);
            }

            fab.Rebellious = false;
            if (Random.Range(0, 100) < _rebeliousChance)
            {
                fab.Rebellious = true;
            }

            int numPossibleTests = Enum.GetNames(typeof(test)).Count();
            fab.DoctorsOrders = (test)Random.Range(0, numPossibleTests);

            return fab;
        }
    }
}
