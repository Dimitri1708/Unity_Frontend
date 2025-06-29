using JetBrains.Annotations;

namespace Dto_s
{
    public static class Environment
    {
        [CanBeNull] public static string environmentId { get; set; }
        public static int environmentXScale { get; set; }
        public static int environmentYScale { get; set; }
    }
}