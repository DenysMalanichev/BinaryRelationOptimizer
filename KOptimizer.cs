namespace OptimizationCalc;

public class KOptimizer
{
    static public void Optimize(bool[,] br)
    {
        var parsedMatrix = ParseBinaryRelationMatrix(br); // matrix contains these elements: I, or P, or N, or ' '

        Console.WriteLine("PIN матриця: \n");
        PrintMatrix(parsedMatrix);

        for (int k = 1; k <= 4; k++)
        {
            Console.WriteLine($"K={k} оптимізація. \n");
            OptimizeOnK(k, parsedMatrix);
        }
    }

    private static void OptimizeOnK(int k, char[,] parsedMatrix)
    {
        List<List<int>> opts = [];
        string options = k switch
        {
            1 => "PIN",
            2 => "PN",
            3 => "PI",
            4 => "P",
            _ => "P",
        };
        Predicate<char> predicate = options.Contains;

        for (int i = 0; i < parsedMatrix.GetLength(0); i++)
        {
            opts.Add([]);
            for (int j = 0; j < parsedMatrix.GetLength(0); j++)
            {
                if (predicate(parsedMatrix[i, j]))
                    opts[i].Add(j);
            }
        }

        List<int> kopt = [];
        for (int i = 0; i < parsedMatrix.GetLength(0); i++)
        {
            if (opts[i].Count == parsedMatrix.GetLength(0))
                kopt.Add(i);
        }

        Console.WriteLine($"{k}-opt = " + "{ " + string.Join(", ", kopt.Select(n => n + 1)) + " }");

        List<int> kmax = [];
        var opt = opts.MaxBy(o => o.Count)?.Count;
        if (opt is not null)
        {
            for (int i = 0; i < parsedMatrix.GetLength(0); i++)
            {
                if (opts[i].Count == opt)
                    kmax.Add(i);
            }
        }


        Console.WriteLine($"{k}-max = " + "{ " + string.Join(", ", kmax.Select(n => n + 1)) + " }");
    }

    private static char[,] ParseBinaryRelationMatrix(bool[,] br)
    {
        var n = br.GetLength(0);
        var parsedMatrix = new char[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (i != j && ((br[i, j] && !br[j, i]) || (!br[i, j] && br[j, i])))
                {
                    if (br[i, j])
                    {
                        parsedMatrix[i, j] = 'P';
                        parsedMatrix[j, i] = ' ';
                    }
                    else
                    {
                        parsedMatrix[j, i] = 'P';
                        parsedMatrix[i, j] = ' ';
                    }
                }
                else if (br[i, j] && br[j, i])
                {
                    parsedMatrix[i, j] = 'I';
                    parsedMatrix[j, i] = 'I';
                }
                else if (!br[i, j] && !br[j, i])
                {
                    parsedMatrix[i, j] = 'N';
                    parsedMatrix[j, i] = 'N';
                }
            }
        }

        return parsedMatrix;
    }

    private static void PrintMatrix(char[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                Console.Write(matrix[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}
