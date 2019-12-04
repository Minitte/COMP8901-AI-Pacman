
namespace ANN
{
    class ArtificalNerualNetwork
    {
        private Neuron[][] neurons;

        private int layerCount;

        private Neuron[] inputLayer { get { return neurons[0]; } }

        private Neuron[] outputLayer { get { return neurons[layerCount - 1]; } }

        public ArtificalNerualNetwork(int[][] structure)
        {
            // must be atleast 2 layers
            UnityEngine.Debug.Assert(structure.Length >= 2);

            System.Random rand = new System.Random();

            neurons = new Neuron[structure.Length][];
            for (int layer = 0; layer < structure.Length; layer++)
            {
                // create layer
                neurons[layer] = new Neuron[structure[layer].Length];

                // create neuron in layer
                for (int n = 0; n < structure[layer].Length; n++)
                {
                    neurons[layer][n] = new Neuron();

                    // create weights if not output layer
                    if (n + 1 >= structure.Length) continue;

                    neurons[layer][n].weights = new float[structure[n + 1].Length];

                    for (int w = 0; w < neurons[layer][n].weights.Length; w++)
                    {
                        //neurons[layer][n].weights[w] = (float)(rand.Next(0, 10) - 5) / 100f;
                        neurons[layer][n].weights[w] = 0f;
                    }
                }
            }
        }

        public float[] Forward(float[] input)
        {
            // input must be same length as input layer
            UnityEngine.Debug.Assert(input.Length == neurons[0].Length);

            float[][] results = CreateResultArray();

            results[0] = input;

            // start at layer 0 (input)
            for (int layer = 0; layer < neurons.Length - 1; layer++)
            {
                float[] sum = new float[neurons[layer + 1].Length];

                // input * weight for all neurons in current layer
                for (int n = 0; n < neurons[layer].Length; n++)
                {
                    float[] weightedValues = neurons[layer][n].Process(results[layer][n]);

                    for (int i = 0; i < sum.Length; i++)
                    {
                        sum[i] += weightedValues[i];
                    }
                }

                // apply activation function
                ApplyActivationFunction(sum, 1.0f);

                results[layer + 1] = sum;
            }

            return results[layerCount - 1];
        }

        private void ApplyActivationFunction(float[] arr, float upper)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] > upper) arr[i] = 1;
                else arr[i] = 0;
            }
        }

        private float[][] CreateResultArray()
        {
            float[][] arr = new float[layerCount][];

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = new float[neurons[i].Length];
            }

            return arr;
        }

        class Neuron
        {
            public float[] weights;

            public float[] Process(float value)
            {
                float[] results = new float[weights.Length];

                for (int i = 0; i < weights.Length; i++)
                {
                    results[i] = value * weights[i];
                }

                return results;
            }
        }
    }

    
}
