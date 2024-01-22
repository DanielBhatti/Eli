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
}
