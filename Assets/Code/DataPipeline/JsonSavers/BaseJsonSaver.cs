using System.Collections.Generic;
using Assets.Code.DataPipeline.JsonBuilders.JsonKeys;
using Assets.Dependencies.Boomlagoon.JSON;
using UnityEngine;

namespace Assets.Code.DataPipeline.JsonSavers
{
    public abstract class BaseJsonSaver
    {
        protected static JSONArray BuildJsonArrayFromStrings(IEnumerable<string> input)
        {
            var fab = new JSONArray();
            foreach(var item in input)
                fab.Add(new JSONValue(item));

            return fab;
        }

        protected static JSONArray BuildJsonArrayFromFloats(IEnumerable<float> input)
        {
            var fab = new JSONArray();
            foreach (var item in input)
                fab.Add(new JSONValue(item));

            return fab;
        }

        protected static JSONArray BuildJsonArrayFromJsonObjects(IEnumerable<JSONObject> input)
        {
            var fab = new JSONArray();
            foreach (var item in input)
                fab.Add(item);

            return fab;
        }

        protected static JSONObject BuildJsonObjectFromColour(Color color)
        {
            return new JSONObject
            {
                {GeneralJsonKey.RedTintComponent, color.r},
                {GeneralJsonKey.GreenTintComponent, color.g},
                {GeneralJsonKey.BlueTintComponent, color.b},
                {GeneralJsonKey.AlphaTintComponent, color.a}
            };
        }
    }
}
