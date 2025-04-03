using System;
using Unity.AI.Navigation;
using UnityEngine;

namespace GameLogic
{
    public class SurfaceReBake : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface _surface;

        private void Awake()
        {
            _surface.BuildNavMesh();
        }
    }
}