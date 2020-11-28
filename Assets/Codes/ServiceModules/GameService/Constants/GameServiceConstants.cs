namespace Codes.ServiceModules.GameService.Constants
{
    public static class GameServiceConstants
    {
        #if HMS_BUILD
            public const string leaderboard_best_scores = "11EDF1563B3325BE12A6655C7D35B7393C754491C764B16D7AB3DAF7D85BE5C8"; // <HMS>
            public const string achievement_reached_to_50_point = "407F6A67DF821A5FE7546F23A612B0421DCC9F669851CE0E05C6B49D1C94E9AA"; // <HMS>
            public const string achievement_reached_to_25_point = "31363640336A595326F40CD0D4E25F844507450AE5C395A50D7F56E56E7CB1C8"; // <HMS>
        #else
            public const string leaderboard_best_scores = "CgkItO6L8ssGEAIQAw"; // <GPGSID>
            public const string achievement_reached_to_50_point = "CgkItO6L8ssGEAIQAg"; // <GPGSID>
            public const string achievement_reached_to_25_point = "CgkItO6L8ssGEAIQAQ"; // <GPGSID>
        #endif
    }
}

