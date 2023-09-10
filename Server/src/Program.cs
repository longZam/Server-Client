using System.Diagnostics;
using FixedMath.NET;
using ECS.Physics;
using ECS;

class Program
{
    static int Main(string[] args)
    {
        World world = new World();
        world.RegisterComponentType<AABB>();
        world.RegisterComponentType<Transform>();
        world.RegisterSystem(new TransformUpSystem());
        world.RegisterSystem(new AABBSeparateSystem());

        Random random = new Random(0);

        for (int i = 0; i < 10; i++)
        {
            int id = world.CreateEntity();
            Vector2 randomPosition = new Vector2(new Fix64(random.Next(-5, 5)), new Fix64(random.Next(-5, 5)));
            world.AddComponent(id, new AABB(randomPosition, Vector2.one));
        }

        int id0 = world.CreateEntity();
        world.AddComponent(id0, new AABB(Vector2.zero, Vector2.one));
        world.AddComponent(id0, new Transform());

        Console.WriteLine("World 생성 성공");
        
        GameRunner runner = new GameRunner(world, 1);
        runner.Run();
        Console.WriteLine("GameRunner 실행 성공");

        ReadCommand();

        runner.Pause();
        Console.WriteLine("GameRunner 종료 성공");

        return 0;
    }

    private static void ReadCommand()
    {
        while (true)
        {
            string? command = Console.ReadLine();

            if (command == null)
                continue;
            
            if (command == "stop")
            {
                break;
            }
        }
    }
}
