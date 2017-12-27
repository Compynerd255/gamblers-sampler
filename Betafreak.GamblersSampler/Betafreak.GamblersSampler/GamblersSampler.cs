using System;
using System.Collections.Generic;
using System.Text;

namespace Betafreak.GamblersSampler
{
    /// <summary>
    /// General implementation of <see cref="ISampler"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GamblersSampler<T> : ISampler<T>
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
                    return $"Leaf:{Outcome.ToString()}: {Total}";
                }
                else
                {
                    return $"Branch:{Division}/{Total-Division}({Total})";
                }
            }
        }

        private Random random;
        private Node root;
        private double severity;

        public GamblersSampler(IEnumerable<T> outcomes, double severity)
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

        public GamblersSampler(IEnumerable<WeightedOutcome<T>> outcomes, double severity)
        {
            Initialize(outcomes, severity);
        }

        public GamblersSampler(SamplerExportState<T> state)
        {
            throw new NotImplementedException();
        }

        private void Initialize(IEnumerable<WeightedOutcome<T>> outcomes, double severity)
        {
            foreach (var outcome in outcomes)
            {
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
            // Get the sample
            Node node = root;
            double pos = random.NextDouble() * root.Total;
            while (!node.IsLeaf)
            {
                if (pos < node.Division)
                {
                    node = node.Lower;
                }
                else
                {
                    pos -= node.Division;
                    node = node.Upper;
                }
            }
            T result = node.Outcome;

            // TODO: Modify the tree when you get the sample

            return result;
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
