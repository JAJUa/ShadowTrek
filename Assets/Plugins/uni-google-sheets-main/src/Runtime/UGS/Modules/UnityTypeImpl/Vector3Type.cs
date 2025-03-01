using GoogleSheet.Type;
using UnityEngine;

namespace UGS.Type
{
    [Type(typeof(Vector3), new string[] { "vector3", "Vector3", "vec3" })]
    public class Vector3Type : IType
    {
        public object DefaultValue => 0;

        /// <summary>
        /// value = google sheet data value. 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object Read(string value)
        {
            
            var values = ReadUtil.GetBracketValueToArray(value);
            float x = float.Parse(values[0]);
            float y = float.Parse(values[1]);
            float z = float.Parse(values[2]); 
            return new Vector3(x,y,z);
            /*
            string[] split = value.Split(',');
            float x = float.Parse(split[0]);
            float y = float.Parse(split[1]);
            float z = float.Parse(split[2]);

            return new UnityEngine.Vector3(x, y, z);*/
        }


        public string Write(object value)
        {
            Vector3 data = (Vector3)value;
            return $"{data.x},{data.y},{data.z}";
        }
    }
}
