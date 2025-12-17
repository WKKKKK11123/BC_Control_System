namespace BC_Control_Models
{
    public interface ILogOpration
    {
        void WriteError(string Message);
        void WriteError(Exception EX);
        void WriteInfo(string Message);
    }
}