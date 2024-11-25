using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M450_Bowlingcounter
{
    public class Player
    {
        public string Name { get; }
        private List<string> _rolls { get; }
        private int _skill { get; set; }

        public Player(string name, int skill)
        {
            _skill = skill;
            Name = name;
            _rolls = new List<string>();
        }

        public void RecordRoll(string roll)
        {
            _rolls.Add(roll);
        }

        public List<string> GetRolls()
        {
            return _rolls;
        }

        public int GetSkill()
        {
            return _skill;
        }

        public int CalculateTotalScore()
        {
            List<string> rolls = this._rolls;
            int score = 0;
            int rollIndex = 0;
            // StringBuilder frameCalculations = new StringBuilder();

            for (int frame = 1; frame <= 10; frame++)
            {
                // string frameString;

                if (frame == 10)
                {
                    // frameString = $"{GetFrameTenString(rolls, rollIndex)} = {CalculateFrameTenScore(rolls, rollIndex)} + {score} = {score + CalculateFrameTenScore(rolls, rollIndex)}";
                    score += CalculateFrameTenScore(rolls, rollIndex);
                    rollIndex += 3; // Handle all rolls in the 10th frame
                }
                else if (IsStrike(rolls, rollIndex))
                {
                    // frameString = $"{GetStrikeFrameString(rolls, rollIndex)} = {CalculateStrikeScore(rolls, rollIndex)} + {score} = {score + CalculateStrikeScore(rolls, rollIndex)}";
                    score += CalculateStrikeScore(rolls, rollIndex);
                    rollIndex += 2;
                }
                else if (IsSpare(rolls, rollIndex))
                {
                    // frameString = $"{GetSpareFrameString(rolls, rollIndex)} = {CalculateSpareScore(rolls, rollIndex)} + {score} = {score + CalculateSpareScore(rolls, rollIndex)}";
                    score += CalculateSpareScore(rolls, rollIndex);
                    rollIndex += 2;
                }
                else
                {
                    // frameString = $"{GetOpenFrameString(rolls, rollIndex)} = {CalculateOpenFrameScore(rolls, rollIndex)} + {score} = {score + CalculateOpenFrameScore(rolls, rollIndex)}";
                    score += CalculateOpenFrameScore(rolls, rollIndex);
                    rollIndex += 2;
                }

                // Append the frame's string to the overall calculation
                // if (frame > 1)
                // frameCalculations.Append(" | ");
                //  frameCalculations.Append(frameString);
            }

            // Print the final string after processing all frames
            // Console.WriteLine("FrameCalc: " + frameCalculations.ToString());


            return score;
        }

        
        private string GetStrikeFrameString(List<string> rolls, int rollIndex)
        {
            int bonus1 = GetRollValue(rolls, rollIndex + 2);
            int bonus2 = GetRollValue(rolls, rollIndex + 3);
            if(bonus2 == -1)
            {
                bonus2 = GetRollValue(rolls, rollIndex + 4);
            }
            return $"10+{bonus1}+{bonus2}";
        }

        private string GetSpareFrameString(List<string> rolls, int rollIndex)
        {
            int firstRoll = GetRollValue(rolls, rollIndex);
            int bonus = GetRollValue(rolls, rollIndex + 2);
            return $"{firstRoll}+/+{bonus}";
        }

        private string GetOpenFrameString(List<string> rolls, int rollIndex)
        {
            int firstRoll = GetRollValue(rolls, rollIndex);
            int secondRoll = GetRollValue(rolls, rollIndex + 1);
            return $"{firstRoll}+{secondRoll}";
        }

        private string GetFrameTenString(List<string> rolls, int rollIndex)
        {
            int firstRoll = GetRollValue(rolls, rollIndex);
            int secondRoll = GetRollValue(rolls, rollIndex + 1);
            int thirdRoll = GetRollValue(rolls, rollIndex + 2);
            return $"{firstRoll}+{secondRoll}+{thirdRoll}";
        }
        


        public int CalculateStrikeScore(List<string> rolls, int rollIndex)
        {
            int bonus1 = GetRollValue(rolls, rollIndex + 2);
            int bonus2 = GetRollValue(rolls, rollIndex + 3);
            if (bonus2 == -1)
            {
                bonus2 = GetRollValue(rolls, rollIndex + 4);
            }

            return 10 + bonus1 + bonus2;
        }

        public int CalculateSpareScore(List<string> rolls, int rollIndex)
        {
            int bonus = GetRollValue(rolls, rollIndex + 2);
            return 10 + bonus;
        }

        public int CalculateOpenFrameScore(List<string> rolls, int rollIndex)
        {
            int roll1 = GetRollValue(rolls, rollIndex);
            int roll2 = GetRollValue(rolls, rollIndex + 1);
            return roll1 + roll2;
        }

        public int CalculateFrameTenScore(List<string> rolls, int rollIndex)
        {
            int score = 0;

            // rules for 10th frame
            if (rolls.Count - rollIndex == 3)
            {
                for (int i = rollIndex; i < rolls.Count; i++)
                {
                    score += GetRollValue(rolls, i);
                }
            }
            else if (rolls.Count - rollIndex >= 2)
            {
                score = GetRollValue(rolls, rollIndex) + GetRollValue(rolls, rollIndex + 1);
                if (IsStrike(rolls, rollIndex) || IsSpare(rolls, rollIndex + 1))
                {
                    score += GetRollValue(rolls, rollIndex + 2);
                }
            }
            else
            { // Nur 1 Wurf im 10. Frame
                score = GetRollValue(rolls, rollIndex);
            }

            return score;
        }

        public int GetRollValue(List<string> rolls, int index)
        {
            if (index >= rolls.Count) return 0;
            if (rolls[index] == "X") return 10; // Strike
            if (rolls[index] == "/") return 10 - (index > 0 ? GetRollValue(rolls, index - 1) : 0); // Spare
            if (rolls[index] == "-") return -1; // skipping this throw because of previous strike
            if (rolls[index] == "G") return 0;  // 0 points for Gutter shot
            if (rolls[index] == "F") return 0;  // 0 points for Foul
            return int.TryParse(rolls[index], out int value) ? value : 0;
        }

        public bool IsStrike(List<string> rolls, int index)
        {
            return index < rolls.Count && rolls[index] == "X";
        }

        public bool IsSpare(List<string> rolls, int index)
        {
            return index + 1 < rolls.Count && rolls[index + 1] == "/";
        }
    }


}
