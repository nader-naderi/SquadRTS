
using System.Collections.Generic;

using UnityEngine;

namespace NDRSquad
{
    public interface IFormable
    {
        void SetPositions(List<Transform> units, Transform target);
    }

    public class LineFormation : IFormable
    {
        public void SetPositions(List<Transform> units, Transform target)
        {
            int count = units.Count;

            for (int i = 0; i < count; i++)
            {
                Vector3 offset = new Vector3(i - (count - 1) / 2f, 0, 0);
                units[i].position = target.position + offset * 2f;
            }
        }
    }

    public class WedgeFormation : IFormable
    {
        public void SetPositions(List<Transform> units, Transform target)
        {
            int count = units.Count;
            float radius = count * 1.5f;
            float angle = 90f / (count - 1);

            for (int i = 0; i < count; i++)
            {
                Vector3 offset = new Vector3(Mathf.Sin(angle * i * Mathf.Deg2Rad), 0, Mathf.Cos(angle * i * Mathf.Deg2Rad)) * radius;
                units[i].position = target.position + offset;
            }
        }
    }


    public class CircleFormation : IFormable
    {
        public void SetPositions(List<Transform> units, Transform target)
        {
            int count = units.Count;
            float angle = 360f / count;

            for (int i = 0; i < count; i++)
            {
                Vector3 offset = Quaternion.Euler(0, angle * i, 0) * Vector3.forward;
                units[i].position = target.position + offset * 2f;
            }
        }
    }

    public class SquareFormation : IFormable
    {
        public void SetPositions(List<Transform> units, Transform target)
        {
            int count = units.Count;
            int rowLength = Mathf.CeilToInt(Mathf.Sqrt(count));
            int columnLength = Mathf.CeilToInt((float)count / rowLength);

            for (int i = 0; i < count; i++)
            {
                int row = i % rowLength;
                int column = i / rowLength;

                Vector3 offset = new Vector3(row - (rowLength - 1) / 2f, 0, column - (columnLength - 1) / 2f);
                units[i].position = target.position + offset * 2f;
            }
        }
    }

    public class ColumnFormation : IFormable
    {
        public void SetPositions(List<Transform> units, Transform target)
        {
            int count = units.Count;

            for (int i = 0; i < count; i++)
            {
                Vector3 offset = new Vector3(0, 0, i);
                units[i].position = target.position + offset * 2f;
            }
        }
    }

    public class IrregularFormation : IFormable
    {
        public void SetPositions(List<Transform> units, Transform target)
        {
            int count = units.Count;

            for (int i = 0; i < count; i++)
            {
                Vector3 offset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                units[i].position = target.position + offset * 2f;
            }
        }
    }
}
