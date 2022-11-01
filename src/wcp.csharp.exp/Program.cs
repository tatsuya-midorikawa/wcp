class FooCommand : Wcp.Command {
  public FooCommand(string[] args) : base(args, false) { }

  [Wcp.Protocol(name: "proto")]
  public void Proto() {
    Console.WriteLine($"cmd= {Ctx.cmd}");
    Console.WriteLine($"q= {Ctx.q}");
    Console.WriteLine($"param= {string.Join(", ", Ctx.parameters.Select(p => $"{p.Key}={p.Value}"))}");
  }
}

class EntryPoint {
  public static void Main(string[] args) {
    new FooCommand(args).Exec();
    Console.WriteLine("end");
    Console.ReadKey();
  }
}