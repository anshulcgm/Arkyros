using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Threading;
using System.Linq;

public class Map
{
    Vector3 planetOrigin;
    float mapSize;
    float planetRad;
    float planetMaxRad;
    float chunksize;
    float spaceBetweenNodes;

    //calculated values from above inputs
    Vector3 mapOrigin;
    int numNodesInChunkLine;
    int numChunksInLength;
    int numChunks;

    CompressedMapWriter c = new CompressedMapWriter("map.bin");
    Stream mapChunkStream = new FileStream("mapChunks.bin", FileMode.Create);


    private class Tuple : IComparable
    {
        public int a, b;
        public Tuple(int a, int b)
        {
            this.a = a; this.b = b;
        }

        public int CompareTo(object obj)
        {
            Tuple t = (Tuple)obj;
            if (t == null)
            {
                return -1;
            }
            return a.CompareTo(t.a);
        }
    }

    public Map(Vector3 planetOrigin, float planetRad, float planetMaxRad, float mapSize, float chunksize, float spaceBetweenNodes)
    {
        //save values
        this.planetOrigin = planetOrigin;
        this.mapSize = mapSize;
        this.planetRad = planetRad;
        this.planetMaxRad = planetMaxRad;
        this.chunksize = chunksize;
        this.spaceBetweenNodes = spaceBetweenNodes;

        //calculate derivative values for easier calculations later
        mapOrigin = planetOrigin + new Vector3(-mapSize / 2, -mapSize / 2, -mapSize / 2);
        numNodesInChunkLine = (int)(chunksize / spaceBetweenNodes);
        numChunks = (int)((mapSize * mapSize * mapSize) / (chunksize * chunksize * chunksize));
        numChunksInLength = (int)(mapSize / chunksize);        
    }

    //calculated values for converting file info to nodes later.
    int numActualChunks = 0;
    int numConnsPerNode = 0;
    
    int i = 0;
    int currActualChunk = 0;
    bool lastChunkForbidden = false;
    List<bool> bitbuffer = new List<bool>();
    /* parameters: none
     * returns: none
     * calculates map and saves binary information to the file with name "mapFileName"
     */
    
    public void CreateMap()
    {
        Debug.Log(numChunks);
        int numIterations = 0;
        for(int i = 0; i < numChunks; i++)
        {
            Vector3 chunkPosn = mapOrigin + new Vector3((i % numChunksInLength) * chunksize,
                                                       ((i / numChunksInLength) % numChunksInLength) * chunksize,
                                                       ((i / (numChunksInLength * numChunksInLength)) % numChunksInLength) * chunksize);


            List<Vector3> chunkCorners = new List<Vector3>
            {
                //original
                chunkPosn,

                //1-axis displacement
                chunkPosn + new Vector3(chunksize, 0, 0),
                chunkPosn + new Vector3(0, chunksize, 0),
                chunkPosn + new Vector3(0, 0, chunksize),

                //2-axis displacement
                chunkPosn + new Vector3(chunksize, chunksize, 0),
                chunkPosn + new Vector3(chunksize, 0, chunksize),
                chunkPosn + new Vector3(0, chunksize, chunksize),

                //3-axis displacement
                chunkPosn + new Vector3(chunksize, chunksize, chunksize)
            };
            
            int numInside = 0;
            int numOutside = 0;
            foreach (Vector3 corner in chunkCorners)
            {
                float sqrDist = Vector3.SqrMagnitude(corner - planetOrigin);
                if (sqrDist < planetRad * planetRad)
                {
                    numInside++;
                }
                if(sqrDist > planetMaxRad * planetMaxRad)
                {
                    numOutside++;
                }
            }
            bool chunkNotInRange = Mathf.Abs(numInside - numOutside) == chunkCorners.Count;

            if (!chunkNotInRange)
            {
                mapChunkStream.Write(BitConverter.GetBytes(c.NumBytesWritten), 0, 4);
                Debug.DrawLine(chunkPosn + new Vector3(chunksize / 2f, chunksize / 2f, chunksize / 2f), chunkPosn + new Vector3(chunksize / 2f, chunksize / 2f, chunksize / 2f) + Vector3.up * chunksize, Color.cyan, 100000.0f);
                numActualChunks++;
                int numBufferedTrue = 0;
                int maxChunkBreak = 4;
                List<int> chunkIds = RecursiveBreak(chunkPosn, chunksize, maxChunkBreak);
                //if the chunk is empty, don't print anything.
                if (chunkIds.Count == 0)
                {
                    numBufferedTrue = -1;
                }
                int currChunkId = 0;
                for (int i1 = 0; i1 < chunkIds.Count; i1++)
                {
                    int delta = chunkIds[i1] - currChunkId;
                    numBufferedTrue += (int)(delta * 3 * chunksize * chunksize * chunksize / (Mathf.Pow(Mathf.Pow(2, maxChunkBreak), 3)));
                    int numNodesInBrokenChunk = (int)((chunksize / Mathf.Pow(2, maxChunkBreak)) / spaceBetweenNodes);
                    int numBrokenChunksInChunkLength = (int)Mathf.Pow(2, maxChunkBreak);
                    Vector3 brokenChunkPosn = chunkPosn + new Vector3((chunkIds[i1] % numBrokenChunksInChunkLength) * chunksize / numBrokenChunksInChunkLength,
                                                                     ((chunkIds[i1] / numBrokenChunksInChunkLength) % numBrokenChunksInChunkLength) * chunksize / numBrokenChunksInChunkLength,
                                                                     ((chunkIds[i1] / (numBrokenChunksInChunkLength * numBrokenChunksInChunkLength)) % numBrokenChunksInChunkLength) * chunksize / numBrokenChunksInChunkLength);
                    for (int a = 0; a < numNodesInBrokenChunk; a++)
                    {
                        for (int b = 0; b < numNodesInBrokenChunk; b++)
                        {
                            numIterations++;
                            numBufferedTrue = ScanChunkLine(brokenChunkPosn + new Vector3(a * spaceBetweenNodes, b * spaceBetweenNodes, 0), brokenChunkPosn + new Vector3(a * spaceBetweenNodes, b * spaceBetweenNodes, (chunksize / Mathf.Pow(2, maxChunkBreak))), c, numBufferedTrue);
                            numBufferedTrue = ScanChunkLine(brokenChunkPosn + new Vector3(a * spaceBetweenNodes, 0, b * spaceBetweenNodes), brokenChunkPosn + new Vector3(a * spaceBetweenNodes, (chunksize / Mathf.Pow(2, maxChunkBreak)), b * spaceBetweenNodes), c, numBufferedTrue);
                            numBufferedTrue = ScanChunkLine(brokenChunkPosn + new Vector3(0, a * spaceBetweenNodes, b * spaceBetweenNodes), brokenChunkPosn + new Vector3((chunksize / Mathf.Pow(2, maxChunkBreak)), a * spaceBetweenNodes, b * spaceBetweenNodes), c, numBufferedTrue);
                        }
                    }
                }
                if (numBufferedTrue != -1)
                {
                    c.Write(numBufferedTrue);
                    c.RoundOff();
                }
            }
            else
            {
                mapChunkStream.Write(BitConverter.GetBytes(-1), 0, 4);
            }
        }
        c.Close();
        mapChunkStream.Write(BitConverter.GetBytes(c.NumBytesWritten), 0, 4);
        mapChunkStream.Flush();
        mapChunkStream.Close();

        Debug.Log(numIterations);
        Debug.Log(numActualChunks);
    }

    int numBufferedTrue = 0;
    
    public struct RecursiveChunk
    {
        public Vector3 chunkPosn;
        public float chunkSize;
        public int level;
    }
    
    //recursively breaks down each chunk until it gets to a maximum level.
    public List<int> RecursiveBreak(Vector3 origin, float orgChunksize, int maxLevel)
    {
        List<RecursiveChunk> chunksToProcess = new List<RecursiveChunk>();
        chunksToProcess.Add(new RecursiveChunk { chunkPosn = origin, chunkSize = orgChunksize, level = 0 });
        List<int> chunkIds = new List<int>();
        while (chunksToProcess.Count > 0)
        {
            Vector3 chunkPosn = chunksToProcess[0].chunkPosn;
            float chunksize = chunksToProcess[0].chunkSize;
            int level = chunksToProcess[0].level;
            chunksToProcess.RemoveAt(0);
            if(Physics.BoxCast(chunkPosn + new Vector3(chunksize / 2f, 0, chunksize / 2f), new Vector3(chunksize / 2f, 1, chunksize / 2f), Vector3.up, Quaternion.identity, chunksize))
            {
                if (level >= maxLevel)
                {
                    int chunkIdX = (int)(((chunkPosn.x - origin.x) * orgChunksize * orgChunksize) / (chunksize * chunksize * chunksize));
                    int chunkIdY = (int)(((chunkPosn.y - origin.y) * orgChunksize) / (chunksize * chunksize));
                    int chunkIdZ = (int)((chunkPosn.z - origin.z) / (chunksize));
                    chunkIds.Add(chunkIdX + chunkIdY + chunkIdZ);                    
                    continue;
                }
                chunksToProcess.Add(new RecursiveChunk { chunkPosn = chunkPosn, chunkSize = chunksize / 2f, level = level + 1 });
                chunksToProcess.Add(new RecursiveChunk { chunkPosn = chunkPosn + new Vector3(chunksize / 2, 0, 0), chunkSize = chunksize / 2f, level = level + 1 });
                chunksToProcess.Add(new RecursiveChunk { chunkPosn = chunkPosn + new Vector3(0, chunksize / 2, 0), chunkSize = chunksize / 2f, level = level + 1 });
                chunksToProcess.Add(new RecursiveChunk { chunkPosn = chunkPosn + new Vector3(0, 0, chunksize / 2), chunkSize = chunksize / 2f, level = level + 1 });
                chunksToProcess.Add(new RecursiveChunk { chunkPosn = chunkPosn + new Vector3(chunksize / 2, chunksize / 2, 0), chunkSize = chunksize / 2f, level = level + 1 });
                chunksToProcess.Add(new RecursiveChunk { chunkPosn = chunkPosn + new Vector3(chunksize / 2, 0, chunksize / 2), chunkSize = chunksize / 2f, level = level + 1 });
                chunksToProcess.Add(new RecursiveChunk { chunkPosn = chunkPosn + new Vector3(0, chunksize / 2, chunksize / 2), chunkSize = chunksize / 2f, level = level + 1 });
                chunksToProcess.Add(new RecursiveChunk { chunkPosn = chunkPosn + new Vector3(chunksize / 2, chunksize / 2, chunksize / 2), chunkSize = chunksize / 2f, level = level + 1 });
            }
        }
        return chunkIds;        
    }
    
    /*
    public List<NavNode> GetChunkAt(Vector3 pos)
    {
        if (!mapExists)
        {
            throw new Exception("Map currently does not exist. Call CreateMap() before calling this function.");
        }

        Vector3 posn = pos - mapOrigin;
        int x, y, z;
        x = (int)(posn.x / chunksize); y = (int)(posn.y / chunksize); z = (int)(posn.z / chunksize);
        int chunkIndex = x + y * numChunksInLength + z * numChunksInLength * numChunksInLength;

        int closestIndex = BinarySearchClosestIndex(chunkMapping, chunkIndex);
        int trueAdjust = chunkMapping[closestIndex].b;
        if (chunkMapping[closestIndex].a > chunkIndex)
        {
            if(closestIndex == 0)
            {
                trueAdjust = 0;
            }
            else
            {
                trueAdjust = chunkMapping[closestIndex - 1].b;
            }
        }

        br.BaseStream.Seek((numBytesTotal / numActualChunks) * (chunkIndex + trueAdjust), SeekOrigin.Begin);
        byte[] bytes = br.ReadBytes(numBytesTotal / numActualChunks);

        List<NavNode> chunk = new List<NavNode>();
        List<bool> buffer = new List<bool>();
        for(int i = 0; i < bytes.Length; i++)
        {
            for(int i1 = 0; i1 < 8; i1++)
            {
                buffer.Add(GetBit(bytes[i], i1));
            }
            while(buffer.Count >= numConnsPerNode)
            {
                chunk.Add(GetFirstNodeFromBuffer(buffer));
            }
        }
        return chunk;
    }
    */
    #region helper functions

    private NavNode GetFirstNodeFromBuffer(List<bool> buffer)
    {
        bool[] cons = new bool[numConnsPerNode];
        if (buffer.Count < numConnsPerNode)
        {
            throw new Exception("buffer must have at least " + numConnsPerNode + " binary values in it.");
        }
        for(int i = 0; i < numConnsPerNode; i++)
        {
            cons[i] = buffer[0];
            buffer.RemoveAt(0);
        }
        return new NavNode(cons);
    }

    private int BinarySearchClosestIndex(List<Tuple> a, int chunkIndex)
    {
        int first = 0;
        int last = a.Count - 1;
        int mid = 0;
        do
        {
            mid = first + (last - first) / 2;
            if (chunkIndex > a[mid].a)
                first = mid + 1;
            else
                last = mid - 1;
            if (a[mid].a == chunkIndex)
                return mid;
        } while (first <= last);
        return mid;
    }


    /* parameters: start position of scan, end position of scan, map writer, number of nodes that were true before calling this function.
     * returns: list of boolean values for node connections in direction.
     */
    public int ScanChunkLine(Vector3 start, Vector3 end, CompressedMapWriter c, int numNodesTrue)
    {
        int _numNodesTrue = numNodesTrue;
        Vector3 dir = (end - start).normalized * spaceBetweenNodes;
        float dirLerp = InverseLerp(start, end, start + dir);
        Vector3 rayCastStart = start;
        //Long story short, Vector3 float equality checking is wierd. DO NOT replace this statement with a !=
        while(!(rayCastStart == end))
        {
            RaycastHit hit;
            if(Physics.Raycast(rayCastStart, dir, out hit, Vector3.Distance(rayCastStart, end),  ~LayerMask.GetMask(new string[] { "ocean" })))
            {
                Vector3 nodePosn = rayCastStart;
                float hitPtLerp = InverseLerp(start, end, hit.point);
                float nodePtLerp = InverseLerp(start, end, nodePosn);                
                c.Write((int)((hitPtLerp - nodePtLerp) /dirLerp));
                c.Write(new List<bool> { false });
                _numNodesTrue = 0;
                rayCastStart = ((int)((hitPtLerp - nodePtLerp) / dirLerp) + 1) * dir + start;
            }
            else
            {
                Vector3 nodePosn = rayCastStart;
                float nodePtLerp = InverseLerp(start, end, nodePosn);
                c.Write((int)((1 - nodePtLerp) / dirLerp));
                rayCastStart = end;
                _numNodesTrue = (int)((1 - nodePtLerp) / dirLerp);
            }
        }
        return _numNodesTrue;
    }

    private float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }
    #endregion
}

public class CompressedMapWriter
{
    //for writing to the file
    private string mapFileName = null;
    private Stream stream;
    private BinaryWriter bw;

    //for storing bits to be written to file
    private List<bool> buffer;
    private int numBufferedTrue = 0;
    private bool bufferIsTrue = true;

    //for keeping track of how many bytes have been written by this writer to the file.
    public int NumBytesWritten { get; internal set; }

    public CompressedMapWriter(string mapFileName)
    {
        this.mapFileName = mapFileName;
        NumBytesWritten = 0;
        //create new file, open stream, open binary writer
        stream = new FileStream(mapFileName, FileMode.Create);
        bw = new BinaryWriter(stream);
        buffer = new List<bool>();
    }

    public void Write(int numTrue)
    {
        numBufferedTrue += numTrue;
    }

    /* parameters: List of bits
     * returns: none
     * writes bits directly to file, or if possible, compresses bits and writes compressed bits to file.
     */
    public void Write(List<bool> bits)
    {
        if (bufferIsTrue)
        {
            int b = 0;
            while (b < bits.Count && bits[b])
            {
                numBufferedTrue++;
                b++;
            }

            if(b == bits.Count)
            {
                return;
            }

            if (!bits[b])
            {
                if (numBufferedTrue < 21)
                {
                    for (int i = 0; i < numBufferedTrue; i++)
                    {
                        buffer.Add(true);
                    }
                    for (int i = b; i < bits.Count; i++)
                    {
                        buffer.Add(bits[i]);
                    }
                    //write bytes to file while we have enough data.
                    while (buffer.Count >= 7)
                    {
                        buffer.Insert(0, false);
                        bw.Write(GetFirstByteFromBuffer(buffer));
                    }
                    bufferIsTrue = true;
                    for(int i = 0; i < buffer.Count; i++)
                    {
                        if (!buffer[i])
                        {
                            bufferIsTrue = false;
                            break;
                        }
                    }
                    numBufferedTrue = bufferIsTrue ? buffer.Count : 0;                    
                }
                //compressed data writing
                else
                {
                    byte[] bytes = BitConverter.GetBytes(numBufferedTrue);
                    List<bool> buff = new List<bool>();
                    for (int i1 = 0; i1 < bytes.Length; i1++)
                    {
                        for (int i2 = 0; i2 < 8; i2++)
                        {
                            buff.Add(GetBit(bytes[i1], i2));
                        }
                    }

                    while (!buff[buff.Count - 1])
                    {
                        buff.RemoveAt(buff.Count - 1);
                    }
                    int n = 6;
                    while (n < buff.Count)
                    {
                        n += 7;
                    }
                    while (buff.Count < n)
                    {
                        buff.Add(false);
                    }
                    buff.Insert(0, true);
                    buff.Add(true);
                    int currPosInBuffer = 7;
                    while (currPosInBuffer < buff.Count - 1)
                    {
                        buff.Insert(currPosInBuffer, false);
                        currPosInBuffer += 8;
                    }
                    //write in compressed data
                    while (buff.Count >= 8)
                    {
                        bw.Write(GetFirstByteFromBuffer(buff));
                    }
                    numBufferedTrue = 0;
                    bufferIsTrue = false;
                    for (int i = b; i < bits.Count; i++)
                    {
                        buffer.Add(bits[i]);
                    }
                }
            }
        }
        else
        {
            buffer.AddRange(bits);
            //write bytes to file while we have enough data.
            while (buffer.Count >= 7)
            {
                buffer.Insert(0, false);
                bw.Write(GetFirstByteFromBuffer(buffer));
            }
            bufferIsTrue = true;
            for (int i = 0; i < buffer.Count; i++)
            {
                if (!buffer[i])
                {
                    bufferIsTrue = false;
                    break;
                }
            }
            numBufferedTrue = bufferIsTrue ? buffer.Count : 0;
        }
    }

    public void RoundOff()
    {
        //write in a false value
        Write(new List<bool> { false });
        //append zeros until we have a complete byte
        while (buffer.Count < 8)
        {
            buffer.Add(false);
        }
        //write in the last byte
        bw.Write(GetFirstByteFromBuffer(buffer));
        numBufferedTrue = 0;
        bufferIsTrue = true;
        buffer = new List<bool>();
    }

    /* parameters: none
     * returns: none
     * closes the writer permanently, converts any last bits to bytes and writes them to the file.
     */
    public void Close()
    {
        bw.Flush();
        bw.Close();
        stream.Close();
    }

    /* parameters: list of booleans
     * returns: the first byte from the list of booleans (a byte is 8 boolean values)
     * returns the first byte it can get from the list of booleans and removes that byte from the list of booleans.
     */
    private byte GetFirstByteFromBuffer(List<bool> bitbuffer)
    {
        NumBytesWritten++;
        if (bitbuffer.Count < 8)
        {
            throw new Exception("buffer must have at least 8 binary values in it.");
        }
        bool[] a = new bool[8];
        for (int b = 0; b < 8; b++)
        {
            a[b] = bitbuffer[0];
            bitbuffer.RemoveAt(0);
        }
        return ConvertBoolArrayToByte(a);
    }

    private byte ConvertBoolArrayToByte(bool[] source)
    {
        byte result = 0;
        // This assumes the array never contains more than 8 elements!
        int index = 8 - source.Length;

        // Loop through the array
        foreach (bool b in source)
        {
            // if the element is 'true' set the bit at that position
            if (b)
                result |= (byte)(1 << (7 - index));

            index++;
        }

        return result;
    }

    private static bool GetBit(byte b, int bitNumber)
    {
        return (b & (1 << bitNumber)) != 0;
    }
}
