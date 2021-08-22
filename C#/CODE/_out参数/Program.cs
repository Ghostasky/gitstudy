using System;

namespace _out参数
{
    class Program
    {
        static void Main(string[] args)
        {
            //返回一个数组中的最大值，最小值，综合，品均值

            int[] number = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            int[] res = GetMMSA(number);

            int max1, min1, sum1;

            Test(number, out max1, out min1, out sum1);
        }
        
        
        //返回多个不同的类型的值时，可以使用out

        public static void Test(int[] nums, out int max, out int min, out int sum)
        {
            max = nums[0];
            min = nums[1];
            sum = 0;
            for (int i = 0;i<nums.Length;i++)
            {
                if (max < nums[i])
                    max = nums[i];
                if (min > nums[i])
                    min = nums[i];
                sum += nums[i];
            }
           
        }


        public static int[] GetMMSA(int[] nums)
        {
            int[] res = new int[4];

            res[0] = nums[0];
            res[1] = nums[0];
            res[2] = 0;
            for (int i =0;i<nums.Length;i++)
            {
                if (nums[i] > res[0])
                    res[0] = nums[i];
                if (nums[i] < res[0])
                    res[1] = nums[i];
                res[2] += nums[i];
            }
            res[3] = nums[2] / nums.Length;
            return res;
        }
    }
}
