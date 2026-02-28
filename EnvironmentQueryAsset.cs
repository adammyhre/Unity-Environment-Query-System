using System.Collections.Generic;
using UnityEngine;

namespace EQS {
    public enum AgentStrategy {
        Continuous,
        OnArrival
    }

    /// <summary>
    /// Asset definition of an environment query: one generator, multiple tests, and optional agent strategy (how the runner/agent use this query).
    /// </summary>
    [CreateAssetMenu(menuName = "EQS/Query", fileName = "EQS_Query")]
    public class EnvironmentQueryAsset : ScriptableObject {
        #region Fields

        [SerializeField] [Tooltip("Produces candidate points. Required.")]
        Generators.EQSGeneratorBase generator;

        [SerializeField] [Tooltip("Tests applied to each point; scores are multiplied. Order does not affect result.")]
        List<Tests.EQSTestBase> tests = new List<Tests.EQSTestBase>();

        [Header("Agent strategy")]
        [SerializeField]
        [Tooltip(
            "Continuous: re-evaluate every frame, switch only if better. OnArrival: re-query after reaching each point + wait.")]
        AgentStrategy agentStrategy = AgentStrategy.Continuous;

        [SerializeField] [Tooltip("For OnArrival: seconds to wait at destination before choosing next point.")]
        float waitDuration = 1f;

        [SerializeField] [Tooltip("For Continuous: run query once and stick to that point (e.g. Find Cover).")]
        bool runOnce = false;

        #endregion

        public IEQSGenerator Generator => generator;
        public IReadOnlyList<IEQSTest> Tests => tests;
        public AgentStrategy Strategy => agentStrategy;
        public float WaitDuration => waitDuration;
        public bool RunOnce => runOnce;

        /// <summary>
        /// Run this query with the given context. Returns best point and full candidate list for debugging.
        /// </summary>
        public EQSRunResult Run(EQSContext context) {
            return EQSExecutor.Run(this, context);
        }
    }
}