using System;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Rent.Models;
using Rent.Models.TimePlanning;

namespace Rent.DTOAssemblers
{
    public class TestExpression
    {
        class Dummy
        {
            public int ID { get; set; }
            public dynamic User { get; set; }
            public dynamic Creator { get; set; }


        }





        static Expression<Func<User, dynamic>> userSelect = u => new { u.FirstName };

        public static Expression<Func<Absence, dynamic>> CreateNewStatement(params string[] fields)
        {
            // input parameter "o"
            var xParameter = Expression.Parameter(typeof(Absence), "o");

            // new statement "new Data()"
            var xNew = Expression.New(typeof(Dummy));

            var dummy = new Dummy();

            // create initializers
            var bindings = fields
                .Select(f =>
                {
                    // property "Field1"
                    var mi = typeof(Absence).GetProperty(f);

                    var t = mi.GetType();

                    var xOriginal = Expression.Property(xParameter, mi);

                    if (f.Equals("User") || f.Equals("Creator"))
                    {


                        ParameterExpression transactionParameter = Expression.Parameter(typeof(User), "t");
                        //MemberExpression transactionDateTime = Expression.Property(transactionParameter, "Creator");

                        MethodInfo maxMethod = GetWhereMethod();

                        var miContain = typeof(User).GetMethod("GetDynamic", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

                        Console.WriteLine(miContain);
                        var call = Expression.Call(maxMethod, xOriginal, xOriginal);

                        //var transactionDate = Expression.Lambda<Func<User, dynamic>>(userSelect, new ParameterExpression[] { transactionParameter });
                        //var transactionDate = Expression.Lambda<Func<User, dynamic>>(, new ParameterExpression[] { transactionParameter });
                        return Expression.Bind((typeof(Dummy)).GetField("Creator"), call);

                        // return Expression.Bind((typeof(Dummy)).GetField("Creator"), lastTransactionDate);




                        /*
                        ParameterExpression transactionParameter = Expression.Parameter(typeof(User), "u");
                        //MemberExpression transactionDateTime = Expression.Property(transactionParameter, mi);


                        // MethodInfo methodInfo = (userSelect.Body as MethodCallExpression).Method;

                        MethodInfo whereMethod = GetWhereMethod();
                        //MethodInfo methodInfo = (userSelect.Compile()).Method;
                        var transactionDate = Expression.Lambda<Func<User, dynamic>>(userSelect, new ParameterExpression[] { transactionParameter });
                        //var transactionDate = Expression.Lambda<Func<User, dynamic>>(xOriginal, new ParameterExpression[] { transactionParameter });

                        //var call = Expression.Call(null, whereMethod, transactionDate);
                        return Expression.Bind(typeof(Dummy).GetProperty(f), transactionDate);
                        //return Expression.Bind(typeof(Dummy).GetProperty(f), call);

                    */
                    }
                    /*
                    if (f.Equals("User") || f.Equals("Creator"))
                    {

                        MethodInfo methodInfo = (userSelect.Body as MethodCallExpression).Method;
                        ParameterExpression parameter = Expression.Parameter(typeof(User), "u");


                        var transactionDate = Expression.Lambda<Func<User, dynamic>>(xOriginal, new ParameterExpression[] { parameter });



                        //Expression.Call(null, )

                        var lastTransactionDate = Expression.Call(null, methodInfo, userSelect);

                        Expression left = Expression.Call(mi, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));


                        return Expression.Bind(typeof(Dummy).GetProperty(f), Expression.Call(dummy, typeof(Dummy).GetMethod(""), xOriginal));
                    }
                    */

                    return Expression.Bind(mi, xOriginal);
                }
            );

            // initialization "new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var xInit = Expression.MemberInit(xNew, bindings);

            // expression "o => new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var lambda = Expression.Lambda<Func<Absence, dynamic>>(xInit, xParameter);

            // compile to Func<Data, Data>
            return lambda;
        }

        private static MethodInfo GetWhereMethod()
        {
            Func<User, dynamic> fake = element => new { };
            Expression<Func<User, dynamic>> lamda = u => new { };
            return (lamda.Compile()).Method;
        }
    }
}
