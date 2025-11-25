public struct MatchResult
{
    public bool isMatch;
    public CardModel a, b;
    public MatchResult(bool isMatch, CardModel a, CardModel b)
    {
        this.isMatch = isMatch;
        this.a = a; this.b = b;
    }
}