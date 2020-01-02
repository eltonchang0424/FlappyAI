using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork{

    int inputNodes;
    int hiddenNodes;
    int outputNodes;

    Matrix weightsIH;
    Matrix weightsHO;

    Matrix biasHidden;
    Matrix biasOutput;

    public NeuralNetwork(int i, int h,  int o)
    {
        inputNodes = i;
        hiddenNodes = h;
        outputNodes = o;

        weightsIH = new Matrix(h, i);
        weightsHO = new Matrix(o, h);

        weightsIH.Randomnize();
        weightsHO.Randomnize();

        biasHidden = new Matrix(h, 1);
        biasOutput = new Matrix(o, 1);

        biasHidden.Randomnize();
        biasOutput.Randomnize();
    }

    public float[] FeedForward(float[] inputArray)
    {
        Matrix input = Matrix.arrayToMatrix(inputArray);

        Matrix hidden = input.Dot(weightsIH);

        Matrix hiddenWithBias = hidden.Add(biasHidden);

        Matrix normalizedHiddenWithBias = Sigmoid(hiddenWithBias);

        Matrix output = normalizedHiddenWithBias.Dot(weightsHO);

        Matrix outputWithBias = output.Add(biasOutput);

        Matrix normalizedOutputWithBias = Sigmoid(outputWithBias);

        /*Debug.Log("weights IH matrix");
        weightsIH.Print();
        Debug.Log("input matrix");
        input.Print();
        Debug.Log("hidden matrix");
        hidden.Print();
        Debug.Log("hidden with bias matrix");
        hiddenWithBias.Print();
        Debug.Log("normalized hidden with bias matrix");
        normalizedHiddenWithBias.Print();
        Debug.Log("output matrix");
        output.Print();
        Debug.Log("output with bias matrix");
        outputWithBias.Print();
        Debug.Log("normalized output with bias matrix");
        normalizedOutputWithBias.Print();*/

        float[] outputArray = normalizedOutputWithBias.matrixToArray();

        return outputArray;
    }

    public Matrix Sigmoid(Matrix m)
    {
        Matrix normalizedMatrix = new Matrix(m.row, m.col);

        for (int i = 0; i < m.row; i++)
        {
            for (int j = 0; j < m.col; j++)
            {
                normalizedMatrix.m[i, j] = 1 / (1 + Mathf.Exp(-1 * m.m[i, j]));
            }
        }

        return normalizedMatrix;
    }

    public NeuralNetwork[] Crossover(NeuralNetwork parent1)
    {
        NeuralNetwork[] children = new NeuralNetwork[2];
        NeuralNetwork child1 = new NeuralNetwork(inputNodes, hiddenNodes, outputNodes);
        NeuralNetwork child2 = new NeuralNetwork(inputNodes, hiddenNodes, outputNodes);
        children[0] = child1;
        children[1] = child2;


        Matrix[] weightsIHs = weightsIH.Crossover(parent1.weightsIH);
        Matrix[] weightsHOs = weightsHO.Crossover(parent1.weightsHO);
        Matrix[] biasHiddens = biasHidden.Crossover(parent1.biasHidden);
        Matrix[] biasOutputs = biasOutput.Crossover(parent1.biasOutput);

        child1.weightsIH = weightsIHs[0];
        child2.weightsIH = weightsIHs[1];
        child1.weightsHO = weightsHOs[0];
        child2.weightsHO = weightsHOs[1];
        child1.biasHidden = biasHiddens[0];
        child2.biasHidden = biasHiddens[1];
        child1.biasOutput = biasOutputs[0];
        child2.biasOutput = biasOutputs[1];

        return children;
    }

    public NeuralNetwork Mutate(float mutationRate)
    {
        NeuralNetwork child = new NeuralNetwork(inputNodes, hiddenNodes, outputNodes);

        child.weightsIH = weightsIH.Mutate(mutationRate);
        child.weightsHO = weightsHO.Mutate(mutationRate);
        child.biasHidden = biasHidden.Mutate(mutationRate);
        child.biasOutput = biasOutput.Mutate(mutationRate);

        return child;
    }

}
