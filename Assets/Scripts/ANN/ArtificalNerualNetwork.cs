
namespace ANN
{
    [System.Serializable]
    public class ArtificalNerualNetwork
    {
        /// <summary>
        /// Gain term for training speed
        /// </summary>
        public float gainTerm = 0.1f;

        /// <summary>
        /// All layers and enurons represented as a jagged 2d array.
        /// </summary>
        private Neuron[][] m_neurons;

        /// <summary>
        /// Layer count
        /// </summary>
        private int layerCount { get { return m_neurons.Length; } }

        /// <summary>
        /// Input layer
        /// </summary>
        private Neuron[] inputLayer { get { return m_neurons[0]; } }

        /// <summary>
        /// Output layer
        /// </summary>
        private Neuron[] outputLayer { get { return m_neurons[layerCount - 1]; } }

        public ArtificalNerualNetwork(int[] structure)
        {
            // must be atleast 2 layers
            UnityEngine.Debug.Assert(structure.Length >= 2);

            System.Random rand = new System.Random();

            m_neurons = new Neuron[structure.Length][];
            for (int layer = 0; layer < structure.Length; layer++)
            {
                // create layer
                m_neurons[layer] = new Neuron[structure[layer]];

                // create neuron in layer
                for (int n = 0; n < structure[layer]; n++)
                {
                    m_neurons[layer][n] = new Neuron();

                    // create weights if not output layer
                    if (layer + 1 >= structure.Length) continue;

                    m_neurons[layer][n].weights = new float[structure[layer + 1]];

                    for (int w = 0; w < m_neurons[layer][n].weights.Length; w++)
                    {
                        m_neurons[layer][n].weights[w] = (float)(rand.Next(0, 10) - 5) / 100f;
                        //m_neurons[layer][n].weights[w] = 0.0f;
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
            UnityEngine.Debug.Assert(input.Length == m_neurons[0].Length);

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
                float[] sum = new float[m_neurons[layer + 1].Length];

                // input * weight for all neurons in current layer
                for (int n = 0; n < m_neurons[layer].Length; n++)
                {
                    float[] weightedValues = m_neurons[layer][n].Process(results[layer][n]);

                    for (int i = 0; i < sum.Length; i++)
                    {
                        sum[i] += weightedValues[i];
                    }
                }

                // apply activation function
                //ApplyActivationFunction_Linear(sum, 1.5f);
                ApplyActivationFunction_Sigmoid(sum);

                results[layer + 1] = sum;
            }

            return results;
        }
        
        /// <summary>
        /// Trains the network with the items
        /// </summary>
        /// <param name="items"></param>
        public void Train(params TrainingItem[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                Train(items[i].input, items[i].expected); 
            }
        }

        /// <summary>
        /// Trains the network with one item
        /// </summary>
        /// <param name="item"></param>
        public void Train(TrainingItem item)
        {
            Train(item.input, item.expected);
        }

        /// <summary>
        /// Trains the network with input and expected values
        /// </summary>
        /// <param name="input"></param>
        /// <param name="expected"></param>
        public void Train(float[] input, float[] expected)
        {
            // input must be same length as input layer
            UnityEngine.Debug.Assert(input.Length == m_neurons[0].Length);

            // expected must be same length as output layer
            UnityEngine.Debug.Assert(expected.Length == outputLayer.Length);

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

        /// <summary>
        /// Performs backpropagation
        /// </summary>
        /// <param name="layerIndex">Layer to back propagate from</param>
        /// <param name="neuronIndex">Neuron in the layer to back propagate from</param>
        /// <param name="error">Error value from test</param>
        /// <param name="results">Results of all neurons after a test</param>
        private void BackProp(int layerIndex, int neuronIndex, float error, float[][] results)
        {
            if (layerIndex <= 0) return;

            Neuron[] prevLayer = m_neurons[layerIndex - 1];

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
        private void ApplyActivationFunction_Linear(float[] arr, float upper)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] >= upper) arr[i] = 1;
                else arr[i] = 0;
            }
        }

        /// <summary>
        /// Applies activation function
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="upper"></param>
        private void ApplyActivationFunction_Sigmoid(float[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = 1f / (1f + UnityEngine.Mathf.Pow(2.71828f, arr[i]));
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
                arr[i] = new float[m_neurons[i].Length];
            }

            return arr;
        }

        [System.Serializable]
        class Neuron
        {
            /// <summary>
            /// Weights of this neuron
            /// </summary>
            public float[] weights;

            /// <summary>
            /// Processes the value and outputs an array of weighted results.
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
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

    [System.Serializable]
    public struct TrainingItem
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
