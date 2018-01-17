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
            public int SampleCount;
            public Node Lower;
            public Node Upper;

            private Node() { }

            public static Node ChildNode(T outcome, double weight)
            {
                Node node = new Node();
                node.IsLeaf = true;
                node.Outcome = outcome;
                node.Total = weight;
                node.SampleCount = 0;
                return node;
            }

            public static Node ParentNode(Node lower, Node upper)
            {
                Node node = new Node();
                node.IsLeaf = false;
                node.Total = lower.Total + upper.Total;
                node.Division = lower.Total;
                node.SampleCount = lower.SampleCount + upper.SampleCount;
                node.Lower = lower;
                node.Upper = upper;
                return node;
            }

            public override string ToString()
            {
                string body;
                string footer;
                if (IsLeaf)
                {
                    body = $"Leaf: {Outcome.ToString()} ";
                }
                else
                {
                    body = $"Branch: {Division}/{Total-Division} ";
                }
                footer = $"({Total}, {SampleCount} times)";
                return body + footer;
            }
        }

        private Random random;
        private Node root;
        private double severity;

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
            T outcome;
            double pos = random.NextDouble() * root.Total;
            root = Next_Internal(root, pos, out outcome);

            return outcome;
        }

        private Node Next_Internal(Node node, double pos, out T outcome)
        {
            if (node.IsLeaf)
            {
                outcome = node.Outcome;
                node.SampleCount++;
            }
            else
            {
                /*
                 * TODO: Use the sample counts to weight against outcomes
                 * that have been sampled more often.
                 * 
                 * It may be observed that our old algorithm - multiplying the
                 * weight of the sampled unit by a constant - caused every
                 * outcome to be sampled with equal probability even though the
                 * actual probability values are being preserved in the tree.
                 * Our new algorithm, which will use these sample counts, must
                 * avoid this bias and actually sample units according to the
                 * uneven biases.
                 */
                if (pos < node.Division)
                {
                    node.Lower = Next_Internal(node.Lower, pos, out outcome);
                }
                else
                {
                    node.Upper = Next_Internal(node.Upper, pos - node.Division, out outcome);
                }
                node.SampleCount = node.Lower.SampleCount + node.Upper.SampleCount;
            }
            return node;
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
