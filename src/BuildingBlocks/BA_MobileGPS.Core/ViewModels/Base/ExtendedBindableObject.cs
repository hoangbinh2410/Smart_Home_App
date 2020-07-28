using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BA_MobileGPS.Core.ViewModels
{
    public abstract class ExtendedBindableObject : BindableBase
    {
        public bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = null, params string[] relatedProperty)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            RaisePropertyChanged(propertyName);

            foreach (var property in relatedProperty)
            {
                RaisePropertyChanged(property);
            }

            return true;
        }

        public void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            RaisePropertyChanged(GetMemberInfo(property).Name);
        }

        private MemberInfo GetMemberInfo(Expression expression)
        {
            MemberExpression operand;
            LambdaExpression lambdaExpression = (LambdaExpression)expression;
            if (lambdaExpression.Body as UnaryExpression != null)
            {
                UnaryExpression body = (UnaryExpression)lambdaExpression.Body;
                operand = (MemberExpression)body.Operand;
            }
            else
            {
                operand = (MemberExpression)lambdaExpression.Body;
            }
            return operand.Member;
        }
    }
}