// Feed-forward mutating neural network

using System.Collections.Generic; // for lists
using System;

public class NeuralNetwork_Craig
{
    private int[] layers;
    private float[][] neurons; // jagged array (aka 'n x m array') -> neurons = new float[ numOfLayers ][ nodesInLayer ];
    private float[][][] weights; // 3d jagged array -> weights = new float[ numOfLayers-1 ] [ connectionsBetweenLayer ] [ connectionToNode ]

    // ex. network
    // ────────────────────────────────────────
    //   L0     L1     L2     L3  -> layers = 4
    // a () \\- () \\- () \- ()
    // b () \-/ () \-/ () -/ () 
    // c () -// () -// () //
    // d () ///
    // ↑     w0    w1     w2      -> weights
    // └ nodes
    // ────────────────────────────────────────
    //
    // ex. neurons variable
    // ┌──           ──┐       
    // | [ a, b, c, d] | L0 [ neuron a, b, etc... ]  
    // | [ a, b, c, d] | L1 [ neuron a, b, etc... ]
    // | [ a, b, c]    | L2 ..
    // | [ a, b]       | L3 ..
    // └──           ──┘
    //
    //  ex. weights variable      
    // ┌──                                                  ──┐  
    // | { [ a, b, c, d ] , [ a, b, c, d ] , [ a, b, c, d ] } | W0 (3 x 4 connections) { connections to L1(a) [ L0(a) -> L1(a), L0(b), L0(c), L0(d) ] , to L1(b) [  L0(a) -> L1(b), L0(b), L0(c), ... ] , to L1(c) [ L0(a), L0(b), ... ] }
    // | { [ a, b, c ] , [ a, b, c ] , [ a, b, c ] }          | W1 (3 x 3) { connections to L2(a) [ L1(a), L1(b), L1(c) ] , to L2(b) [ L1(a), ... ] , ... }
    // | { [ a, b, c ] , [ a, b, c ] }                        | W2 (2 x 3) { to L3(a) [ L2(a), L2(b), ... ] , to L3(b) [ ... ] }
    // └──                                                  ──┘
    // 
    // alt + 196 ─ , 191 ┐ , 192 └ , 217 ┘, 218 ┌


    // *note 'weights' means the connection values between the neurons

    // constructor
    public NeuralNetwork_Craig(int[] layers)
    {
        this.layers = new int[layers.Length]; // sest the number of layers
        for (int i = 0; i < layers.Length; i++) // populate the layers
        {
            this.layers[i] = layers[i];
        }

        // generate matrix
        InitNeurons();
        InitWeights();
    }

    /// <summary>
    /// deep copy constructor
    /// </summary>
    /// <param name="copyNetwork">Network to copy</param>
    public NeuralNetwork_Craig(NeuralNetwork_Craig copyNetwork)
    {
        this.layers = new int[copyNetwork.layers.Length];
        for (int i = 0; i < copyNetwork.layers.Length; i++)
        {
            this.layers[i] = copyNetwork.layers[i];
        }
        
        InitNeurons();
        InitWeights();
    }

    /// <summary>
    /// copy one networks weights to another
    /// </summary>
    /// <param name="copyWeights">the weights top copy</param>
    public void CopyWeights(float[][][] copyWeights)
    {
        // iterate through all the layers of the matrix
        for (int i = 0; i < weights.Length; i++)
        {
            // iterate through all the neurons of the layer
            for (int j = 0; j < weights[i].Length; j++)
            {
                // iterate through all the connections to the neuron
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = copyWeights[i][j][k];
                }
            }
        }
    }

    private void InitWeights()
    {
        //create neuron array/matrix
        List<float[]> neuronsList = new List<float[]>();

        for (int i = 0; i < layers.Length; i++)
        {
            neuronsList.Add(new float[layers[i]]); // add layer to neuron list
        }

        neurons = neuronsList.ToArray(); // convert list to array
    }

    //each layer needs it's own weight matrix
    private void InitNeurons()
    {
        List<float[][]> weightsList = new List<float[][]>(); // list of 2d float arrays

        for (int i = 0; i < layers.Length; i++) // iterate over every neuron that has a weight connection -- start with first hidden layer (technically 2nd layer) -> in layer matrix => index 1 )
        {
            List<float[]> layerWeightsList = new List<float[]>();

            int neuronsInPrevLayers = layers[i - 1];

            // iterate over all neurons in current layer
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPrevLayers];

                // itterate through all the neuron's connections, set weights randomly between 1 and -1
                for (int k = 0; k < neuronsInPrevLayers; k++)
                {
                    // give random weights (all the connections of the current neuron)
                    neuronWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
                }

                layerWeightsList.Add(neuronWeights);
            }

            weightsList.Add(layerWeightsList.ToArray());
        }
        weights = weightsList.ToArray();
    }

    public float[] FeedForward(float[] inputs)
    {
        //iterate through inputs and add to input layer in matrix
        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i]; //put contents of input into first layer of neuron matrix
        }

        // --- iterate through all the neurons with connections (starts from the second layer; index 1) ---
        // iterate through layers
        for (int i = 1; i < inputs.Length; i++)
        {
            //iterate through each neuron in layer
            for (int j = 0; j < neurons[i].Length; j++)
            {
                //value ultimately computed from the summation of all the neuron values of the previous layer with their weights
                float value = 0.25f; // 0.25f is constant bias

                //iterate through each neuron in previous layer
                for (int k = 0; k < neurons[i - 1].Length; k++)
                {
                    value += weights[i - 1][j][k] * neurons[i - 1][k]; //calculating value for neuron at j
                                                                       // the weight of the connection from the current neuron [j] to the neuron [k] in the previous layer [i-1] ; multiply by the neuron [k] of the previous layer [i - 1]
                                                                       // ex. weights[0][1][2] => weight of the connection from L0(c) to L1(b)
                }

                // save value of j after applying an 'activation' to it - in this case a hyperbolic tangent activation (convert value between -1 and 1)
                neurons[i][j] = (float)Math.Tanh(value);

            }
        }

        return neurons[neurons.Length - 1];
    }

    //mutates (randomly changes) network weights
    public void Mutate()
    {
        // iterate through all the layers of the matrix
        for (int i = 0; i < weights.Length; i++)
        {
            // iterate through all the neurons of the layer
            for (int j = 0; j < weights[i].Length; j++)
            {
                // iterate through all the connections to the neuron
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    float newWeight = weights[i][j][k];

                    //mutate
                    float randNum = UnityEngine.Random.Range(0f, 1000f); // 0.8% chance a mutation will occur, at 0.2% each

                    if (randNum <= 2f) //if 1 
                    { //flip sign of weight
                        newWeight *= -1f;
                    }
                    if (randNum <= 4f) //if 2 
                    { // pick weight between -0.5f and 0.5f
                        newWeight = UnityEngine.Random.Range(-0.5f, 0.5f);

                    }
                    if (randNum <= 6f)//if 3 
                    {//randomly increase by 0 - 100 %
                        float factor = UnityEngine.Random.Range(0f, 1f) + 1f;
                        newWeight *= factor;

                    }
                    if (randNum <= 8f)//if 4 
                    {//randomly decrease by 0 - 100 %
                        float factor = UnityEngine.Random.Range(0f, 1f);
                        newWeight *= factor;
                    }


                    weights[i][j][k] = newWeight;
                }
            }

        }
    }
}
