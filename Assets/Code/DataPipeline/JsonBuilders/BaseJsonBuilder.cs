using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.DataPipeline.JsonBuilders.JsonKeys;
using Assets.Code.Models;
using Assets.Dependencies.Boomlagoon.JSON;
using UnityEngine;

namespace Assets.Code.DataPipeline.JsonBuilders
{
    public class BaseJsonBuilder
    {
        protected bool SafelyGetBool(JSONObject input, string key, bool defaultValue)
        {
            var result = defaultValue;
            if (input != null && input.ContainsKey(key))
                result = input.GetBoolean(key);
            else
                Debug.Log("NOTE! missing expected bool field " + key);

            return result;
        }

        protected string SafelyGetString(JSONObject input, string key, string defaultValue)
        {
            var result = defaultValue;
            if (input != null && input.ContainsKey(key))
                result = input.GetString(key);
            else
                Debug.Log("NOTE! missing expected string field " + key);

            return result;
        }

        protected int SafelyGetInt(JSONObject input, string key, int defaultValue)
        {
            var result = defaultValue;
            if (input != null && input.ContainsKey(key))
                result = (int)input.GetNumber(key);
            else
                Debug.Log("NOTE! missing expected int field " + key);

            return result;
        }

        protected float SafelyGetFloat(JSONObject input, string key, float defaultValue)
        {
            var result = defaultValue;
            if (input != null && input.ContainsKey(key))
                result = (float)input.GetNumber(key);
            else
                Debug.Log("NOTE! missing expected float field " + key);

            return result;
        }

        protected Guid SafelyGetGuid(JSONObject input, string key, Guid defaultValue)
        {
            var result = defaultValue;
            if (input != null && input.ContainsKey(key))
                result = new Guid(input.GetString(key));
            else
                Debug.Log("NOTE! missing expected guid field " + key);

            return result;
        }

        protected List<string> SafelyGetStringList(JSONObject input, string key, List<string> defaultValue)
        {
            var result = defaultValue;
            if (input != null && input.ContainsKey(key))
                result = input.GetArray(key).Select(item => item.Str).ToList();
            else
                Debug.Log("NOTE! missing expected string list field " + key);

            return result;
        }

        protected List<float> SafelyGetFloatList(JSONObject input, string key, List<float> defaultValue)
        {
            var result = defaultValue;
            if (input != null && input.ContainsKey(key))
                result = input.GetArray(key).Select(item => (float)item.Number).ToList();
            else
                Debug.Log("NOTE! missing expected float list field " + key);

            return result;
        }

        protected Color SafelyGetTint(JSONObject input, string key, Color defaultValue)
        {
            var result = defaultValue;
            if (input != null && input.ContainsKey(key))
                result = BuildTint(input.GetObject(key));
            else
                Debug.Log("NOTE! missing expected tint field " + key);

            return result;
        }

        protected JSONObject SafelyGetJsonObject(JSONObject input, string key, JSONObject defaultValue)
        {
            var result = defaultValue;
            if (input != null && input.ContainsKey(key))
                result = input.GetObject(key);
            else
                Debug.Log("NOTE! missing expected json object field " + key);

            return result;
        }

        protected JSONArray SafelyGetJsonArray(JSONObject input, string key, JSONArray defaultValue)
        {
            var result = defaultValue;
            if (input != null && input.ContainsKey(key))
                result = input.GetArray(key);
            else
                Debug.Log("NOTE! missing expected json array field " + key);

            return result;
        }

        protected T SafelyGetEnum<T>(JSONObject input, string key, T defaultValue) where T : IConvertible
        {
            var result = defaultValue;

            if (input != null && input.ContainsKey(key))
                result = (T)Enum.Parse(typeof(T), input.GetString(key), true);
            else
                Debug.Log("NOTE! missing expected enum field " + key);

            return result;
        }

        protected Vector3 SafelyGetVector3(JSONObject input, string key, Vector3 defaultValue)
        {
            var result = defaultValue;
            if (input != null && input.ContainsKey(key))
                result = BuildVector3(input.GetObject(key));
            else
                Debug.Log("NOTE! missing expected vector3 field " + key);

            return result;
        }

        protected Color BuildTint(JSONObject input)
        {
            return new Color(SafelyGetFloat(input, GeneralJsonKey.RedTintComponent, 1),
                             SafelyGetFloat(input, GeneralJsonKey.GreenTintComponent, 1),
                             SafelyGetFloat(input, GeneralJsonKey.BlueTintComponent, 1),
                             SafelyGetFloat(input, GeneralJsonKey.AlphaTintComponent, 1));
        }

        protected Vector3 BuildVector3(JSONObject input)
        {
            return new Vector3(
                SafelyGetFloat(input, GeneralJsonKey.X, 0),
                SafelyGetFloat(input, GeneralJsonKey.Y, 0),
                SafelyGetFloat(input, GeneralJsonKey.Z, 0)
            );
        }
    }
}
