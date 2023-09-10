using System.Collections;
using System.Runtime.InteropServices;

namespace ECS;

// 자료 묶어두는 용도로만 씀
public interface IComponentBag
{

}

// 컴포넌트를 청크 단위로 관리하고 청크에 따른 인덱싱을 제공
public class ComponentBag<T> : IComponentBag where T : struct, IComponent
{
    private const int ComponentChunkSizeKb = 16;
    private const int ComponentChunkSizeBytes = 1024 * ComponentChunkSizeKb;
    private readonly int ComponentChunkSize = ComponentChunkSizeBytes / Marshal.SizeOf<T>();

    private readonly List<T[]> chunks;


    public ComponentBag()
    {
        chunks = new List<T[]>();
    }

    public ref T this[int i]
    {
        get
        {
            int chunkIndex = i / ComponentChunkSize;
            int index = i % ComponentChunkSize;

            if (chunkIndex >= chunks.Count)
                throw new IndexOutOfRangeException();

            return ref chunks[chunkIndex][index];
        }
    }

    public void AddComponent(int id, T data)
    {
        int chunkIndex = id / ComponentChunkSize;
        int index = id % ComponentChunkSize;

        int needChunkAmount = chunkIndex - chunks.Count + 1;

        for (int i = 0; i < needChunkAmount; i++)
            AddChunk();
        
        chunks[chunkIndex][index] = data;
    }

    private void AddChunk()
    {
        chunks.Add(new T[ComponentChunkSize]);
    }
}