
namespace ANN
{
    class ArtificalNerualNetwork
    {
        private Neuron[][] neurons;

        private int layerCount;

        private Neuron[] inputLayer { get { return neurons[0]; } }

        private Neuron[] outputLayer { get { return neurons[layerCount - 1]; } }

        ArtificalNerualNetwork(int[][] structure)
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
                        neurons[layer][n].weights[w] = (float)(rand.Next(0, 10) - 5) / 100f;
                    }
                }
            }
        }

        public float[] Forward(float[] input)
        {
            // input must be same length as input layer
            UnityEngine.Debug.Assert(input.Length == neurons[0].Length);

            float[] results = new float[neurons[neurons.Length].Length];

            for (int layer = 0; layer < structure.Length; layer++)
            {

            }
            return results;
        }

        struct Neuron
        {
            public float[] weights;
        }
    }

    
}
