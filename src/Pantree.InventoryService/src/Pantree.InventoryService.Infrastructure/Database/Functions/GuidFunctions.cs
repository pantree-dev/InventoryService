using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Pantree.InventoryService.Infrastructure.Database.Functions;

/// <summary>
/// Extension methods that help EF Core do cursor based pagination when the primary key/value
/// you want to select your cursor for is a GUID/char(36) db type.
/// </summary>
public static class GuidFunctions {
    
    public static bool IsGreaterThan(this Guid left, Guid right) => left.CompareTo(right) > 0;
    
    public static bool IsGreaterThanOrEqual(this Guid left, Guid right) => left.CompareTo(right) >= 0;
    
    public static bool IsLessThan(this Guid left, Guid right) => left.CompareTo(right) < 0;
    
    public static bool IsLessThanOrEqual(this Guid left, Guid right) => left.CompareTo(right) <= 0;
    
    public static void Register(ModelBuilder builder) {
        RegisterFunction(builder, nameof(IsGreaterThan), ExpressionType.GreaterThan);
        RegisterFunction(builder, nameof(IsGreaterThanOrEqual), ExpressionType.GreaterThanOrEqual);
        RegisterFunction(builder, nameof(IsLessThan), ExpressionType.LessThan);
        RegisterFunction(builder, nameof(IsLessThanOrEqual), ExpressionType.LessThanOrEqual);
    }
    
    static void RegisterFunction(ModelBuilder builder, string name, ExpressionType type) {
        var method = typeof(GuidFunctions).GetMethod(name, new[] { typeof(Guid), typeof(Guid) });
        builder.HasDbFunction(method!).HasTranslation(parameters => {
            var left = parameters.ElementAt(0);
            var right = parameters.ElementAt(1);
            return new SqlBinaryExpression(type, left, right, typeof(bool), null);
            //return Expression.MakeBinary(type, left, right, false, method);
        });
    }
}