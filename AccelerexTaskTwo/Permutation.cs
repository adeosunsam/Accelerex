namespace AccelerexTaskTwo;

public class Permutation
{
    public static int[] InversePermutation(int N, int[] input)
    {
        int[] result = new int[N];
        if (input.Distinct().Count() == N && input.Max() == N && input.Min() == 1) // checking the items in input are unique
        {
            for (int i = 0; i < input.Length; i++)
            {
                result[input[i] - 1] = i + 1;
            }
            return result;
        }
        return Array.Empty<int>();
    }
}
