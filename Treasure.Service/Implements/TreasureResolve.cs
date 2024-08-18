using Pii = (double, double);

namespace Treasure.Service.Implements
{
    internal static class TreasureResolve
    {
        private readonly static int maxn = 500+1;
        private readonly static ulong maxc = ulong.MaxValue;

        private static double Dis(Pii u, Pii v) => u.Item1 == 0 && u.Item2 == 0 ?
            Math.Sqrt((v.Item1 - 1) * (v.Item1 - 1) + (v.Item2 - 1) * (v.Item2 - 1)) :
            Math.Sqrt((u.Item1 - v.Item1) * (u.Item1 - v.Item1) + (u.Item2 - v.Item2) * (u.Item2 - v.Item2));

        public static double Solve(int n, int m, int p, List<List<int>> matrix)
        {
            int[,] arr2d = new int[maxn+1, maxn+1];
            double[] ans = new double[maxn];
            double[,] min_dis = new double[maxn, maxn];
            List<Pii>[] b = new List<Pii>[maxn*maxn];

            for (int i = 0; i < (maxn * maxn); i++) b[i] = new List<Pii>();
            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix[i].Count; j++)
                {
                        var init = matrix[i][j];
                        arr2d[i + 1, j + 1] = init;
                        b[init].Add((i + 1, j + 1));
                }
            }


            for (int i = 0; i <= p; i++)
                ans[i] = maxc;

            for (int i = 0; i <= n; i++)
                for (int j = 0; j <= m; j++)
                    min_dis[i, j] = maxc;

            ans[0] = 0;
            b[0].Add((0, 0));

            var q = new PriorityQueue<Pii, double>();
            q.Enqueue((0, 0), 0);

            int iterationCount = 0;

            while (q.TryDequeue(out var top, out var priority))
            {
                iterationCount++;
                double distance = -priority;
                var u = top;
                int key_num = arr2d[(int)u.Item1, (int)u.Item2];

                if (min_dis[(int)u.Item1, (int)u.Item2] < distance) continue;

                min_dis[(int)u.Item1, (int)u.Item2] = distance;
                ans[key_num] = Math.Min(ans[key_num], distance);

                foreach (var v in b[key_num + 1])
                {
                    double new_dis = distance + Dis(u, v);
                    q.Enqueue(v, -new_dis);
                }

                if (iterationCount > 1000000) 
                {
                    Console.WriteLine("Possible infinite loop detected. Exiting...");
                    break;
                }
            }

            return ans[p];
        }

    }
}
