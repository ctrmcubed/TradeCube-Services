using System.Diagnostics;

[DebuggerDisplay("Cube={Cube}, DataItem={DataItem}, Node={Node}, Layer={Layer}, TimeNode={TimeNode}, Value={Value}, TimeNodeType={TimeNodeType}")]
public class CubeDataDataObject
{
    public string Cube { get; set; }
    public string DataItem { get; set; }
    public string Node { get; set; }
    public string TimeNode { get; set; }
    public string TimeNodeType { get; set; }
    public string Layer { get; set; }
    public decimal? Value { get; set; }
}