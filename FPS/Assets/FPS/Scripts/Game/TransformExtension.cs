using UnityEngine;
public static class TransformExtension
    {
        public static Transform GetChild(this GameObject goParent, string childName)
        {
            return GetChild(goParent.transform,childName);
        }
        public static Transform GetChild(this Transform parent, string childName)
        {
            //if (parent==null)return null;
            Transform searchTrans = parent.transform.Find(childName);
            if (searchTrans == null)
            {
                foreach (Transform trans in parent.transform)
                {
                    searchTrans = GetChild(trans.gameObject, childName);
                    if (searchTrans != null)
                    {
                        return searchTrans;
                    }
                }
            }
            return searchTrans;
        }
        public static T GetChild<T>(this GameObject goParent, string childName="") where T: Component
        {
            return GetTheChildComponent<T>(goParent.transform,childName);
        }
        public static T GetChild<T>(this Transform parent, string childName="") where T: Component
        {
            return GetTheChildComponent<T>(parent, childName);
        }
        public static T GetChild<T>(this RectTransform parent, string childName="") where T: Component
        {
            return GetTheChildComponent<T>(parent, childName);
        }
        /// <summary>
        /// 获取子物体的脚本
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="goParent">父对象</param>
        /// <param name="childName">子对象名称</param>
        /// <returns></returns>
        public static T GetTheChildComponent<T>(GameObject goParent, string childName) where T : Component
        {
            return GetTheChildComponent<T>(goParent.transform, childName);
        }
        public static T GetTheChildComponent<T>(this Transform parent, string childName) where T : Component
        {
            Transform searchTrans = GetChild(parent, childName);
            if (searchTrans != null)
            {
                var component=searchTrans.gameObject.GetComponent<T>();
                if (component==null)
                {
                    component=searchTrans.gameObject.AddComponent<T>();
                }

                return component;
            }
            return null;
        }

    }