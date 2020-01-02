using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix{

    public int col;
    public int row;
    public float[,] m;

    public Matrix(int r, int c)
    {
        col = c;
        row = r;

        m = new float[r, c];

        for (int i = 0; i < r; i++)
        {
            for (int j = 0; j < c; j++)
            {
                m[i, j] = 0;
            }
        }

    }

    public void Randomnize()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                float randomWeight = Random.value * 2 - 1;
                m[i, j] = randomWeight;
            }
        }
    }

    public Matrix Add(Matrix m1)
    {
        Matrix result = new Matrix(row, col);

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                result.m[i, j] = m[i, j] + m1.m[i, j];
            }
        }

        return result;
    }

    public Matrix Dot(Matrix m1)
    {
        Matrix dot = new Matrix(m1.row, col);
        if (m1.col == row)
        {
            for (int i = 0; i < m1.row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    float sum = 0;
                    for (int x = 0; x < m1.col; x++)
                    {
                        sum += m1.m[i, x] * m[x, j];
                    }
                    dot.m[i, j] = sum;
                }
            }

            return dot;
        }
        else
        {
            throw new System.Exception("m1: " + m1.row + ", " + m1.col + " m: " + row + ", " + col);
        }
    }

    public static Matrix arrayToMatrix(float[] inputArray)
    {
        Matrix m1 = new Matrix(inputArray.Length, 1);

        for (int i = 0; i < inputArray.Length; i++)
        {
            m1.m[i, 0] = inputArray[i];
        }

        return m1;
    }

    public float[] matrixToArray()
    {
        float[] array = new float[row * col];
        int count = 0;

        for (int i = 0; i < row; i++)
        {
            array[i] = m[i, 0];
        }

        return array;
    }

    public Matrix[] Crossover(Matrix parent)
    {
        Matrix[] children = new Matrix[2];
        Matrix child1 = new Matrix(row, col);
        Matrix child2 = new Matrix(row, col);
        int crossoverPoint = Random.Range(0, row);

        children[0] = child1;
        children[1] = child2;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (i < crossoverPoint)
                {
                    child1.m[i, j] = m[i, j];
                }
                else
                {
                    child1.m[i, j] = parent.m[i, j];
                }
            }
        }

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (i < crossoverPoint)
                {
                    child2.m[i, j] = parent.m[i, j];
                }
                else
                {
                    child2.m[i, j] = m[i, j];
                }
            }
        }

        return children;
    }

    public Matrix Mutate(float mutationRate)
    {
        Matrix mutatedMatrix = new Matrix(row, col);
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (Random.value < mutationRate)
                {
                    mutatedMatrix.m[i, j] = m[i, j] + Random.Range(-0.5f, 0.5f);
                }
                else
                {
                    mutatedMatrix.m[i, j] = m[i, j];
                }
            }
        }

        return mutatedMatrix;
    }

    public void Print()
    {
        string output = "";

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                output += m[i, j] + " ";
            }
            output += "\n";
        }

        Debug.Log(output);
    }
}
