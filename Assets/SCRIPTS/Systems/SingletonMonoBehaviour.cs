using System;
using UnityEngine;

namespace AirHockey.Systems
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get {
                if (_instance != null) return _instance;

                var t = typeof(T);
                _instance = (T)FindObjectOfType(t);
                if (_instance == null)
                {
                    Debug.LogError(t + " をアタッチしているGameObjectはありません");
                }

                return _instance;
            }
        }

        public static bool Instantiated() => _instance != null;

        protected virtual void Awake()
        {
            // 他のGameObjectにアタッチされているか調べる.
            // アタッチされている場合は破棄する.
            if (this != Instance)
            {
                Destroy(this);
                //Destroy(this.gameObject);
                Debug.LogError(
                    typeof(T) +
                    " は既に他のGameObjectにアタッチされているため、コンポーネントを破棄しました." +
                    " アタッチされているGameObjectは " + Instance.gameObject.name + " です.");
                return;
            }

            // なんとかManager的なSceneを跨いでこのGameObjectを有効にしたい場合は
            // ↓コメントアウト外してください.
            //DontDestroyOnLoad(this.gameObject);
        }

    }
}
