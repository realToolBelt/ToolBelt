namespace ToolBelt.Validation
{
    public interface IValidity
    {
        bool IsValid { get; }

        void ClearValidationErrors();
    }
}
