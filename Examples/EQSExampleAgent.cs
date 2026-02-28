using UnityEngine;
using UnityEngine.AI;
using UnityUtils;

namespace EQS.Examples {
    /// <summary>
    /// Runs an EQS query and moves the agent to the best point. Behavior, wait duration and run-once come from the assigned query asset.
    /// </summary>
    [RequireComponent(typeof(EnvironmentQueryRunner))]
    public class EQSExampleAgent : MonoBehaviour {
        #region Fields

        [SerializeField] [Tooltip("Stop when within this distance of the current goal.")]
        float stoppingDistance = 1.5f;

        [SerializeField] [Tooltip("Max distance to sample best point onto NavMesh.")]
        float navMeshSampleDistance = 5f;

        EnvironmentQueryRunner runner;
        NavMeshAgent agent;
        float currentGoalScore = -1f;
        bool ranOnce;
        float arrivedAtTime = -1f;
        bool hasSetDestination;

        const float SameDestinationEpsilon = 0.1f;

        #endregion

        protected void Awake() {
            runner = GetComponent<EnvironmentQueryRunner>();
            agent = gameObject.GetOrAdd<NavMeshAgent>();
        }

        protected void Update() {
            if (runner.Query == null || agent == null) return;

            if (runner.Query.Strategy == AgentStrategy.OnArrival) {
                UpdateOnArrival();
            } else {
                UpdateContinuous();
            }
        }

        void UpdateContinuous() {
            if (runner.Query.RunOnce && !ranOnce) {
                runner.RunQuery();
                ranOnce = true;
            }

            if (!TryGetBestNavMeshDestination(out Vector3 destination)) return;

            if (IsAtGoal(destination)) {
                currentGoalScore = -1f;
                return;
            }

            float bestScore = runner.LastResult.BestScore;
            if (bestScore <= currentGoalScore && currentGoalScore >= 0f) return;
            if (Vector3.Distance(agent.destination, destination) < SameDestinationEpsilon) return;
            currentGoalScore = bestScore;
            agent.SetDestination(destination);
        }

        void UpdateOnArrival() {
            bool atDestination = hasSetDestination && !agent.pathPending && agent.remainingDistance <= stoppingDistance;

            if (atDestination) {
                if (arrivedAtTime < 0f) {
                    arrivedAtTime = Time.time;
                }

                if (Time.time - arrivedAtTime >= runner.Query.WaitDuration) {
                    runner.RunQuery();
                    arrivedAtTime = -1f;
                }
            } else {
                arrivedAtTime = -1f;
            }

            if (!TryGetBestNavMeshDestination(out Vector3 destination)) return;
            if (IsAtGoal(destination)) return;

            if (Vector3.Distance(agent.destination, destination) >= SameDestinationEpsilon) {
                agent.SetDestination(destination);
                hasSetDestination = true;
            }

        }

        bool IsAtGoal(Vector3 goal) {
            return Vector3.Distance(transform.position, goal) <= stoppingDistance;
        }

        bool TryGetBestNavMeshDestination(out Vector3 destination) {
            destination = default;
            return runner.HasValidResult && TryGetNavMeshDestination(runner.BestPosition, out destination);
        }

        bool TryGetNavMeshDestination(Vector3 desired, out Vector3 destination) {
            destination = desired;
            if (!NavMesh.SamplePosition(desired, out NavMeshHit hit, navMeshSampleDistance, NavMesh.AllAreas)) return false;

            destination = hit.position;
            return true;
        }
    }
}