﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;
using System.Linq;

namespace NsLib.Config {

    public static class ConfigDictionary {
        public static Dictionary<K, V> ToWrap<K, V>(TextAsset asset, out bool isJson, bool isLoadAll = false) where V: ConfigBase<K> {
            isJson = false;
            if (asset == null)
                return null;
            return ToWrap<K, V>(asset.bytes, out isJson, isLoadAll);
        }

        #if UNITY_EDITOR
        // 因为List<T>里获得T类型有一个数组分配所以建议游戏运行时，不要用这个函数
        // 只用在测试中
        public static Dictionary<K, V> TestToWrap<K, V>(byte[] buffer, 
            out bool isJson, bool isLoadAll = false,
            UnityEngine.MonoBehaviour loadAllCortine = null) where V:class {
            isJson = false;
            if (buffer == null || buffer.Length <= 0)
                return null;

            Dictionary<K, V> ret = ConfigWrap.TestCommonToObject<K, V>(buffer, isLoadAll, loadAllCortine);
            if (ret == null) {
                try {
                    string text = System.Text.Encoding.UTF8.GetString(buffer);
                    ret = JsonMapper.ToObject<Dictionary<K, V>>(text);
                    isJson = true;
                } catch {
                    ret = null;
                }
            }

            return ret;
        }
        #endif

        public static Dictionary<K, V> ToWrap<K, V>(byte[] buffer, out bool isJson, bool isLoadAll = false) where V : ConfigBase<K> {
            isJson = false;
            if (buffer == null || buffer.Length <= 0)
                return null;

            Dictionary<K, V> ret = ConfigWrap.ToObject<K, V>(buffer, isLoadAll);
            if (ret == null) {
                try {
                    string text = System.Text.Encoding.UTF8.GetString(buffer);
                    ret = JsonMapper.ToObject<Dictionary<K, V>>(text);
                    isJson = true;
                } catch {
                    ret = null;
                }
            }
            return ret;
        }

        public static void PreloadWrap<K, V>(Dictionary<K, V> maps, byte[] buffer,
            MonoBehaviour mono, out bool isJson,
            Action<IDictionary> onEnd, Action<float> onProcess) where V : ConfigBase<K> {

            isJson = false;
            if (maps == null || buffer == null || buffer.Length <= 0 || mono == null) {
                if (onEnd != null)
                    onEnd(null);
                return;
            }

            maps.Clear();

            MemoryStream stream = new MemoryStream(buffer);


            Coroutine cor = ConfigWrap.ToObjectAsync<K, V>(stream, maps, mono, true, onEnd, onProcess);
            if (cor == null) {
                stream.Close();
                stream.Dispose();

                Dictionary<K, V> ret;
                try {
                    string text = System.Text.Encoding.UTF8.GetString(buffer);
                    maps = JsonMapper.ToObject<Dictionary<K, V>>(text);
                    ret = maps;
                    isJson = true;
                } catch {
                    ret = null;
                }

                if (onEnd != null) {
                    onEnd(ret);
                }
            }

        }

        // 预加载用
        public static void PreloadWrap<K, V>(Dictionary<K, V> maps, TextAsset asset,
            MonoBehaviour mono, out bool isJson,
            Action<IDictionary> onEnd, Action<float> onProcess) where V : ConfigBase<K> {
            isJson = false;
            if (maps == null || asset == null || mono == null) {
                if (onEnd != null)
                    onEnd(null);
                return;
            }

            PreloadWrap<K, V>(maps, asset.bytes, mono, out isJson, onEnd, onProcess);


        }

        public static void PreloadWrap<K1, K2, V>(Dictionary<K1, Dictionary<K2, V>> maps, TextAsset asset,
            MonoBehaviour mono, out bool isJson,
            Action<IDictionary> onEnd, Action<float> onProcess) where V : ConfigBase<K2>{
            isJson = false;
            if (maps == null || asset == null || mono == null) {
                if (onEnd != null)
                    onEnd(null);
                return;
            }

            PreloadWrap<K1, K2, V>(maps, asset.bytes, mono, out isJson, onEnd, onProcess);
        }

        public static void PreloadWrap<K1, K2, V>(Dictionary<K1, Dictionary<K2, V>> maps, byte[] buffer,
            MonoBehaviour mono, out bool isJson, Action<IDictionary> onEnd,
            Action<float> onProcess = null) where V : ConfigBase<K2> {
            isJson = false;
            if (maps == null || buffer == null || buffer.Length <= 0 || mono == null) {
                if (onEnd != null)
                    onEnd(null);
                return;
            }

            maps.Clear();

            MemoryStream stream = new MemoryStream(buffer);

            Coroutine cor = ConfigWrap.ToObjectMapAsync<K1, K2, V>(stream,
                maps, mono, true, onEnd, onProcess);

            if (cor == null) {
                stream.Close();
                stream.Dispose();

                Dictionary<K1, Dictionary<K2, V>> ret;
                try {
                    string text = System.Text.Encoding.UTF8.GetString(buffer);
                    maps = JsonMapper.ToObject<Dictionary<K1, Dictionary<K2, V>>>(text);
                    ret = maps;
                    isJson = true;
                } catch {
                    ret = null;
                }

                if (onEnd != null) {
                    onEnd(ret);
                }
            }
        }

        public static void PreloadWrap<K, V>(Dictionary<K, List<V>> maps, byte[] buffer,
            MonoBehaviour mono, out bool isJson,
            Action<IDictionary> onEnd, Action<float> onProcess = null) where V : ConfigBase<K> {

            isJson = false;
            if (maps == null || buffer == null || buffer.Length <= 0 || mono == null) {
                if (onEnd != null)
                    onEnd(null);
                return;
            }

            maps.Clear();

            MemoryStream stream = new MemoryStream(buffer);


            Coroutine cor = ConfigWrap.ToObjectListAsync<K, V>(stream, maps, mono, true, onEnd, onProcess);
            if (cor == null) {
                stream.Close();
                stream.Dispose();

                Dictionary<K, List<V>> ret;
                try {
                    string text = System.Text.Encoding.UTF8.GetString(buffer);
                    maps = JsonMapper.ToObject<Dictionary<K, List<V>>>(text);
                    ret = maps;
                    isJson = true;
                } catch {
                    ret = null;
                }

                if (onEnd != null) {
                    onEnd(ret);
                }
            }

        }

        public static void PreloadWrap<K, V>(Dictionary<K, List<V>> maps, TextAsset asset,
            MonoBehaviour mono, out bool isJson,
            Action<IDictionary> onEnd, Action<float> onProcess = null) where V : ConfigBase<K> {
            isJson = false;
            if (maps == null || asset == null || mono == null) {
                if (onEnd != null)
                    onEnd(null);
                return;
            }

            PreloadWrap<K, V>(maps, asset.bytes, mono, out isJson, onEnd, onProcess);
        }

        public static Dictionary<K, List<V>> ToWrapList<K, V>(TextAsset asset,
            out bool isJson,
            bool isLoadAll = false) where V : ConfigBase<K> {
            isJson = false;
            if (asset == null)
                return null;
            return ToWrapList<K, V>(asset.bytes, out isJson, isLoadAll);
        }

        public static Dictionary<K, List<V>> ToWrapList<K, V>(byte[] buffer,
            out bool isJson,
            bool isLoadAll = false) where V : ConfigBase<K> {

            isJson = false;
            if (buffer == null || buffer.Length <= 0)
                return null;

            Dictionary<K, List<V>> ret = ConfigWrap.ToObjectList<K, V>(buffer, isLoadAll);
            if (ret == null) {
                try {
                    string text = System.Text.Encoding.UTF8.GetString(buffer);
                    ret = JsonMapper.ToObject<Dictionary<K, List<V>>>(text);
                    isJson = true;
                } catch {
                    ret = null;
                }
            }
            return ret;
        }

        public static Dictionary<K1, Dictionary<K2, V>> ToWrapMap<K1, K2, V>(TextAsset asset,
            out bool isJson,
            bool isLoadAll = false) where V : ConfigBase<K2> {
            isJson = false;
            if (asset == null)
                return null;

            return ToWrapMap<K1, K2, V>(asset.bytes, out isJson, isLoadAll);
        }

        public static Dictionary<K1, Dictionary<K2, V>> ToWrapMap<K1, K2, V>(byte[] buffer,
            out bool isJson,
            bool isLoadAll = false) where V : ConfigBase<K2> {

            isJson = false;
            if (buffer == null || buffer.Length <= 0)
                return null;

            Dictionary<K1, Dictionary<K2, V>> ret = ConfigWrap.ToObjectMap<K1, K2, V>(buffer, isLoadAll);

            if (ret == null) {
                try {
                    string text = System.Text.Encoding.UTF8.GetString(buffer);
                    ret = JsonMapper.ToObject<Dictionary<K1, Dictionary<K2, V>>>(text);
                    isJson = true;
                } catch {
                    ret = null;
                }
            }

            return ret;
        }

    }

    public interface IConfigVoMap<K> {
        bool ContainsKey(K key);
        bool IsJson {
            get;
        }

        bool LoadFromTextAsset(TextAsset asset, bool isLoadAll = false);

        bool LoadFromBytes(byte[] buffer, bool isLoadAll = false);

        // 预加载
        bool Preload(TextAsset asset, UnityEngine.MonoBehaviour mono, 
            Action<IConfigVoMap<K>> onEnd, Action<float> onProcess = null);
        bool Preload(byte[] buffer, UnityEngine.MonoBehaviour mono, 
            Action<IConfigVoMap<K>> onEnd, Action<float> onProcess = null);
			
		void Clear();
    }

    
    // 两个配置
    public class ConfigVoMap<K, V>: IConfigVoMap<K> where V: ConfigBase<K> {
        private bool m_IsJson = true;
        private Dictionary<K, V> m_Map = null;
		
		public void Clear() {
            if (m_Map != null)
                m_Map.Clear();
            m_IsJson = true;
        }

        // 尽量少用这个方法，因为这样会导致配置全加载
        public List<V> ValueList {
            get {
                if (m_Map == null)
                    return null;
                List<V> ret = m_Map.Values.ToList();
                if (m_IsJson)
                    return ret;
                if (ret != null) {
                    for (int i = 0; i < ret.Count; ++i) {
                        V v = ret[i];
                        v.ReadValue();
                    }
                }
                return ret;
            }
        }

        public struct Enumerator {
            private bool m_IsJson;
            public Enumerator(bool isJson, IEnumerator<KeyValuePair<K, V>> iter) {
                m_IsJson = isJson;
                Iteror = iter;
            }

            internal IEnumerator<KeyValuePair<K, V>> Iteror {
                get;
                private set;
            }

            public KeyValuePair<K, V> Current {
                get {
                    if (Iteror == null)
                        return new KeyValuePair<K, V>();
                    if (m_IsJson) {
                        return Iteror.Current;
                    }
                    V config = Iteror.Current.Value;
                    if (config == null) {
                        return new KeyValuePair<K, V>();
                    }

                    if (config.IsReaded)
                        return Iteror.Current;

                    config.StreamSeek();
                    config.ReadValue();
                    return Iteror.Current;
                }
            }

            public void Dispose() {
                if (Iteror == null)
                    return;
                Iteror.Dispose();
            }
            public bool MoveNext() {
                if (Iteror == null)
                    return false;
                return Iteror.MoveNext();
            }
        }

        public bool IsJson {
            get {
                return m_IsJson;
            }
        }

        public Enumerator GetEnumerator() {
            if (m_Map == null)
                return new Enumerator();
            var iter = m_Map.GetEnumerator();
            Enumerator ret = new Enumerator(m_IsJson, iter);
            return ret;
        }

        public bool ContainsKey(K key) {
            if (m_Map == null)
                return false;
            return m_Map.ContainsKey(key);
        }

        public bool TryGetValue(K key, out V value) {
            value = default(V);
            if (m_Map == null)
                return false;
            if (m_IsJson) {
                return m_Map.TryGetValue(key, out value);
            }
            if (!ConfigWrap.ConfigTryGetValue<K, V>(m_Map, key, out value)) {
                value = default(V);
                return false;
            }
            return true;
        }


        public bool LoadFromTextAsset(TextAsset asset, bool isLoadAll = false) {
            if (asset == null)
                return false;
            m_Map = ConfigDictionary.ToWrap<K, V>(asset, out m_IsJson, isLoadAll);
            return m_Map != null;
        }

        public bool LoadFromBytes(byte[] buffer, bool isLoadAll = false) {
            if (buffer == null || buffer.Length <= 0)
                return false;
            m_Map = ConfigDictionary.ToWrap<K, V>(buffer, out m_IsJson, isLoadAll);
            return m_Map != null;
        }

        public V this[K key] {
            get {
                if (m_Map == null)
                    return default(V);
                V ret;
                if (m_IsJson) {
                    if (!m_Map.TryGetValue(key, out ret))
                        ret = default(V);
                    return ret;
                }
                if (!TryGetValue(key, out ret))
                    ret = default(V);
                return ret;
            }
        } 

        public bool Preload(TextAsset asset, UnityEngine.MonoBehaviour mono, 
            Action<IConfigVoMap<K>> onEnd, Action<float> onProcess = null) {
            if (asset == null || mono == null)
                return false;
            if (m_Map == null)
                m_Map = new Dictionary<K, V>();
            else
                m_Map.Clear();
            ConfigDictionary.PreloadWrap<K, V>(m_Map, asset, mono, out m_IsJson, 
                (IDictionary maps) => {
                    IConfigVoMap<K> ret = maps != null ? this : null;
                    if (onEnd != null)
                        onEnd(ret);
                }, onProcess);
            return true;
        }

        public bool Preload(byte[] buffer, UnityEngine.MonoBehaviour mono, 
            Action<IConfigVoMap<K>> onEnd, Action<float> onProcess) {
            if (buffer == null || mono == null)
                return false;
            if (m_Map == null)
                m_Map = new Dictionary<K, V>();
            else
                m_Map.Clear();
            ConfigDictionary.PreloadWrap<K, V>(m_Map, buffer, mono, out m_IsJson,
                (IDictionary maps) => {
                    IConfigVoMap<K> ret = maps != null ? this : null;
                    if (onEnd != null)
                        onEnd(ret);
                }, onProcess);
            return true;
        }
    }

    public class ConfigVoMapMap<K1, K2, V>: IConfigVoMap<K1> where V : ConfigBase<K2> {
        private bool m_IsJson = true;
        private Dictionary<K1, Dictionary<K2, V>> m_Map = null;
		
		public void Clear() {
            if (m_Map != null)
                m_Map.Clear();
            m_IsJson = true;
        }

        public bool IsJson {
            get {
                return m_IsJson;
            }
        }

        public bool ContainsKey(K1 key) {
            if (m_Map == null)
                return false;
            return m_Map.ContainsKey(key);
        }

        public bool TryGetValue(K1 key, out Dictionary<K2, V> value) {
            value = null;
            if (m_Map == null)
                return false;
            if (m_IsJson) {
                return m_Map.TryGetValue(key, out value);
            }
            if (!ConfigWrap.ConfigTryGetValue<K1, K2, V>(m_Map, key, out value)) {
                value = null;
                return false;
            }
            return true;
        }

        public bool Preload(byte[] buffer, UnityEngine.MonoBehaviour mono,
            Action<IConfigVoMap<K1>> onEnd, Action<float> onProcess) {

            if (buffer == null || mono == null || buffer.Length <= 0)
                return false;
            if (m_Map == null)
                m_Map = new Dictionary<K1, Dictionary<K2, V>>();
            else
                m_Map.Clear();
            ConfigDictionary.PreloadWrap<K1, K2, V>(m_Map, buffer, mono, out m_IsJson,
                (IDictionary maps) => {
                    IConfigVoMap<K1> ret = maps != null ? this : null;
                    if (onEnd != null)
                        onEnd(ret);
                }, onProcess);
            return true;

        }

        public bool Preload(TextAsset asset, UnityEngine.MonoBehaviour mono,
            Action<IConfigVoMap<K1>> onEnd, Action<float> onProcess) {
            if (asset == null || mono == null)
                return false;
            if (m_Map == null)
                m_Map = new Dictionary<K1, Dictionary<K2, V>>();
            else
                m_Map.Clear();
            ConfigDictionary.PreloadWrap<K1, K2, V>(m_Map, asset, mono, out m_IsJson,
                (IDictionary maps) => {
                    IConfigVoMap<K1> ret = maps != null ? this : null;
                    if (onEnd != null)
                        onEnd(ret);
                }, onProcess);
            return true;
        }

        public Dictionary<K2, V> this[K1 key] {
            get {
                if (m_Map == null)
                    return null;

                Dictionary<K2, V> ret;
                if (m_IsJson) {
                    if (!m_Map.TryGetValue(key, out ret))
                        ret = null;
                    return ret;
                }
                if (!this.TryGetValue(key, out ret))
                    ret = null;
                return ret;
            }
        }

        public struct Enumerator {
            private bool m_IsJson;
            public Enumerator(bool isJson, IEnumerator<KeyValuePair<K1, Dictionary<K2, V>>> iter) {
                m_IsJson = isJson;
                Iteror = iter;
            }

            internal IEnumerator<KeyValuePair<K1, Dictionary<K2, V>>> Iteror {
                get;
                private set;
            }

            public KeyValuePair<K1, Dictionary<K2, V>> Current {
                get {
                    if (Iteror == null)
                        return new KeyValuePair<K1, Dictionary<K2, V>>();
                    if (m_IsJson) {
                        return Iteror.Current;
                    }
                    Dictionary<K2, V> map = Iteror.Current.Value;
                    var iter = map.GetEnumerator();
                    try {
                        if (iter.MoveNext()) {
                            V config = iter.Current.Value;
                            if (config.IsReaded)
                                return Iteror.Current;
                            config.StreamSeek();
                            config.ReadValue();

                            while (iter.MoveNext()) {
                                config = iter.Current.Value;
                                config.ReadValue();
                            }

                        } else {
                            return new KeyValuePair<K1, Dictionary<K2, V>>();
                        }
                    } finally {
                        iter.Dispose();
                    }
                    return Iteror.Current;
                }
            }

            public void Dispose() {
                if (Iteror == null)
                    return;
                Iteror.Dispose();
            }
            public bool MoveNext() {
                if (Iteror == null)
                    return false;
                return Iteror.MoveNext();
            }
        }

        public Enumerator GetEnumerator() {
            if (m_Map == null)
                return new Enumerator();
            var iter = m_Map.GetEnumerator();
            Enumerator ret = new Enumerator(m_IsJson, iter);
            return ret;
        }

        public bool LoadFromTextAsset(TextAsset asset, bool isLoadAll = false) {
            if (asset == null)
                return false;
            m_Map = ConfigDictionary.ToWrapMap<K1, K2, V>(asset, out m_IsJson, isLoadAll);
            return m_Map != null;
        }

        public bool LoadFromBytes(byte[] buffer, bool isLoadAll = false) {
            if (buffer == null || buffer.Length <= 0)
                return false;
            m_Map = ConfigDictionary.ToWrapMap<K1, K2, V>(buffer, out m_IsJson, isLoadAll);
            return m_Map != null;
        }

        public List<Dictionary<K2, V>> ValueList {
            get {
                if (m_Map == null)
                    return null;
                List<Dictionary<K2, V>> ret = m_Map.Values.ToList();
                if (m_IsJson)
                    return ret;
                if (ret != null) {
                    for (int i = 0; i < ret.Count; ++i) {
                        Dictionary<K2, V> vs = ret[i];
                        if (vs != null && vs.Count > 0) {
                            var iter = vs.GetEnumerator();
                            if (iter.MoveNext()) {
                                V v = iter.Current.Value;
                                if (v.IsReaded)
                                    continue;
                                v.StreamSeek();
                                v.ReadValue();
                                while (iter.MoveNext()) {
                                    v = iter.Current.Value;
                                    v.ReadValue();
                                }
                            }
                            iter.Dispose();
                        }
                    }
                }

                return ret;
            }
        }
    }

    public class ConfigVoListMap<K, V> : IConfigVoMap<K> where V : ConfigBase<K> {

        private bool m_IsJson = true;
        private Dictionary<K, List<V>> m_Map = null;
		
		public void Clear() {
            if (m_Map != null)
                m_Map.Clear();
            m_IsJson = true;
        }

        public List<List<V>> ValueList {
            get {
                if (m_Map == null)
                    return null;
                List<List<V>> ret = m_Map.Values.ToList();
                if (m_IsJson)
                    return ret;
                if (ret != null) {
                    
                    for (int i = 0; i < ret.Count; ++i) {
                        List<V> vs = ret[i];
                        if (vs != null && vs.Count > 0) {
                            V v = vs[0];
                            if (v.IsReaded)
                                continue;
                            v.StreamSeek();
                            v.ReadValue();
                            for (int j = 1; j < vs.Count; ++j) {
                                v = vs[j];
                                v.ReadValue();
                            }
                        }
                    }

                }
                return ret;
            }
        }

        public struct Enumerator {
            private bool m_IsJson;
            public Enumerator(bool isJson, IEnumerator<KeyValuePair<K, List<V>>> iter) {
                m_IsJson = isJson;
                Iteror = iter;
            }

            internal IEnumerator<KeyValuePair<K, List<V>>> Iteror {
                get;
                private set;
            }

            public KeyValuePair<K, List<V>> Current {
                get {

                    if (Iteror == null)
                        return new KeyValuePair<K, List<V>>();

                    if (m_IsJson)
                        return Iteror.Current;

                    List<V> vs = Iteror.Current.Value as List<V>;
                    if (vs == null || vs.Count <= 0)
                        return new KeyValuePair<K, List<V>>();

                    var v = vs[0];
                    if (v.IsReaded)
                        return Iteror.Current;

                    v.StreamSeek();
                    v.ReadValue();

                    for (int i = 1; i < vs.Count; ++i) {
                        v = vs[i];
                        v.ReadValue();
                    }
                    return Iteror.Current;
                }
            }

            public void Dispose() {
                if (Iteror == null)
                    return;
                Iteror.Dispose();
            }
            public bool MoveNext() {
                if (Iteror == null)
                    return false;
                return Iteror.MoveNext();
            }
        }

        public Enumerator GetEnumerator() {
            if (m_Map == null)
                return new Enumerator();
            Enumerator ret = new Enumerator(m_IsJson, m_Map.GetEnumerator());
            return ret;
        }

        public bool IsJson {
            get {
                return m_IsJson;
            }
        }

        public bool ContainsKey(K key) {
            if (m_Map == null)
                return false;
            return m_Map.ContainsKey(key);
        }

        public bool TryGetValue(K key, out List<V> value) {
            value = null;
            if (m_Map == null)
                return false;
            if (m_IsJson) {
                return m_Map.TryGetValue(key, out value);
            }
            if (!ConfigWrap.ConfigTryGetValue<K, V>(m_Map, key, out value)) {
                value = null;
                return false;
            }
            return true;
        }

        public List<V> this[K key] {
            get {
                if (m_Map == null)
                    return null;

                List<V> ret;
                if (m_IsJson) {
                    if (!m_Map.TryGetValue(key, out ret))
                        ret = null;
                    return ret;
                }
                if (!this.TryGetValue(key, out ret))
                    ret = null;
                return ret;
            }
        }

        public bool LoadFromTextAsset(TextAsset asset, bool isLoadAll = false) {
            if (asset == null)
                return false;
            m_Map = ConfigDictionary.ToWrapList<K, V>(asset, out m_IsJson, isLoadAll);
            return m_Map != null;
        }

        public bool LoadFromBytes(byte[] buffer, bool isLoadAll = false) {
            if (buffer == null || buffer.Length <= 0)
                return false;
            m_Map = ConfigDictionary.ToWrapList<K, V>(buffer, out m_IsJson, isLoadAll);
            return m_Map != null;
        }

        public bool Preload(byte[] buffer, UnityEngine.MonoBehaviour mono, 
            Action<IConfigVoMap<K>> onEnd, Action<float> onProcess) {
            if (buffer == null || mono == null || buffer.Length <= 0)
                return false;
            if (m_Map == null)
                m_Map = new Dictionary<K, List<V>>();
            else
                m_Map.Clear();
            ConfigDictionary.PreloadWrap<K, V>(m_Map, buffer, mono, out m_IsJson,
                (IDictionary maps) => {
                    IConfigVoMap<K> ret = maps != null ? this : null;
                    if (onEnd != null)
                        onEnd(ret);
                }, onProcess);
            return true;
        }

        // 预加载
        public bool Preload(TextAsset asset, UnityEngine.MonoBehaviour mono, 
            Action<IConfigVoMap<K>> onEnd, Action<float> onProcess) {
            if (asset == null || mono == null)
                return false;
            if (m_Map == null)
                m_Map = new Dictionary<K, List<V>>();
            else
                m_Map.Clear();
            ConfigDictionary.PreloadWrap<K, V>(m_Map, asset, mono, out m_IsJson,
                (IDictionary maps) => {
                    IConfigVoMap<K> ret = maps != null ? this : null;
                    if (onEnd != null)
                        onEnd(ret);
                }, onProcess);
            return true;
        }
    }

}