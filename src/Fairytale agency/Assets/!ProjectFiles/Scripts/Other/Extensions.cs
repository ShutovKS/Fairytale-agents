using System;
using System.Text;
using Newtonsoft.Json;
using Services.DynamicData;
using UnityEngine;

namespace Other
{
    public static class Extensions
    {
        public static T ToDeserialized<T>(this string json) => JsonConvert.DeserializeObject<T>(json);

        public static string ToJson(this object obj) => JsonConvert.SerializeObject(obj);
        public static int RoundToNearest(this int number, int divisor)
        {
            if (divisor == 0)
            {
                Debug.LogError("Divisor cannot be zero.");
                return number;
            }

            var result = Mathf.RoundToInt((float)number / divisor) * divisor;
            return result;
        }

        public static byte[] ToByteArray(this PlayerProgress playerProgress)
        {
            var playerProgressJson = JsonConvert.SerializeObject(playerProgress);
            
            var playerData = Encoding.UTF8.GetBytes(playerProgressJson);
            
            return playerData;
        }
        
        public static bool TryParseByteArrayToPlayerProgress(this byte[] arrBytes)
        {
            if (arrBytes == null || arrBytes.Length == 0)
            {
                Debug.LogError("Byte array is null or empty.");
                return false;
            }

            try
            {
                var playerProgressJson = Encoding.UTF8.GetString(arrBytes);
                var deserializedObject = JsonConvert.DeserializeObject<PlayerProgress>(playerProgressJson);

                return deserializedObject != null;
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to deserialize PlayerProgress: " + ex.Message);
                return false;
            }
        }

        
        public static PlayerProgress ByteArrayToPlayerProgress(this byte[] arrBytes)
        {
            if (arrBytes == null || arrBytes.Length == 0)
            {
                Debug.LogError("Byte array is null or empty.");
                return null;
            }

            try
            {
                var playerProgressJson = Encoding.UTF8.GetString(arrBytes);
                
                return JsonConvert.DeserializeObject<PlayerProgress>(playerProgressJson);
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to deserialize PlayerProgress: " + ex.Message);
                return null;
            }
        }
    }
}