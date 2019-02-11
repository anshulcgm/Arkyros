using System;

public class Player
{
    public static int EXP;
    public static int LEVEL;

	public Player()
	{
        EXP = 1;
        LEVEL = 1;
	}

    public static void IncreaseExp (int exp)
    {
        EXP += exp;
        UpdateLevel();
    }

    public static void UpdateLevel()
    {
        LEVEL = (int)Math.Sqrt(EXP);
    }
}
