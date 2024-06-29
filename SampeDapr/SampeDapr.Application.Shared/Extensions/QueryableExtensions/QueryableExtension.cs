using System.Linq.Expressions;

namespace SampeDapr.Application.Shared.Extensions
{
    public static class QueryableExtension
    {
        public enum Order
        {
            Asc,
            Desc
        }

        public static IQueryable<T> OrderByDynamic<T>(
            this IQueryable<T> query,
            string orderByMember,
            Order direction)
        {
            var queryElementTypeParam = Expression.Parameter(typeof(T));

            var memberAccess = Expression.PropertyOrField(queryElementTypeParam, orderByMember);

            var keySelector = Expression.Lambda(memberAccess, queryElementTypeParam);

            var orderBy = Expression.Call(
                typeof(Queryable),
                direction == Order.Asc ? "OrderBy" : "OrderByDescending",
                new Type[] { typeof(T), memberAccess.Type },
                query.Expression,
                Expression.Quote(keySelector));

            return query.Provider.CreateQuery<T>(orderBy);
        }

        public static Expression<Func<T, bool>>? BuildExpressionByComparison<T>(string propertyName, 
            string value, 
            string comparison, 
            string? inToContains = null)
        {
            switch (comparison)
            {
                case "Contains":
                    return BuildContainsExpression<T>(propertyName, value);

                case "In":
                    return BuildInExpression<T>(propertyName, value, inToContains);

                case "==":
                    return BuildEqualExpression<T>(propertyName, value);

                case "ContainsAny":
                    return BuildContainsAnyExpression<T>(propertyName, value);
                case ">=":
                    return BuildGreaterThanOrEqualToExpression<T>(propertyName, value);

                case "<=":
                    return BuildLessThanOrEqualToExpression<T>(propertyName, value);

                case ">":
                    return BuildGreaterThanExpression<T>(propertyName, value);

                case "<":
                    return BuildLessThanExpression<T>(propertyName, value);

                case "StartsWith":
                    return BuildStartsWithExpression<T>(propertyName, value);

                case "EndsWith":
                    return BuildEndsWithExpression<T>(propertyName, value);

                case "Recently":
                    return BuildRecentlyExpression<T>(propertyName, value);
            }

            return _ => false;
        }

        #region Build Sort
        public static Expression<Func<T, object>> BuildSortExpression<T>(string propertyName)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "p");
            MemberExpression property = Expression.Property(param, propertyName);
            UnaryExpression body = Expression.Convert(property, typeof(object));
            return Expression.Lambda<Func<T, object>>(body, param);
        }
        #endregion

        #region Build Equals
        public static Expression<Func<T, bool>>? BuildEqualExpression<T>(string propertyName, string value)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "p");
            BinaryExpression body = Expression.Equal(Expression.Property(param, propertyName), Expression.Constant(value));
            return Expression.Lambda<Func<T, bool>>(body, param);
        }
        #endregion

        #region Build Or
        /// <summary>
        /// Put 'or' operator between items in group (item count >= 2).
        /// </summary>
        public static Expression<Func<T, bool>> CombineExpressionsWithOr<T>(IEnumerable<Expression<Func<T, bool>>> expressions)
        {
            if (expressions.Any() is false) return _ => false;
            Expression initial = expressions.First().Body;
            ParameterExpression param = expressions.First().Parameters[0];
            foreach (var expression in expressions)
            {
                initial = Expression.OrElse(initial, Expression.Invoke(expression, param));
            }
            return Expression.Lambda<Func<T, bool>>(initial, param);
        }
        #endregion

        #region Build In
        public static Expression<Func<T, bool>> BuildInExpression<T>(string propertyName, string value, string? inToContains = null)
        {
            if (!string.IsNullOrWhiteSpace(inToContains)
                && propertyName.Equals(inToContains))
                return BuildInToContainsExpression<T>(propertyName, value);

            Expression? combined = null;
            ParameterExpression param = Expression.Parameter(typeof(T), "p");
            List<string> values = value.Split(',').ToList();
            foreach (string item in values)
            {
                BinaryExpression body = Expression.Equal(Expression.Property(param, propertyName), Expression.Constant(item));
                combined = combined == null ? body : Expression.OrElse(combined, body);
            }
            if (combined == null) return _ => false;
            return Expression.Lambda<Func<T, bool>>(combined, param);
        }

        public static Expression<Func<T, bool>> BuildInToContainsExpression<T>(string propertyName, string value, string keySplit = "[,]")
        {
            Expression? combined = null;
            ParameterExpression param = Expression.Parameter(typeof(T), "p");
            List<string> valueArray = value.Split(keySplit).ToList();
            foreach (string valueItem in valueArray)
            {
                // Tạo biểu thức kiểm tra xem giá trị của thuộc tính có chứa giá trị cần tìm kiếm không
                MethodCallExpression containsExpression = Expression.Call(
                    Expression.Property(param, propertyName),
                    typeof(string).GetMethod("Contains", new[] { typeof(string) })!,
                    Expression.Constant(valueItem));

                // Kết hợp các biểu thức kiểm tra bằng phép OR
                combined = combined == null ? (Expression)containsExpression : Expression.OrElse(combined, containsExpression);
            }
            if (combined == null) return _ => false;
            return Expression.Lambda<Func<T, bool>>(combined, param);
        }

        /// <summary>
        /// If property is boolean and value is true
        /// </summary>
        /// <param name="propertyInput"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> BuildInExpression<T>(IEnumerable<string> propertyNames)
        {
            Expression? combined = null;
            ParameterExpression param = Expression.Parameter(typeof(T), "p");
            foreach (string propertyName in propertyNames)
            {
                BinaryExpression body = Expression.Equal(Expression.Property(param, propertyName), Expression.Constant(true));
                combined = combined == null ? body : Expression.OrElse(combined, body);
            }
            if (combined == null) return _ => false;
            return Expression.Lambda<Func<T, bool>>(combined, param);
        }
        #endregion

        #region Build Contains
        /// <summary>
        /// One Property and one value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> BuildContainsExpression<T>(string propertyName, string value)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "p");
            MethodCallExpression body = Expression.Call(
                Expression.Property(param, propertyName),
                typeof(string).GetMethod("Contains", new[] { typeof(string) })!,
                Expression.Constant(value));
            return Expression.Lambda<Func<T, bool>>(body, param);
        }

        /// <summary>
        /// Many Properties and one value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyNames"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> BuildKeywordsExpression<T>(IEnumerable<string> propertyNames, string value)
        {
            Expression? combined = null;
            ParameterExpression param = Expression.Parameter(typeof(T), "p");
            foreach (string propertyName in propertyNames)
            {
                MethodCallExpression body = Expression.Call(
                    Expression.Property(param, propertyName),
                    typeof(string).GetMethod("Contains", new[] { typeof(string) })!,
                    Expression.Constant(value));

                combined = combined == null ? body : Expression.OrElse(combined, body);
            }
            if (combined == null) return _ => false;
            return Expression.Lambda<Func<T, bool>>(combined, param);
        }

        /// <summary>
        /// Many Properties and many values (join by ",")
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>>? BuildContainsAnyExpression<T>(this string propertyName, string value, string keySplit = "[,]")
        {
            Expression? combined = null;
            ParameterExpression param = Expression.Parameter(typeof(T), "p");
            List<string> values = value.Split(keySplit).ToList();
            foreach (string item in values)
            {
                MethodCallExpression body = Expression.Call(
                    Expression.Property(param, propertyName),
                    typeof(string).GetMethod("Contains", new[] { typeof(string) })!,
                    Expression.Constant(item));

                combined = combined == null ? body : Expression.OrElse(combined, body);
            }
            if (combined == null) return _ => false;
            return Expression.Lambda<Func<T, bool>>(combined, param);
        }
        #endregion

        #region Build Greater/Less than
        public static Expression<Func<T, bool>> BuildGreaterThanExpression<T>(string propertyName, string value)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "p");
            Expression propertyExpr = Expression.Property(param, propertyName);
            // Parse the input string as a DateTime
            if (DateTime.TryParse(value, out DateTime recentDate))
            {
                recentDate = recentDate.Date.AddDays(1).AddMilliseconds(-1);
                // Calculate the start date based on the provided value
                // Assuming the property is of type DateTime, adjust accordingly if it's a different type
                if (propertyExpr.Type == typeof(DateTime))
                {
                    BinaryExpression body = Expression.GreaterThan(propertyExpr, Expression.Constant(recentDate));
                    return Expression.Lambda<Func<T, bool>>(body, param);
                }
                else if (propertyExpr.Type == typeof(DateTime?))
                {
                    BinaryExpression body = Expression.GreaterThan(propertyExpr, Expression.Constant(recentDate, typeof(DateTime?)));
                    return Expression.Lambda<Func<T, bool>>(body, param);
                }
                else
                {
                    // Handle the case where the property is not DateTime (e.g., log an error or throw an exception)
                    throw new InvalidOperationException($"Property {propertyName} is not of type DateTime.");
                }
            }
            else
            {
                // Handle the case where the input string is not a valid DateTime
                BinaryExpression body = Expression.GreaterThan(Expression.Property(param, propertyName), Expression.Constant(value));
                return Expression.Lambda<Func<T, bool>>(body, param);
            }
        }

        public static Expression<Func<T, bool>> BuildGreaterThanOrEqualToExpression<T>(string propertyName, string value)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "p");
            Expression propertyExpr = Expression.Property(param, propertyName);
            // Parse the input string as a DateTime
            if (DateTime.TryParse(value, out DateTime recentDate))
            {
                // Calculate the start date based on the provided value
                // Assuming the property is of type DateTime, adjust accordingly if it's a different type
                if (propertyExpr.Type == typeof(DateTime))
                {
                    BinaryExpression body = Expression.GreaterThanOrEqual(propertyExpr, Expression.Constant(recentDate));
                    return Expression.Lambda<Func<T, bool>>(body, param);
                }
                else if (propertyExpr.Type == typeof(DateTime?))
                {
                    BinaryExpression body = Expression.GreaterThanOrEqual(propertyExpr, Expression.Constant(recentDate, typeof(DateTime?)));
                    return Expression.Lambda<Func<T, bool>>(body, param);
                }
                else
                {
                    // Handle the case where the property is not DateTime (e.g., log an error or throw an exception)
                    throw new InvalidOperationException($"Property {propertyName} is not of type DateTime.");
                }
            }
            else
            {
                // Handle the case where the input string is not a valid DateTime
                BinaryExpression body = Expression.GreaterThanOrEqual(Expression.Property(param, propertyName), Expression.Constant(value));
                return Expression.Lambda<Func<T, bool>>(body, param);
            }
        }

        public static Expression<Func<T, bool>> BuildLessThanExpression<T>(string propertyName, string value)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "p");
            Expression propertyExpr = Expression.Property(param, propertyName);
            // Parse the input string as a DateTime
            if (DateTime.TryParse(value, out DateTime recentDate))
            {
                recentDate = recentDate.Date.AddMilliseconds(-1);
                // Assuming the property is of type DateTime, adjust accordingly if it's a different type
                if (propertyExpr.Type == typeof(DateTime))
                {
                    BinaryExpression body = Expression.LessThan(propertyExpr, Expression.Constant(recentDate));
                    return Expression.Lambda<Func<T, bool>>(body, param);
                }
                else if (propertyExpr.Type == typeof(DateTime?))
                {
                    BinaryExpression body = Expression.LessThan(propertyExpr, Expression.Constant(recentDate, typeof(DateTime?)));
                    return Expression.Lambda<Func<T, bool>>(body, param);
                }
                else
                {
                    // Handle the case where the property is not DateTime (e.g., log an error or throw an exception)
                    throw new InvalidOperationException($"Property {propertyName} is not of type DateTime.");
                }
            }
            else
            {
                // Handle the case where the input string is not a valid DateTime
                BinaryExpression body = Expression.LessThan(Expression.Property(param, propertyName), Expression.Constant(value));
                return Expression.Lambda<Func<T, bool>>(body, param);
            }
        }

        public static Expression<Func<T, bool>> BuildLessThanOrEqualToExpression<T>(string propertyName, string value)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "p");
            Expression propertyExpr = Expression.Property(param, propertyName);
            // Parse the input string as a DateTime
            if (DateTime.TryParse(value, out DateTime recentDate))
            {
                recentDate = recentDate.Date.AddDays(1).AddMilliseconds(-1);
                // Calculate the start date based on the provided value
                // Assuming the property is of type DateTime, adjust accordingly if it's a different type
                if (propertyExpr.Type == typeof(DateTime))
                {
                    BinaryExpression body = Expression.LessThanOrEqual(propertyExpr, Expression.Constant(recentDate));
                    return Expression.Lambda<Func<T, bool>>(body, param);
                }
                else if (propertyExpr.Type == typeof(DateTime?))
                {
                    BinaryExpression body = Expression.LessThanOrEqual(propertyExpr, Expression.Constant(recentDate, typeof(DateTime?)));
                    return Expression.Lambda<Func<T, bool>>(body, param);
                }
                else
                {
                    // Handle the case where the property is not DateTime (e.g., log an error or throw an exception)
                    throw new InvalidOperationException($"Property {propertyName} is not of type DateTime.");
                }
            }
            else
            {
                // Handle the case where the input string is not a valid DateTime
                BinaryExpression body = Expression.LessThanOrEqual(Expression.Property(param, propertyName), Expression.Constant(value));
                return Expression.Lambda<Func<T, bool>>(body, param);
            }
        }
        #endregion

        #region Build Width
        public static Expression<Func<T, bool>> BuildStartsWithExpression<T>(string propertyName, string value)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "p");
            MethodCallExpression body = Expression.Call(
                Expression.Property(param, propertyName),
                typeof(string).GetMethod("StartsWith", new[] { typeof(string) })!,
                Expression.Constant(value)
            );

            return Expression.Lambda<Func<T, bool>>(body, param);
        }

        public static Expression<Func<T, bool>> BuildEndsWithExpression<T>(string propertyName, string value)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "p");
            MethodCallExpression body = Expression.Call(
                Expression.Property(param, propertyName),
                typeof(string).GetMethod("EndsWith", new[] { typeof(string) })!,
                Expression.Constant(value)
            );
            return Expression.Lambda<Func<T, bool>>(body, param);
        }
        #endregion

        #region Build Recently
        public static Expression<Func<T, bool>> BuildRecentlyExpression<T>(string propertyName, string value)
        {
            // Implement your logic for Recently
            // You may need to parse the value and compare it with the current date or some other criteria.
            // For simplicity, let's assume Recently means within the last X days.
            DateTime recentDate = DateTime.UtcNow.AddDays(-Convert.ToDouble(value));

            ParameterExpression param = Expression.Parameter(typeof(T), "p");
            BinaryExpression body = Expression.GreaterThanOrEqual(Expression.Property(param, propertyName), Expression.Constant(recentDate));

            return Expression.Lambda<Func<T, bool>>(body, param);
        }
        #endregion
    }
}
