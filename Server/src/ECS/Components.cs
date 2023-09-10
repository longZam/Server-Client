namespace ECS;


public class Components
{
    private readonly Dictionary<Type, IComponentBag> components;
    private readonly List<Type> registerTypes;


    public Components()
    {
        components = new Dictionary<Type, IComponentBag>();
        registerTypes = new List<Type>();
    }

    public void RegisterComponentType<T>() where T : struct, IComponent
    {
        if (registerTypes.Contains(typeof(T)))
            throw new Exception();

        registerTypes.Add(typeof(T));
        components.Add(typeof(T), new ComponentBag<T>());
    }

    public void AddComponent<T>(int id, T data) where T : struct, IComponent
    {
        ComponentBag<T> bag = (ComponentBag<T>)components[typeof(T)];
        bag.AddComponent(id, data);
    }

    public ComponentBag<T> GetComponentBag<T>() where T : struct, IComponent
    {
        return (ComponentBag<T>)components[typeof(T)];
    }

    public long GetMask(params Type[] types)
    {
        for (int i = 0; i < types.Length; i++)
            if (!registerTypes.Contains(types[i]))
                throw new Exception();

        long result = 0;

        for (int i = 0; i < types.Length; i++)
            result |= (long)1 << registerTypes.IndexOf(types[i]);

        return result;
    }

    public bool IsRegister(params Type[] types)
    {
        foreach (Type type in types)
            if (!registerTypes.Contains(type))
                return false;
        
        return true;
    }
}