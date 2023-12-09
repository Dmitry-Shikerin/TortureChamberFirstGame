using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EzCombine : MonoBehaviour
{
    [System.Serializable]
    public class OptionsClass
    {
        [Tooltip("Size in wolrd space x and z")]
        public int chunkSize;
    }
    [System.Serializable]
    public class CombineClass
    {
        public string chunkName;
        [Tooltip("Set layer,  0 = Default")]
        public int layer;
        [Tooltip("Set tag")]
        public string tag;
        [Tooltip("Transforms with gameobjects to combine")]
        public Transform rootTransform;
        [Tooltip("Combine only static gameobjects")]
        public bool onlyStatic;
        [Tooltip("Remove gameobjects with collider")]
        public bool removeColliders;
        [Tooltip("Add MeshCollider to created objects")]
        public bool addMeshCollider;
    }
    [System.Serializable]
    public class CombinePack
    {
        public CombineClass combineClass;
        public Vector2 chunkKey;
        public Material material;
        public List<MeshRenderer> renderers;
        public List<MeshFilter> meshFilters;
    }

    Dictionary<Vector2, Dictionary<Material, CombinePack>> combineDictionary = new Dictionary<Vector2, Dictionary<Material, CombinePack>>();
    [Tooltip("chunks gameobjects will be parent to this Transform")]
    public Transform chunksParent;
    private List<CombinePack> packs = new List<CombinePack>();
    public OptionsClass options;
    public CombineClass[] combine;
    public List<Transform> chunks = new List<Transform>();
    void Awake()
    {
        FCombine();
        combineDictionary.Clear();
        packs.Clear();
    }


    void FCombine()
    {
        int i = 0;
        Vector2 chunkKey = Vector2.zero;
        MeshFilter meshfilter = null;
        MeshRenderer meshrenderer = null;
        Material material = null;
        foreach (CombineClass c in combine)
        {
            for (int ch = 0; ch < c.rootTransform.childCount; ch++)
            {
                Transform child = c.rootTransform.GetChild(ch);
                if (!c.onlyStatic || child.gameObject.isStatic == true)
                {
                    chunkKey.x = Mathf.RoundToInt(child.position.x / options.chunkSize);
                    chunkKey.y = Mathf.RoundToInt(child.position.z / options.chunkSize);
                    meshfilter = child.GetComponent<MeshFilter>();
                    meshrenderer = child.GetComponent<MeshRenderer>();
                    if(meshrenderer && meshfilter)
                    {
                        material = meshrenderer.sharedMaterial;
                        if (combineDictionary.ContainsKey(chunkKey))
                        {
                            if (combineDictionary[chunkKey].ContainsKey(material))
                            {
                                combineDictionary[chunkKey][material].renderers.Add(meshrenderer);
                                combineDictionary[chunkKey][material].meshFilters.Add(meshfilter);
                            }
                            else
                            {
                                CombinePack pack = new CombinePack();
                                pack.renderers = new List<MeshRenderer>();
                                pack.meshFilters = new List<MeshFilter>();
                                pack.combineClass = c;
                                pack.material = material;
                                pack.renderers.Add(meshrenderer);
                                pack.meshFilters.Add(meshfilter);
                                pack.chunkKey = chunkKey;
                                packs.Add(pack);
                                combineDictionary[chunkKey].Add(material, pack);
                            }
                        }
                        else
                        {
                            Dictionary<Material, CombinePack> newDictionary = new Dictionary<Material, CombinePack>();
                            CombinePack pack = new CombinePack();
                            pack.renderers = new List<MeshRenderer>();
                            pack.meshFilters = new List<MeshFilter>();
                            pack.combineClass = c;
                            pack.material = material;
                            pack.renderers.Add(meshrenderer);
                            pack.meshFilters.Add(meshfilter);
                            pack.chunkKey = chunkKey;
                            packs.Add(pack);
                            newDictionary.Add(pack.material, pack);
                            combineDictionary.Add(chunkKey, newDictionary);
                        }
                    }
                }
            }
        }
        Dictionary<Vector2, Transform> chunkParents = new Dictionary<Vector2, Transform>();
        for (int p = 0; p < packs.Count; p++)
        {
            if (!chunkParents.ContainsKey(packs[p].chunkKey))
            {
                Transform newChunkParent = new GameObject().transform;
                newChunkParent.name = "Chunk " + Mathf.RoundToInt(packs[p].chunkKey.x) +"/"+ Mathf.RoundToInt(packs[p].chunkKey.y);
                newChunkParent.position = new Vector3(packs[p].chunkKey.x * options.chunkSize, 0, packs[p].chunkKey.y * options.chunkSize);
                chunkParents.Add(packs[p].chunkKey, newChunkParent);
                if (chunksParent)
                    newChunkParent.parent = chunksParent;
                chunks.Add(newChunkParent);
            }
            CombineInstance[] combineInstance = new CombineInstance[packs[p].meshFilters.Count];
            i = 0;
            GameObject newChunk = new GameObject();
            newChunk.isStatic = true;
            if (packs[p].combineClass.chunkName != "")
            {
                newChunk.name = packs[p].combineClass.chunkName;
            }
            else
            {
                newChunk.name = packs[p].material.name;
            }
            newChunk.transform.parent = chunkParents[packs[p].chunkKey];
            newChunk.transform.localPosition = Vector3.zero;
            if(packs[p].combineClass.tag != "")
            {
                newChunk.tag = packs[p].combineClass.tag;
            }
            newChunk.layer = packs[p].combineClass.layer;
            while (i < packs[p].meshFilters.Count)
            {
                combineInstance[i].mesh = packs[p].meshFilters[i].sharedMesh;
                Matrix4x4 matrix = new Matrix4x4();
                matrix.SetTRS(packs[p].meshFilters[i].transform.position - newChunk.transform.position, packs[p].meshFilters[i].transform.rotation, packs[p].meshFilters[i].transform.lossyScale);
                matrix.ValidTRS();
                combineInstance[i].transform = matrix;
                if (packs[p].combineClass.removeColliders)
                {
                    Destroy(packs[p].meshFilters[i].gameObject);
                }
                else
                {
                    Destroy(packs[p].meshFilters[i]);
                    Destroy(packs[p].renderers[i]);
                }
                i++;
            }
            MeshFilter mf = newChunk.AddComponent<MeshFilter>();
            MeshRenderer mr = newChunk.AddComponent<MeshRenderer>();
            mf.mesh.CombineMeshes(combineInstance);
            mr.material = packs[p].material;
            if (packs[p].combineClass.addMeshCollider)
            {
                MeshCollider mc = newChunk.AddComponent<MeshCollider>();
                mc.sharedMesh = mf.mesh;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Vector3 size = new Vector3(options.chunkSize,0, options.chunkSize);
        for (int i = 0; i < chunks.Count;i++)
        {
            Gizmos.DrawWireCube(chunks[i].position, size);
        }
    }
}
