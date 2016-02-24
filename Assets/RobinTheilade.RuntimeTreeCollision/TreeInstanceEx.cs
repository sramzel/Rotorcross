using UnityEngine;

/// <summary>
/// Provides a set of helper methods for working with the <see cref="T:UnityEngine.TreeInstance"/> class.
/// </summary>
public static class TreeInstanceEx
{
    /// <summary>
    /// Compares two instances of <see cref="T:UnityEngine.TreeInstance"/>.
    /// </summary>
    /// <param name="instance1">
    /// One of the <see cref="T:UnityEngine.TreeInstance"/>.
    /// </param>
    /// <param name="instance2">
    /// The other <see cref="T:UnityEngine.TreeInstance"/>.
    /// </param>
    /// <returns>
    /// true if the state of both instances are the same; otherwise false.
    /// </returns>
    public static bool Same(this TreeInstance instance1, TreeInstance instance2)
    {
        // comparison ordered by what fields are most likely to be different
        return
            instance1.position == instance2.position &&
            instance1.prototypeIndex == instance2.prototypeIndex &&
            instance1.heightScale == instance2.heightScale &&
            instance1.widthScale == instance2.widthScale &&
			instance1.color.Equals(instance2.color) &&
			instance1.lightmapColor.Equals(instance2.lightmapColor)
            ;
    }
}
