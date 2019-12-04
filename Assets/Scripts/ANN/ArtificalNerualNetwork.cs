
namespace ANN
{
    class ArtificalNerualNetwork
    {
        public float gainTerm = 0.5f;

        private Neuron[][] neurons;

        private int layerCount { get { return neurons.Length; } }

        private Neuron[] inputLayer { get { return neurons[0]; } }

        private Neuron[] outputLayer { get { return neurons[layerCount - 1]; } }

        public ArtificalNerualNetwork(int[] structure)
        {
            // must be atleast 2 layers
            UnityEngine.Debug.Assert(structure.Length >= 2);

            System.Random rand = new System.Random();

            neurons = new Neuron[structure.Length][];
            for (int layer = 0; layer < structure.Length; layer++)
            {
                // create layer
                neurons[layer] = new Neuron[structure[layer]];

                // create neuron in layer
                for (int n = 0; n < structure[layer]; n++)
                {
                    neurons[layer][n] = new Neuron();

                    // create weights if not output layer
                    if (layer + 1 >= structure.Length) continue;

                    neurons[layer][n].weights = new float[structure[layer + 1]];

                    for (int w = 0; w < neurons[layer][n].weights.Length; w++)
                    {
                        //neurons[layer][n].weights[w] = (float)(rand.Next(0, 10) - 5) / 100f;
                        neurons[layer][n].weights[w] = 0.0f;
                        //neurons[layer][n].weights[w] = (float)(rand.Next(0, 100)) / 100f;
                    }
                }
            }
        }

        /// <summary>
        /// Feeds the input forward through the network
        /// </summary>
        /// <param name="input">Input values for the input layer</param>
        /// <returns>The array of values of the output layer</returns>
        public float[] Forward(float[] input)
        {
            // input must be same length as input layer
            UnityEngine.Debug.Assert(input.Length == neurons[0].Length);

            float[][] results = Forward2(input);

            return results[layerCount - 1];
        }

        /// <summary>
        /// Feeds the input forward through the network
        /// </summary>
        /// <param name="input">Input values for the input layer</param>
        /// <returns>The 2d array of values of all layers</returns>
        public float[][] Forward2(float[] input)
        {
            float[][] results = CreateResultArray();

            results[0] = input;

            // start at layer 0 (input)
            for (int layer = 0; layer < layerCount - 1; layer++)
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
                ApplyActivationFunction(sum, 1.5f);

                results[layer + 1] = sum;
            }

            return results;
        }

        public void Train(params TrainingItem[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                Train(items[i].input, items[i].expected); 
            }
        }

        public void Train(TrainingItem item)
        {
            Train(item.input, item.expected);
        }

        public void Train(float[] input, float[] expected)
        {
            // input must be same length as input layer
            UnityEngine.Debug.Assert(input.Length == neurons[0].Length);

            // expected must be same length as output layer
            UnityEngine.Debug.Assert(expected.Length == neurons[layerCount - 1].Length);

            // perform forward/eval
            float[][] results = Forward2(input);

            // for each output
            for (int o = 0; o < outputLayer.Length; o++)
            {
                float error = expected[o] - results[layerCount - 1][o];

                // if the error is less than 0.01, skip
                if (UnityEngine.Mathf.Abs(error) < 0.01f) continue;

                BackProp(layerCount - 1, o, error, results);
            }
        }

        private void BackProp(int layerIndex, int neuronIndex, float error, float[][] results)
        {
            if (layerIndex <= 0) return;

            Neuron[] prevLayer = neurons[layerIndex - 1];

            for (int n = 0; n < prevLayer.Length; n++)
            {
                prevLayer[n].weights[neuronIndex] += gainTerm * error * results[layerIndex - 1][n];

                BackProp(layerIndex - 1, n, error, results);
            }
        }

        /// <summary>
        /// Applies activation function
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="upper"></param>
        private void ApplyActivationFunction(float[] arr, float upper)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] >= upper) arr[i] = 1;
                else arr[i] = 0;
            }
        }

        /// <summary>
        /// Creates array with the dimensions for layer outputs of network
        /// </summary>
        /// <returns></returns>
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

    struct TrainingItem
    {
        public float[] input;

        public float[] expected;

        public TrainingItem(float[] input, float[] expected)
        {
            this.input = input;
            this.expected = expected;
        }
    }

    
}
