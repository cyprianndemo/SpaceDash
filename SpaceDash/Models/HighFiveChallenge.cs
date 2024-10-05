namespace SpaceDash.Models
{
    public class HighFiveChallenge : Challenge
    {
        public string ChallengeDescription { get; set; } // Description for the HighFive challenge
        public int RequiredHighFives { get; set; } = 1; // Number of high-fives needed
    }

}
