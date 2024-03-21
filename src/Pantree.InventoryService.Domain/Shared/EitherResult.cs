namespace Pantree.InventoryService.Domain.Shared;

public class EitherResult<TLeft, TRight> {

    private readonly TLeft? _left;
    private readonly TRight? _right;
    private readonly bool _isLeft;

    public T Match<T>(Func<TLeft, T> left, Func<TRight, T> right) {
        return _isLeft ? left(_left!) : right(_right!);
    }

    public EitherResult(TLeft left) {
        _left = left;
        _isLeft = true;
    }

    public EitherResult(TRight right) {
        _right = right;
        _isLeft = false;
    }
}