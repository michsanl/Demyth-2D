// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using UnityEngine.AI;

namespace PixelCrushers
{

    /// <summary>
    /// Saves a GameObject's position.
    /// </summary>
    [AddComponentMenu("")] // Use wrapper instead.
    public class TransformSaver : Saver
    {

        [Tooltip("If set, save position of target. Otherwise save this GameObject's position.")]
        [SerializeField]
        private Transform m_target = null;

        [Tooltip("When changing scenes, if a player spawnpoint is specified, move this GameObject to the spawnpoint.")]
        [SerializeField]
        private bool m_usePlayerSpawnpoint = false;

        [Tooltip("Record positions in every scene. If unticked, only records position in most recent scene.")]
        [SerializeField]
        private bool m_multiscene = false;

        [Serializable]
        public class TransformData
        {
            public int scene = -1;
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 scale;
        }

        [Serializable]
        public class SceneTransformData
        {
            public int scene;
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 scale;
            public SceneTransformData(int _scene, Vector3 _position, Quaternion _rotation, Vector3 _scale)
            {
                scene = _scene;
                position = _position;
                rotation = _rotation;
                scale = _scale;
            }
        }

        [Serializable]
        public class MultisceneTransformData
        {
            public List<SceneTransformData> positions = new List<SceneTransformData>();
        }

        protected TransformData m_data;
        protected MultisceneTransformData m_multisceneData;
        protected NavMeshAgent m_navMeshAgent;

        public Transform target
        {
            get { return (m_target == null) ? this.transform : m_target; }
            set { m_target = value; }
        }

        public bool usePlayerSpawnpoint
        {
            get { return m_usePlayerSpawnpoint; }
            set { m_usePlayerSpawnpoint = value; }
        }

        protected bool multiscene { get { return m_multiscene; } }

        public override void Awake()
        {
            base.Awake();
            if (m_multiscene) m_multisceneData = new MultisceneTransformData();
            else m_data = new TransformData();
            m_navMeshAgent = target.GetComponent<NavMeshAgent>();
        }

        public override string RecordData()
        {
            var currentScene = SceneManager.GetActiveScene().buildIndex;
            if (multiscene)
            {
                var found = false;
                for (int i = 0; i < m_multisceneData.positions.Count; i++)
                {
                    if (m_multisceneData.positions[i].scene == currentScene)
                    {
                        found = true;
                        m_multisceneData.positions[i].position = target.transform.position;
                        m_multisceneData.positions[i].rotation = target.transform.rotation;
                        m_multisceneData.positions[i].scale = target.transform.localScale;
                        break;
                    }
                }
                if (!found)
                {
                    m_multisceneData.positions.Add(new SceneTransformData(currentScene, target.transform.position, target.transform.rotation, target.transform.localScale));
                }
                return SaveSystem.Serialize(m_multisceneData);
            }
            else
            {
                m_data.scene = currentScene;
                m_data.position = target.transform.position;
                m_data.rotation = target.transform.rotation;
                m_data.scale = target.transform.localScale;
                return SaveSystem.Serialize(m_data);
            }
        }

        public override void ApplyData(string s)
        {
            if (usePlayerSpawnpoint && SaveSystem.playerSpawnpoint != null)
            {
                SetPosition(SaveSystem.playerSpawnpoint.transform.position, SaveSystem.playerSpawnpoint.transform.rotation, SaveSystem.playerSpawnpoint.transform.localScale);
            }
            else if (!string.IsNullOrEmpty(s))
            {
                var currentScene = SceneManager.GetActiveScene().buildIndex;
                if (multiscene)
                {
                    var multisceneData = SaveSystem.Deserialize<MultisceneTransformData>(s, m_multisceneData);
                    if (multisceneData == null) return;
                    m_multisceneData = multisceneData;
                    for (int i = 0; i < m_multisceneData.positions.Count; i++)
                    {
                        if (m_multisceneData.positions[i].scene == currentScene)
                        {
                            SetPosition(m_multisceneData.positions[i].position, m_multisceneData.positions[i].rotation, m_multisceneData.positions[i].scale);
                            break;
                        }
                    }
                }
                else
                {
                    var data = SaveSystem.Deserialize<TransformData>(s, m_data);
                    if (data == null) return;
                    m_data = data;
                    SetPosition(data.position, data.rotation, data.scale);
                }
            }
        }

        protected virtual void SetPosition(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            if (m_navMeshAgent != null)
            {
                m_navMeshAgent.Warp(position);
            }
            else
            {
                target.transform.position = position;
            }
            target.transform.rotation = rotation;
            target.transform.localScale = scale;
            Debug.Log("asd");
        }

    }
}
