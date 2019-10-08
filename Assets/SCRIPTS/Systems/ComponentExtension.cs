using UnityEngine;

namespace AirHockey.Systems
{
    /// <summary>
    /// Componentの拡張
    /// </summary>
    public static class ComponentExtension
    {
        /// <summary>
        /// Componentの継承制約をかけているので、多分インターフェースなどは取ってこれない
        /// </summary>
        /// <param name="component"></param>
        /// <param name="tComponent"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryGetComponent<T>(this Component component, out T tComponent)  where T : Component
        {
            tComponent = component.GetComponent<T>();
            return tComponent != null;
        }
    }
}
