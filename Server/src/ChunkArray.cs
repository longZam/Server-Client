using System.Runtime.InteropServices;

public class ChunkArray<T>
{
    private const int ChunkSizeKb = 16;
    private const int ChunkSizeBytes = 1024 * ChunkSizeKb;
    private readonly int ChunkSize = ChunkSizeBytes / Marshal.SizeOf<T>();

    private readonly List<T[]> chunks;

    public int Length => chunks.Count * ChunkSize;


    public ChunkArray(int capacity = 0)
    {
        int initChunkCount = capacity / ChunkSize + 1;

        if (capacity % ChunkSize > 0)
            initChunkCount += 1;

        chunks = new List<T[]>(initChunkCount);
        CreateChunk(initChunkCount);
    }

    public ref T this[int i]
    {
        get
        {
            int chunkIndex = i / ChunkSize;
            int index = i % ChunkSize;

            int createChunkAmount = chunkIndex - chunks.Count + 1;
            CreateChunk(createChunkAmount);

            return ref chunks[chunkIndex][index];
        }
    }


    private void CreateChunk(int amount)
    {
        for (int i = 0; i < amount; i++)
            chunks.Add(new T[ChunkSize]);
    }
}