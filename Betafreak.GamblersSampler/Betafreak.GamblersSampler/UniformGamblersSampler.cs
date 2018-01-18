using System;
using System.Collections.Generic;
using System.Text;

namespace Betafreak.GamblersSampler
{
    /// <summary>
    /// An implementation of <see cref="ISampler(T)"/> for a small set of outcomes with the same asymptotic probability.
    /// </summary>
    /// <typeparam name="T">The datatype of the outcomes</typeparam>
    public class UniformGamblersSampler<T> : ISampler<T>
    {
        private class Node
        {
            public bool IsLeaf;
            public T Outcome;
            public double StartingTotal;
            public double CurrentTotal;
            public double Division;
            public Node Lower;
            public Node Upper;

            private Node() { }

            public static Node ChildNode(T outcome, double weight)
            {
                Node node = new Node();
                node.IsLeaf = true;
                node.Outcome = outcome;
                node.StartingTotal = weight;
                node.CurrentTotal = weight;
                return node;
            }

            public static Node ParentNode(Node lower, Node upper)
            {
                Node node = new Node();
                node.IsLeaf = false;
                node.StartingTotal = lower.StartingTotal + upper.StartingTotal;
                node.CurrentTotal = lower.CurrentTotal + upper.CurrentTotal;
                node.Division = lower.CurrentTotal;
                node.Lower = lower;
                node.Upper = upper;
                return node;
            }

            public override string ToString()
            {
                if (IsLeaf)
                {
                    return $"Leaf: {Outcome.ToString()} ({CurrentTotal} vs. {StartingTotal})";
                }
                else
                {
                    return $"Branch: {Division}/{CurrentTotal - Division} ({CurrentTotal} vs. {StartingTotal})";
                }
            }
        }

        private Random random;
        private Node root;
        private double severity;

        public UniformGamblersSampler(IEnumerable<T> outcomes, double severity)
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
                if (node.Division < node.CurrentTotal / 2)
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
            double startingTotal = root.CurrentTotal;
            double pos = random.NextDouble() * startingTotal;
            root = Next_Internal(root, pos, out outcome);

            if (root.CurrentTotal < 0)
            {
                throw new InvalidOperationException();
            }
            if (root.CurrentTotal < root.StartingTotal / 2
                || root.CurrentTotal > root.StartingTotal * 2)
            {
                Scale_Node(root, root.StartingTotal / root.CurrentTotal);
            }

            return outcome;
        }

        private Node Next_Internal(Node node, double pos, out T outcome)
        {
            double startingTotal = node.CurrentTotal;
            if (node.IsLeaf)
            {
                outcome = node.Outcome;
                double weightDifference = node.CurrentTotal - (node.CurrentTotal * severity);
                if (weightDifference > node.CurrentTotal) weightDifference = node.CurrentTotal;
                node.CurrentTotal -= weightDifference;
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
                node.CurrentTotal = node.Lower.CurrentTotal + node.Upper.CurrentTotal;
                node.Division = node.Lower.CurrentTotal;
            }
            return node;
        }

        private void Scale_Node(Node node, double scaleFactor)
        {
            node.CurrentTotal *= scaleFactor;
            node.Division *= scaleFactor;
            if (!node.IsLeaf)
            {
                Scale_Node(node.Lower, scaleFactor);
                Scale_Node(node.Upper, scaleFactor);
            }
        }

        public void Force(T outcome)
        {
            throw new NotImplementedException();
        }
    }
}
