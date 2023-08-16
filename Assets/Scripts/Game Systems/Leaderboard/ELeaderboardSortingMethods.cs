/*UPDATED AS OF: THURSDAY, FEBRUARY 2, 2017*/

/*
Description: Enum used to store all the different ways that the leaderboard can be sorted. Math is done using this enum. Therefore
is imperative that the sorting methods members are declared in pairs, the "highest" method followed by the "lowest" method.
Parameters(Optional): 
Creator: Alvaro Chavez Mixco
Creation Date:  Sunday, Novemeber 13, 2016
Extra Notes: 
*/
public enum ELeaderboardSortingMethods
{
    Alphabetically,
    InverseAlphabetically,
    HighestScore,
    LowestScore,
    HighestStreak,
    LowestStreak,
    HighestTime,
    LowestTime,
    HighestShotsFired,
    LowestShotsFired,
    HighestShotsHit,
    LowestShotsHit,
    HighestAccuracy,
    LowestAccuracy,
    HighestNumberOfTricks,
    LowestNumberOfTricks,
    HighestNumberOfCombos,
    LowestNumberOfCombos,
}