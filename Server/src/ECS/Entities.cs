namespace ECS;


public class Entities
{
    private const int EntityChunkSizeKb = 16;
    private const int EntityChunkSizeBytes = 1024 * EntityChunkSizeKb;
    private const int EntityChunkSize = EntityChunkSizeBytes / sizeof(long);

    private readonly List<long[]> entityChunks;
    public int Count { get; private set; }


    public Entities()
    {
        this.entityChunks = new List<long[]>();
        this.Count = 0;
    }

    public ref long this[int i]
    {
        get
        {
            if (i >= Count)
                throw new IndexOutOfRangeException();

            int chunkIndex = i / EntityChunkSize;
            int index = i % EntityChunkSize;

            return ref entityChunks[chunkIndex][index];
        }
    }


    public int CreateEntity()
    {
        int chunkIndex = Count / EntityChunkSize;
        int index = Count % EntityChunkSize;

        if (chunkIndex >= entityChunks.Count)
            AddEntityChunk();

        entityChunks[chunkIndex][index] = 0;

        return Count++;
    }

    private void AddEntityChunk()
    {
        entityChunks.Add(new long[EntityChunkSize]);
    }

}