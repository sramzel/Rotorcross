using RobinTheilade.Framework;
using System;
using UnityEngine;

namespace RobinTheilade.RuntimeTreeCollision
{
    /// <summary>
    /// Provides a reference to the tree that was collided with.
    /// </summary>
    [AddComponentMenu("Physics/Runtime Tree Colliders - Tree Instance Info")]
    [RequireComponent(typeof(CapsuleCollider))]
    public class TreeInstanceInfo : MonoBehaviour
    {
        /// <summary>
        /// The reference to the tree that was collided with.
        /// </summary>
        [Tooltip("The current tree the collider is applied to.")]
        public TreeInstance treeInstance;

        /// <summary>
        /// The reference to the terrain the tree belongs to.
        /// </summary>
        [Tooltip("The terrain the tree belongs to.")]
        public Terrain terrain;

        /// <summary>
        /// Gets the prototype of the tree.
        /// </summary>
        public TreePrototype TreePrototype
        {
            get
            {
                return this.terrain.terrainData.treePrototypes[this.treeInstance.prototypeIndex];
            }
        }

        /// <summary>
        /// Gets the index of the instance in the <see cref="F:UnityEngine.TerrainData.treeInstances"/> array.
        /// </summary>
        public int TreeInstanceIndex
        {
            get
            {
                return Array.FindIndex(
                    terrain.terrainData.treeInstances,
                    i => i.Same(treeInstance)
                    );
            }
        }
    }
}
