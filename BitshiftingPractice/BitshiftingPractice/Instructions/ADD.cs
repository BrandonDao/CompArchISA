namespace BitshiftingPractice.Instructions
{
    public class ADD : MathAndLogic
    {
        public override byte OpCode => instructionData[0];

        public ADD(string instructionStr)
        {
            instructionData = base.InstructionToInstructionData(instructionStr, OpCodes.ADD);
        }
    }
}
