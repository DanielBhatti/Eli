namespace Eli.Math;

public class MarkovChain<TState>(Dictionary<TState, Dictionary<TState, double>> transitions) where TState : notnull
{
    public MarkovChain() : this(new Dictionary<TState, Dictionary<TState, double>>()) { }

    private Dictionary<TState, Dictionary<TState, double>> Transitions { get; } = transitions;
    private Random Random { get; } = new Random();

    public void AddTransition(TState fromState, TState toState, double probability)
    {
        if(!Transitions.ContainsKey(fromState)) Transitions[fromState] = new Dictionary<TState, double>();
        Transitions[fromState][toState] = probability;
    }

    public void AddTransitionsFromMatrix(TState[] states, double[,] matrix)
    {
        if(states.Length != matrix.GetLength(0) || states.Length != matrix.GetLength(1)) throw new ArgumentException("The size of the states array must match the dimensions of the matrix.");
        for(var i = 0; i < states.Length; i++) for(var j = 0; j < states.Length; j++) AddTransition(states[i], states[j], matrix[i, j]);
    }

    public TState NextState(TState currentState)
    {
        if(!Transitions.ContainsKey(currentState)) throw new Exception("No transitions defined for the current state.");

        var roll = Random.NextDouble();
        var cumulative = 0.0;

        foreach(var transition in Transitions[currentState])
        {
            cumulative += transition.Value;
            if(roll < cumulative) return transition.Key;
        }
        throw new Exception("Invalid transition probabilities.");
    }

    public TState PredictStateAfterSteps(TState initialState, int steps)
    {
        var currentState = initialState;
        for(var i = 0; i < steps; i++) currentState = NextState(currentState);
        return currentState;
    }

    public static MarkovChain<int> FromSequence(IEnumerable<decimal> sequence, decimal binWidth)
    {
        var chain = new MarkovChain<int>();

        int getBinCenter(decimal decimalValue)
        {
            var binNumber = (int)decimal.Floor(decimalValue / binWidth);
            var centralValue = binNumber * binWidth + binWidth / 2;
            return (int)centralValue;
        }

        var binnedSequence = sequence.Select(getBinCenter).ToList();
        var distinctStates = new HashSet<int>(binnedSequence);
        var stateIndex = distinctStates.OrderBy(x => x).ToList();

        var matrix = new double[stateIndex.Count, stateIndex.Count];
        var transitionCount = binnedSequence.Count - 1;

        for(var i = 0; i < transitionCount; i++)
        {
            var fromIndex = stateIndex.IndexOf(binnedSequence[i]);
            var toIndex = stateIndex.IndexOf(binnedSequence[i + 1]);
            matrix[fromIndex, toIndex]++;
        }

        for(var i = 0; i < stateIndex.Count; i++)
        {
            var rowSum = matrix.Cast<double>().Skip(i * stateIndex.Count).Take(stateIndex.Count).Sum();
            for(var j = 0; j < stateIndex.Count; j++) matrix[i, j] /= rowSum;
        }

        for(var i = 0; i < stateIndex.Count; i++) for(var j = 0; j < stateIndex.Count; j++) chain.AddTransition(stateIndex[i], stateIndex[j], matrix[i, j]);

        return chain;
    }
}
