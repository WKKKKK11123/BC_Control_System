namespace BC_Control_Models
{
    public interface IVariable
    {
        string DataType { get; set; }
        float Offset { get; set; }
        string Remark { get; set; }
        string VarName { get; set; }
        object VarValue { get; set; }
        int OffsetOrLength { get; set; }
        float Scale { get; set; }
    }
}