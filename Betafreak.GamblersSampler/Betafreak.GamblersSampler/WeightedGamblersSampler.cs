using System;
using System.Collections.Generic;
using System.Text;

namespace Betafreak.GamblersSampler
{
    /// <summary>
    /// General implementation of <see cref="ISampler(T)"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WeightedGamblersSampler<T> : ISampler<T>
    {
        private class Node
        {
            public bool IsLeaf;
            public T Outcome;
            public double Total;
            public double Division;
            public Node Lower;
            public Node Upper;

            private Node() { }

            public static Node ChildNode(T outcome, double weight)
            {
                Node node = new Node();
                node.IsLeaf = true;
                node.Outcome = outcome;
                node.Total = weight;
                return node;
            }

            public static Node ParentNode(Node lower, Node upper)
            {
                Node node = new Node();
                node.IsLeaf = false;
                node.Total = lower.Total + upper.Total;
                node.Division = lower.Total;
                node.Lower = lower;
                node.Upper = upper;
                return node;
            }

            public override string ToString()
            {
                if (IsLeaf)
                {
                    return $"Leaf: {Outcome.ToString()} ({Total})";
                }
                else
                {
                    return $"Branch: {Division}/{Total - Division} ({Total})";
                }
            }
        }

        private Random random;
        private Node root;
        private double severity;
        private int outcomeCount;

        public WeightedGamblersSampler(IEnumerable<T> outcomes, double severity)
        {
            Initialize(ConvertToWeightedOutcomeSequence(outcomes), severity);
        }

        private static IEnumerable<WeightedOutcome<T>> ConvertToWeightedOutcomeSequence(IEnumerable<T> outcomes)
        {
            foreach (var t in outcomes)
            {
                yield return new WeightedOutcome<T>(t, 1);
            }
        }

        public WeightedGamblersSampler(IEnumerable<WeightedOutcome<T>> outcomes, double severity)
        {
            Initialize(outcomes, severity);
        }

        public WeightedGamblersSampler(SamplerExportState<T> state)
        {
            throw new NotImplementedException();
        }

        private void Initialize(IEnumerable<WeightedOutcome<T>> outcomes, double severity)
        {
            outcomeCount = 0;
            foreach (var outcome in outcomes)
            {
                outcomeCount++;
                root = AddOutcome(root, outcome);
            }
            if (root == null)
            {
                throw new ArgumentException("Outcomes should be non-empty");
            }
            this.severity = severity;
            this.random = new Random();
        }

        private Node AddOutcome(Node node, WeightedOutcome<T> outcome)
        {
            if (node == null)
            {
                node = Node.ChildNode(outcome.Outcome, outcome.Weight);
            }
            else if (node.IsLeaf)
            {
                node = Node.ParentNode(node, AddOutcome(null, outcome));
            }
            else
            {
                if (node.Division < node.Total / 2)
                {
                    node = Node.ParentNode(AddOutcome(node.Lower, outcome), node.Upper);
                }
                else
                {
                    node = Node.ParentNode(node.Lower, AddOutcome(node.Upper, outcome));
                }
            }
            return node;
        }

        public T Next()
        {
            T outcome;
            double startingTotal = root.Total;
            double pos = random.NextDouble() * startingTotal;
            root = Next_Internal(root, pos, out outcome);

            if (root.Total < 0)
            {
                throw new InvalidOperationException();
            }
            if (root.Total < outcomeCount / 2)
            {
                Scale_Node(root, 3);
            }
            if (root.Total > outcomeCount * 2)
            {
                Scale_Node(root, 1 / 3);
            }

            return outcome;
        }

        private Node Next_Internal(Node node, double pos, out T outcome)
        {
            double startingTotal = node.Total;
            if (node.IsLeaf)
            {
                outcome = node.Outcome;
                double weightDifference = node.Total - (node.Total * severity);
                if (weightDifference > node.Total) weightDifference = node.Total;
                node.Total -= weightDifference;
            }
            else
            {
                if (pos < node.Division)
                {
                    node.Lower = Next_Internal(node.Lower, pos, out outcome);
                }
                else
                {
                    node.Upper = Next_Internal(node.Upper, pos - node.Division, out outcome);
                }
                node.Total = node.Lower.Total + node.Upper.Total;
                node.Division = node.Lower.Total;
            }
            return node;
        }

        private void Scale_Node(Node node, double scaleFactor)
        {
            node.Total *= scaleFactor;
            node.Division *= scaleFactor;
            if (!node.IsLeaf)
            {
                Scale_Node(node.Lower, scaleFactor);
                Scale_Node(node.Upper, scaleFactor);
            }
        }

        public SamplerExportState<T> ExportState()
        {
            throw new NotImplementedException();
        }

        public void Force(T outcome)
        {
            throw new NotImplementedException();
        }
    }
}
