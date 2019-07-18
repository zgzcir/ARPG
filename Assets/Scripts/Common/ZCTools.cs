using System;

public class ZCTools
{
    public static int RDInt(int min, int max, Random rd = null)
    {
        if (rd == null)
        {
            rd = new Random();
        }
        int val = rd.Next(min, max + 1);
        return val;
    }
}