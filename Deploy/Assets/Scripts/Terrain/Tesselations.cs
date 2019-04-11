using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Line
{
    int x1, y1, x2, y2;

    public Line(int x1, int y1, int x2, int y2)
    {
        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;
    }

    public int getX1()
    {
        return x1;
    }

    public int getY1()
    {
        return y1;
    }

    public int getX2()
    {
        return x2;
    }

    public int getY2()
    {
        return y2;
    }

    public bool pointIntersectsLine(int x, int y)
    {
        double a = Math.Sqrt(((y - y1) * (y - y1)) + ((x - x1) * (x - x1)));
        double b = Math.Sqrt(((y - y2) * (y - y2)) + ((x - x2) * (x - x2)));

        double p = Math.Sqrt(((y2 - y1) * (y2 - y1)) + ((x2 - x1) * (x2 - x1)));

        return p == (a + b);
    }

    public bool lineIntersectsLine(int x3, int y3, int x4, int y4, bool type)
    { // 0 for lineIntersectsLine normal (all); 1 for lineIntersectsLine mild (only between points); 2 for lineIntersectsLine for points (no interline intersection)    
        int tXM = (x2 - x1);
        int tXB = (x1);
        int tYM = (y2 - y1);
        int tYB = (y1);

        int uXM = (x3 - x4);
        int uXB = (x3);
        int uYM = (y3 - y4);
        int uYB = (y3);

        double determinant = 1.0 / ((tXM) * (uYM) - (uXM) * (tYM));
        double inverseA = (uYM) * determinant;
        double inverseB = (0 - uXM) * determinant;
        double inverseC = (0 - tYM) * determinant;
        double inverseD = (tXM) * determinant;

        double t = (inverseA * (double)(uXB - tXB)) + (inverseB * (double)(uYB - tYB));
        double u = (inverseC * (double)(uXB - tXB)) + (inverseD * (double)(uYB - tYB));

        if (type)
        {
            if (t > 0 && t < 1 && u > 0 && u < 1)
            { // no vertexes (endpoints)
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
            { // 0 is beginning of line and 1 is end of it; yes vertexes (endpoints) // fix
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

public class Primary : Line
{
    int orient;

    public Primary(int x1, int y1, int x2, int y2, int orient) : base(x1, y1, x2, y2)
    {
        this.orient = orient;
    }

    public bool intersectLine(Line line)
    {
        if (base.Equals(line) || (base.pointIntersectsLine(line.getX1(), line.getY1()) && !(base.pointIntersectsLine(line.getX2(), line.getY2()))) || (!(base.pointIntersectsLine(line.getX1(), line.getY1())) && base.pointIntersectsLine(line.getX2(), line.getY2())))
        {
            return false;
        }
        else if (base.lineIntersectsLine(line.getX1(), line.getY1(), line.getX2(), line.getY2(), false))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool intersectOrientationalLine(Line line)
    {
        if (base.pointIntersectsLine(line.getX1(), line.getY1()) && base.pointIntersectsLine(line.getX2(), line.getY2()))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int getOrient()
    {
        return orient;
    }
}

public class Secondary : Primary
{
    int x1, y1, x2, y2;


    public Secondary(int x1, int y1, int x2, int y2) : base(x1, y1, x2, y2, 0)
    {
    }
}


public class Rectangle
{
    private int x, y, w, h;

    public Rectangle(int x, int y, int w, int h)
    {
        this.x = x;
        this.y = y;
        this.w = w;
        this.h = h;
    }

    public int getX()
    {
        return x;
    }

    public int getY()
    {
        return y;
    }

    public int getWidth()
    {
        return w;
    }

    public int getHeight()
    {
        return h;
    }
}

public class Tessellations
{
    public const int LINE_MAX = 10000;
    public const int LINE_SIZE_MAX = 10;

    public static List<Rectangle> GetTesselation()
    {
        List<Primary> primary = new List<Primary>();
        List<Secondary> secondary = new List<Secondary>();
        List<Rectangle> squares = new List<Rectangle>();

        System.Random r = new System.Random();
        squares.Add(new Rectangle(0, 0, 400, 400));

        int x1 = (int)squares[0].getX(), y1 = (int)squares[0].getY();
        int x2 = x1 + (int)squares[0].getWidth(), y2 = y1 + (int)squares[0].getHeight();

        primary.Add(new Primary(x1, y1, x2, y1, 0));
        primary.Add(new Primary(x2, y1, x2, y2, 1));
        primary.Add(new Primary(x1, y2, x2, y2, 2));
        primary.Add(new Primary(x1, y1, x1, y2, 3));

        Algo(primary, r, secondary, squares);
        return squares;
    }

    public static void Algo(List<Primary> primary, System.Random r, List<Secondary> secondary, List<Rectangle> squares)
    {
        int i = 0;
        while (i < LINE_MAX)
        {
            int x1 = (int)primary[0].getX1();
            int y1 = (int)primary[0].getY1();
            int x2 = (int)primary[0].getX2();
            int y2 = (int)primary[0].getY2();
            int orient = (int)primary[0].getOrient();
            //System.out.println(i);

            if (y1 == y2 && orient == 0 && ((y1 <= (int)squares[0].getY()) || (y1 < (int)(squares[0].getY() + squares[0].getHeight()) && (x1 > (int)(squares[0].getX() + squares[0].getWidth()) || x2 < (int)squares[0].getX()))))
            { // up orig-square[0]
                if (x2 - x1 >= LINE_SIZE_MAX + LINE_SIZE_MAX)
                {
                    int intersectLine = r.Next(x2 - x1 - LINE_SIZE_MAX - LINE_SIZE_MAX + 1) + LINE_SIZE_MAX;
                    remove(primary, x1, y1, x2, y2, secondary);
                    if ((overlap(new Line(x1, y1, x2, y2), primary, secondary)) && (overlap(new Line(x1, y1 - intersectLine, x1, y1), primary, secondary)) && (overlap(new Line(x1, y1 - intersectLine, x1 + intersectLine, y1 - intersectLine), primary, secondary)) && (overlap(new Line(x1 + intersectLine, y1 - intersectLine, x1 + intersectLine, y1), primary, secondary)) && (overlap(new Line(x1 + intersectLine, y1 - (x2 - x1) + intersectLine, x1 + intersectLine, y1), primary, secondary)) && (overlap(new Line(x1 + intersectLine, y1 - (x2 - x1) + intersectLine, x2, y1 - (x2 - x1) + intersectLine), primary, secondary)) && (overlap(new Line(x2, y1 - (x2 - x1) + intersectLine, x2, y1), primary, secondary)))
                    {
                        if (overlapOrientationalLines(new Line(x1, y1 - intersectLine, x1, y1), primary, secondary))
                        {
                            primary.Add(new Primary(x1, y1 - intersectLine, x1, y1, 3));
                        }
                        else
                        {
                            secondary.Add(new Secondary(x1, y1 - intersectLine, x1, y1));
                        }
                        primary.Add(new Primary(x1, y1 - intersectLine, x1 + intersectLine, y1 - intersectLine, 0));

                        if (intersectLine < (x2 - x1) / 2)
                        {
                            primary.Add(new Primary(x1 + intersectLine, y1 - (x2 - x1) + intersectLine, x1 + intersectLine, y1 - intersectLine, 3));
                        }
                        else if (intersectLine > (x2 - x1) / 2)
                        {
                            primary.Add(new Primary(x1 + intersectLine, y1 - (x2 - x1) + intersectLine, x1 + intersectLine, y1 - intersectLine, 1));
                        }

                        secondary.Add(new Secondary(x1 + intersectLine, y1 - intersectLine, x1 + intersectLine, y1));

                        primary.Add(new Primary(x1 + intersectLine, y1 - (x2 - x1) + intersectLine, x2, y1 - (x2 - x1) + intersectLine, 0));
                        if (overlapOrientationalLines(new Line(x2, y1 - (x2 - x1) + intersectLine, x2, y1), primary, secondary))
                        {
                            primary.Add(new Primary(x2, y1 - (x2 - x1) + intersectLine, x2, y1, 1));
                        }
                        else
                        {
                            secondary.Add(new Secondary(x2, y1 - (x2 - x1) + intersectLine, x2, y1));
                        }

                        squares.Add(new Rectangle(x1, y1 - intersectLine, intersectLine, intersectLine));
                        squares.Add(new Rectangle(x1 + intersectLine, y1 - (x2 - x1) + intersectLine, x2 - (x1 + intersectLine), y1 - (y1 - (x2 - x1) + intersectLine)));

                        i += 6;
                    }
                }
                else
                {
                    secondary.Add(new Secondary(x1, y1, x2, y2));
                    primary.RemoveAt(0);
                }
            }

            else if (x1 == x2 && orient == 1 && ((x1 >= (int)(squares[0].getX() + squares[0].getWidth())) || (x1 > (int)squares[0].getX() && (y2 < (int)squares[0].getY() || y1 > (int)(squares[0].getY() + squares[0].getHeight())))))
            { // right orig-square[2]
                if (y2 - y1 >= LINE_SIZE_MAX + LINE_SIZE_MAX)
                {
                    int intersectLine = r.Next(y2 - y1 - LINE_SIZE_MAX - LINE_SIZE_MAX + 1) + LINE_SIZE_MAX;
                    remove(primary, x1, y1, x2, y2, secondary);

                    if ((overlap(new Line(x1, y1, x2, y2), primary, secondary)) && (overlap(new Line(x1, y1, x1 + intersectLine, y1), primary, secondary)) && (overlap(new Line(x1 + intersectLine, y1, x1 + intersectLine, y1 + intersectLine), primary, secondary)) && (overlap(new Line(x1, y1 + intersectLine, x1 + intersectLine, y1 + intersectLine), primary, secondary)) && (overlap(new Line(x1, y1 + intersectLine, x1 + (y2 - y1) - intersectLine, y1 + intersectLine), primary, secondary)) && (overlap(new Line(x1 + (y2 - y1) - intersectLine, y1 + intersectLine, x1 + (y2 - y1) - intersectLine, y2), primary, secondary)) && (overlap(new Line(x1, y2, x1 + (y2 - y1) - intersectLine, y2), primary, secondary)))
                    {
                        if (overlapOrientationalLines(new Line(x1, y1, x1 + intersectLine, y1), primary, secondary))
                        {
                            primary.Add(new Primary(x1, y1, x1 + intersectLine, y1, 0));
                        }
                        else
                        {
                            secondary.Add(new Secondary(x1, y1, x1 + intersectLine, y1));
                        }
                        primary.Add(new Primary(x1 + intersectLine, y1, x1 + intersectLine, y1 + intersectLine, 1));

                        if (intersectLine < (y2 - y1) / 2)
                        {
                            primary.Add(new Primary(x1 + (y2 - y1) - intersectLine, y1 + intersectLine, x1 + intersectLine, y1 + intersectLine, 0));
                        }
                        else if (intersectLine > (y2 - y1) / 2)
                        {
                            primary.Add(new Primary(x1 + (y2 - y1) - intersectLine, y1 + intersectLine, x1 + intersectLine, y1 + intersectLine, 2));
                        }

                        secondary.Add(new Secondary(x1, y1 + intersectLine, x1 + intersectLine, y1 + intersectLine));

                        primary.Add(new Primary(x1 + (y2 - y1) - intersectLine, y1 + intersectLine, x1 + (y2 - y1) - intersectLine, y2, 1));
                        if (overlapOrientationalLines(new Line(x1, y2, x1 + (y2 - y1) - intersectLine, y2), primary, secondary))
                        {
                            primary.Add(new Primary(x1, y2, x1 + (y2 - y1) - intersectLine, y2, 2));
                        }
                        else
                        {
                            secondary.Add(new Secondary(x1, y2, x1 + (y2 - y1) - intersectLine, y2));
                        }

                        squares.Add(new Rectangle(x1, y1, intersectLine, intersectLine));
                        squares.Add(new Rectangle(x1, y1 + intersectLine, (y2 - y1) - intersectLine, y2 - (y1 + intersectLine)));

                        i += 6;
                    }
                }
                else
                {
                    secondary.Add(new Secondary(x1, y1, x2, y2));
                    primary.RemoveAt(0);
                }
            }

            else if (y1 == y2 && orient == 2 && ((y1 >= (int)(squares[0].getY() + squares[0].getHeight())) || (y1 > (int)squares[0].getY() && (x1 > (int)(squares[0].getX() + squares[0].getWidth()) || x2 < (int)squares[0].getX()))))
            { // down orig-square[3]
                if (x2 - x1 >= LINE_SIZE_MAX + LINE_SIZE_MAX)
                {
                    int intersectLine = r.Next(x2 - x1 - LINE_SIZE_MAX - LINE_SIZE_MAX + 1) + LINE_SIZE_MAX;
                    remove(primary, x1, y1, x2, y2, secondary);

                    if ((overlap(new Line(x1, y1, x2, y2), primary, secondary)) && (overlap(new Line(x1, y1, x1, y1 + intersectLine), primary, secondary)) && (overlap(new Line(x1 + intersectLine, y1, x1 + intersectLine, y1 + intersectLine), primary, secondary)) && (overlap(new Line(x1, y1 + intersectLine, x1 + intersectLine, y1 + intersectLine), primary, secondary)) && (overlap(new Line(x1 + intersectLine, y1, x1 + intersectLine, y1 + (x2 - x1) - intersectLine), primary, secondary)) && (overlap(new Line(x2, y1, x2, y1 + (x2 - x1) - intersectLine), primary, secondary)) && (overlap(new Line(x1 + intersectLine, y1 + (x2 - x1) - intersectLine, x2, y1 + (x2 - x1) - intersectLine), primary, secondary)))
                    {
                        if (overlapOrientationalLines(new Line(x1, y1, x1, y1 + intersectLine), primary, secondary))
                        {
                            primary.Add(new Primary(x1, y1, x1, y1 + intersectLine, 3));
                        }
                        else
                        {
                            secondary.Add(new Secondary(x1, y1, x1, y1 + intersectLine));
                        }
                        primary.Add(new Primary(x1, y1 + intersectLine, x1 + intersectLine, y1 + intersectLine, 2));

                        if (intersectLine < (x2 - x1) / 2)
                        {
                            primary.Add(new Primary(x1 + intersectLine, y1 + intersectLine, x1 + intersectLine, y1 + (x2 - x1) - intersectLine, 3));
                        }
                        else if (intersectLine > (x2 - x1) / 2)
                        {
                            primary.Add(new Primary(x1 + intersectLine, y1 + intersectLine, x1 + intersectLine, y1 + (x2 - x1) - intersectLine, 1));
                        }

                        secondary.Add(new Secondary(x1 + intersectLine, y1, x1 + intersectLine, y1 + intersectLine));

                        if (overlapOrientationalLines(new Line(x2, y1, x2, y1 + (x2 - x1) - intersectLine), primary, secondary))
                        {
                            primary.Add(new Primary(x2, y1, x2, y1 + (x2 - x1) - intersectLine, 1));
                        }
                        else
                        {
                            secondary.Add(new Secondary(x2, y1, x2, y1 + (x2 - x1) - intersectLine));
                        }
                        primary.Add(new Primary(x1 + intersectLine, y1 + (x2 - x1) - intersectLine, x2, y1 + (x2 - x1) - intersectLine, 2));

                        squares.Add(new Rectangle(x1, y1, intersectLine, intersectLine));
                        squares.Add(new Rectangle(x1 + intersectLine, y1, x2 - (x1 + intersectLine), (x2 - x1) - intersectLine));

                        i += 6;
                    }
                }
                else
                {
                    secondary.Add(new Secondary(x1, y1, x2, y2));
                    primary.RemoveAt(0);
                }
            }
            else if (x1 == x2 && orient == 3 && ((x2 <= (int)squares[0].getX()) || (x1 < (int)(squares[0].getY() + squares[0].getHeight()) && (y2 < (int)squares[0].getY() || y1 > (int)(squares[0].getY() + squares[0].getHeight())))))
            { // left // x2 <= ?? orig-square[0]
                if (y2 - y1 >= LINE_SIZE_MAX + LINE_SIZE_MAX)
                {
                    int intersectLine = r.Next(y2 - y1 - LINE_SIZE_MAX - LINE_SIZE_MAX + 1) + LINE_SIZE_MAX;
                    remove(primary, x1, y1, x2, y2, secondary);

                    if ((overlap(new Line(x1, y1, x2, y2), primary, secondary)) && (overlap(new Line(x1 - intersectLine, y1, x1 - intersectLine, y1 + intersectLine), primary, secondary)) && (overlap(new Line(x1 - intersectLine, y1, x1, y1), primary, secondary)) && (overlap(new Line(x1 - intersectLine, y1 + intersectLine, x1, y1 + intersectLine), primary, secondary)) && (overlap(new Line(x1 - (y2 - y1) + intersectLine, y1 + intersectLine, x1 - (y2 - y1) + intersectLine, y2), primary, secondary)) && (overlap(new Line(x1 - (y2 - y1) + intersectLine, y1 + intersectLine, x2, y1 + intersectLine), primary, secondary)) && (overlap(new Line(x1 - (y2 - y1) + intersectLine, y2, x2, y2), primary, secondary)))
                    {
                        primary.Add(new Primary(x1 - intersectLine, y1, x1 - intersectLine, y1 + intersectLine, 3));
                        if (overlapOrientationalLines(new Line(x1 - intersectLine, y1, x1, y1), primary, secondary))
                        {
                            primary.Add(new Primary(x1 - intersectLine, y1, x1, y1, 0)); // left squares on up side (left)
                        }
                        else
                        {
                            secondary.Add(new Secondary(x1 - intersectLine, y1, x1, y1));
                        }

                        if (intersectLine < (y2 - y1) / 2)
                        {
                            primary.Add(new Primary(x1 - (y2 - y1) + intersectLine, y1 + intersectLine, x1 - intersectLine, y1 + intersectLine, 0));
                        }
                        else if (intersectLine > (y2 - y1) / 2)
                        {
                            primary.Add(new Primary(x1 - (y2 - y1) + intersectLine, y1 + intersectLine, x1 - intersectLine, y1 + intersectLine, 2));
                        }

                        secondary.Add(new Secondary(x1 - intersectLine, y1 + intersectLine, x1, y1 + intersectLine));

                        primary.Add(new Primary(x1 - (y2 - y1) + intersectLine, y1 + intersectLine, x1 - (y2 - y1) + intersectLine, y2, 3));
                        if (overlapOrientationalLines(new Line(x1 - (y2 - y1) + intersectLine, y2, x2, y2), primary, secondary))
                        {
                            primary.Add(new Primary(x1 - (y2 - y1) + intersectLine, y2, x2, y2, 2));
                        }
                        else
                        {
                            secondary.Add(new Secondary(x1 - (y2 - y1) + intersectLine, y2, x2, y2));
                        }

                        squares.Add(new Rectangle(x1 - intersectLine, y1, intersectLine, intersectLine));
                        squares.Add(new Rectangle(x1 - (y2 - y1) + intersectLine, y1 + intersectLine, x2 - (x1 - (y2 - y1) + intersectLine), y2 - (y1 + intersectLine)));

                        i += 6;
                    }
                }
                else
                {
                    secondary.Add(new Secondary(x1, y1, x2, y2));
                    primary.RemoveAt(0);
                }
            }
            else
            {
                // coordinates do not fit into any of above criteria
                secondary.Add(new Secondary(x1, y1, x2, y2));
                primary.RemoveAt(0);
            }
        }
    }

    public static void remove(List<Primary> primary, int x1, int y1, int x2, int y2, List<Secondary> secondary)
    {
        for (int i = 0; i < primary.Count; i++)
        {
            if (primary[i].getX1() == x1 && primary[i].getY1() == y1 && primary[i].getX2() == x2 && primary[i].getY2() == y2)
            {
                secondary.Add(new Secondary(x1, y1, x2, y2));
                primary.RemoveAt(i);
            }
        }
    }

    public static bool overlapOrientationalLines(Line line, List<Primary> primary, List<Secondary> secondary)
    {
        for (int i = 0; i < primary.Count; i++)
        {
            if (primary[i].intersectOrientationalLine(line))
            {
                return false;
            }
        }
        for (int i = 0; i < secondary.Count; i++)
        {
            if (secondary[i].intersectOrientationalLine(line))
            {
                return false;
            }
        }
        return true;
    }

    public static bool overlap(Line line, List<Primary> primary, List<Secondary> secondary)
    {
        for (int i = 0; i < primary.Count; i++)
        {
            if (primary[i].intersectLine(line))
            {
                return false;
            }
        }
        for (int i = 0; i < secondary.Count; i++)
        {
            if (secondary[i].intersectLine(line))
            {
                return false;
            }
        }
        return true;
    }

}


