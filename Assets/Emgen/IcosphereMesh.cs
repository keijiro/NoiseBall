using UnityEngine;

namespace Emgen
{
    public class IcosphereMesh : ScriptableObject
    {
        #region Public Properties

        [SerializeField, Range(0, 5)]
        int _subdivisionLevel = 1;

        public int subdivisionLevel {
            get { return _subdivisionLevel; }
        }

        [SerializeField]
        bool _splitVertices = true;

        public bool splitVertices {
            get { return _splitVertices; }
        }

        [SerializeField, HideInInspector]
        Mesh _mesh;

        public Mesh sharedMesh {
            get { return _mesh; }
        }

        #endregion

        #region Public Methods

        public void RebuildMesh()
        {
            if (_mesh == null)
            {
                Debug.LogError("Mesh asset is missing.");
                return;
            }

            _mesh.Clear();

            var builder = new Emgen.IcosphereBuilder();
            for (var i = 0; i < _subdivisionLevel; i++) builder.Subdivide();

            var vc = builder.vertexCache;
            if (_splitVertices)
            {
                _mesh.vertices = vc.MakeVertexArrayForFlatMesh();
                _mesh.SetIndices(vc.MakeIndexArrayForFlatMesh(), MeshTopology.Triangles, 0);
            }
            else
            {
                _mesh.vertices = vc.MakeVertexArrayForSmoothMesh();
                _mesh.SetIndices(vc.MakeIndexArrayForSmoothMesh(), MeshTopology.Triangles, 0);
            }
            _mesh.RecalculateNormals();
        }

        #endregion

        #region ScriptableObject Functions

        void OnEnable()
        {
            if (_mesh == null)
            {
                _mesh = new Mesh();
                _mesh.name = "Icosphere Mesh";
            }
        }

        #endregion
    }
}
