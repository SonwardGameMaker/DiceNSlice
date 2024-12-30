public class Modifier
{
    #region fields
    private int _value;
    private ModifierType _type;
    private object _source;
    #endregion

    #region init
    public Modifier(int value, ModifierType type, object source)
    {
        _value = value;
        _type = type;
        _source = source;
    }
    #endregion

    #region properties
    public int Value { get => _value; }
    public ModifierType Type { get => _type; }
    public object Source { get => _source; }
    #endregion
}
