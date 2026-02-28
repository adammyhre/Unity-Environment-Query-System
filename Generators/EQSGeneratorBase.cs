using UnityEngine;
using System.Collections.Generic;

namespace EQS.Generators {
    /// <summary>
    /// Base for asset-based EQS generators. Create assets via CreateAssetMenu and assign to an EnvironmentQuery.
    /// </summary>
    public abstract class EQSGeneratorBase : ScriptableObject, IEQSGenerator {
        public abstract IReadOnlyList<QueryPoint> Generate(EQSContext context);
    }
}