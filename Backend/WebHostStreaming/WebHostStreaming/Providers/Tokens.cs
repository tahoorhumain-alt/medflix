using System;

namespace WebHostStreaming.Helpers
{
    public static class Tokens
    {
        public static string TmdbApiKey =>
            Environment.GetEnvironmentVariable("TMDB_API_KEY")
            ?? throw new InvalidOperationException(
                "TMDB_API_KEY environment variable is not set.");
    }
}
