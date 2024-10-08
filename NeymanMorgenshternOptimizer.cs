namespace OptimizationCalc;
public class NeymanMorgenshternOptimizer
{
    public static int[] Optimize(bool[,] br)
    {
        var totalS = new List<int>();
        var sk = new List<List<int>>
        {
            BuildS0(br)
        };

        totalS.AddRange(sk[0]);

        Console.WriteLine();

        for (int i = 0; ; i++)
        {
            if (totalS.Count == br.GetLength(0))
                break;

            sk.Add(BuildSk_Sk1(sk[i], totalS, br));
            totalS.AddRange(sk[i + 1]);

            Console.WriteLine($"S{i + 1}\\S{i} = " + "{ " + $"{string.Join(", ", sk[i + 1].Select(n => n + 1))}" + " }");
        }

        var q = new List<int>();
        q.AddRange(sk[0]);

        Console.WriteLine();

        Console.WriteLine($"Q{0} = " + "{ " + $"{string.Join(", ", q.Select(n => n + 1))}" + " }\n");

        for (int i = 1; i < sk.Count; i++)
        {
            q.AddRange(BuildQk(q, sk[i], br));
            if (q.Count > 0)
            {
                Console.WriteLine($"Q{i} = " + "{ " + $"{string.Join(", ", q.Select(n => n + 1))}" + " }");
            }
            else
            {
                Console.WriteLine($"Q{i} = " + "{ }");
            }

        }

        Console.WriteLine($"Внутрішя стійкість: {IsInnerRack(br, q)}");
        Console.WriteLine($"Зовнішня стійкість: {IsOuterRack(br, q)}");

        return q.ToArray();
    }

    private static List<int> BuildS0(bool[,] br)
    {
        var s0 = new List<int>();
        for (int i = 0; i < br.GetLength(0); i++)
        {
            var isGood = true;
            for (int j = 0; j < br.GetLength(0); j++)
                if (br[j, i])
                {
                    isGood = false;
                    break;
                }
            if (isGood)
                s0.Add(i);
        }

        Console.WriteLine($"S0 = " + "{ " + $"{string.Join(", ", s0.Select(n => n + 1))}" + " }\n");

        return s0;
    }

    private static List<int> BuildSk_Sk1(List<int> sk_1, List<int> s, bool[,] br)
    {
        var sk = new List<int>();
        for (int i = 0; i < br.GetLength(0); i++)
        {
            if (s.Contains(i))
                continue;

            var isGood = true;
            for (int j = 0; j < br.GetLength(0); j++)
            {
                if (br[j, i] && !s.Contains(j))
                {
                    isGood = false;
                    break;
                }
            }

            var contains = false;
            foreach (var n in sk_1)
            {
                if (br[n, i])
                {
                    contains = true;
                    break;
                }

            }

            if (isGood && contains)
                sk.Add(i);
        }

        return sk;
    }

    private static List<int> BuildQk(List<int> qk_1, List<int> sk, bool[,] br)
    {
        var qk = new List<int>();
        foreach (var i in sk)
        {
            var isGood = true;
            foreach (var j in qk_1)
            {
                if (br[j, i])
                {
                    isGood = false;
                    break;
                }
            }

            if (isGood)
                qk.Add(i);
        }

        return qk;
    }

    private static bool IsInnerRack(bool[,] br, List<int> q)
    {
        foreach (var i in q)
            foreach (var j in q)
                if (br[i, j] || br[j, i])
                    return false;

        return true;
    }

    private static bool IsOuterRack(bool[,] br, List<int> q)
    {
        for (int i = 0; i < br.GetLength(0); i++)
        {
            var isOptionOuterRack = false;
            foreach (var j in q)
                if (i != j || br[j, i])
                {
                    isOptionOuterRack = true;
                    break;
                }
            if (!isOptionOuterRack)
                return false;
        }

        return true;
    }

    public static bool IsAcyclic(bool[,] br)
    {
        int n = br.GetLength(0);

        for (int i = 0; i < n; i++)
            if (br[i, i])
            {
                Console.WriteLine($"Бінарне відношення не є ациклічним, бо є рефлексивна петля ([{i + 1}, {i + 1}]).\n");
                return false;
            }

        bool[] visited = new bool[n];
        bool[] recursionStack = new bool[n];

        for (int node = 0; node < n; node++)
        {
            if (IsCyclicDFS(node, visited, recursionStack, br))
            {
                Console.WriteLine("Бінарне відношення не є ациклічним, бо є рефлексивна петля.\n");
                return false;
            }
        }

        Console.WriteLine("Бінарне відношення ациклічне.\n");
        return true;
    }

    private static bool IsCyclicDFS(int node, bool[] visited, bool[] recursionStack, bool[,] matrix)
    {
        if (recursionStack[node])
        {
            return true;
        }

        if (visited[node])
        {
            return false;
        }

        visited[node] = true;
        recursionStack[node] = true;

        for (int neighbor = 0; neighbor < matrix.GetLength(0); neighbor++)
        {
            if (matrix[node, neighbor])
            {
                if (IsCyclicDFS(neighbor, visited, recursionStack, matrix))
                {
                    return true;
                }
            }
        }

        recursionStack[node] = false;
        return false;
    }
}