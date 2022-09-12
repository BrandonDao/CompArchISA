namespace BitshiftingPractice.Instructions
{
    public class SUB : MathAndLogic
    {
        public override byte OpCode => instructionData[0];

        public SUB(string instructionStr)
        {
            instructionData = base.InstructionToInstructionData(instructionStr, OpCodes.SUB);
        }
    }
}
