using FixedMath.NET;

namespace ECS;


public class World
{
    #region Delegates

    public delegate void ForEachDelegate<T1>(int index, ref T1 arg1);
    public delegate void ForEachDelegate<T1, T2>(int index, ref T1 arg1, ref T2 arg2);
    public delegate void ForEachDelegate<T1, T2, T3>(int index, ref T1 arg1, ref T2 arg2, ref T3 arg3);
    public delegate void ForEachDelegate<T1, T2, T3, T4>(int index, ref T1 arg1, ref T2 arg2, ref T3 arg3, ref T4 arg4);
    public delegate void ForEachDelegate<T1, T2, T3, T4, T5>(int index, ref T1 arg1, ref T2 arg2, ref T3 arg3, ref T4 arg4, ref T5 arg5);
    public delegate void ForEachDelegate<T1, T2, T3, T4, T5, T6>(int index, ref T1 arg1, ref T2 arg2, ref T3 arg3, ref T4 arg4, ref T5 arg5, ref T6 arg6);
    public delegate void ForEachDelegate<T1, T2, T3, T4, T5, T6, T7>(int index, ref T1 arg1, ref T2 arg2, ref T3 arg3, ref T4 arg4, ref T5 arg5, ref T6 arg6, ref T7 arg7);
    public delegate void ForEachDelegate<T1, T2, T3, T4, T5, T6, T7, T8>(int index, ref T1 arg1, ref T2 arg2, ref T3 arg3, ref T4 arg4, ref T5 arg5, ref T6 arg6, ref T7 arg7, ref T8 arg8);

    #endregion

    private readonly Entities Entities;
    private readonly Components Components;
    private readonly Systems Systems;

    public int CurrentTick { get; private set; }


    public World()
    {
        Entities = new Entities();
        Components = new Components();
        Systems = new Systems();
    }

    public void Tick(Fix64 deltaTime)
    {
        Systems.Tick(this, deltaTime);
        CurrentTick += 1;
    }

    #region Query

    public void Test<T1>(ForEachDelegate<T1> action)
        where T1 : struct, IComponent
    {
        if (!Components.IsRegister(typeof(T1)))
            throw new Exception();

        long mask = Components.GetMask(typeof(T1));

        var t1 = Components.GetComponentBag<T1>();

        for (int i = 0; i < Entities.Count; i++)
        {
            if ((Entities[i] & mask) == mask)
                action(i, ref t1[i]);
        }
    }

    public void ForEach<T1>(ForEachDelegate<T1> action)
        where T1 : struct, IComponent
    {
        if (!Components.IsRegister(typeof(T1)))
            throw new Exception();

        long mask = Components.GetMask(typeof(T1));

        var t1 = Components.GetComponentBag<T1>();

        Parallel.For(0, Entities.Count, i =>
        {
            if ((Entities[i] & mask) == mask)
                action(i, ref t1[i]);
        });
    }

    public void ForEach<T1, T2>(ForEachDelegate<T1, T2> action)
        where T1 : struct, IComponent
        where T2 : struct, IComponent
    {
        if (!Components.IsRegister(typeof(T1), typeof(T2)))
            throw new Exception();

        long mask = Components.GetMask(typeof(T1), typeof(T2));

        var t1 = Components.GetComponentBag<T1>();
        var t2 = Components.GetComponentBag<T2>();

        Parallel.For(0, Entities.Count, i =>
        {
            if ((Entities[i] & mask) == mask)
                action(i, ref t1[i], ref t2[i]);
        });
    }

    public void ForEach<T1, T2, T3>(ForEachDelegate<T1, T2, T3> action)
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
    {
        if (!Components.IsRegister(typeof(T1), typeof(T2), typeof(T3)))
            throw new Exception();

        long mask = Components.GetMask(typeof(T1), typeof(T2), typeof(T3));

        var t1 = Components.GetComponentBag<T1>();
        var t2 = Components.GetComponentBag<T2>();
        var t3 = Components.GetComponentBag<T3>();

        Parallel.For(0, Entities.Count, i =>
        {
            if ((Entities[i] & mask) == mask)
                action(i, ref t1[i], ref t2[i], ref t3[i]);
        });
    }

    public void ForEach<T1, T2, T3, T4>(ForEachDelegate<T1, T2, T3, T4> action)
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
    {
        if (!Components.IsRegister(typeof(T1), typeof(T2), typeof(T3), typeof(T4)))
            throw new Exception();

        long mask = Components.GetMask(typeof(T1), typeof(T2), typeof(T3), typeof(T4));

        var t1 = Components.GetComponentBag<T1>();
        var t2 = Components.GetComponentBag<T2>();
        var t3 = Components.GetComponentBag<T3>();
        var t4 = Components.GetComponentBag<T4>();

        Parallel.For(0, Entities.Count, i =>
        {
            if ((Entities[i] & mask) == mask)
                action(i, ref t1[i], ref t2[i], ref t3[i], ref t4[i]);
        });
    }

    public void ForEach<T1, T2, T3, T4, T5>(ForEachDelegate<T1, T2, T3, T4, T5> action)
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
    {
        if (!Components.IsRegister(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)))
            throw new Exception();

        long mask = Components.GetMask(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));

        var t1 = Components.GetComponentBag<T1>();
        var t2 = Components.GetComponentBag<T2>();
        var t3 = Components.GetComponentBag<T3>();
        var t4 = Components.GetComponentBag<T4>();
        var t5 = Components.GetComponentBag<T5>();

        Parallel.For(0, Entities.Count, i =>
        {
            if ((Entities[i] & mask) == mask)
                action(i, ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i]);
        });
    }

    public void ForEach<T1, T2, T3, T4, T5, T6>(ForEachDelegate<T1, T2, T3, T4, T5, T6> action)
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
    {
        if (!Components.IsRegister(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)))
            throw new Exception();

        long mask = Components.GetMask(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));

        var t1 = Components.GetComponentBag<T1>();
        var t2 = Components.GetComponentBag<T2>();
        var t3 = Components.GetComponentBag<T3>();
        var t4 = Components.GetComponentBag<T4>();
        var t5 = Components.GetComponentBag<T5>();
        var t6 = Components.GetComponentBag<T6>();

        Parallel.For(0, Entities.Count, i =>
        {
            if ((Entities[i] & mask) == mask)
                action(i, ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i]);
        });
    }

    public void ForEach<T1, T2, T3, T4, T5, T6, T7>(ForEachDelegate<T1, T2, T3, T4, T5, T6, T7> action)
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent
    {
        if (!Components.IsRegister(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7)))
            throw new Exception();

        long mask = Components.GetMask(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));

        var t1 = Components.GetComponentBag<T1>();
        var t2 = Components.GetComponentBag<T2>();
        var t3 = Components.GetComponentBag<T3>();
        var t4 = Components.GetComponentBag<T4>();
        var t5 = Components.GetComponentBag<T5>();
        var t6 = Components.GetComponentBag<T6>();
        var t7 = Components.GetComponentBag<T7>();

        Parallel.For(0, Entities.Count, i =>
        {
            if ((Entities[i] & mask) == mask)
                action(i, ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i]);
        });
    }

    public void ForEach<T1, T2, T3, T4, T5, T6, T7, T8>(ForEachDelegate<T1, T2, T3, T4, T5, T6, T7, T8> action)
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent
        where T8 : struct, IComponent
    {
        if (!Components.IsRegister(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8)))
            throw new Exception();

        long mask = Components.GetMask(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));

        var t1 = Components.GetComponentBag<T1>();
        var t2 = Components.GetComponentBag<T2>();
        var t3 = Components.GetComponentBag<T3>();
        var t4 = Components.GetComponentBag<T4>();
        var t5 = Components.GetComponentBag<T5>();
        var t6 = Components.GetComponentBag<T6>();
        var t7 = Components.GetComponentBag<T7>();
        var t8 = Components.GetComponentBag<T8>();

        Parallel.For(0, Entities.Count, i =>
        {
            if ((Entities[i] & mask) == mask)
                action(i, ref t1[i], ref t2[i], ref t3[i], ref t4[i], ref t5[i], ref t6[i], ref t7[i], ref t8[i]);
        });
    }

    #endregion


    public void RegisterComponentType<T>() where T : struct, IComponent => Components.RegisterComponentType<T>();
    public int CreateEntity() => Entities.CreateEntity();
    public void AddComponent<T>(int id, T data) where T : struct, IComponent
    {
        Components.AddComponent(id, data);
        Entities[id] |= Components.GetMask(typeof(T));
    }
    public void RegisterSystem<T>(T system) where T : System => Systems.RegisterSystem(system);
}