class FooCommand : Wcp.Command
{
  public FooCommand(string[] args) : base(args, false) { }

  [Wcp.Protocol(name: "proto")]
  public void Run()
  {
    Console.WriteLine($"cmd= {ctx.cmd}");
    Console.WriteLine($"q= {ctx.q}");
    Console.WriteLine($"param= {string.Join(", ", ctx.parameters.Select(p => $"{p.Key}={p.Value}"))}");
  }

}

class EntryPoint
{
  public static void Main(string[] args)
  {
    new FooCommand(args).run();
    Console.WriteLine("end");
    Console.ReadKey();
  }
}