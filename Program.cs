using OptimizationCalc;

string filePath = "TaskV70.txt";

var relations = ReadBinaryRelationsFromFile(filePath);

for (int i = 0; i < relations.Count; i++)
{
    Console.WriteLine($"R{i+1} -----------------");
    PrintMatrix(relations[i]);
    
    var isAcyclic = NeymanMorgenshternOptimizer.IsAcyclic(relations[i]);

    if(isAcyclic)
    {
        NeymanMorgenshternOptimizer.Optimize(relations[i]);
    }
    else
    {
        KOptimizer.Optimize(relations[i]);
    }

    Console.WriteLine();
}

static List<bool[,]> ReadBinaryRelationsFromFile(string filePath)
{
    List<bool[,]> relations = [];
    List<List<bool>> matrix = [];

    foreach (var line in File.ReadLines(filePath))
    {
        if (string.IsNullOrWhiteSpace(line)) continue;

        if (line.StartsWith('R'))
        {
            if (matrix.Count > 0)
            {
                relations.Add(ConvertTo2DArray(matrix));
                matrix.Clear();
            }
        }
        else
        {
            var row = new List<bool>();
            foreach (var charValue in line.Trim().Split("  "))
            {
                row.Add(charValue == "1");
            }
            matrix.Add(row);
        }
    }

    if (matrix.Count > 0)
    {
        relations.Add(ConvertTo2DArray(matrix));
    }

    return relations;
}

static bool[,] ConvertTo2DArray(List<List<bool>> list)
{
    int rows = list.Count;
    int cols = list[0].Count;
    bool[,] array = new bool[rows, cols];

    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < cols; j++)
        {
            array[i, j] = list[i][j];
        }
    }

    return array;
}

static void PrintMatrix(bool[,] matrix)
{
    for (int i = 0; i < matrix.GetLength(0); i++)
    {
        for (int j = 0; j < matrix.GetLength(0); j++)
        {
            Console.Write(matrix[i, j] ? "1 " : "0 ");
        }
        Console.WriteLine();
    }
}
