

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

namespace NDRSquad
{
    public enum FormationType
    {
        Line,
        Column,
        Vee,
        Wedge,
        Irregular,
        Circular
    }

    public class SquadMovement : MonoBehaviour
    {
        [SerializeField] private FormationType formationType;
        [SerializeField] private float unitSpacing = 1.5f;

        [SerializeField] private Unit[] units;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] Transform target;
        private Vector3[] targetPositions;
        private Vector3 formationDirection;
        private Vector3 averagePosition;

        private Dictionary<FormationType, Vector3> formationDirections = new Dictionary<FormationType, Vector3>
{
    { FormationType.Line, Vector3.forward },
    { FormationType.Column, Vector3.right },
    { FormationType.Vee, Vector3.Cross(Vector3.up, Vector3.forward).normalized },
    { FormationType.Wedge, Vector3.Cross(Vector3.up, -Vector3.forward).normalized }
};
        private Dictionary<FormationType, System.Func<int, Vector3>> formationCalculators;


        private void Awake()
        {
            targetPositions = new Vector3[units.Length];
            formationCalculators = new Dictionary<FormationType, System.Func<int, Vector3>>
            {
                {FormationType.Line, CalculateLineFormation},
                {FormationType.Column, CalculateColumnFormation},
                {FormationType.Vee, CalculateVeeFormation},
                {FormationType.Wedge, CalculateWedgeFormation},
                {FormationType.Irregular, CalculateIrregularFormation},
                {FormationType.Circular, CalculateCircular}
            };
        }
        private void Start()
        {
            // Check if units and navmesh agent are properly assigned
            if (units == null || units.Length == 0)
            {
                Debug.LogError("SquadMovement: Units array is not assigned or is empty.");
                return;
            }

            if (!agent)
            {
                Debug.LogError("SquadMovement: NavMeshAgent is not assigned.");
                return;
            }

        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                SetDestinaion(target.position);
        }

        private void SetDestinaion(Vector3 newPos)
        {
            float unitDistance = 2f;

            for (int i = 0; i < units.Length; i++)
            {

                Vector3 targetPos = Vector3.zero;
                CalculateFormationDirection();

                targetPos = CalculateFormationPosition(i, targetPos);

                Vector3 offset = (targetPos - newPos);
                offset = offset.normalized * unitDistance;

                // Apply the offset to the target position
                targetPos = newPos + offset + formationDirection * i * unitSpacing;

                // Set the target position for the unit
                targetPositions[i] = targetPos;
                units[i].SetDestination(targetPos);
            }

            // Calculate the average position of all units
            CalculateAveragePosition();
        }

        private Vector3 CalculateFormationPosition(int i, Vector3 targetPos)
        {
            System.Func<int, Vector3> formationCalculator = formationCalculators[formationType];
            return formationCalculator(i);
        }

        private Vector3 CalculateLineFormation(int i)
        {
            return transform.position + formationDirection * i * unitSpacing;
        }

        private Vector3 CalculateColumnFormation(int i)
        {
            return transform.position + formationDirection * i * unitSpacing;
        }

        private Vector3 CalculateVeeFormation(int i)
        {
            return transform.position + formationDirection * i * unitSpacing +
                                                (i % 2 == 0 ? Vector3.left : Vector3.right) * unitSpacing / 2f;
        }

        private Vector3 CalculateIrregularFormation(int i)
        {
            return averagePosition + new Vector3(Random.Range(-10f,
                                        10f), 0, Random.Range(-10f, 10f));
        }

        private Vector3 CalculateCircular(int i)
        {
            Vector3 targetPos;
            float radius = unitSpacing * units.Length / (2f * Mathf.PI);
            float angle = i * (360f / units.Length);
            targetPos = averagePosition + RotateY(formationDirection * radius, angle);
            return targetPos;
        }

        private Vector3 CalculateWedgeFormation(int i)
        {
            Vector3 targetPos;
            float wedgeAngle = 30f; // adjust the angle as needed
            float radius = unitSpacing * units.Length / (2 * Mathf.Sin(wedgeAngle * Mathf.Deg2Rad));

            float angle = i * wedgeAngle - (units.Length - 1) * wedgeAngle / 2f;
            targetPos = averagePosition + RotateY(formationDirection * radius, angle);
            return targetPos;
        }

        private void CalculateAveragePosition()
        {
            averagePosition = Vector3.zero;
            for (int i = 0; i < units.Length; i++)
                averagePosition += units[i].transform.position;

            averagePosition /= units.Length;

            agent.SetDestination(averagePosition);
        }

        private void CalculateFormationDirection()
        {
            if (formationDirections.TryGetValue(formationType, out Vector3 direction))
                formationDirection = direction;
            else
                Debug.LogWarning("Formation direction not defined for type: " + formationType);
        }

        public void SetFormationType(FormationType type)
        {
            formationType = type;
        }

        public void SetUnitSpacing(float spacing)
        {
            unitSpacing = spacing;
        }

        public static Vector3 RotateY(Vector3 vector, float angleDegrees)
        {
            float angleRadians = angleDegrees * Mathf.Deg2Rad;
            float sin = Mathf.Sin(angleRadians);
            float cos = Mathf.Cos(angleRadians);
            return new Vector3(vector.x * cos + vector.z * sin, vector.y, -vector.x * sin + vector.z * cos);
        }
    }
}