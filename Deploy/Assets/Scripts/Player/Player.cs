using System;

public class Player
{
    public static int EXP;
    public static int LEVEL;

    private static int Weapon; //might be public
    private static int[] Abilities;

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

    public static void ChangeWeapon(int newWeaponID)
    {
        Weapon = newWeaponID;
    }

    public static void ChangeAbility(int position, int abilityID)
    {
        Abilities[position] = abilityID;
    }
}
