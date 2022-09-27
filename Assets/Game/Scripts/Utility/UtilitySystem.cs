using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

using UnityEngine;

using Object = System.Object;

namespace RougeFW
{

    public class UtilitySystem : MonoBehaviour
    {

        public static string GetRndomString(int length)
        {
            string s = "", str = "";
            str += "0123456789";
            str += "abcdefghijklmnopqrstuvwxyz";
            str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(UnityEngine.Random.Range(0, str.Length - 1), 1);
            }
            return s;
        }


        public static string GetDateTimeString( string date_format = "yyyyMMddHHmmss")
        {
            return DateTime.Now.ToString(date_format);
        }


        public static void ArchiveDecryption(ref string decryption_text , string pass = "Mpmf2022")
        {
            byte[] inputByteArray = System.Convert.FromBase64String(decryption_text);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(pass);
                des.IV = ASCIIEncoding.ASCII.GetBytes(pass);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                decryption_text = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
            }
        }

        public static void ArchiveEncryption(ref string encryption_text , string pass = "Mpmf2022")
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = System.Text.Encoding.UTF8.GetBytes(encryption_text);
                des.Key = ASCIIEncoding.ASCII.GetBytes(pass);
                des.IV = ASCIIEncoding.ASCII.GetBytes(pass);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }

                encryption_text = System.Convert.ToBase64String(ms.ToArray());
                ms.Close();
            }
        }





        public static T GetAnItemFromList<T>(T template, List<T> item_list , int from_index = 0) where T : Component
        {
            for (int i = from_index; i < item_list.Count; i++)
                if (item_list[i].gameObject.activeInHierarchy == false )
                    return item_list[i];

            T new_item = Instantiate(template, template.transform.parent);
            item_list.Add(new_item);

            return new_item;
        }



        public static GameObject GetAnGameObjectFromList(GameObject template, List<GameObject> item_list, int from_index = 0)
        {
            for (int i = from_index; i < item_list.Count; i++)
                if (item_list[i].activeInHierarchy == false)
                    return item_list[i];

            GameObject new_item = Instantiate(template, template.transform.parent);
            item_list.Add(new_item);

            return new_item;
        }

        


        public static void AddItemToDictionaryWithListValue<T1,T2>(GenericDictionary<T1, List<T2>> dictionary , T1 key_name, T2 value_action)
        {
            if (dictionary.ContainsKey(key_name) == true)
            {
                if (dictionary[key_name].Contains(value_action) == false)
                    dictionary[key_name].Add(value_action);
            }
            else
                dictionary.Add(key_name, new List<T2>() { value_action });
        }



        public static void RemoveItemToDictionaryWithListValue<T1, T2>(GenericDictionary<T1, List<T2>> dictionary, T1 key_name, T2 value_action)
        {
            if (dictionary.ContainsKey(key_name) == true)
            {
                if (dictionary[key_name].Contains(value_action) == true)
                    dictionary[key_name].Remove(value_action);
            }
        }




        public static Vector3 V3RotateAround(Vector3 source, Vector3 axis, float angle_degree)
        {
            Quaternion q = Quaternion.AngleAxis(angle_degree, axis);// 旋转系数
            return q * source;// 返回目标点
        }

        protected static Vector3 look_direction = Vector3.zero;
        protected static Quaternion look_rotation = Quaternion.identity;
        public static void UpdateRotation(Vector3 look_position, Transform obj_transform, float rot_factor = 1.0f)
        {
            look_direction = look_position - obj_transform.position;
            look_direction.y = 0.001f;

            look_rotation = Quaternion.LookRotation(look_direction);

            obj_transform.rotation = Quaternion.Lerp(obj_transform.rotation, look_rotation, Time.deltaTime * rot_factor);
        }


        public static void UpdateRotation2(Vector3 look_position, Transform obj_transform, float rot_factor = 1.0f)
        {
            look_direction = look_position - obj_transform.position;

            look_rotation = Quaternion.LookRotation(look_direction);

            obj_transform.rotation = Quaternion.Lerp(obj_transform.rotation, look_rotation, Time.deltaTime * rot_factor);
        }



        public static bool IsLayerContains( LayerMask mask, int layer )
        {
            return mask == (mask | (1 << layer));
        }


        public static float ClampAngle180Sign( float angle_degree )
        {
            float angle = angle_degree % 360.0f;
            return angle > 180.0f ? angle - 360.0f : angle ;
        }



        public static float GetVectorAngleOnPlane(Vector3 a, Vector3 b, Vector3 plane_normal )
        {
            Vector3 projectionA = Vector3.ProjectOnPlane(a, plane_normal);
            Vector3 projectionB = Vector3.ProjectOnPlane(b, plane_normal);

            return Vector3.Angle(projectionA, projectionB);
        }



        public static bool IsListSourceContainListCheckItem<T>( List<T> list_soure , List<T> list_check )
        {
            for (int i = 0; i < list_check.Count; i++)
                if (list_soure.Contains(list_check[i]) == true)
                    return true;

            return false;
        }




        public static T FindNearestInTheList<T>( T from , List<T> to) where T : Component
        {
            float min_dis = 0;
            int min_index = -1;

            for( int i = 0; i < to.Count; i ++ )
            {
                float distance = Vector3.Distance( from.transform.position , to[i].transform.position );

                if( distance < min_dis || min_index < 0  )
                {
                    min_index = i;
                    min_dis = distance;
                }
            }


            return min_index <0 ? null : to[min_index];
        }







        public static T2 GetValueFromTagDictionary<T1,T2>( List<T1> tags , GenericDictionary<T1, T2> from_tag_dictionary, T2 default_value )
        {
            for (int i = 0; i < tags.Count; i++)
                if (from_tag_dictionary.ContainsKey(tags[i]) == true)
                    return from_tag_dictionary[tags[i]];

            return default_value;
        }





    }
}
